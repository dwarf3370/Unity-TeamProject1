using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] bool spawnMonsterOnEnter = false; // 트리거에 진입 시 몬스터 스폰 여부
    [SerializeField] bool spawnMonsterWhenClear = false; // 몬스터가 전부 죽었을시 다음몬스터 스폰여부
    [SerializeField] bool openDoorWhenClear = false; // 몬스터 전부 제거시 문 열기 여부

    [Header("Trigger Components")]
    [SerializeField] GameObject[] monsters; // 첫 번째 스폰 몬스터 배열
    [SerializeField] GameObject[] nextMonsters; // 다음 웨이브 몬스터 배열
    [SerializeField] GameObject door; // 열릴 문 객체
    public UnityAction<Trigger> triggerDestroyed; // 트리거 비활성화 시 실행될 이벤트

    private int remainingMonsters;

    private void Awake()
    {
        GameManager.Instance.SetTrigger(this);
        remainingMonsters = 0;
    }

    private void OnDestroy()
    {
        triggerDestroyed?.Invoke(this);
        OpenDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && spawnMonsterOnEnter == true || openDoorWhenClear == true)
        {
            if (spawnMonsterOnEnter)
            {
                SpawnMonsters(monsters);
                monsters = new GameObject[0];
            }
            else if (openDoorWhenClear == true && remainingMonsters == 0 && nextMonsters.Length == 0)
            {
                OpenDoor();
            }

            // 트리거 비활성화 호출을 이 시점에서 수행하지 않음
        }
    }

    // 지정된 몬스터 배열을 스폰하는 메서드
    public void SpawnMonsters(GameObject[] monstersToSpawn)
    {
        if (monstersToSpawn.Length > 0)
        {
            remainingMonsters = monstersToSpawn.Length;
            foreach (GameObject monster in monstersToSpawn)
            {
                monster.SetActive(true);
                var monsterState = monster.GetComponent<MonsterState>();
                if (monsterState != null)
                {
                    monsterState.OnDead += OnMonsterDeath;
                }
            }
        }
    }

    // 몬스터가 사망할 때 호출되는 메서드
    private void OnMonsterDeath(MonsterState monster)
    {
        remainingMonsters--;
        monster.OnDead -= OnMonsterDeath;
        // 모든 몬스터가 죽었을 때 처리
        if (remainingMonsters <= 0)
        {
            if (openDoorWhenClear == true && spawnMonsterWhenClear == false)
            {
                OpenDoor();
                Destroy(gameObject);
            }
            else if (spawnMonsterWhenClear == true && nextMonsters.Length > 0)
            {
                // 다음 웨이브 몬스터 스폰
                SpawnMonsters(nextMonsters);
                nextMonsters = new GameObject[0]; // 다음 몬스터가 다시 스폰되지 않도록 빈 배열로 설정
            }
            else if (openDoorWhenClear == true && nextMonsters.Length == 0)
            {
                OpenDoor();
                Destroy(gameObject);
            }
        }
    }

    public void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false);
        }
    }
}
