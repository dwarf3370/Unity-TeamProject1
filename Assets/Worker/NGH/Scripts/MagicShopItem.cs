using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MagicShopItem : MonoBehaviour
{
    [SerializeField] int skillID;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] ConfirmWindow confirmWindow;
    [SerializeField] Image image;

    private void Awake()
    {
        price.text = $"{DataManager.Instance.SkillDict[skillID].Price}";
        image.sprite = DataManager.Instance.SkillDict[skillID].SkillIcon;
    }
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => 
        {
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
            confirmWindow.gameObject.SetActive(true);
            confirmWindow.SetInfo(skillID);
        });
    }
   
}
