using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBallAttackBuff : BuffBase
{
    public override void DoTick()
    {
        MonsterState monster = _owner.GetComponent<MonsterState>();

        if (monster == null)
            return;

        monster.IsHit(_damagePerTick);
        Debug.Log($"LightBallAttackBuff ¹ßµ¿, Damage : {_damagePerTick}");
        base.DoTick();
    }
}
