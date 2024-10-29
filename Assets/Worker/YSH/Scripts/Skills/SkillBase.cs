using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBase : MonoBehaviour
{
    // ��ų ������
    protected SkillData _skillData;
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected ParticleSystem castEffect;

    protected ParticleSystem _castEffectInstance;

    protected Transform _startPos;
    protected GameObject _user;
    
    // ������ ������ �� �� �ִ� ���� �ʿ�

    public Transform StartPos { get { return _startPos; } set { _startPos = value; } }
    public GameObject User { get { return _user; } set { _user = value; } }

    public SkillData SkillData { get { return _skillData; } }

    // ������ �����Ͱ� �ƴ� ���� �������� ��Ÿ��
    protected float _currentCoolTime;
    public float CurrentCoolTime { get { return _currentCoolTime; } }

    public virtual void SetData(int id)
    {
        if (DataManager.Instance.SkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError($"Skill SetData Error! / ID : {id}");
            return;
        }

        _skillData = data;
    }

    private void Update()
    {
        if (_currentCoolTime > 0)
        {
            _currentCoolTime -= Time.deltaTime;
        }
    }

    public virtual void DoCast()
    {
        if (castEffect == null)
            return;

        if (_castEffectInstance == null)
        {
            ParticleSystem particle = Instantiate(castEffect);
            _castEffectInstance = particle;
        }
    }

    public virtual void DoSkill(float attackPoint)
    {
        Debug.Log($"Do Skill : {_skillData.Name}");
        // ��ų ��� ���� ���� �ൿ
        _currentCoolTime = _skillData.CoolTime;
    }

    public virtual void StopCast()
    {
        _castEffectInstance?.Stop();
    }

    public virtual void StopSkill()
    {
        // ��ų �ߴ� ���� ���� �ൿ
    }
}