using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkClient
{
    //void ConnectToLocalhostNode(int port);

    LocalHostConnectionInfo ConnectToLocalHostNode(int port);

    void ConnectToLocalHostNode(int port, string nickname);

    void DisconnectFromLocalhostNode(int connectionID);

    //void BroadcastMessageToLocalHostNodes(CustomNetworkMessageBase messageBaseObject, List<int> ConnectionIDList);

    void SendMessageToLocalhostNode(CustomNetworkMessageBase messageBaseObject, int ConnectionID);
}
