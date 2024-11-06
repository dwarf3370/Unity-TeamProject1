using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayer : MonoBehaviour
{
    SkillHandler handler;
    [SerializeField] Transform firePos;

    [SerializeField] KeyCode basicSkillKey;
    [SerializeField] KeyCode[] skillKeys = new KeyCode[(int)Enums.PlayerSkillSlot.Length];

    [SerializeField] float _attackPoint;

    private void Awake()
    {
        handler = GetComponent<SkillHandler>();
        testInit();
    }

    public void testInit()
    {
        //handler.SetBasicSkill(9);
        handler.EquipSkill(0, Enums.PlayerSkillSlot.Slot1);
        handler.EquipSkill(1, Enums.PlayerSkillSlot.Slot2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(basicSkillKey))
        {
            handler.DoBasicSkill(firePos.transform, _attackPoint);
        }
        else
        {
            for (int i = 0; i < skillKeys.Length; i++)
            {
                if (Input.GetKeyDown(skillKeys[i]))
                {
                    handler.DoSkill((Enums.PlayerSkillSlot)i, firePos.transform, _attackPoint);
                }
            }
        }
    }
}
