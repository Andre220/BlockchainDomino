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

    public int hostId;

    //public List<int> connectionsID = new List<int>(); //Hold the ID of each connection that this client have requested
    public List<LocalHostConnectionInfo> localHostConnectedNodes = new List<LocalHostConnectionInfo>(); //Hold the info of each connection that i estabilish

    private int reliableChannel;
    private int unreliableChannel;

    private byte error;

    private void Start()
    {
        ConfigureNetworkInit();
    }

    void ConfigureNetworkInit()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);
    }

    /*Connect and Disconnect*/

    #region LocalHost Connection and Disconnection

    public LocalHostConnectionInfo ConnectToLocalhostNode(int port)
    {
        int connectionID = NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error);

        //connectionsID.Add(connectionID);

        LocalHostConnectionInfo localHostConnectionInfo = new LocalHostConnectionInfo
        {
            ConnectionID = connectionID,
            LocalhostPort = port,
            NickName = "Guest " + DateTime.Now
        };

        //localHostConnectedNodes.Add(localHostConnectionInfo);

        return localHostConnectionInfo;
    }

    public void ConnectToLocalhostNode(int port, string nickname)
    {
        int connectionID = NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error);

        //connectionsID.Add(connectionID);

        LocalHostConnectionInfo connectionInfo = new LocalHostConnectionInfo
        {
            ConnectionID = connectionID,
            LocalhostPort = port,
            NickName = nickname
        };

        CustomNetworkMessageBase message = new CustomNetworkMessageBase(CustomDataEventsEnum.ConnectionInfoRequest, connectionInfo);

        //localHostConnectedNodes.Add(connectionInfo);

        SendMessageToLocalhostNode(message, connectionID);
    }

    public void DisconnectFromLocalhostNode(int connectionID)
    {
        bool success = NetworkTransport.Disconnect(hostId, connectionID, out error);

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

    #region Connection and Disconnection

    #endregion

    #region Send and Broadcast Messages

    public void SendMessageToLocalhostNode(CustomNetworkMessageBase messageBaseObject, int ConnectionID)
    {
        string messageBaseObjectJson = JsonConvert.SerializeObject(messageBaseObject);
        byte[] buffer = Encoding.Unicode.GetBytes(messageBaseObjectJson);
        NetworkTransport.Send(hostId, ConnectionID, unreliableChannel, buffer, messageBaseObjectJson.Length * sizeof(char), out error);
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
                "localhost port " + CILH.LocalhostPort + " |  " +
                "Player Nickname " + CILH.NickName + "|  "
            );
        }
    }

    #endregion
}
