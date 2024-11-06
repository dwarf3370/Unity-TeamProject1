using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile : Projectile
{
    BoxCollider _triggerCollider;

    ParticleSystem _particle;
    [SerializeField] float _destroyTime;

    private void Awake()
    {
        _triggerCollider = GetComponent<BoxCollider>();

        if (_triggerCollider != null)
            _triggerCollider.enabled = false;

        _particle = GetComponent<ParticleSystem>();

        if (_particle != null)
            _destroyTime = _particle.main.duration;

        StartCoroutine(DestroyAreaProjectile());
    }

    public void SetTriggerSize(float xSize)
    {
        _triggerCollider.size = new Vector3(xSize, _triggerCollider.size.y, _triggerCollider.size.z);
    }

    public void EnableTrigger()
    {
        _triggerCollider.enabled = true;
    }

    IEnumerator DestroyAreaProjectile()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_destroyTime);
        yield return waitTime;
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (useCallBack == false)
        {
            if (hitEffect != null)
            {
                ParticleSystem effect = Instantiate(hitEffect, other.transform.position, Quaternion.identity);
                Destroy(effect.gameObject, effect.main.duration);
            }

            if (!string.IsNullOrEmpty(hitAudioClipName))
                SoundManager.Instance.Play(Enums.ESoundType.SFX, hitAudioClipName);

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
    }

    // 파티클 시스템의 Collision 이용 시 
    private void OnParticleCollision(GameObject other)
    {
        if (useCallBack == false)
        {
            if (hitEffect != null)
            {
                ParticleSystem effect = Instantiate(hitEffect, other.transform.position, Quaternion.identity);
                Destroy(effect.gameObject, effect.main.duration);
            }

            if (!string.IsNullOrEmpty(hitAudioClipName))
                SoundManager.Instance.Play(Enums.ESoundType.SFX, hitAudioClipName);

            MonsterState monster = other.GetComponent<MonsterState>();
            if (monster != null)
            {
                monster.IsHit(_damage);
            }

            Debug.Log($"Projectile Damage : {_damage}");
        }
        else
        {
            OnHit?.Invoke(other);
        }
    }
}
