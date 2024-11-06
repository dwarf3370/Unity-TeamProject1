using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    public enum ItemType { Skill, Potion, Gold }
    public ItemType type;
    public Image image;
    public Sprite potionSprite;
    public int goldAmount;
    public int skillID;

    public void Initialize(ItemType itemType, int value)
    {
        this.type = itemType;
        switch (itemType)
        {
            case ItemType.Skill:
                skillID = value;
                image.sprite = DataManager.Instance.SkillDict[skillID].SkillIcon;
                break;
            case ItemType.Gold:
                goldAmount = value;
                break;
            case ItemType.Potion:
                image.sprite = potionSprite;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(type + "과 충돌");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(type + "과 충돌 해제");
        }
    }

    public void GetItem()
    {
        switch (type) 
        { 
            case ItemType.Skill :
                GameManager.Instance.battleUI.ShowSkillSlotUI(skillID);
                GameManager.Instance.player.Player_Freeze();
                break;
            case ItemType.Gold :
                GameManager.Instance.AddGold(goldAmount);
                SoundManager.Instance.Play(Enums.ESoundType.SFX, "Coins");
                break;
            case ItemType.Potion :
                GameManager.Instance.player.stats.Heal();
                SoundManager.Instance.Play(Enums.ESoundType.SFX, "Heal");
                break;
        }

        Destroy(gameObject);
    }
}