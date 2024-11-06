using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : SkillBase
{
    Coroutine _skillRoutine;
    float WaitTime = 0.2f;
    WaitForSeconds waitTime;

    int arrowCount = 3;
    float Yinterval = 0.2f;

    private void Awake()
    {
        waitTime = new WaitForSeconds(WaitTime);
    }

    public override void SetData(int id)
    {
        base.SetData(id);
    }

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

        float yPos = StartFirePos.y + Yinterval;
        Vector3 initPos = new Vector3(StartFirePos.x, yPos, StartFirePos.z);

        for (int i = 0; i < arrowCount; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab, initPos, Quaternion.identity);
            projectile.SetDamage(_skillData.Damage * _attackPoint);
            projectile.transform.Rotate(0, 0, -(90f * StartDir));
            projectile.Fire(this, initPos, projectile.transform.up, _skillData.ProjectileSpeed);
            initPos.y += -Yinterval;

            if (i < arrowCount-1)
                yield return waitTime;
        }

        _skillRoutine = null;
    }
}
