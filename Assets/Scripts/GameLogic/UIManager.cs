using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Most part of this class hold functions to be called when some button is clicked in interface
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Forms Area")]
    //LoginForm
    public GameObject LoginForm;
    public Text UITextPort;
    public Text UITextNickName;

    public GameObject LobbyForm;

    public GameObject ConnectionRequestForm;

    [Header("Node Configuration")]
    [Space(30)]
    public GameObject Node;
    public string MyNickName;

    [Header("Prefabs")]
    [Space(30)]
    public GameObject NodeInfoPrefab;
    public Transform NodeInfoFather;

    public GameObject ConnectionRequesrPrefab;
    public Transform ConnectionRequestsFather;

    [Header("Others")]
    [Space(30)]
    public Text TitleText;

    public Transform OnlineNodesFather;
    public GameObject PeerInfoPrefab;

    public GameObject ConnectionUI;
    public GameObject LocalHostPlayerConnectionRequest;

    public GameObject GamePlayUI;

    public GameObject GameUI;

    INetworkClient _networkClientService;
    INetworkServer _networkServerService;

    DominoAdm domino = new DominoAdm();


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
    }

    #region basic UI Function to enable and disable UI Objects

    public void HelperEnableUIElement(GameObject UIElement)
    {
        UIElement.SetActive(true);
    }

    public void HelperDisableUIElement(GameObject UIElement)
    {
        UIElement.SetActive(false);
    }

    public void HelperSceneTitleChange(string newName)
    {
        TitleText.text = newName;
    }

    #endregion

    #region Form voids

    //Function used in Unity OnClick Events, exposed to be used in editor

    public void UILoginForm(Text portText) //Called when player click in Login Button
    {
        SetupPeerServerAndClient(portText.text);
    } 

    public void UISetupNickName(Text nickNameText) // Called to setup player nickname
    {
        SetupNickName(nickNameText.text);
    }  

    public void UIConnectToPeer(Text peertToConnectPort) //Open form to connect to another player
    {
        ConnectToPeer(peertToConnectPort.text);
    }

    public void UIConnectionAccepted(LocalHostConnectionInfo lhci) //Used in the button that player click the "accept" button in the connection Request Form
    {
        SetupNodeListElement(lhci);
    }

    public void UIConnectionDeclined() // Used in the button that player click the "Decline" button in the connection Request Form
    {
        ConnectionRequestForm.SetActive(false);
    }

    //public void UISendPlayRequest(NodeInfo NI)
    //{
    //    _networkClientService.SendMessageToLocalhostNode
    //    (
    //        new CustomNetworkMessageBase(CustomDataEventsEnum.PlayRequest, null),
    //        NI.ConnectionID
    //    );
    //}

    public void UIStartGame(NodeInfo NI) // Called when player click in another node in nodes list UI
    {
        //Create game info as P1 pieces and p2 pieces, pieces to buy and start piece

        GamePecas GP = domino.GamePecasForNetwork();

        //Send game info

        CustomNetworkMessageBase gameStartMessage = new CustomNetworkMessageBase(CustomDataEventsEnum.PlayRequestAccept, GP);

        //Change scene??

    }

    #endregion

    #region configuration voids

    private void SetupPeerServerAndClient(string port)
    {
        Node.AddComponent<Server>().serverPort = int.Parse(port);
        Node.AddComponent<Client>();

        _networkServerService = Node.GetComponent<Server>();
        _networkClientService = Node.GetComponent<Client>();

        _networkServerService.ConnectEvent += ReceiveConnectionRequest;
    }

    private void SetupNickName(string UINickName)
    {
        MyNickName = UINickName;
    }

    private void ConnectToPeer(string peertToConnectPort)
    {
        LocalHostConnectionInfo localHostConnectionInfo = _networkClientService.ConnectToLocalHostNode(int.Parse(peertToConnectPort));
    }

    private void ReceiveConnectionRequest(LocalHostConnectionInfo connectedNode)
    {
        LocalHostConnectionInfo lhci = ConnectionRequestForm.GetComponent<LocalHostConnectionInfo>();
        lhci.ConnectionID = connectedNode.ConnectionID;
        lhci.LocalhostPort = connectedNode.LocalhostPort;
        lhci.NickName = connectedNode.NickName;

        ConnectionRequestForm.SetActive(true);
    }

    private void ChangeScene(int id)
    {
        SceneManager.LoadScene(id, LoadSceneMode.Additive);
    }

    #endregion

    #region UI Setup Voids

    private void SetupNodeListElement(LocalHostConnectionInfo lhci)
    {
        GameObject g = Instantiate(NodeInfoPrefab, NodeInfoFather);

        RectTransform rectTranform = g.GetComponent<RectTransform>();

        rectTranform.anchoredPosition = new Vector2(rectTranform.anchoredPosition.x, rectTranform.sizeDelta.y * NodeInfoFather.childCount);

        NodeInfo NI = g.GetComponent<NodeInfo>();

        g.GetComponent<Button>().onClick.AddListener(() => { UIStartGame(NI); });

        NI.ConnectionID = lhci.ConnectionID;
        NI.LocalhostPort = lhci.LocalhostPort;
        NI.NickName = lhci.NickName;
        NI._server = _networkServerService;
    }

    #endregion

    public void SendDebugMessage(Text message)
    {
        CustomNetworkMessageBase networkMessage = new CustomNetworkMessageBase(
            CustomDataEventsEnum.ConnectionInfoRequest, new LocalHostConnectionInfo
            {
                ConnectionID = -1,
                LocalhostPort = 00000,
                NickName = "Tester"
            }
        );

        _networkClientService.SendMessageToLocalhostNode(networkMessage, 1);
    }
}
