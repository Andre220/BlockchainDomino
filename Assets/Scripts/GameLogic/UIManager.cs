using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public string nickName;

    public GameObject Node;

    INetworkClient _networkClientService;
    INetworkServer _networkServerService;

    public Transform OnlineNodesFather;
    public GameObject PeerInfoPrefab;

    public GameObject ConnectionUI;
    public GameObject LocalHostPlayerConnectionRequest;

    public GameObject GameUI;

    void Start()
    {
        if (Node == null)
        {
            if (GameObject.Find("Node") == null)
            {
                //Instantiate the gameObject. Actually i will drop error, whatever ...
                Debug.LogError("Node object isn`t created. Please create a gameObject with the name \"Node\".");
            }
            else
            {
                Node = GameObject.Find("Node");
            }
        }
        _networkServerService.ConnectEvent += StartGame;
    }

    #region basic UI Function to enable and disable UI Objects

    public void ShowUIElement(GameObject UIElement)
    {
        UIElement.SetActive(true);
    }

    public void UnshowUIElement(GameObject UIElement)
    {
        UIElement.SetActive(false);
    }

    #endregion

    public void ShowConnectedPeerInfo()
    {

    }

    public void SetupPeerServerAndClient(Text myListeningPort)
    {
        Node.AddComponent<Server>().serverPort = int.Parse(myListeningPort.text);
        Node.AddComponent<Client>();
        _networkServerService = Node.GetComponent<Server>();
        _networkClientService = Node.GetComponent<Client>();
    }

    public void SetupNickName(Text MyNickName)
    {
        nickName = MyNickName.text;
    }

    public void ConnectToPeer(Text peertToConnectPort)
    {
        _networkClientService.ConnectToLocalhostNode(int.Parse(peertToConnectPort.text));
    }

    public void ConnectToPeerWithNickName(Text peertToConnectPort)
    {
        _networkClientService.ConnectToLocalhostNode(int.Parse(peertToConnectPort.text), nickName);
    }

    void AddConnectionToConnectionsUI()
    {

    }

    public void ShowConnectionEventUI()
    {
        ConnectionUI.SetActive(true);
        _networkServerService.ConnectEvent -= ShowConnectionEventUI;
    }

    public void AcceptConnectionRequest()
    {
        //Add node to list and other things ...
        //_networkService.SendMessageToLocalhostNode("test", 1);
    }

    public void SendDebugMessage(Text message)
    {
        NetworkMessageBase networkMessage = new NetworkMessageBase(
            NetworkMessageType.ConnectionInfo, new ConnectionInfoLocalHost
            {
                ConnectionID = 1,
                LocalhostPort = 123,
                NickName = "Tester"
            }
        );

        _networkClientService.SendMessageToLocalhostNode(networkMessage, 1);
    }

    public void DeclineConnectionRequest(int connectionID)
    {
        //_networkService.LocalHostDisconnect(connectionID);
    }

    public void EnableGameUI()
    {
        GameUI.SetActive(true);
    }

    public void StartGame()
    {

    }
}
