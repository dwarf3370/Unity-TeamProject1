using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SkillHandler : MonoBehaviour
{
    SkillBase[] _playerSkillSlot = new SkillBase[(int)Enums.PlayerSkillSlot.Length];

    public SkillBase[] PlayerSkillSlot { get { return _playerSkillSlot; } }

    Coroutine _castRoutine;

    public UnityAction OnChangedSkillSlot;

    public void EquipSkill(int skillID, Enums.PlayerSkillSlot slot)
    {
        // ID 검사
        if (DataManager.Instance.SkillDict.TryGetValue(skillID, out SkillData data) == false)
        {
            Debug.LogError($"SkillHandler EquipSkill failed... / ID : {skillID}");
            Debug.LogError("Please Check data");
            return;
        }

        SkillBase prefab = ResourceManager.Instance.Load<SkillBase>($"Prefabs/Skills/{data.ClassName}");
        if (prefab == null)
        {
            Debug.LogError($"Can't find SkillBase Component! / ID : {skillID}");
            return;
        }

        SkillBase skill = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        skill.SetData(data.ID);
        skill.transform.SetParent(gameObject.transform);

        // 해당 슬롯에 스킬이 존재하면 해제한다.
        if (_playerSkillSlot[(int)slot] != null)
            UnEquipSkill(slot);

        _playerSkillSlot[(int)slot] = skill;

        OnChangedSkillSlot?.Invoke();
    }

    public void UnEquipSkill(Enums.PlayerSkillSlot slot)
    {
        if (_playerSkillSlot[(int)slot] = null)
            return;

        // slot에 있는 스킬을 튀어나오게 해야함 (fix 됨)


        // slot에서 삭제
        _playerSkillSlot[(int)slot] = null;

        OnChangedSkillSlot?.Invoke();
    }

    public void DoSkill(Enums.PlayerSkillSlot slot, Transform startPos, float attackPoint)
    {
        // 슬롯에 스킬이 존재하는지 비교
        if (_playerSkillSlot[(int)slot] == null)
            return;

        // 쿨타임 체크
        if (_playerSkillSlot[(int)slot].CurrentCoolTime > 0)
            return;

        if (_castRoutine != null)
            return;

        Cast(slot, startPos, attackPoint);
    }

    public void Cast(Enums.PlayerSkillSlot slot, Transform startPos, float attackPoint)
    {
        _playerSkillSlot[(int)slot].StartPos = startPos;
        _playerSkillSlot[(int)slot].User = gameObject;
        // 유저 방향 설정 필요
        _castRoutine = StartCoroutine(CastRoutine(slot, attackPoint));
    }

    IEnumerator CastRoutine(Enums.PlayerSkillSlot slot, float attackPoint)
    {
        WaitForSeconds castTime = new WaitForSeconds(_playerSkillSlot[(int)slot].SkillData.CastTime);

        Debug.Log($"Start Cast : {_playerSkillSlot[(int)slot].SkillData.Name}");
        _playerSkillSlot[(int)slot].DoCast();

        yield return castTime;
        _playerSkillSlot[(int)slot].StopCast();
        _playerSkillSlot[(int)slot].DoSkill(attackPoint);
        _castRoutine = null;
    }

    public void StopSkill(Enums.PlayerSkillSlot slot)
    {
        // 슬롯에 스킬이 존재하는지 비교
        if (_playerSkillSlot[(int)slot] == null)
            return;

        if (_castRoutine != null)
        {
            _playerSkillSlot[(int)slot].StopCast();
            StopCoroutine(_castRoutine);
            _castRoutine = null;
            return;
        }

        _playerSkillSlot[(int)slot].StopSkill();
    }
}
