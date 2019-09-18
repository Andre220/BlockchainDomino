using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomNetworkMessageBase
{
    public CustomDataEventsEnum messageType;
    public object MessageObj;

    public CustomNetworkMessageBase(CustomDataEventsEnum type, object messageObj)
    {
        messageType = type;
        MessageObj = messageObj;
    }
}
