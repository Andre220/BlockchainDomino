using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkManager
{
    void SetupPeerServerAndClient(string port, GameObject Node, string NickName, Action<LocalHostConnectionInfo> SetupUIElement);

    LocalHostConnectionInfo ConnectToPeer(string peertToConnectPort);

    void SendCustomMessage(CustomNetworkMessageBase message, LocalHostConnectionInfo lhci);

    void GameDataReceived(Action<GamePecas, int, LocalHostConnectionInfo> gamePecas);
}
