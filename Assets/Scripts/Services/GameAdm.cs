﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Services
{
    public class GameAdm : MonoBehaviour
    {
        public static GameAdm instance = null;
        public List<GameObject> DontDestroyGameObjects;

        public Text NameInput;
        public Text PortInput;

        void Start()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            foreach (GameObject g in DontDestroyGameObjects)
            {
                DontDestroyOnLoad(g);
            }
        }

        public void Login()//pass port and name to client and server scripts, them change to next scene
        {
            //Server.instance.port = int.Parse(PortInput.text); //Server - port that i use to listen
            //Client.instance.port = int.Parse(PortInput.text); //Client - port that i want to send something, so dont define it now

            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }

        public void ChangeScene(string name)
        {
            
        }
    }
}