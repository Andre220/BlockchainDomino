using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomNetworkMessageBase
{
    public LocalHostConnectionInfo SenderInfo;
    public CustomDataEventsEnum MessageType;
    public object MessageObj;

    public CustomNetworkMessageBase(CustomDataEventsEnum type, object messageObj)
    {
        MessageType = type;
        MessageObj = messageObj;
        SenderInfo = GlobalNetworkConfig.ThisNodeInfo;
    }

    //public CustomNetworkMessageBase(CustomDataEventsEnum type, object messageObj, LocalHostConnectionInfo  senderInfo)
    //{
    //    MessageType = type;
    //    MessageObj = messageObj;
    //    SenderInfo = senderInfo;
    //} //i think that i can use the global config instance to setup this connectionData
}
