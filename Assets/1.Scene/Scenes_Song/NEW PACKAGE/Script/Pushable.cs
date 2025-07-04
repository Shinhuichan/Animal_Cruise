using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class Pushable : NetworkBehaviour
{
    void Start()
    {
        
    }
    public void RequestBoxAuthority()
    {
        NetworkIdentity i = GetComponent<NetworkIdentity>();
        var player = NetworkClient.connection.identity.GetComponent<MyRoomPlayer>();
        player.CmdRequestAuthority(i);
    }
}
