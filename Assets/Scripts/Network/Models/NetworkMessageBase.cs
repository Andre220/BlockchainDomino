using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetworkMessageType
{
    ConnectionInfo = 0,
    ConnectionResponse = 1,
    GameplayInfo = 2
}

public class NetworkMessageBase
{
    public NetworkMessageType messageType { get; set; }
    public dynamic MessageInfo { get; set; }

    public NetworkMessageBase(NetworkMessageType type, dynamic messageInfo)
    {
        messageType = type;
        MessageInfo = messageInfo;
    }
}
