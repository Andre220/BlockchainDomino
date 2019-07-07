using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    public static Client instance = null;

    public Text ClientDebugText;

    //Need to find a way to define that gaming connection is diferent from blockchain connection
    private const int MAX_CONNECTION = 125; // As bitcoin, by default we can receice 125 connections by peer.

    public int portToConnect;
    private int myServerPort;

    private int hostId;
    private int connectionID;

    private int reliableChannel;
    private int unreliableChannel;

    private byte error;

    public string message;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        myServerPort = Server.instance.serverPort;

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);
    }

    private void Update()
    {
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;

        //sendMessageToServer("Sending message from node at port " + myHostPort + "||" + " to node at port" + portToConnect);

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        switch (recData)
        {
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                message = msg;
                OnDataEvent();
                break;
        }
    }

    public void SendConnectionRequest(int portToConnect)
    {
        string infoToSend = @"{port:" + myServerPort + "}";

        byte[] buffer = Encoding.Unicode.GetBytes(JsonUtility.ToJson(infoToSend));//Envia a porta dessa instancia na solicitacao de conexao, para que o no solicitado responda.

       // NetworkTransport.Send(hostId, connectionID, unreliableChannel, buffer, infoToSend.Length * sizeof(char), out error);//Here could be a json to be easier

        connectionID = NetworkTransport.Connect(hostId, "127.0.0.1", portToConnect, 0, out error);//improve this method to send info to every node in network
    }

    private void OnDataEvent()
    {
        Debug.Log("We got a message " + message);
    }

    public void sendMessageToServer(string infotToSend)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(infotToSend);
        NetworkTransport.Send(hostId, connectionID, unreliableChannel, buffer, infotToSend.Length * sizeof(char), out error);//Here could be a json to be easier
    }
}
