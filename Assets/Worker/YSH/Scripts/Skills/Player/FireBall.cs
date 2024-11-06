using UnityEngine;

public class FireBall : SkillBase
{
    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoSkill()
    {
        Projectile projectile = Instantiate(projectilePrefab, StartFirePos, Quaternion.identity);
        projectile.SetDamage(_skillData.Damage * _attackPoint);
        projectile.transform.rotation = Quaternion.LookRotation(new Vector3(StartDir, 0, 0));
        projectile.Fire(this, StartFirePos, projectile.transform.forward, _skillData.ProjectileSpeed);
        base.DoSkill();
    }
}
