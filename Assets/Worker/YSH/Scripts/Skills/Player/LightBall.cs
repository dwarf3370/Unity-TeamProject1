using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBall : SkillBase
{
    [SerializeField] ParticleSystem ligthBallBuffEffect;

    [Header("Audio")]
    [SerializeField] string lightBuffEffectAudioClip;

    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoSkill()
    {
        Projectile projectile = Instantiate(projectilePrefab, StartFirePos, Quaternion.identity);
        projectile.OnHit += LightBallCallBack;
        projectile.SetDamage(_skillData.Damage * _attackPoint);
        projectile.transform.rotation = Quaternion.LookRotation(new Vector3(StartDir, 0, 0));
        projectile.Fire(this, StartFirePos, projectile.transform.forward, _skillData.ProjectileSpeed);
        base.DoSkill();
    }

    public void LightBallCallBack(GameObject go)
    {
        MonsterState monster = go.GetComponent<MonsterState>();
        if (monster != null)
        {
            ParticleSystem particle = Instantiate(ligthBallBuffEffect, go.transform);
            Destroy(particle.gameObject, _skillData.Second);
            BuffBase lightBallBuff = new LightBallAttackBuff();
            lightBallBuff.SetInfo(go, _skillData.Second, _skillData.Tick, _attackPoint, _skillData.Damage, lightBuffEffectAudioClip);
            monster.GetComponent<BuffHandler>()?.ApplyBuff(lightBallBuff);
            Debug.Log($"LightBallCallBack Apply : {go.name}");
        }
    }
}
