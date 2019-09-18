using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeInfo : MonoBehaviour
{
    public int ConnectionID;
    public int LocalhostPort;
    public string NickName;

    //public bool IsConnected;

    public INetworkServer _server;

    public Text UI_ConnectionID;
    public Text UI_LocalhostPort;
    public Text UI_NickName;

    void Start()
    {
        SetupUIGameObject();

        if (_server == null)
        {
            _server = GameObject.FindGameObjectWithTag("NodeObject").GetComponent<Server>();
        }

        _server.DisconnectEvent += DestroyThisInfo;
    }

    void SetupUIGameObject()
    {
        UI_ConnectionID.text = "CID:" + ConnectionID.ToString();
        UI_LocalhostPort.text = "Port:" + LocalhostPort.ToString();
        UI_NickName.text = NickName;
    }

    void DestroyThisInfo(int connectionID)
    {
        if (connectionID == ConnectionID)
        {
            Destroy(gameObject);
        }
    }
}
