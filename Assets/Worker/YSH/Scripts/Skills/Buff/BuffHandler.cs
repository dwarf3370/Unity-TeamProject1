using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    List<BuffBase> _appliedBuffs = new List<BuffBase>();
    List<BuffBase> _removeBuffs = new List<BuffBase>();

    public void ApplyBuff(BuffBase buff)
    {
        _appliedBuffs.Add(buff);
        Debug.Log($"ApplyBuff : {buff.GetType().Name}");
    }

    public void RemoveBuff()
    {
        foreach(BuffBase buff in _removeBuffs)
        {
            _appliedBuffs.Remove(buff);
            Debug.Log($"RemoveBuff : {buff.GetType().Name}");
        }
    }

    void Update()
    {
        if (_appliedBuffs.Count <= 0)
            return;

        foreach (BuffBase buff in _appliedBuffs)
        {
            if (buff.BuffTimer <= 0)
            {
                _removeBuffs.Add(buff);
                continue;
            }

            buff.UpdateBuff();
        }

        if (_removeBuffs.Count > 0)
        {
            RemoveBuff();
            _removeBuffs.Clear();
        }
    }
}
