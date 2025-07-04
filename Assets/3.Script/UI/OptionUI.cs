using Mirror;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
    [Header("Option UI Panel")]
    public GameObject optionsPanel;

    void Start()
    {
        // 처음엔 숨겨두기
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    void Update()
    {
        OptionKey();
    }

    void OptionKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsPanel == null)
            {
                Debug.LogWarning("Option UI가 할당되지 않았습니다!");
                return;
            }

            // 패널이 켜져 있으면 끄고 게임 재개, 꺼져 있으면 켜고 일시정지
            bool isOpen = optionsPanel.activeSelf;
            optionsPanel.SetActive(!isOpen);
            //Time.timeScale = isOpen ? 1f : 0f;

            Debug.Log(isOpen
                ? "[Option] 닫힘, Time.timeScale = 1"
                : "[Option] 열림, Time.timeScale = 0");
        }
    }

    public void OnClickNoButton()
    {
        if (optionsPanel == null)
        {
            Debug.LogWarning("Option UI가 할당되지 않았습니다!");
            return;
        }

        optionsPanel.SetActive(false);
        //Time.timeScale = 1f;
        Debug.Log(Time.time);
    }
    public void OnClickYesButton()
    {
        // NetworkClient.localPlayer는 현재 게임 씬의 플레이어를 가리킴
        var localPlayer = NetworkClient.localPlayer?.GetComponent<GamePlayerRestart>();
        
        if (localPlayer == null)
        {
            Debug.LogError("[OptionUI] GamePlayerRestart 컴포넌트를 찾을 수 없습니다!");
            
            // 대안: 씬에서 직접 찾기
            localPlayer = FindFirstObjectByType<GamePlayerRestart>();
            if (localPlayer == null)
            {
                Debug.LogError("[OptionUI] 씬에서도 GamePlayerRestart를 찾을 수 없습니다!");
                return;
            }
        }

        Debug.Log("[OptionUI] 종료 요청 전송");
        localPlayer.CmdRequestQuit();
        
        // 옵션 패널 닫기
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

}
