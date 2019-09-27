using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface INetworkServer
{
    //Unity LLAPI network events 
    event Action<LocalHostConnectionInfo> ConnectEvent;
    event Action DataReceiveEvent;
    event Action<int> DisconnectEvent;
    event Action BroadcastEvent;

    //Domino Custom events
    event Action PlayRequestEvent;

    event Action<GamePecas, int> PlayRequestAccept;


    List<LocalHostConnectionInfo> LocalHostKnowNodes { get; set; }

    void OnConnectEvent(int hostId, int connectionId, NetworkError error);

    void OnDataReceiveEvent(int hostId, int connectionId, byte[] buffer, NetworkError error);

    void OnDisconnectEvent(int hostId, int connectionId, NetworkError error);
}
