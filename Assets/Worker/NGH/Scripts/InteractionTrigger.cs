using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private Image interactionIcon;  // 상호작용 아이콘 이미지

    private void Start()
    {
        if (interactionIcon != null)
        {
            interactionIcon.gameObject.SetActive(false);  // 시작 시 아이콘을 비활성화
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactionIcon != null)
        {
            interactionIcon.gameObject.SetActive(true);  // 트리거 진입 시 아이콘 활성화
            UpdateIconPosition();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactionIcon != null)
        {
            interactionIcon.gameObject.SetActive(false);  // 트리거 벗어날 시 아이콘 비활성화
        }
    }

    private void Update()
    {
        if (interactionIcon.gameObject.activeSelf)
        {
            UpdateIconPosition();  // 매 프레임마다 아이콘 위치 업데이트
        }
    }

    private void UpdateIconPosition()
    {
        if (interactionIcon != null)
        {
            interactionIcon.transform.position = transform.position + new Vector3(0f, 1f, -2f);  // 아이콘 위치를 iconPosition 위치로 설정
        }
    }
}
