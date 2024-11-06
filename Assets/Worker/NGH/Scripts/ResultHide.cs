using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultHide : MonoBehaviour
{
    private void OnEnable()
    {
        // 2초 후 resultWindow를 비활성화하는 코루틴 시작
        StartCoroutine(HideResultWindowAfterDelay());
    }

    private IEnumerator HideResultWindowAfterDelay()
    {
        yield return new WaitForSeconds(2);  // 2초 대기
        gameObject.SetActive(false);       // 2초 후 resultWindow 비활성화
    }
}
