using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// This shit is old
/// </summary>
namespace Assets.Scripts.Services
{
    public class GameAdm : MonoBehaviour
    {
        public static GameAdm instance = null;


        [HideInInspector]
        public GameObject Node;//Hold the gameobject that network scripts (server and client) will be add.


        [HideInInspector]
        public Text ConnectionInfo;

        [Space(10)]

        [Header("UI Variables")]
        [Header("Network Variables")]
        public Text ServerFormPortInput;
        public Text ClientFormPortInput;
        [Space(10)]

        [Header("UI Group Variables")]
        public GameObject ServerForm;
        public GameObject ClientForm;
        public GameObject LobbyForm;
        public GameObject Description;
        public GameObject PlayResquestForm;

        public Text PlayerRequestPort;

        [Header("Management Variables")]
        public List<GameObject> DontDestroyGameObjects;

        void Start()
        {
            if (Node == null)
                Node = GameObject.Find("Node");

            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);

            DontDestroyGameObjects.Add(gameObject);

            foreach (GameObject g in DontDestroyGameObjects)
            {
                DontDestroyOnLoad(g);
            }
        }

        public void CreateServer()
        {
            /*Node.AddComponent<Server>().ListeningPort = int.Parse(ServerFormPortInput.text);
            Node.AddComponent<Client>().clientPort = int.Parse(ServerFormPortInput.text);*/
           // Node.AddComponent<NetworkPeer>().ListeningPort = int.Parse(ServerFormPortInput.text);
            ServerForm.SetActive(false);
            ClientForm.SetActive(true);
        }

        public void CreateClient()
        {
            ClientForm.SetActive(false);
            LobbyForm.SetActive(true);
        }

        public void ShowGameDescription()
        {
            if (Description.activeSelf)
            {
                ServerForm.SetActive(true);
                Description.SetActive(false);
            }
            else
            {
                ServerForm.SetActive(false);
                Description.SetActive(true);
            }
        }

        public void PlayRequest()
        {
            ServerForm.SetActive(false);
            ClientForm.SetActive(false);
            LobbyForm.SetActive(false);
            PlayResquestForm.SetActive(true);
        }

        public void CreateConnection()
        {
            //NetworkPeer.instance.LocalHostConnect(int.Parse(ClientFormPortInput.text));
        }

        public void ChangeScene(int index)
        {
            SceneManager.LoadScene(index, LoadSceneMode.Single);
        }
    }
}
