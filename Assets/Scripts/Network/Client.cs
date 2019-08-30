using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour, INetworkClient
{
    //public static Client instance = null;

    public const int MAX_CONNECTION = 2;

    public int hostId;
    private List<int> connectionsID = new List<int>(); //Hold the ID of each connection that this client have.
    private List<ConnectionInfoLocalHost> localHostConnectedNodes = new List<ConnectionInfoLocalHost>(); //Hold the info of each connection

    private int reliableChannel;
    private int unreliableChannel;

    private byte error;

    private void Start()
    {
        ConfigureNetworkInit();
    }

    void Update()
    {
        ConnectionInfoLocalHost connectionInfo = new ConnectionInfoLocalHost
        {
            ConnectionID = 0,
            LocalhostPort = 0,
            NickName = "pipi"
        };

        SendMessageToLocalhostNode(new NetworkMessageBase(NetworkMessageType.ConnectionInfo, connectionInfo), 1);
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

    #region Connect and Disconnect

    public void ConnectToLocalhostNode(int port)
    {
        int connectionID = NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error);

        connectionsID.Add(connectionID);

        localHostConnectedNodes.Add(new ConnectionInfoLocalHost
        {
            ConnectionID = connectionID,
            LocalhostPort = port,
            NickName = "Guest " + DateTime.Now
        });
    }

    public void ConnectToLocalhostNode(int port, string nickname)
    {
        int connectionID = NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error);

        connectionsID.Add(connectionID);

        ConnectionInfoLocalHost connectionInfo = new ConnectionInfoLocalHost
        {
            ConnectionID = connectionID,
            LocalhostPort = port,
            NickName = nickname
        };

        NetworkMessageBase message = new NetworkMessageBase(NetworkMessageType.ConnectionInfo, connectionInfo);

        //localHostConnectedNodes.Add(connectionInfo);

        SendMessageToLocalhostNode(message, connectionID);
    }

    public void DebugConnectedNodes(List<ConnectionInfoLocalHost> connectionInfoLocalHost)
    {
        foreach (ConnectionInfoLocalHost CILH in connectionInfoLocalHost)
        {
            print
            (
                "Connection info ID " + CILH.ConnectionID + "| \n" +
                "localhost port " + CILH.LocalhostPort + " | \n" +
                "Player Nickname " + CILH.NickName + "| \n"
            );
        }
    }

    public void DisconnectFromLocalhostNode(int connectionID)
    {
        bool success = NetworkTransport.Disconnect(hostId, connectionID, out error);

        if (success)
        {
            connectionsID.Remove(connectionID);

            localHostConnectedNodes.Remove(localHostConnectedNodes.Find(x => x.ConnectionID == connectionID));
        }
        else
        {
            Debug.LogError("Couldn`t disconnect user with connection ID " + connectionID + ". You should look better for what happened.");
        }
    }

    #endregion

    #region Send and Broadcast Messages

    public void BroadcastMessageToLocalHostNodes(NetworkMessageBase messageBaseObject, List<int> ConnectionIDList)
    {
        foreach (int ConnectionID in ConnectionIDList)
        {
            SendMessageToLocalhostNode(messageBaseObject, ConnectionID);
        }
    }

    public void sendMessageToConnectedNodes(string MessageToStream)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(MessageToStream);
        for (int i = 0; i < connectionsID.Count - 1; i++)
        {
            NetworkTransport.Send(hostId, connectionsID[i], unreliableChannel, buffer, MessageToStream.Length * sizeof(char), out error);
        }
    }

    public void SendMessageToLocalhostNode(NetworkMessageBase messageBaseObject, int ConnectionID)
    {
        string messageBaseObjectJson = JsonUtility.ToJson(messageBaseObject);
        byte[] buffer = Encoding.Unicode.GetBytes(messageBaseObjectJson);
        NetworkTransport.Send(hostId, ConnectionID, unreliableChannel, buffer, messageBaseObjectJson.Length * sizeof(char), out error);
    }

    #endregion
}
