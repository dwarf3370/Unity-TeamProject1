using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : SkillBase
{
    [SerializeField] int maxHitCount;
    [SerializeField] ParticleSystem LightningEffect;

    [SerializeField] string LightningAudioClip;

    int _hitCount = 0;

    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoSkill()
    {
        _hitCount = 0;

        AreaProjectile projectile = Instantiate(projectilePrefab, _areaProjectilePos, Quaternion.identity) as AreaProjectile;
        projectile.OnHit += LightningCallBack;
        projectile.SetDamage(_skillData.Damage * _attackPoint);
        projectile.SetTriggerSize(_skillData.Radius);
        projectile.EnableTrigger();
        base.DoSkill();
    }

    public void LightningCallBack(GameObject go)
    {
        // 최대 공격 가능한 몬스터 수 체크
        if (_hitCount >= maxHitCount)
            return;

        MonsterState monster = go.GetComponent<MonsterState>();
        if (monster != null)
        {
            if (!string.IsNullOrEmpty(LightningAudioClip))
                SoundManager.Instance.Play(Enums.ESoundType.SFX, LightningAudioClip);

            monster.IsHit(_skillData.Damage * _attackPoint);
            _hitCount++;
            Debug.Log($"{gameObject.name} Damage : {_skillData.Damage * _attackPoint}");
            ParticleSystem particle = Instantiate(LightningEffect, go.transform);
            Destroy(particle.gameObject, particle.main.duration);
        }
    }
}
