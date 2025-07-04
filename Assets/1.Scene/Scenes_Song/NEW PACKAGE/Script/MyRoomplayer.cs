using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyRoomPlayer : NetworkRoomPlayer
{
    [SyncVar]
    public int selectedCharacterIndex = -1;

    [Command]
    public void CmdSetCharacterIndex(int index)
    {
        selectedCharacterIndex = index;
        Debug.Log($"[Server] 캐릭터 인덱스 설정됨: {index} by connId={connectionToClient.connectionId}");

        // 선택 완료 즉시 Ready 처리
        if (!readyToBegin)
            CmdChangeReadyState(true);
    }

   [Command]
public void CmdRequestAuthority(NetworkIdentity target)
{
    if (!target.isOwned)  //권환을 클라이언트한테 넘김
        target.AssignClientAuthority(connectionToClient);
}


    [ClientRpc]
    public void RpcQuitGame()
    {
        Application.Quit(); // 게임 종료!
    }
}
