using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalNetworkConfig
{
    public static INetworkManager _networkManager;

    public static ushort GlobalPacketSize { get; set; } = 2000;

    public static ushort GlobalFragmentSize { get; set; } = 665;

    public static LocalHostConnectionInfo ThisNodeInfo { get; set; }
}
