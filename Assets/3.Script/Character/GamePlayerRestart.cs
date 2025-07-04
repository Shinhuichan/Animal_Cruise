using Mirror;
using UnityEngine;

public class GamePlayerRestart : NetworkBehaviour
{
    // UI에서 눌렀을 때 이 컴포넌트를 통해 서버에 재시작 요청
    [Command]
    public void CmdRequestQuit()
    {
        Debug.Log("[CmdRequestRestart] 서버에서 재시작 요청 수신");
        var mgr = FindFirstObjectByType<MyRoomPlayer>();
        if (mgr != null)
        {
            Debug.Log("[CmdRequestRestart] MyRoomPlayer 발견");
            mgr.RpcQuitGame();
        }
        else Debug.LogError("[CmdRequestRestart] MyRoomPlayer를 찾을 수 없음!");
    }   
}
