using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface INetworkServer
{
    event Action ConnectEvent;
    event Action DataReceiveEvent;
    event Action DisconnectEvent;
    event Action ConnectionInfoEvent;

    List<ConnectionInfoLocalHost> LocalHostKnowNodes { get; set; }

    void OnConnectEvent(int hostId, int connectionId, NetworkError error);

    void OnDataReceiveEvent(int hostId, int connectionId, byte[] buffer, NetworkError error);

    void OnDisconnectEvent(int hostId, int connectionId, NetworkError error);
}
