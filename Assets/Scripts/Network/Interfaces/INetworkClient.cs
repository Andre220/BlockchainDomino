using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkClient
{
    void ConnectToLocalhostNode(int port);

    void ConnectToLocalhostNode(int port, string nickname);

    void DisconnectFromLocalhostNode(int connectionID);

    void BroadcastMessageToLocalHostNodes(NetworkMessageBase messageBaseObject, List<int> ConnectionIDList);

    void SendMessageToLocalhostNode(NetworkMessageBase messageBaseObject, int ConnectionID);
}
