using Assets.Scripts.Services;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    //public IList<int> KnowNodes; //Here i storage the nodes (the port value) of any node that already connect to me;

    public static Server instance = null;

    private const int MAX_CONNECTION = 15;

    public int ListeningPort;//Default port is 23500

    private int hostId;

    private int reliableChannel;
    private int unreliableChannel;

    private byte error;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        // KnowNodes = new List<int>();

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, ListeningPort, null);// the ip is null because we are at localhost - i should test it with 127.0.0.1 to see how it behave
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

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        switch (recData)
        {
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                string connectEventMsg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                Debug.Log("Player " + connectionId + " has conected and send " + connectEventMsg);
                GameAdm.instance.PlayRequest();
                break;

            case NetworkEventType.DataEvent:
                string dataEventMsg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                Debug.Log("Player " + connectionId + " has sended " + dataEventMsg);
                break;

            case NetworkEventType.DisconnectEvent:
                string disconnectEventMsg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                Debug.Log("Player " + connectionId + " has disconnected and send ");
                break;
            case NetworkEventType.BroadcastEvent:
                break;
        }
    }
}
/*
*/
