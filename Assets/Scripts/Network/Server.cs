using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour, INetworkServer
{

    public List<int> connectionsID = new List<int>(); //Hold the ID of each connection that this server received
    public List<LocalHostConnectionInfo> LocalHostKnowNodes { get; set; } //Here i storage the nodes that already connect to me;

    //public static Server instance = null;

    //Unity LLAPI basic network message typs
    public event Action<LocalHostConnectionInfo> ConnectEvent;
    public event Action DataReceiveEvent;
    public event Action<int> DisconnectEvent;
    public event Action BroadcastEvent;

    // public event Action ConnectionInfoEvent;

    public event Action PlayRequestEvent;
    public event Action<GamePecas, int, LocalHostConnectionInfo> PlayRequestAccept;

    public event Action EnemyReady;

    private const int MAX_CONNECTION = 20;

    public int hostId;

    private int reliableChannel;
    private int unreliableChannel;

    private byte error;

    public int serverPort; // Port that is open and listening for messages.

    void Start()
    {
        ConfigureNetworkInit();
    }

    void Update()
    {
        LocalHostServer(); //Listening 
    }

    void ConfigureNetworkInit()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        cc.PacketSize = GlobalNetworkConfig.GlobalPacketSize;
        cc.FragmentSize = GlobalNetworkConfig.GlobalFragmentSize;

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, serverPort, null);// the ip is null because we are at localhost - i should test it with 127.0.0.1 to see how it behave

        GlobalNetworkConfig.ThisNodeInfo.HostId = hostId;

        LocalHostKnowNodes = new List<LocalHostConnectionInfo>();
    }

    void LocalHostServer()
    {
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[2500];
        int bufferSize = 2500;
        int dataSize;
        byte error;

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.LogError((NetworkError)error);
        }
        else
        {
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
    }

    #region DEFAULT Connection Events Handlers

    public void OnConnectEvent(int hostId, int connectionId, NetworkError error)
    {
        connectionsID.Add(connectionId);

        LocalHostConnectionInfo connectedNode = new LocalHostConnectionInfo
        {
            HostId = hostId,
            ConnectionID = connectionId,
            NickName = "Guest" + DateTime.Now
        };

        //Adding guest node that will update his info when his info come
        //UPDATE: CHANGING THE LOGIC, THIS STILL NECESSARY?
        LocalHostKnowNodes.Add(connectedNode);

        ConnectEvent?.Invoke(connectedNode);

        print("|Connection event: " +
            " |HostId: " + hostId + 
            " |ConnectionId : " + connectionId + 
            " |Error: " + error.ToString());
    }

    public void OnDataReceiveEvent(int hostId, int connectionId, byte[] buffer, NetworkError error)
    {
        DataReceiveEvent?.Invoke();

        var JsonText = Encoding.Unicode.GetString(buffer);

        CustomNetworkMessageBase message = JsonConvert.DeserializeObject<CustomNetworkMessageBase>(JsonText);

        switch (message.MessageType)
        {
            case CustomDataEventsEnum.ConnectionInfoRequest: // Get connection info and save in connections pool
                //OnConnectionInfoEvent(hostId, connectionId, message, buffer, error);
                break;
            case CustomDataEventsEnum.PlayRequest: // if connection response is ok, continue. Else, disconnect from node.
                PlayRequestEvent?.Invoke();
                break;
            case CustomDataEventsEnum.PlayRequestAccept:
                //Every time that you receive a game request, you are the player 2 (index 1) and with that index the correct pieces will be show to you
                PlayRequestAccept?.Invoke((GamePecas)message.MessageObj, 1, (LocalHostConnectionInfo)message.SenderInfo);
                break;
            case CustomDataEventsEnum.PlayRequestDecline:
                //OnPlayRequestResponseEvent();
                break;
            case CustomDataEventsEnum.AdversaryConfigurationResponse:
                Debug.Log("Adversary Accepted");
                EnemyReady?.Invoke();
                break;
            case CustomDataEventsEnum.PlayerMove: // call event to deal with this event and every gameplay script who sould know about network info should do your action

                break;
        }

        print("|Data event: " +
            "/n|HostId: " + hostId + 
            "/n|ConnectionId : " + connectionId + 
            "/n|Error: " + error.ToString());
    }

    public void OnDisconnectEvent(int hostId, int connectionId, NetworkError error)
    {
        DisconnectEvent?.Invoke(connectionId);


        print("|Disconnect event: " +
            "/n|HostId: " + hostId +
            "/n|ConnectionId : " + connectionId +
            "/n|Error: " + error.ToString());
    }

    public void OnBroadcastEvent(int hostId, int connectionId, NetworkError error)
    {
        BroadcastEvent?.Invoke();

        print("|Boradcast event: " +
            "/n|HostId: " + hostId +
            "/n|ConnectionId : " + connectionId +
            "/n|Error: " + error.ToString());
    }

    #endregion

    #region CUSTOM Connection Events Handlers

    /*public void OnConnectionInfoEvent(int hostId, int connectionId, NetworkCustomMessageBase networkMessageBase, byte[] buffer, NetworkError error)
    {
        LocalHostConnectionInfo infoFromConnectedPeer = JsonConvert.DeserializeObject<LocalHostConnectionInfo>(networkMessageBase.MessageObj.ToString());

        ConnectionInfoEvent?.Invoke();

        LocalHostKnowNodes.Add(infoFromConnectedPeer);
    }

    public void OnPlayRequestEvent()
    {
        PlayRequestEvent?.Invoke();
    }

    public void OnPlayRequestResponseEvent()
    {
        PlayRequestResponseEvent?.Invoke();
    }*/


    #endregion
}