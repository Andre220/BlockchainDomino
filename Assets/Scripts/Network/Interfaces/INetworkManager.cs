using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkManager
{
    void SetupPeerServerAndClient(string port, GameObject Node, Action<LocalHostConnectionInfo> SetupUIElement);

    void ConnectToPeer(string peertToConnectPort);

    void SendCustomMessage(CustomNetworkMessageBase message, LocalHostConnectionInfo lhci);
}
