using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenSword : SkillBase
{
    const int swordCount = 3;

    [SerializeField] float yInterval = 0.575f;
    [SerializeField] float xInterval = 2f;
    [SerializeField] Projectile[] SwordPrefab = new Projectile[swordCount];

    Coroutine _skillRoutine;
    [SerializeField] float[] DelayTimes = new float[swordCount];

    Vector3[] _positions = new Vector3[swordCount];

    public override void DoSkill()
    {
        if (_skillRoutine != null)
            return;

        Debug.Log("IceArrow DoSkill");
        _skillRoutine = StartCoroutine(SkillRoutine());
    }

    IEnumerator SkillRoutine()
    {
        base.DoSkill();

        _positions[0] = new Vector3(AreaProjectilePos.x, AreaProjectilePos.y - yInterval, AreaProjectilePos.z);
        _positions[1] = new Vector3(AreaProjectilePos.x - xInterval, AreaProjectilePos.y - yInterval, AreaProjectilePos.z);
        _positions[2] = new Vector3(AreaProjectilePos.x + xInterval, AreaProjectilePos.y - yInterval, AreaProjectilePos.z);

        for (int i = 0; i < SwordPrefab.Length; i++)
        {
            AreaProjectile projectile = Instantiate(SwordPrefab[i], _positions[i], Quaternion.identity) as AreaProjectile;
            ParticleSystem.MainModule main = projectile.gameObject.GetComponent<ParticleSystem>().main;
            main.startDelay = DelayTimes[i];
            projectile.SetDamage(_skillData.Damage * _attackPoint);
            projectile.SetTriggerSize(_skillData.Radius);
            projectile.EnableTrigger();

            yield return null;
        }

        _skillRoutine = null;
    }
}
