using Assets.Scripts.Services;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class works like a perr in the network, so it had the client and server functionality.
/// This class implements the INetworkPeer
/// </summary>
public class NetworkPeer : MonoBehaviour
{
    public event Action ConnectEvent;
    public event Action DataReceiveEvent;
    public event Action DisconnectEvent;

    //public static NetworkPeer instance = null;

    //public IList<Node> KnowNodes; //Here i storage the nodes (a class with connection id info) of any node that already connect to me;

    private const int MAX_CONNECTION = 2; //My adversary and the node that will randomize our domino pieces

    private int socketID;
    private List<int> connectionsID = new List<int>(); //Hold the ID of each connection that this client have.
    private List<ConnectionInfoLocalHost> localhostConnectionsID = new List<ConnectionInfoLocalHost>(); //Hold the peer connection info of each connection

    private int reliableChannel;
    private int unreliableChannel;

    public int ListeningPort; //Port where server will listen

    private byte error;

    void Start()
    {
        /*if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);*/

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        socketID = NetworkTransport.AddHost(topo, ListeningPort, null);// the ip is null because we are at localhost - i should test it with 127.0.0.1 to see how it behave

        print("socketID: " + socketID + " | \n" +
                "reliableChannel: " + reliableChannel + " | \n" +
                "unreliableChannel: " + unreliableChannel + " | \n" +
                "ListeningPort: " + ListeningPort + " | \n" +
                "error: " + error + " | \n");
    }

    void Update()
    {
        LocalHostServer();
    }

    void LocalHostServer()
    {
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;

        NetworkEventType recData = (NetworkEventType)NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        switch (recData)
        {
            case NetworkEventType.Nothing:
                //print("Nothing happen");
                break;

            case NetworkEventType.ConnectEvent:
                OnConnectEvent(recHostId, connectionId, (NetworkError)error);
                break;

            case NetworkEventType.DataEvent:

                string dataEventMsg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);

                var json = JsonConvert.DeserializeObject<object>(dataEventMsg);

                print(json.ToString());

                break;

            case NetworkEventType.DisconnectEvent:
                OnDisconnectEvent(recHostId, connectionId, (NetworkError)error);
                break;

            case NetworkEventType.BroadcastEvent:
                break;

            default:
                print("Some unknow network message came");
                break;
        }

        print("recHostId: " + recHostId + " | \n" +
            "connectionId: " + connectionId + " | \n" +
            "channelId: " + channelId + " | \n" +
            "recBuffer: " + recBuffer + " | \n" +
            "bufferSize: " + bufferSize + " | \n" +
            "dataSize: " + dataSize + " | \n" +
            "error: " + error + " | \n");
    }

    #region networkEventsHandler

    void OnConnectEvent(int hostId, int connectionId, NetworkError error)
    {
        ConnectEvent?.Invoke();

        print("Connection event = |hostId: " + hostId + " | connectionId : "
           + connectionId + " | error: " + error.ToString() + "|");
    }

    void OnDataReceiveEvent(int hostId, int connectionId, NetworkError error)
    {
        DataReceiveEvent?.Invoke();

        print("Disconnection event = |hostId: " + hostId + " | connectionId : "
           + connectionId + " | error: " + error.ToString() + "|");
    }

    void OnDisconnectEvent(int hostId, int connectionId, NetworkError error)
    {
        DisconnectEvent?.Invoke();

        print("Disconnection event = |hostId: " + hostId + " | connectionId : "
           + connectionId + " | error: " + error.ToString() + "|");
    }

    #endregion

    /*#region ConnectionRegion

    public void LocalHostConnect(int port)
    {
        ConnectionInfoLocalHost connectionInfo = new ConnectionInfoLocalHost()
        {
            localhostIp = "127.0.0.1",
            localhostPort = port,
            ID = NetworkTransport.Connect(socketID, "127.0.0.1", port, 0, out error),
            nickName = "guest at " + port,
        };

        localhostConnectionsID.Add(connectionInfo);
    } // Simple connection without nickname - you is called guest at port `pontnumber`.

    public void LocalHostConnect(int port, string nickName) // Connects to another localhost peer (another port)
    {
        ConnectionInfoLocalHost connectionInfo = new ConnectionInfoLocalHost()
        {
            localhostIp = "127.0.0.1",
            localhostPort = port,
            nickName = nickName,
            ID = NetworkTransport.Connect(socketID, "127.0.0.1", port, 0, out error),
        };

        localhostConnectionsID.Add(connectionInfo);
    }

    public void LocalHostDisconnect(int connectionID)
    {
        NetworkTransport.Disconnect(socketID, connectionID, out error);
    }

    public void LocalHostConnectionsDebug()
    {
        for (int i = 0; i < localhostConnectionsID.Count; i++)
        {
            /*print($"" +
                $"ID: {localhostConnectionsID[i].ID} | " +
                $"nickName: {localhostConnectionsID[i].NickName} | " +
                $"ip and port: {localhostConnectionsID[i].localhostIp}:{localhostConnectionsID[i].localhostPort}");
        }
    } // Show in console windows every connection id

    #endregion

    #region networkObjectGeneration

    //maybe this section will die

    byte[] generateByteData(object obj)
    {
        string JsonData = JsonUtility.ToJson(obj);
        byte[] networkBuffer = Encoding.Unicode.GetBytes(JsonData);
        return networkBuffer;
    }

    object retrieveByteData(byte[] data, int size)
    {
        string JsonData = Encoding.Unicode.GetString(data, 0, size);
        return JsonUtility.FromJson<object>(JsonData);
    }

    #endregion

    #region networkMessageSender

    public void SendMessageToLocalhostNode(object messageObj, int connectionID)
    {
        string JsonData = JsonUtility.ToJson(messageObj);

        byte[] jsonBuffer = Encoding.Unicode.GetBytes(JsonData);

        NetworkTransport.Send(socketID, connectionID, unreliableChannel, jsonBuffer, JsonData.Length * sizeof(char), out error);
    }

    public void SendMessageToLocalhostNode(string message, int connectionID)
    {
        string JsonData = JsonUtility.ToJson(message);

        byte[] jsonBuffer = Encoding.Unicode.GetBytes(JsonData);

        NetworkTransport.Send(socketID, connectionID, unreliableChannel, jsonBuffer, JsonData.Length * sizeof(char), out error);
    }


    public void StreamToConnectedNodes(object messageObj)
    {
        string JsonData = JsonUtility.ToJson(messageObj);

        byte[] jsonBuffer = Encoding.Unicode.GetBytes(JsonData);

        for (int i = 0; i < connectionsID.Count - 1; i++)
        {
            NetworkTransport.Send(socketID, connectionsID[i], unreliableChannel, jsonBuffer, JsonData.Length * sizeof(char), out error);
        }
    }

    #endregion*/
}
