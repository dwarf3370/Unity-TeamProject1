using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Vector3 direction;

    public Rigidbody rigid;

    public Transform groundCheck;

    public LayerMask groundLayer;

    [SerializeField] float speed = 5; // 이동 속도
    [SerializeField] float jumpForce = 12; // 점프 파워
    [SerializeField] float maxFallSpeed = -10f; // 최대 하강 속도 제한
    [SerializeField] float fallMultiplier = 2.5f; // 기본 하강 가속도 배율
    [SerializeField] float fallAcceleration = 1.2f; // 하강 가속도 증가율
    [SerializeField] float maxFallMultiplier = 10f; // 최대 하강 가속도 배율
    [SerializeField] float customGravity = -20f; // 기본 중력
    [SerializeField] float lowJumpMultiplier = 2f; // 낮은 점프 가속도 배율

    public Animator animator;

    public Transform model;

    public SkillHandler handler;

    bool AbleDoubleJump = true;

    bool moveStop = false;

    [SerializeField] Transform firePos;

    [SerializeField] KeyCode basicSkillKey;
    [SerializeField] KeyCode[] skillKeys = new KeyCode[(int)Enums.PlayerSkillSlot.Length];

    public PlayerStats stats = new PlayerStats();

    public SkillSlotUI skillui = new SkillSlotUI();

    [SerializeField] float ignoreHalfBlockDelay;
    GameObject currentPlatform = null;
    CapsuleCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        handler = GetComponent<SkillHandler>();
        GameManager.Instance.SetPlayer(this);

        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
    }

    private void Start()
    {
        // 이벤트 추가
        GameManager.Instance.player.stats.OnChangedHP += TakeDamageAnimation;
        GameManager.Instance.player.stats.Dead += PlayerDead;
    }   

    void Update()
    {
        if (moveStop)
            return;

        // 점프 애니메이션
        float height = rigid.velocity.y;

        animator.SetFloat("height", Mathf.Abs(height));

        // 기본 공격
        if (Input.GetKeyDown(basicSkillKey))
        {
            handler.DoBasicSkill(firePos, stats.attackPower);
        }
        else
        {
            for (int i = 0; i < skillKeys.Length; i++)
            {
                if (Input.GetKeyDown(skillKeys[i]))
                {
                    handler.DoSkill((Enums.PlayerSkillSlot)i, firePos, stats.attackPower);
                }
            }
        }

        // 좌우 입력
        float hInput = Input.GetAxisRaw("Horizontal");
        direction.x = hInput * speed;

        animator.SetFloat("speed", Mathf.Abs(hInput));

        // 점프 및 이중 점프
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            AbleDoubleJump = true;

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else if (AbleDoubleJump && Input.GetButtonDown("Jump"))
        {
            DoubleJump();
        }

        // 최대 하강 속도 제한
        if (rigid.velocity.y < maxFallSpeed)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, maxFallSpeed, rigid.velocity.z);
        }

        // 방향에 따른 캐릭터 회전
        if (hInput != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(hInput, 0, 0));
            model.rotation = newRotation;
        }

        // 무적 상태
        stats.UpdateInvincibleTime(Time.deltaTime);

        // 상호 작용
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (rewardChest != null)
            {
                rewardChest.OpenChest();
                rewardChest = null;
            }
            else if(curDropItem != null)
            {
                curDropItem.GetItem();
                curDropItem = null;
            }
            else if (gate != null)
            {
                gate.MoveNextScene();
                gate = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentPlatform != null)
            {
                StartCoroutine(CoDownJump());
            }
        }
    }

    // 밑으로 점프
    IEnumerator CoDownJump()
    {
        Collider col;
        col = currentPlatform.GetComponent<Collider>();

        Physics.IgnoreCollision(_collider, col, true);
        yield return new WaitForSeconds(ignoreHalfBlockDelay);
        Physics.IgnoreCollision(_collider, col, false);
    }

    private void FixedUpdate()
    {
        //캐릭터 움직임
        rigid.velocity = new Vector3(direction.x, rigid.velocity.y, 0);

        if (rigid.velocity.y < 0) // 낙하 중일 때
        {
            rigid.AddForce(Vector3.up * customGravity * fallMultiplier, ForceMode.Acceleration);
        }
        else // 상승 중일 때 기본 중력 적용
        {
            rigid.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
        }
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        GameManager.Instance.player.stats.OnChangedHP -= TakeDamageAnimation;
        GameManager.Instance.player.stats.Dead -= PlayerDead;
    }

    private void Jump()
    {
        Debug.Log("점프");
        // 점프
        rigid.velocity = new Vector3(rigid.velocity.x, jumpForce, rigid.velocity.z);
    }

    private void DoubleJump()
    {
        // 더블 점프
        rigid.velocity = new Vector3(rigid.velocity.x, jumpForce, rigid.velocity.z);

        AbleDoubleJump = false;
    }

    private void TakeDamageAnimation()
    {
        // 피격 모션
        animator.SetTrigger("damage");
    }

    private void PlayerDead()
    {
        // 사망
        Player_Freeze();

        animator.SetTrigger("die");

        Destroy(gameObject, 3f);
    }

    public void PlayCast()
    {
        Player_Freeze();
        animator.SetBool("cast", true);
    }

    public void StopCast()
    {
        Player_Release();
        animator.SetBool("cast", false);
    }

    public void Player_Freeze()
    {
        Debug.Log("Player_Freeze");
        // 멈춤
        moveStop = true;
    }

    public void Player_Release()
    {
        Debug.Log("Player_Release");
        // 멈춤 해제
        moveStop = false;
    }

    DropItem curDropItem;
    RewardChest rewardChest;
    Gate gate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RewardChest"))
        {
            RewardChest reward = other.GetComponent<RewardChest>();
            if (reward.isOpened == false)
            {
                rewardChest = other.GetComponent<RewardChest>();
            }
        }
        else if (other.gameObject.CompareTag("DropItem"))
        {
            curDropItem = other.GetComponent<DropItem>();
            Debug.Log($"{other.gameObject.name} 등록");
        }
        else if (other.gameObject.CompareTag("Gate"))
        {
            gate = other.GetComponent<Gate>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (rewardChest != null && rewardChest.gameObject == other.gameObject) 
        {
            rewardChest = null;
        }
        else if (curDropItem != null && curDropItem.gameObject == other.gameObject)
        {
            curDropItem = null;
            Debug.Log($"{other.gameObject.name} 해제");
        }
        else if (gate != null && gate.gameObject == other.gameObject)
        {
            gate = null;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HalfBlock"))
        {
            currentPlatform = other.gameObject;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HalfBlock"))
        {
            currentPlatform = null;
        }
    }
}
