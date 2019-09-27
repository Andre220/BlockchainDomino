using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ._______.
/// |   A   |
/// |_______|
/// |   B   |
/// |_______|
/// </summary>

public class Peca : MonoBehaviour
{
    public DominoAdm dominoAdm;
    
    [Header("Model Variables")]
    public int ValorA;
    public int ValorB;

    [Header("Model Variables")]
    public Text UI_ValorA;
    public Text UI_ValorB;

    void Start()
    {
        dominoAdm = GameObject.FindObjectOfType<DominoAdm>();
        SetupUIGameObject();
    }

    void SetupUIGameObject()
    {
        UI_ValorA.text = ValorA.ToString();
        UI_ValorB.text = ValorB.ToString();
    }

    public void OnClick(Peca p)
    {
        dominoAdm.ValidarJogada(p);
    }
}
