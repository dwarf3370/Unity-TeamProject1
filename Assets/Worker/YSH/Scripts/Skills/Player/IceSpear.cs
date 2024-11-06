using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpear : SkillBase
{
    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoSkill()
    {
        Projectile projectile = Instantiate(projectilePrefab, StartFirePos, Quaternion.identity);
        projectile.SetDamage(_skillData.Damage * _attackPoint);
        projectile.transform.Rotate(0, 0, -(90f * StartDir));
        projectile.Fire(this, StartFirePos, projectile.transform.up, _skillData.ProjectileSpeed);
        base.DoSkill();
    }
}
