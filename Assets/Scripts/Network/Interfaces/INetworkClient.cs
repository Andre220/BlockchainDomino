using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkClient
{
    //void ConnectToLocalhostNode(int port);

    LocalHostConnectionInfo ConnectToLocalhostNode(int port);

    void ConnectToLocalhostNode(int port, string nickname);

    void DisconnectFromLocalhostNode(int connectionID);

    //void BroadcastMessageToLocalHostNodes(CustomNetworkMessageBase messageBaseObject, List<int> ConnectionIDList);

    void SendMessageToLocalhostNode(CustomNetworkMessageBase messageBaseObject, int ConnectionID);
}
