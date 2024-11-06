using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChain : SkillBase
{
    [SerializeField] ParticleSystem ligthChainEffect;

    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoSkill()
    {
        Projectile projectile = Instantiate(projectilePrefab, StartFirePos, Quaternion.identity);
        projectile.OnHit += LightChainCallBack;
        projectile.SetDamage(_skillData.Damage * _attackPoint);
        projectile.transform.rotation = Quaternion.LookRotation(new Vector3(StartDir, 0, 0));
        projectile.Fire(this, StartFirePos, projectile.transform.forward, _skillData.ProjectileSpeed);
        base.DoSkill();
    }

    public void LightChainCallBack(GameObject go)
    {
        MonsterState monster = go.GetComponent<MonsterState>();
        if (monster != null)
        {
            monster.IsHit(_skillData.Damage * _attackPoint);
            Debug.Log($"{gameObject.name} Damage : {_skillData.Damage * _attackPoint}");
            monster.Stunned(_skillData.Second);
            ParticleSystem particle = Instantiate(ligthChainEffect, go.transform);
            Destroy(particle.gameObject, particle.main.duration);
        }
    }
}
