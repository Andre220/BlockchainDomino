using System;

/// <summary>
/// hold info of people connect to you server.
/// Remember that this info is about the OTHER PLAYER CONNECTED TO YOU SERVER.
/// 
/// </summary>

[Serializable]
public class LocalHostConnectionInfo
{
    public int HostId;
    //public int LocalhostPort;
    public int ConnectionID; //Used if this localHostConnectionInfo is about a connection to another node

    public string NickName;
}
