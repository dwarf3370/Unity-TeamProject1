using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    // 배틀쪽 UI 요소
    [Header("Skill Icon")]
    public Image[] skillimage = new Image[(int)Enums.PlayerSkillSlot.Length];
    public Image[] cooldownImages = new Image[(int)Enums.PlayerSkillSlot.Length]; // 쿨타임을 위한 이미지 배열
    public TextMeshProUGUI[] cooldownTexts = new TextMeshProUGUI[(int)Enums.PlayerSkillSlot.Length]; // 쿨타임 텍스트 배열
    public GameObject GameOver;
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI goldText;
    public SkillSlotUI skillSlotUI;

    private void Awake()
    {
        skillSlotUI.gameObject.SetActive(false);
        GameManager.Instance.SetBattleUI(this);
    }

    private void Start()
    {
        // hpBar초기화
        hpBar.minValue = 0;
        hpBar.maxValue = GameManager.Instance.player.stats.maxHealth;

        // 이벤트 등록
        GameManager.Instance.player.handler.OnChangedSkillSlot += UpdateSkill;
        GameManager.Instance.player.stats.OnChangedHP += UpdateHp;
        GameManager.Instance.OnGoldChanged += UpdateGold;
        GameManager.Instance.player.handler.OnSkillUsed += StartCooldown;

        UpdateGold();
        UpdateHp();
        UpdateSkill();
    }

    private void Update()
    {
        // gold UI TEST Code
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance.AddGold(1000);
        }
    }

    private void OnDestroy()
    {
        Debug.Log($"BattleUI 파괴됨 {gameObject.name}");
        // 이벤트 해제
        if(GameManager.Instance.player != null)
        {
            GameManager.Instance.player.handler.OnChangedSkillSlot -= UpdateSkill;
            GameManager.Instance.player.stats.OnChangedHP -= UpdateHp;
            GameManager.Instance.player.handler.OnSkillUsed -= StartCooldown; // 쿨타임 이벤트 해제
        }
        GameManager.Instance.OnGoldChanged -= UpdateGold;
    }

    public void ShowSkillSlotUI(int skillID)
    {
        skillSlotUI.SetInfo(skillID);
        skillSlotUI.gameObject.SetActive(true);
    }

    // 전투중 QWER스킬중 하나가 바뀌었을때 작동되는 코드
    public void UpdateSkill()
    {
        for (int i = 0; i < (int)Enums.PlayerSkillSlot.Length; i++)
        {
            // 빈 곳이 있으면 null
            if (GameManager.Instance.player.handler.PlayerSkillSlot[i] == null)
            {
                skillimage[i].gameObject.SetActive(false);
                cooldownImages[i].fillAmount = 0; // 초기화
                cooldownImages[i].gameObject.SetActive(false); // 쿨타임 이미지 비활성화
                cooldownTexts[i].gameObject.SetActive(false); // 쿨타임 텍스트 비활성화
            }
            // 스킬이 슬롯에 있으면 그 스킬 데이터를 읽어서 스킬에 맞는 아이콘으로 교체
            else
            {
                skillimage[i].sprite = GameManager.Instance.player.handler.PlayerSkillSlot[i].SkillData.SkillIcon;
                skillimage[i].gameObject.SetActive(true);
                cooldownImages[i].gameObject.SetActive(true); // 쿨타임 이미지 활성화
            }
        }
    }

    public void UpdateHp()
    {
        // 현재 체력과 최대 체력을 가져와서 UI를 업데이트
        float currentHealth = GameManager.Instance.player.stats.currentHealth;
        float maxHealth = GameManager.Instance.player.stats.maxHealth;

        hpBar.value = currentHealth;
        hpText.text = $"{currentHealth} / {maxHealth}";
    }

    public void UpdateGold()
    {
        int gold = GameManager.Instance.GetGold();

        goldText.text = gold.ToString();
    }

    private void StartCooldown(int skillIndex, float cooldownTime)
    {
        StartCoroutine(CooldownCoroutine(skillIndex, cooldownTime));
    }

    private IEnumerator CooldownCoroutine(int skillIndex, float cooldownTime)
    {
        float elapsed = 0f;
        cooldownTexts[skillIndex].gameObject.SetActive(true); // 쿨타임 텍스트 활성화

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            cooldownImages[skillIndex].fillAmount = elapsed / cooldownTime; // 쿨타임 비율 업데이트

            // 남은 시간 계산하여 소숫점 1자리로 표시
            float remainingTime = cooldownTime - elapsed;
            cooldownTexts[skillIndex].text = remainingTime.ToString("F1");

            yield return null;
        }

        cooldownImages[skillIndex].fillAmount = 0; // 쿨타임 완료
        cooldownTexts[skillIndex].gameObject.SetActive(false); // 쿨타임 종료 시 텍스트 숨김 처리
    }
}
