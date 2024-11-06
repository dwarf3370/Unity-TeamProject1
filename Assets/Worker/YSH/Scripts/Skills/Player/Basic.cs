using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic : SkillBase
{
    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoSkill()
    {
        Projectile projectile = Instantiate(projectilePrefab, FireTransform.position, Quaternion.identity);
        projectile.SetDamage(_skillData.Damage * _attackPoint);
        projectile.transform.rotation = Quaternion.LookRotation(_fireTransform.forward);
        projectile.Fire(this, _fireTransform.position, projectile.transform.forward, _skillData.ProjectileSpeed);
        base.DoSkill();
    }
}

