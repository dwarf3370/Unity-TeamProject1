using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [Header("Effect")]
    [SerializeField] protected ParticleSystem flashEffect;
    [SerializeField] protected ParticleSystem hitEffect;
    [Header("Audio")]
    [SerializeField] protected string launchAudioClipName;
    [SerializeField] protected string hitAudioClipName;
    [SerializeField] protected bool useCallBack = false;

    protected Coroutine _moveRoutine;

    protected float _damage;
    public float Damage { get { return _damage; } }

    protected SkillBase _ownerSkill;

    public UnityAction<GameObject> OnHit;

    public virtual void SetDamage(float damage)
    {
        _damage = damage;
    }

    public virtual void Fire(SkillBase skill, Vector3 startPos, Vector3 dir, float speed)
    {
        _ownerSkill = skill;

        if (flashEffect != null)
        {
            ParticleSystem effect = Instantiate(flashEffect, startPos, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
        }

        if (!string.IsNullOrEmpty(launchAudioClipName))
            SoundManager.Instance.Play(Enums.ESoundType.SFX, launchAudioClipName);

        _moveRoutine = StartCoroutine(MoveRoutine(skill, startPos, dir, speed));
    }

    protected IEnumerator MoveRoutine(SkillBase skill, Vector3 startPos, Vector3 dir, float speed)
    {
        while (true)
        {
            Vector3 moveDist = transform.position + dir * speed * Time.deltaTime;
            if ((moveDist - startPos).sqrMagnitude > skill.SkillData.Range * skill.SkillData.Range)
            {
                break;
            }

            transform.position += dir * speed * Time.deltaTime;

            yield return null;
        }

        _moveRoutine = null;
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, other.transform.position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
        }

        if (!string.IsNullOrEmpty(hitAudioClipName))
            SoundManager.Instance.Play(Enums.ESoundType.SFX, hitAudioClipName);

        if (useCallBack == false)
        {
            MonsterState monster = other.GetComponent<MonsterState>();
            if (monster != null)
            {
                monster.IsHit(_damage);
            }

            Debug.Log($"Projectile Damage : {_damage}");
        }
        else
        {
            OnHit?.Invoke(other.gameObject);
        }

        // 관통 여부 확인
        if (_ownerSkill.SkillData.CanPenetration == false)
            Destroy(gameObject);
    }

    public void GenerateEffect(Vector3 pos, Quaternion quaternion)
    {
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, pos, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
        }
    }
}
