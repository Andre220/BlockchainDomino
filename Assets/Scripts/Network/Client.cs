using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour, INetworkClient
{
    //public static Client instance = null;

    public const int MAX_CONNECTION = 20;

    //public int hostId;

    //public List<int> connectionsID = new List<int>(); //Hold the ID of each connection that this client have requested
    public List<LocalHostConnectionInfo> localHostConnectedNodes = new List<LocalHostConnectionInfo>(); //Hold the info of each connection that i estabilish

   int reliableChannel;
   int reliableFragmentedChannel;
   int unreliableChannel;

    private byte error;

    void Start()
    {
        ConfigureNetworkInit();
    }

    void ConfigureNetworkInit()
    {
        ConnectionConfig cc = new ConnectionConfig();

        cc.PacketSize = GlobalNetworkConfig.GlobalPacketSize;
        cc.FragmentSize = GlobalNetworkConfig.GlobalFragmentSize;

        reliableChannel = cc.AddChannel(QosType.Reliable);
        reliableFragmentedChannel = cc.AddChannel(QosType.ReliableFragmented);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        NetworkTransport.AddHost(topo, 0);
    }

    #region Connection and Disconnection

    public LocalHostConnectionInfo ConnectToLocalHostNode(int port)
    {
        int connectionID = NetworkTransport.Connect(GlobalNetworkConfig.ThisNodeInfo.HostId, "127.0.0.1", port, 0, out error);

        LocalHostConnectionInfo localHostConnectionInfo = new LocalHostConnectionInfo()
        {
            ConnectionID = connectionID,
            //LocalhostPort = port,
            NickName = "Guest " + DateTime.Now
        };

        return localHostConnectionInfo;
    }

    public void ConnectToLocalHostNode(int port, string nickname)
    {
        int connectionID = NetworkTransport.Connect(GlobalNetworkConfig.ThisNodeInfo.HostId, "127.0.0.1", port, 0, out error);

        //connectionsID.Add(connectionID);

        LocalHostConnectionInfo connectionInfo = new LocalHostConnectionInfo
        {
            ConnectionID = connectionID,
            //LocalhostPort = port,
            NickName = nickname
        };

        CustomNetworkMessageBase message = new CustomNetworkMessageBase(CustomDataEventsEnum.ConnectionInfoRequest, connectionInfo);

        //localHostConnectedNodes.Add(connectionInfo);

        //SendMessageToLocalhostNode(message, connectionID);
    }

    public void DisconnectFromLocalhostNode(int connectionID)
    {
        bool success = NetworkTransport.Disconnect(GlobalNetworkConfig.ThisNodeInfo.HostId, connectionID, out error);

        if (success)
        {
            //connectionsID.Remove(connectionID);

            localHostConnectedNodes.Remove(localHostConnectedNodes.Find(x => x.ConnectionID == connectionID));
        }
        else
        {
            Debug.LogError("Couldn`t disconnect user with connection ID " + connectionID + ". You should look better for what happened.");
        }
    }

    #endregion

    #region Send and Broadcast Messages

    public void SendMessageToLocalhostNode(CustomNetworkMessageBase messageBaseObject, LocalHostConnectionInfo lhci)
    {
        //string messageBaseObjectJson = JsonConvert.SerializeObject(messageBaseObject);
        //byte[] buffer = Encoding.Unicode.GetBytes(messageBaseObjectJson);

        CustomNetworkMessageBase mess = new CustomNetworkMessageBase(CustomDataEventsEnum.ConnectionInfoRequest, "Teste");

        string messageTeste = JsonConvert.SerializeObject(mess);
        byte[] buffer2 = Encoding.Unicode.GetBytes(messageTeste); 

        NetworkTransport.Send(GlobalNetworkConfig.ThisNodeInfo.HostId, lhci.ConnectionID, reliableChannel, buffer2, messageTeste.Length * sizeof(char), out error);

        if((NetworkError)error != NetworkError.Ok)
        {
            Debug.LogError((NetworkError)error);
        }
    }

    /*public void sendMessageToConnectedNodes(string MessageToStream)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(MessageToStream);
        for (int i = 0; i < connectionsID.Count - 1; i++)
        {
            NetworkTransport.Send(hostId, connectionsID[i], unreliableChannel, buffer, MessageToStream.Length * sizeof(char), out error);
        }
    }*/

    /*public void BroadcastMessageToLocalHostNodes(NetworkMessageBase messageBaseObject, List<int> ConnectionIDList)
    {
        foreach (int ConnectionID in ConnectionIDList)
        {
            SendMessageToLocalhostNode(messageBaseObject, ConnectionID);
        }
    }*/

    #endregion

    #region Support Methods

    public void DebugConnectedNodes(List<LocalHostConnectionInfo> LocalHostConnectionInfo)
    {
        foreach (LocalHostConnectionInfo CILH in LocalHostConnectionInfo)
        {
            print
            (
                "Connection info ID " + CILH.ConnectionID + "|  " +
                "host id or socket id" + CILH.HostId + " |  " +
                "Player Nickname " + CILH.NickName + "|  "
            );
        }
    }

    #endregion
}
