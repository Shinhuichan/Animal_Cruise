using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyRoomManager : NetworkRoomManager
{
    public List<Transform> SpawnPoints = new List<Transform>();

    public void Awake()
    {
        if (FindObjectsOfType<MyRoomManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayerGameObject)
    {
        MyRoomPlayer customRoomPlayer = roomPlayerGameObject.GetComponent<MyRoomPlayer>();
        if (customRoomPlayer == null)
        {
            Debug.LogError("RoomPlayer에 MyRoomPlayer 컴포넌트 없음");
            return null;
        }

        GameObject gamePlayer = Instantiate(playerPrefab);

        var characterSet = gamePlayer.GetComponent<CharacterSet>();
        if (characterSet != null)
        {
            characterSet.characterIndex = customRoomPlayer.selectedCharacterIndex; // 
            Debug.Log($"[Server] GamePlayer 생성됨 - connId: {conn.connectionId}, characterIndex = {characterSet.characterIndex}");
        }

        return gamePlayer;
    }

    public override void OnRoomServerPlayersReady()
{
    if (AllPlayersSelectedCharacters())
    {
        Debug.Log("All players ready & characters selected. Changing scene.");
        ServerChangeScene(GameplayScene);
    }
    else
    {
        Debug.LogWarning("Not all players selected characters yet.");
    }
}

private bool AllPlayersSelectedCharacters()
{
    foreach (var player in roomSlots)
    {
        var myRoomPlayer = player as MyRoomPlayer;
        if (myRoomPlayer == null || myRoomPlayer.selectedCharacterIndex == -1)
            return false;
    }
    return true;
}


    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        Debug.Log($"Player ready: {conn.connectionId}");
    }

    // Ready 버튼 클릭 처리
    public void OnReadyButtonClicked()
    {
        if (NetworkClient.connection?.identity != null)
        {

            var roomPlayer = NetworkClient.connection.identity.GetComponent<MyRoomPlayer>();
            if (roomPlayer != null)
            {
                roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
                Debug.Log($"Ready state changed to: {!roomPlayer.readyToBegin}");

                // MyRoomManager roomManager = FindFirstObjectByType<MyRoomManager>();
                
                // roomManager.CmdCharacterIndex();
            }
            else  Debug.LogError("MyRoomPlayer component not found!");


        }
        else  Debug.LogError("No network connection or identity found!");
    }   




    // 게임 씬 로드 완료 후 호출
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if (sceneName == GameplayScene)
        {
            Debug.Log("Game scene loaded successfully!");
            // 게임 시작 후 추가 초기화 로직
            NetworkIdentity[] allNetworkIdentities = FindObjectsOfType<NetworkIdentity>();
            foreach (var i in allNetworkIdentities)
            {
                RequestBoxAuthority(i);
            }
        }
    }

    public void RequestBoxAuthority(NetworkIdentity box)
    {
        var player = NetworkClient.connection.identity.GetComponent<MyRoomPlayer>();
        player.CmdRequestAuthority(box);
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        NetworkServer.SpawnObjects();

    }


    // 만들어야할 코드.... 왜 동작을 안 하는지 모르겠음
    // [Server]
    //   public void ChangeScene(string nextScene)
    // {
    //     if (!NetworkServer.active) return;

    //     Debug.Log(" switching to next Scene");
    //     FindAnyObjectByType<NetworkRoomManager>().ServerChangeScene(nextScene);
    // }


    // [Server]
    // public void EndGameForAllClients()
    // {
    //     if (!NetworkServer.active) return;

    //     Debug.Log("서버: 모든 클라이언트에게 게임 종료 신호를 보냅니다!");

    //     foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
    //     {
    //         NetworkIdentity playerIdentity = conn.identity;

    //         if (playerIdentity != null)
    //         {
    //             MyRoomPlayer roomPlayer = playerIdentity.GetComponent<MyRoomPlayer>();

    //             if (roomPlayer != null) roomPlayer.RpcQuitGame();
    //             else Debug.LogWarning($"서버: 연결 {conn.connectionId}의 플레이어 객체에서 MyRoomPlayer 컴포넌트를 찾을 수 없습니다.");
    //         }
    //         else Debug.LogWarning($"서버: 연결 {conn.connectionId}에 플레이어 객체가 없습니다.");
    //     }
    //     Application.Quit();
    // }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log($"[Server] 연결 종료됨: connId {conn.connectionId}");
        int connectedClientCount = 0;

        foreach (var kvp in NetworkServer.connections)
            if (kvp.Value != null && kvp.Value.identity != null) connectedClientCount++;

        Debug.Log($"[Server] 남은 클라이언트 수: {connectedClientCount}");

        // 클라이언트가 모두 나갔을 때
        if (connectedClientCount == 0)
        {
            Debug.Log("[Server] 모든 클라이언트가 나갔습니다. 헤드리스 서버 종료.");
            Application.Quit();
        }
    }
}