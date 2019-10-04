using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : INetworkManager
{
    INetworkClient _networkClientService;
    INetworkServer _networkServerService;

    public void SetupPeerServerAndClient(string port, GameObject Node, string NickName, Action<LocalHostConnectionInfo> SetupUIElement)
    {
        Node.AddComponent<Server>().serverPort = int.Parse(port);
        Node.AddComponent<Client>();

        _networkServerService = Node.GetComponent<Server>();
        _networkClientService = Node.GetComponent<Client>();
        
        GlobalNetworkConfig.ThisNodeInfo = new LocalHostConnectionInfo
        {

            //LocalhostPort = int.Parse(port),
            NickName = NickName,
        };

        _networkServerService.ConnectEvent += SetupUIElement;
    }

    public LocalHostConnectionInfo ConnectToPeer(string peertToConnectPort)
    {
        LocalHostConnectionInfo localHostConnectionInfo = _networkClientService.ConnectToLocalHostNode(int.Parse(peertToConnectPort));

        return localHostConnectionInfo;
    }

    public void SendCustomMessage(CustomNetworkMessageBase message, LocalHostConnectionInfo lhci)
    {
        _networkClientService.SendMessageToLocalhostNode(message, lhci);
    }

    public void GameDataReceived(Action<GamePecas, int, LocalHostConnectionInfo> gamePecas)
    {
        _networkServerService.PlayRequestAccept += gamePecas;
    }

    public void EnemyReady(Action startGame)
    {
        _networkServerService.EnemyReady += startGame;
    }
}
