using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] GameObject[] cursors;
    int skillID;
    int slotIndex;
    int SlotIndex {  get { return slotIndex; } set { cursors[slotIndex].SetActive(false); slotIndex = value; cursors[slotIndex].SetActive(true); } }

    private void OnEnable()
    {
        SlotIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "PlayButton");
            if (slotIndex < cursors.Length-1)
            {
                SlotIndex++;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "PlayButton");
            if (slotIndex > 0)
            { 
                SlotIndex--;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "PlayButton");
            GameManager.Instance.player.Player_Release();

            GameManager.Instance.player.handler.EquipSkill(skillID, (Enums.PlayerSkillSlot)slotIndex);
            gameObject.SetActive(false);
        }
    }

    public void SetInfo(int skillID)
    {
        this.skillID = skillID;
    }
}
