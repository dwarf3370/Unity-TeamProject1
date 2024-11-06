using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardChest : MonoBehaviour
{
    public int dataIndex; // 사용할 테이블 인덱스
    public GameObject hpItemPrefab, skillPrefab, goldPrefab, rareGoldPrefab;

    public bool isOpened;
    [SerializeField] Animator animator;
    [SerializeField] float chestOpenDelay = 1f;

    [SerializeField] GameObject gate;

    private void Awake()
    {
        isOpened = false;
        GameManager.Instance.SetRewardChest(gameObject);
        gameObject.SetActive(false);
    }

    public void OpenChest()
    {
        if (isOpened) return; // 이미 열린 상자는 다시 열리지 않도록
        isOpened = true;

        Debug.Log("상자 열림");

        // 상자 오픈 애니메이션 재생
        animator.SetTrigger("Open");

        SoundManager.Instance.Play(Enums.ESoundType.SFX, "OpenChest");

        // 일정 시간 후에 보상을 생성하는 코루틴 시작
        StartCoroutine(GenerateRewardsAfterDelay());
    }

    private IEnumerator GenerateRewardsAfterDelay()
    {
        Debug.Log("보상 생성됨");

        // chestOpenDelay만큼 기다린 후 보상을 생성
        yield return new WaitForSeconds(chestOpenDelay);

        // 스테이지에 등록된 보상테이블을 로드해서 보상을 생성
        DropData selectedTable = DataManager.Instance.DropDict[dataIndex];
        GenerateRewards(selectedTable);

        // 보상 생성 후 상자 비활성화
        gameObject.SetActive(false);

        gate.SetActive(true);
    }

    private void GenerateRewards(DropData table)
    {
        // 골드 생성
        int goldAmount = Random.Range(table.MinGold, table.MaxGold);
        Vector3 goldPosition = new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, 0);
        GameObject goldDrop = Instantiate(goldPrefab, goldPosition, Quaternion.identity);
        goldDrop.GetComponent<DropItem>().Initialize(DropItem.ItemType.Gold, goldAmount);

        // HP템 드랍 여부 확인
        if (table.IsDropHP)
        {
            Vector3 potionPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
            GameObject potionDrop = Instantiate(hpItemPrefab, potionPosition, Quaternion.identity);
            potionDrop.GetComponent<DropItem>().Initialize(DropItem.ItemType.Potion,0);
        }

        List<int> availableLowSkills = new List<int>();
        List<int> availableMidSkills = new List<int>();
        List<int> availableHighSkills = new List<int>();

        // 해금된 스킬들을 대상으로 반복
        for (int i= 0; i < SkillUnlockManager.Instance.unlockedSkills.Count; i++)
        {
            Enums.Grade grade = DataManager.Instance.SkillDict[SkillUnlockManager.Instance.unlockedSkills[i]].Grade;
            switch(grade)
            {
                case Enums.Grade.Low :
                    availableLowSkills.Add(SkillUnlockManager.Instance.unlockedSkills[i]);
                        break;
                case Enums.Grade.Mid:
                    availableMidSkills.Add(SkillUnlockManager.Instance.unlockedSkills[i]);
                    break;
                case Enums.Grade.High:
                    availableHighSkills.Add(SkillUnlockManager.Instance.unlockedSkills[i]);
                    break;
            }
        }

        // 스킬 ID 리스트 (0~8까지)에서 QWER에 등록되지 않은 스킬만 선택하기 위해 목록을 구성합니다.
        List<int> equippedSkills = new List<int>();
        for (int i = 0; i < (int)Enums.PlayerSkillSlot.Length; i++)
        {
            if (GameManager.Instance.player.handler.PlayerSkillSlot[i] != null)
            {
                equippedSkills.Add(GameManager.Instance.player.handler.PlayerSkillSlot[i].SkillData.ID);
            }
        }
        availableLowSkills.RemoveAll(skill => equippedSkills.Contains(skill));
        availableMidSkills.RemoveAll(skill => equippedSkills.Contains(skill));
        availableHighSkills.RemoveAll(skill => equippedSkills.Contains(skill));

        // 스킬 드랍 확률 체크
        float skillRoll = Random.value;
        if (skillRoll <= table.LowGradePercent && availableLowSkills.Count > 0)
        {
            // lowSkill 중에서 랜덤 선택
            int skillID = availableLowSkills[Random.Range(0, availableLowSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }
        else if (skillRoll <= table.LowGradePercent + table.MidGradePercent && availableMidSkills.Count > 0)
        {
            // midSkill 중에서 랜덤 선택
            int skillID = availableMidSkills[Random.Range(0, availableMidSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }
        else if (skillRoll <= table.LowGradePercent + table.MidGradePercent + table.HighGradePercent && availableHighSkills.Count > 0)
        {
            // highSkill 중에서 랜덤 선택
            int skillID = availableHighSkills[Random.Range(0, availableHighSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }

        // 뽕맛 골드 드랍 체크
        if (Random.value <= table.BonusGoldPercent)
        {
            int bounsGoldAmount = Random.Range(table.MinGold, table.MaxGold) * 10;
            Vector3 bonusGoldPosition = new Vector3(transform.position.x + 2f, transform.position.y + 0.5f, 0);
            GameObject bonusGoldDrop = Instantiate(rareGoldPrefab, bonusGoldPosition, Quaternion.identity);
            bonusGoldDrop.GetComponent<DropItem>().Initialize(DropItem.ItemType.Gold, bounsGoldAmount);
        }
    }

    private void CreateSkill(DropItem.ItemType skillType, int skillID)
    {
        Vector3 skillPosition = new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, 0);
        GameObject skillDrop = Instantiate(skillPrefab, skillPosition, Quaternion.identity);
        skillDrop.GetComponent<DropItem>().Initialize(skillType, skillID);
    }
}