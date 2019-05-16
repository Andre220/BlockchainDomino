using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    //public static Client instance = null;

    private const int MAX_CONNECTION = 15;

    public int portToConnect;
    //private int myPort;

    private int hostId;
    private int connectionID;

    private int reliableChannel;
    private int unreliableChannel;

    private byte error;

    public string message;

    private void Start()
    {
        /*if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        myPort = Server.instance.port;*/

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);

        connectionID = NetworkTransport.Connect(hostId, "127.0.0.1", portToConnect, 0, out error);//improve this method to send info to every node in network
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
        sendMessageToServer("Sending message from node at port " + "||" + " to node at port");

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
