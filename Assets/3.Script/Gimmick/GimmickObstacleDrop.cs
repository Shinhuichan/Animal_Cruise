using Mirror;
using UnityEngine;

public class GimmickObstacleDrop : MonoBehaviour
{
    public Rigidbody obstaclesRd;         // 떨어질 장애물
    public string targetTag = "Player";   // 감지할 대상의 태그 (예: "Player")
    
    private void Start()
    {
        // 시작할 때 장애물을 공중에 고정시킴
        if (obstaclesRd != null)
            obstaclesRd.isKinematic = true;

        obstaclesRd = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player만 감지되면 장애물 떨어뜨리기
        if (other.CompareTag(targetTag) && obstaclesRd != null)
        {
            obstaclesRd.isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetTag == "Player" && obstaclesRd != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
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
            }
        }
    }
}
