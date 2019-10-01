using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class used to manage Lobby Forms
/// </summary>
/// 
public class LobbyManager : MonoBehaviour
{
    #region public property`s
    [Header("Node Data")]
    public GameObject Node;
    public string MyNickName;

    [Header("Login Form")]
    [Space(50)]
    public GameObject LoginForm;
    public Text UITextPort;
    public Text UITextNickName;

    [Header("Login Form")]
    [Space(50)]
    public GameObject LobbyForm;
    public GameObject ConnectionRequestForm;

    [Header("Prefabs")]
    [Space(30)]
    public GameObject NodeInfoPrefab;
    public GameObject ConnectionRequesrPrefab;

    [Header("Transforms")]
    [Space(30)]
    public Transform NodeInfoFatherTransform;
    public Transform ConnectionRequestsFatherTransform;

    [Header("Others")]
    [Space(30)]
    public Text SceneTitleText;

    #endregion

    #region private property`s

    INetworkManager _networkManager;

    #endregion

    void Start()
    {
        _networkManager = new NetworkManager();

        if (Node == null)
        {
            if (GameObject.Find("Node") == null)
            {
                GameObject node = new GameObject();
                Node.name = "Node";
                Instantiate(Node);

                Node = node;
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
        SceneTitleText.text = newName;
    }

    #endregion

    #region Buttons voids

    public void UIButtonLogin(Text portText) //Called when player click in Login Button
    {
        _networkManager.SetupPeerServerAndClient(portText.text, Node, ReceiveConnectionRequest);
    } 

    public void SetupNickName(Text nickNameText) // Called to setup player nickname
    {
        MyNickName = nickNameText.text;
    }

    public void UIButtonConnectToPeer(Text peertToConnectPort) //Open form to connect to another player
    {
        _networkManager.ConnectToPeer(peertToConnectPort.text);
    }

    //public void UIConnectionAcceptedButton(LocalHostConnectionInfo lhci) //Used in the button that player click the "accept" button in the connection Request Form
    //{
    //    SetupNodeListElement(lhci);
    //}

    #endregion

    #region configuration voids

    private void SetupNickName(string UINickName)
    {
        MyNickName = UINickName;
    }

    private void ReceiveConnectionRequest(LocalHostConnectionInfo connectedNode)
    {
        ConnectionRequestForm.GetComponent<UILocalHostConnectionInfo>().localHostConnectionInfoModel = connectedNode;

        ConnectionRequestForm.SetActive(true);
    }

    private void UIStartGame(UILocalHostConnectionInfo NI) // Called when player click in another node in nodes list UI
    {
        GamePecas GP = GameManager.instance.GamePecasForNetwork();

        //Send game info

        CustomNetworkMessageBase gameStartMessage = new CustomNetworkMessageBase(CustomDataEventsEnum.PlayRequestAccept, GP);
        //CustomNetworkMessageBase gameStartMessage = new CustomNetworkMessageBase(CustomDataEventsEnum.PlayRequestAccept, "Testing the issue");

        //CustomNetworkMessageBase gameStartMessage = new CustomNetworkMessageBase(CustomDataEventsEnum.PlayRequestAccept, gp2);

        _networkManager.SendCustomMessage(gameStartMessage, NI.localHostConnectionInfoModel);

        //StartTheGame

        GameManager.instance.DominoPrint(GP, 0);

        //_networkServerService.PlayRequestAccept += domino.DominoPrint;
    }

    #endregion

    #region UI Setup Voids

    public void UIButtonConnectionRequestAccepted(UILocalHostConnectionInfo lhci)
    {
        GameObject g = Instantiate(NodeInfoPrefab, NodeInfoFatherTransform);

        RectTransform rectTranform = g.GetComponent<RectTransform>();

        rectTranform.anchoredPosition = new Vector2(rectTranform.anchoredPosition.x, rectTranform.sizeDelta.y * NodeInfoFatherTransform.childCount);

        UILocalHostConnectionInfo NI = g.GetComponent<UILocalHostConnectionInfo>();

        NI.localHostConnectionInfoModel = lhci.localHostConnectionInfoModel;

        NI.SetupUIGameObject();

        g.GetComponent<Button>().onClick.AddListener(() => { UIStartGame(NI); });
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

        //_networkClientService.SendMessageToLocalhostNode(networkMessage, 1);
    }

    #endregion
}
