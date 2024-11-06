using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SkillHandler : MonoBehaviour
{
    SkillBase _basicSkill;

    SkillBase[] _playerSkillSlot = new SkillBase[(int)Enums.PlayerSkillSlot.Length];

    public SkillBase[] PlayerSkillSlot { get { return _playerSkillSlot; } }

    Coroutine _castRoutine;

    public UnityAction OnChangedSkillSlot;

    public UnityAction<int, float> OnSkillUsed;

    public void SetBasicSkill(int skillID)
    {
        // ID 검사
        if (DataManager.Instance.SkillDict.TryGetValue(skillID, out SkillData data) == false)
        {
            Debug.LogError($"SkillHandler SetBasicSkill failed... / ID : {skillID}");
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

        _basicSkill = skill;
    }

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

        // 해당 슬롯에 스킬이 존재하면 해제한다.
        if (_playerSkillSlot[(int)slot] != null)
            UnEquipSkill(slot);

        SkillBase skill = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        skill.SetData(data.ID);
        skill.transform.SetParent(gameObject.transform);

        _playerSkillSlot[(int)slot] = skill;

        GameManager.Instance.PlayerSkillSlotID[(int)slot] = skill.SkillData.ID;

        OnChangedSkillSlot?.Invoke();
    }

    public void UnEquipSkill(Enums.PlayerSkillSlot slot)
    {
        if (_playerSkillSlot[(int)slot] == null)
            return;

        // slot에 있는 스킬을 Drop 해야함 (fix 됨)
        DropSkill(_playerSkillSlot[(int)slot].SkillData.ID);

        Destroy(_playerSkillSlot[(int)slot].gameObject);

        // slot에서 삭제
        _playerSkillSlot[(int)slot] = null;

        GameManager.Instance.PlayerSkillSlotID[(int)slot] = null;

        OnChangedSkillSlot?.Invoke();
    }

    public void DropSkill(int skillID)
    {
        Vector3 skillPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        GameObject prefab = ResourceManager.Instance.Load<GameObject>("Prefabs/DropItem");
        DropItem skillDrop = Instantiate(prefab, skillPosition, Quaternion.identity).GetComponent<DropItem>();
        skillDrop.Initialize(DropItem.ItemType.Skill, skillID);
    }

    public void DoBasicSkill(Transform startPos, float attackPoint)
    {
        // 스킬이 존재하는지 비교
        if (_basicSkill == null)
            return;

        // 쿨타임 체크
        if (_basicSkill.CurrentCoolTime > 0)
            return;

        if (_castRoutine != null)
            return;

        BasicCast(startPos, attackPoint);
    }

    public void BasicCast(Transform fireTransform, float attackPoint)
    {
        _basicSkill.FireTransform = fireTransform;
        _basicSkill.User = gameObject;
        _basicSkill.AttackPoint = attackPoint;
        // 유저 방향 설정 필요
        _castRoutine = StartCoroutine(BasicCastRoutine(attackPoint));
    }

    IEnumerator BasicCastRoutine(float attackPoint)
    {
        WaitForSeconds castTime = new WaitForSeconds(_basicSkill.SkillData.CastTime);

        Debug.Log($"Start Cast : {_basicSkill.SkillData.Name}");
        _basicSkill.DoCast();

        yield return castTime;
        _basicSkill.StopCast();
        _basicSkill.DoSkill();
        _castRoutine = null;
    }

    public void DoSkill(Enums.PlayerSkillSlot slot, Transform fireTransform, float attackPoint)
    {
        // 슬롯에 스킬이 존재하는지 비교
        if (_playerSkillSlot[(int)slot] == null)
            return;

        // 쿨타임 체크
        if (_playerSkillSlot[(int)slot].CurrentCoolTime > 0)
            return;

        if (_castRoutine != null)
            return;

        Cast(slot, fireTransform, attackPoint);
    }

    public void Cast(Enums.PlayerSkillSlot slot, Transform fireTransform, float attackPoint)
    {
        _playerSkillSlot[(int)slot].FireTransform = fireTransform;
        _playerSkillSlot[(int)slot].StartFirePos = fireTransform.position;
        _playerSkillSlot[(int)slot].StartDir = fireTransform.forward.x;
        _playerSkillSlot[(int)slot].User = gameObject;
        _playerSkillSlot[(int)slot].StartUserPos = gameObject.transform.position;
        _playerSkillSlot[(int)slot].AttackPoint = attackPoint;
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
        _playerSkillSlot[(int)slot].DoSkill();
        OnSkillUsed?.Invoke((int)slot, _playerSkillSlot[(int)slot].SkillData.CoolTime * (1 - GameManager.Instance.skillCooltimeReduce / 100f));
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
