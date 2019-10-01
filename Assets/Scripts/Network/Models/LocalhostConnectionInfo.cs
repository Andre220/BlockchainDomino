using System;

/// <summary>
/// hold info of people connect to you server.
/// Remember that this info is about the OTHER PLAYER CONNECTED TO YOU SERVER.
/// 
/// </summary>

[Serializable]
public class LocalHostConnectionInfo
{
    public int ConnectionID;
    public int LocalhostPort;
    public string NickName;
}
