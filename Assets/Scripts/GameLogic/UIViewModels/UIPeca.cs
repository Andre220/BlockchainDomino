using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPeca : MonoBehaviour
{


    [Header("Peca values on UI")]
    public Text ValorA;
    public Text ValorB;

    public Peca pecaModel;

    // Start is called before the first frame update
    void Start()
    {
        // if peca model not null (?) then insert his value into this UI Text
        ValorA.text = pecaModel?.ValorA.ToString();
        ValorA.text = pecaModel?.ValorB.ToString();
    }

    // Update is called once per frame
    public void OnClick()
    {
        GameManager.instance.ValidarJogada(pecaModel);
    }
}
