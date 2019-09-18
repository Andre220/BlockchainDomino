using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// hold info of people connect to you server.
/// Remember that this info is about the OTHER PLAYER CONNECTED TO YOU SERVER.
/// 
/// </summary>

[Serializable]
public class LocalHostConnectionInfo : MonoBehaviour
{
    public int ConnectionID;
    public int LocalhostPort;
    public string NickName;
}
