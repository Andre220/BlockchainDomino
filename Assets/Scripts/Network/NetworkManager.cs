using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : INetworkManager
{
    INetworkClient _networkClientService;
    INetworkServer _networkServerService;

    public void SetupPeerServerAndClient(string port, GameObject Node, Action<LocalHostConnectionInfo> SetupUIElement)
    {
        Node.AddComponent<Server>().serverPort = int.Parse(port);
        Node.AddComponent<Client>();

        _networkServerService = Node.GetComponent<Server>();
        _networkClientService = Node.GetComponent<Client>();

        _networkServerService.ConnectEvent += SetupUIElement;
    }

    public void ConnectToPeer(string peertToConnectPort)
    {
        LocalHostConnectionInfo localHostConnectionInfo = _networkClientService.ConnectToLocalHostNode(int.Parse(peertToConnectPort));
    }

    public void SendCustomMessage(CustomNetworkMessageBase message, LocalHostConnectionInfo lhci)
    {
        _networkClientService.SendMessageToLocalhostNode(message, lhci.ConnectionID);
    }
}
