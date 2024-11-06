using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject magicShop;
    public GameObject statusUpgradeShop;

    public Button StartButton;

    private void Start()
    {
        LinkButton();
    }

    public void LinkButton()
    {
        if (StartButton != null)
        {
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
            StartButton.onClick.AddListener(GameManager.Instance.StartGame);
        }
    }

    // 마법 상점 열기
    public void EnterMagicShop()
    {
        SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
        gameObject?.SetActive(false);
        magicShop?.SetActive(true);
    }

    // 마법 상점 닫기
    public void ExitMagicShop()
    {
        SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
        gameObject?.SetActive(true);
        magicShop?.SetActive(false);
    }

    // 스테이터스 상점 열기
    public void EnterStatusShop()
    {
        SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
        gameObject?.SetActive(false);
        statusUpgradeShop?.SetActive(true);

    }

    // 스테이터스 상점 닫기
    public void ExitStatusShop()
    {
        SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
        gameObject?.SetActive(true);
        statusUpgradeShop?.SetActive(false);
    }

    private void OpenOptionWindow()
    {

    }

    private void CloseOptionWindow()
    {

    }
}
