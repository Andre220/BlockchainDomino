using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalHostConnectionInfo : MonoBehaviour
{
    //public bool IsConnected;

    public INetworkServer _server;

    public Text UI_ConnectionID;
    public Text UI_LocalhostPort;
    public Text UI_NickName;

    public LocalHostConnectionInfo localHostConnectionInfoModel;

    void Start()
    {
        //SetupUIGameObject();

        if (_server == null)//Needs to be improved
        {
            _server = GameObject.FindGameObjectWithTag("NodeObject").GetComponent<Server>();
        }

        _server.DisconnectEvent += DestroyThisInfo;
    }

    void OnEnable()
    {
        //SetupUIGameObject();
    }

    public void SetupUIGameObject()
    {
        if (localHostConnectionInfoModel != null)
        {
            UI_ConnectionID.text = "CID:" + localHostConnectionInfoModel.ConnectionID.ToString();
            UI_LocalhostPort.text = "Port:" + localHostConnectionInfoModel.LocalhostPort.ToString();
            UI_NickName.text = localHostConnectionInfoModel.NickName;
        }
    }

    void DestroyThisInfo(int DesconnectedconnectionID)
    {
        if (DesconnectedconnectionID == localHostConnectionInfoModel.ConnectionID)
        {
            Destroy(gameObject);
        }
    }
}
