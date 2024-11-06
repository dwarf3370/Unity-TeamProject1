using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicShopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;

    private void OnEnable()
    {
        UpdateUI();
        GameManager.Instance.OnGoldChanged += UpdateUI;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGoldChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        goldText.text = $"Gold: {GameManager.Instance.GetGold()}";
    }
}
