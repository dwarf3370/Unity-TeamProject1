using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance;

    // 플레이어의 골드
    [SerializeField] private int gold;

    // 골드 변화가 있을시 작동하는 이벤트
    public UnityAction OnGoldChanged;

    int?[] playerSkillSlotID = new int?[(int)Enums.PlayerSkillSlot.Length];
    public int?[] PlayerSkillSlotID { get { return playerSkillSlotID; } }

    public Player_Controller player;
    public BattleUI battleUI;
    GameObject rewardChest;
    public int monsterCount = 0;
    public int triggerCount = 0;

    public float battlePlayerMaxHP;
    public float battlePlayerAtk;
    public float battlePlayerDef;
    public float battlePlayerCurrentHP;
    public float skillCooltimeReduce;
    public bool playerCurHpSetOnce = true;

    private void Awake()
    {
        //싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerSaveManager.LoadGold(out gold);
    }

    private void Update()
    {
        // 테스트용 배틀 씬 전환
        if (Input.GetKeyDown(KeyCode.P))
        {
            TestGame();
        }
    }

    private void OnEnable()
    {
        // 씬이 로드되면 호출될 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 비활성화 되면 이벤트 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        Debug.Log("GameManager 파괴됨");
        PlayerSaveManager.SaveGold(gold);
    }

    // 게임 시작 시 타이틀 화면에서 호출
    public void StartGame()
    {
        // 게임 로딩 시작
        StartCoroutine(LoadMainGame());
    }

    // 메인 게임 비동기 로딩
    private IEnumerator LoadMainGame()
    {
        // 메인 게임 씬을 비동기로 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync("Stage1-1");

        // 로딩 진행 상황 업데이트
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // 테스트 씬 전환용 코드
    public void TestGame()
    {
        // 게임 로딩 시작
        StartCoroutine(LoadTestGame());
    }

    // 테스트용 씬으로 전환
    private IEnumerator LoadTestGame()
    {
        // 메인 게임 씬을 비동기로 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync("BattleTest");

        // 로딩 진행 상황 업데이트
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // 다음 씬으로 넘어가기 위한 코드
    public void LoadScene(string sceneName)
    {
        // 게임 로딩 시작
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    // 다음 스테이지 비동기 로딩
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // 다음 스테이지를 비동기로 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 로딩 진행 상황 업데이트
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // 로비 화면으로 귀환
    public void ReturnToLobby()
    {
        // 로비 로딩 시작
        StartCoroutine(LoadLobby());
    }

    // 로비 씬 비동기 로딩
    private IEnumerator LoadLobby()
    {
        // 로비 씬을 비동기 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync("LobbyTest");

        // 로딩 진행 상황 업데이트
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // 씬이 로드됐을때 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded : " + scene.name);

        if(scene.name == "LobbyTest")
        {
            InitializeLobbyScene();
            return;
        }

        // 씬 로드후 초기화
        InitializeBattleScene();
        
    }

    public void SetPlayer(Player_Controller player)
    {
        this.player = player;
        SetPlayerStatus();
        if (playerCurHpSetOnce)
        {
            player.stats.currentHealth = player.stats.maxHealth;
            SetPlayerCurrentHP();
            playerCurHpSetOnce = false;
        }
        else
        {
            player.stats.currentHealth = battlePlayerCurrentHP;
        }
        player.stats.Dead += GameOver;
        player.stats.OnChangedHP += SetPlayerCurrentHP;

        // 추후 수정 필요 (데이터에서 기본스킬 찾도록)
        player.handler.SetBasicSkill(9);

        for (int i = 0; i < playerSkillSlotID.Length; i++)
        {
            if (playerSkillSlotID[i] != null)
            {
                player.handler.EquipSkill((int)playerSkillSlotID[i], (Enums.PlayerSkillSlot)i);
            }
        }
    }

    public void SetRewardChest(GameObject rewardChest)
    {
        this.rewardChest = rewardChest;
    }

    public void SetBattleUI(BattleUI battleUI)
    {
        this.battleUI = battleUI;
    }

    public void SetMonster(MonsterState monster)
    {
        monsterCount++;
        monster.OnDead += DecreaseMonster;
    }

    public void DecreaseMonster(MonsterState monster)
    {
        monsterCount--;
        Debug.Log($"{monster.gameObject.name} 사망");
        monster.OnDead -= DecreaseMonster;
        if (triggerCount == 0 && monsterCount == 0 && rewardChest != null)
        {
            rewardChest.SetActive(true);
            rewardChest = null;
        }
    }

    public void SetTrigger(Trigger trigger)
    {
        triggerCount++;
        trigger.triggerDestroyed += DecreaseTrigger;
    }

    public void DecreaseTrigger(Trigger trigger)
    {
        triggerCount--;
        trigger.triggerDestroyed -= DecreaseTrigger;
        if (triggerCount == 0 && monsterCount == 0 && rewardChest != null)
        {
            rewardChest.SetActive(true);
            rewardChest = null;
        }
    }

    // 씬 전환후 연결 초기화
    private void InitializeBattleScene()
    {
        
    }

    private void InitializeLobbyScene()
    {
        if (player != null)
        {
            ResetPlayerStatus();
            player.stats.Dead -= GameOver;
            player.stats.OnChangedHP -= SetPlayerCurrentHP;
        }
        playerCurHpSetOnce = true;
        Time.timeScale = 1f;
        triggerCount = 0;
        monsterCount = 0;
        playerSkillSlotID = new int?[(int)Enums.PlayerSkillSlot.Length];
    }

    // 골드를 추가하는 메서드
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("골드 추가: " + amount + ", 현재 골드: " + gold);

        OnGoldChanged?.Invoke();
    }

    // 골드를 차감하는 메서드
    public void SpendGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            gold -= amount;
            Debug.Log("골드 차감: " + amount + ", 남은 골드: " + gold);

            OnGoldChanged?.Invoke();
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    // 골드가 충분한지 확인하는 메서드
    public bool HasEnoughGold(int amount)
    {
        return gold >= amount;
    }

    // 현재 골드 수량을 가져오는 메서드 (필요 시 추가)
    public int GetGold()
    {
        return gold;
    }

    private void SetPlayerStatus()
    {
        player.stats.maxHealth = battlePlayerMaxHP;
        player.stats.attackPower = battlePlayerAtk;
        player.stats.defense = battlePlayerDef;
    }

    private void ResetPlayerStatus()
    {
        player.stats.maxHealth = 100;
        player.stats.attackPower = 10f;
        player.stats.defense = 5f;
        skillCooltimeReduce = 0f;
    }

    private void SetPlayerCurrentHP()
    {
        battlePlayerCurrentHP = player.stats.currentHealth;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(3);

        battleUI.GameOver.SetActive(true);

        yield return new WaitForSeconds(3);

        ReturnToLobby();
    }
}
