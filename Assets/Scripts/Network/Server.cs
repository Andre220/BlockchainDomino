using Assets.Scripts.Services;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour, INetworkServer
{
    public List<ConnectionInfoLocalHost> LocalHostKnowNodes { get; set; } //Here i storage the nodes that already connect to me;

    //public static Server instance = null;

    public event Action ConnectEvent;
    public event Action DataReceiveEvent;
    public event Action DisconnectEvent;
    public event Action ConnectionInfoEvent;


    private const int MAX_CONNECTION = 2;

    public int hostId;

    private int reliableChannel;
    private int unreliableChannel;

    private byte error;

    public int serverPort; // Port that is open and listening for messages.

    void Start()
    {
        /*if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);*/

        // KnowNodes = new List<int>();

        ConfigureNetworkInit();
    }

    void ConfigureNetworkInit()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, serverPort, null);// the ip is null because we are at localhost - i should test it with 127.0.0.1 to see how it behave

        LocalHostKnowNodes = new List<ConnectionInfoLocalHost>();

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
        byte[] recBuffer = new byte[178];
        int bufferSize = 178;
        int dataSize;
        byte error;

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        switch (recData)
        {
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                OnConnectEvent(recHostId, connectionId, (NetworkError)error);
                break;

            case NetworkEventType.DataEvent:
                OnDataReceiveEvent(recHostId, connectionId, recBuffer, (NetworkError)error);
                break;

            case NetworkEventType.DisconnectEvent:
                OnDisconnectEvent(recHostId, connectionId, (NetworkError)error);
                break;
            case NetworkEventType.BroadcastEvent:
                break;
        }
    }

    #region network Events Handlers

    public void OnConnectEvent(int hostId, int connectionId, NetworkError error)
    {
        ConnectEvent?.Invoke();

        print("Connection event = |hostId: " + hostId + " | connectionId : "
           + connectionId + " | error: " + error.ToString() + "|");
    }

    public void OnDataReceiveEvent(int hostId, int connectionId, byte[] buffer, NetworkError error)
    {
        DataReceiveEvent?.Invoke();

        var JsonText = Encoding.Unicode.GetString(buffer);

        NetworkMessageBase message = JsonConvert.DeserializeObject<NetworkMessageBase>(JsonText);

        switch (message.messageType)
        {
            case NetworkMessageType.ConnectionInfo: // Get connection info and save in connections pool
                OnConnectionInfoEvent(hostId, connectionId, message, buffer, error);
                break;
            case NetworkMessageType.ConnectionResponse: // if connection response is ok, continue. Else, disconnect from node.
                break;
            case NetworkMessageType.GameplayInfo: // call event to deal with this event and every gameplay script who sould know about network info should do your action
                break;
        }

        print("Data event = |hostId: " + hostId + " | connectionId : "
           + connectionId + " | error: " + error.ToString() + "|");
    }

    public void OnDisconnectEvent(int hostId, int connectionId, NetworkError error)
    {
        DisconnectEvent?.Invoke();

        print("Disconnection event = |hostId: " + hostId + " | connectionId : "
           + connectionId + " | error: " + error.ToString() + "|");
    }

    public void OnConnectionInfoEvent(int hostId, int connectionId, NetworkMessageBase networkMessageBase, byte[] buffer, NetworkError error)
    {
        ConnectionInfoLocalHost infoFromConnectedPeer = JsonConvert.DeserializeObject<ConnectionInfoLocalHost>(networkMessageBase.MessageObj.ToString());

        ConnectionInfoEvent?.Invoke();

        LocalHostKnowNodes.Add(infoFromConnectedPeer);
    }

    #endregion
}