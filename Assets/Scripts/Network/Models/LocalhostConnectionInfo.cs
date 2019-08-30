using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// hold info of people connect to you server.
/// Remember that this info is about the OTHER PLAYER CONNECTED TO YOU SERVER.
/// </summary>
public class ConnectionInfoLocalHost : MonoBehaviour
{
    public int ConnectionID { get; set; }
    public int LocalhostPort { get; set; }
    public string NickName { get; set; }
}
