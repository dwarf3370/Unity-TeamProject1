using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBase : MonoBehaviour
{
    // 스킬 데이터
    protected SkillData _skillData;
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected ParticleSystem castEffect;

    [Header("Audio")]
    [SerializeField] protected string castAudioClipName;

    protected ParticleSystem _castEffectInstance;

    // firePos의 transform, 계속 변함
    protected Transform _fireTransform;

    // Vector3형은 값타입이므로 변하지않음
    // 캐스팅 시작 시 firePos의 위치를 기억
    protected Vector3 _startFirePos;

    protected float _startDir;
    protected GameObject _user;

    // AreaProjectile이 발생하는 위치는 castPos가 아닌 플레이어 기준으로 해야함
    protected Vector3 _startUserPos;
    protected Vector3 _areaProjectilePos;

    protected float _attackPoint;
    
    // 유저의 방향을 알 수 있는 변수 필요

    public Transform FireTransform { get { return _fireTransform; } set { _fireTransform = value; } }
    public Vector3 StartFirePos { get { return _startFirePos; } set { _startFirePos = value; } }
    public float StartDir { get { return _startDir; } set { _startDir = value; } }

    public GameObject User { get { return _user; } set { _user = value; } }
    public Vector3 StartUserPos { get { return _startUserPos; } set { _startUserPos = value; } }
    public Vector3 AreaProjectilePos { get { return _areaProjectilePos; } }
    public float AttackPoint { get { return _attackPoint; } set { _attackPoint = value; } } 

    public SkillData SkillData { get { return _skillData; } }

    // 고정된 데이터가 아닌 실제 진행중인 쿨타임
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
        GameManager.Instance.player.PlayCast();

        Vector3 dist = new Vector3(_startDir * _skillData.Range, 0, 0);
        _areaProjectilePos = _startUserPos + dist;

        if (castEffect == null)
            return;

        if (!string.IsNullOrEmpty(castAudioClipName))
            SoundManager.Instance.Play(Enums.ESoundType.SFX, castAudioClipName);

        if (_castEffectInstance == null)
        {
            ParticleSystem particle = Instantiate(castEffect);
            _castEffectInstance = particle;
        }
        else
        {
            _castEffectInstance.gameObject.SetActive(true);
        }
    }

    public virtual void DoSkill()
    {
        Debug.Log($"Do Skill : {_skillData.Name}");
        // 스킬 사용 시의 공통 행동
        _currentCoolTime = _skillData.CoolTime * (1 - GameManager.Instance.skillCooltimeReduce / 100f);
    }

    public virtual void StopCast()
    {
        GameManager.Instance.player.StopCast();

        _castEffectInstance?.Stop();
        _castEffectInstance?.gameObject?.SetActive(false);
    }

    public virtual void StopSkill()
    {
        // 스킬 중단 시의 공통 행동
    }
}
