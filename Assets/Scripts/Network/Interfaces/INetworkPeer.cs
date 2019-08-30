using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkPeer 
{
    event Action ConnectEvent;
    event Action DataReceiveEvent;
    event Action DisconnectEvent;

    void LocalHostConnect(int port);

    void LocalHostConnect(int port, string nickName);

    void LocalHostDisconnect(int connectionID);

    void SendMessageToLocalhostNode(string message, int connectionID);
}
