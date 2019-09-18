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
    public string NickName;

    [Header("Prefabs")]
    [Space(30)]
    public GameObject NodeInfoPrefab;
    public Transform NodeInfoFather;


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

    public void FormLogin(Text portText)
    {
        SetupPeerServerAndClient(portText.text);
    }

    public void FormSetupNickName(Text nickNameText)
    {
        SetupNickName(nickNameText.text);
    }

    public void FormConnectToPeer(Text peertToConnectPort) //Open form to connect to another player
    {
        ConnectToPeer(peertToConnectPort.text);
    }

    public void FormConnectionEventAccepted() //Used in the button that player click the "accept" button in the connection Request Form
    {
        //Let's test the custom connection info workflow

        //Add node to connected nodes form
    }

    public void FormSendPlayRequest(NodeInfo NI)
    {
        _networkClientService.SendMessageToLocalhostNode
        (
            new CustomNetworkMessageBase(CustomDataEventsEnum.PlayRequest, null),
            NI.ConnectionID
        );
    }

    public void FormConnectionEventDeclined() // Used in the button that player click the "Decline" button in the connection Request Form
    {
        ConnectionRequestForm.SetActive(false);
    }

    private void FormAddPeerToUIList(LocalHostConnectionInfo lhci)
    {
        GameObject g = Instantiate(NodeInfoPrefab, NodeInfoFather);

        RectTransform rectTranform = g.GetComponent<RectTransform>();

        rectTranform.anchoredPosition = new Vector2(rectTranform.anchoredPosition.x, rectTranform.sizeDelta.y * NodeInfoFather.childCount);

        NodeInfo NI = g.GetComponent<NodeInfo>();

        g.GetComponent<Button>().onClick.AddListener(delegate() { FormSendPlayRequest(NI);});

        NI.ConnectionID = lhci.ConnectionID;
        NI.LocalhostPort = lhci.LocalhostPort;
        NI.NickName = lhci.NickName;
        NI._server = _networkServerService;
    }


    #endregion

    #region configuration voids

    private void SetupPeerServerAndClient(string port)
    {
        Node.AddComponent<Server>().serverPort = int.Parse(port);
        Node.AddComponent<Client>();
        _networkServerService = Node.GetComponent<Server>();
        _networkClientService = Node.GetComponent<Client>();

        _networkServerService.ConnectEvent += ShowFormConnectionRequest;





        //NEED TO BE FIXED BECAUSE THIS IS GAMBIARRA!
        _networkServerService.PlayRequestEvent += ChangeScene;





        //_networkServerService.DisconnectEvent += RemoverPeerFromPeerList;

        //_networkServerService.ConnectEvent += ShowPlayRequestUI;
    }

    private void SetupNickName(string MyNickName)
    {
        NickName = MyNickName;
    }

    private void ConnectToPeer(string peertToConnectPort)
    {
        LocalHostConnectionInfo localHostConnectionInfo = _networkClientService.ConnectToLocalhostNode(int.Parse(peertToConnectPort));

        FormAddPeerToUIList(localHostConnectionInfo);
    }

    private void ShowFormConnectionRequest()
    {
        ConnectionRequestForm.SetActive(true);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    #endregion

    public void ShowConnectionEventUI()
    {
        ConnectionUI.SetActive(true);
        _networkServerService.ConnectEvent -= ShowConnectionEventUI;
    }

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

    /*public void ShowPlayRequestUI()
    {
        GamePlayUI.SetActive(true);
        _networkServerService.ConnectionInfoEvent -= ShowPlayRequestUI;
    }

    public void PlayResquestAccept()
    {
        NetworkMessageBase message = new NetworkMessageBase(NetworkMessageType.PlayRequestResponse, true);
        
        _networkClientService.SendMessageToLocalhostNode(message, 1);

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void PlayResquestDecline()
    {
        NetworkMessageBase message = new NetworkMessageBase(NetworkMessageType.PlayRequestResponse, false);

        _networkClientService.SendMessageToLocalhostNode(message, 1);
    }*/
}
