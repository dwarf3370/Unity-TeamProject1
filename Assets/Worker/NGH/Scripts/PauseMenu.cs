using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // 설정창 패널
    private bool isPaused = false; // 게임 멈춤 상태 확인용
    public Button returnLobbyButton;

    void Update()
    {
        // ESC 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // 게임 재개
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // 설정창 숨기기
        Time.timeScale = 1f; // 게임 시간 재개
        isPaused = false;
    }

    // 게임 일시정지
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // 설정창 보이기
        Time.timeScale = 0f; // 게임 시간 멈춤
        if(returnLobbyButton != null)
        {
            returnLobbyButton.onClick.AddListener(GameManager.Instance.ReturnToLobby);
        }
        isPaused = true;
    }

    // 게임 종료 함수 (필요시 Quit 버튼에 연결)
    public void QuitGame()
    {
        Application.Quit(); // 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
