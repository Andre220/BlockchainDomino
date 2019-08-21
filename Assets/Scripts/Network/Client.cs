using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    public static Client instance = null;

    private const int MAX_CONNECTION = 15;

    public int clientPort;

    //public int portToConnect;
    //private int clientPort;

    private int hostId;
    private List<int> connectionsID; //Hold the ID of each connection that this client have.

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

        //myPort = Server.instance.port;

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);

        //connectionsID.Add(NetworkTransport.Connect(hostId, "127.0.0.1", portToConnect, 0, out error));//improve this method to send info to every node in network
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


        //sendMessageToServer("Sending message from node at port " + myPort + " to node at port " + portToConnect);
        //sendMessageToServer("Sending message from node at port " + "||" + " to node at port " + portToConnect);
        //sendMessageToServer("Sending message from node at port " + SendingPort + " to node at port" + "||");

        /*NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        switch (recData)
        {
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                message = msg;
                OnDataEvent();
                break;
        }*/
    }

    private void OnDataEvent()
    {
        Debug.Log("We got a message " + message);
    }

    public void CreateConnection(int port)
    {
        connectionsID.Add(NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error));//improve this method to send info to every node in network
        sendMessageToNode(clientPort.ToString(), connectionsID[connectionsID.Count -1]);//Passing my port to server
    }

    public void sendMessageToConnectedNodes(string MessageToStream)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(MessageToStream);
        for (int i = 0; i < connectionsID.Count - 1; i++)
        {
            NetworkTransport.Send(hostId, connectionsID[i], unreliableChannel, buffer, MessageToStream.Length * sizeof(char), out error);
        }

    }

    public void sendMessageToNode(string MessageToStream, int connectionID)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(MessageToStream);
        NetworkTransport.Send(hostId, connectionID, unreliableChannel, buffer, MessageToStream.Length * sizeof(char), out error);
    }

    /*public void sendMessageToServer(string infotToSend)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(infotToSend);
        NetworkTransport.Send(hostId, connectionID, unreliableChannel, buffer, infotToSend.Length * sizeof(char), out error);//Here could be a json to be easier
    }*/
}
