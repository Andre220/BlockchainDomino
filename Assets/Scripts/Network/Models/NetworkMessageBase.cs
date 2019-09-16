using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetworkMessageType
{
    ConnectionInfo = 0,
    ConnectionResponse = 1,
    GameplayInfo = 2
}

[Serializable]
public class NetworkMessageBase
{
    public NetworkMessageType messageType;
    public object MessageObj;

    public NetworkMessageBase(NetworkMessageType type, object messageObj)
    {
        messageType = type;
        MessageObj = messageObj;
    }
}
