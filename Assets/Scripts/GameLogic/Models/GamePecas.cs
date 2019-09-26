using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamePecas : MonoBehaviour
{
    public Peca[] playerAPecas = new Peca[7];
    public Peca[] playerBPecas = new Peca[7];
    public Peca[] pecasParaComprar = new Peca[13];

    public Peca pecaInicial;
}
