using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    CreatingTable = 0,
    WaitingForData = 1,
    PlayerTurn = 2,
    OpponentTurn = 3,
}

public class DominoAdm : MonoBehaviour
{
    public static GameState gameState;

    public Transform TabuleiroDoGame;
    public Transform BaralhoParaComprar;
    public Transform PlayerBaralho;
    public Transform OponenteBaralho;
    public Transform PecaInicialTransform;

    public Transform PecaEsquerda;
    public Transform PecaDireita;

    public GameObject PecaPrefab;

    public List<GameObject> Baralho;

    public int ExtremidadeEsquerda;
    public int ExtremidadeDireita;

    List<Peca> pecasGeradas = new List<Peca>();
    GamePecas pecas = new GamePecas();

    //public Peca[] pecas = new Peca[28];


    // Start is called before the first frame update
    void Start() //Needs to detect that i am the host or player
    {
        gameState = GameState.CreatingTable;

        GerarBaralhoDePecas();

        /*foreach (Transform child in TabuleiroDoGame)
        {
            Baralho.Add(child.gameObject);
        }*/

        DistribuirPecas();

        SortearPecaInicial();
    }

    void DistribuirPecas()
    {
        for (int i = 0; i < 14; i++)
        {
            int choosed = UnityEngine.Random.Range(0, Baralho.Count);

            if (i < 7)//Setando baralho do player 01
            {
                Baralho[choosed].transform.SetParent(PlayerBaralho);
                Baralho[choosed].transform.localPosition = new Vector3(-150 + (i * 50), PlayerBaralho.transform.position.y, 0);
                // print($"Player01 |{Baralho[choosed].GetComponent<Peca>().ValorA}:{Baralho[choosed].GetComponent<Peca>().ValorB}|");
            }
            else//Setando baralho do player 02
            {
                Baralho[choosed].transform.SetParent(OponenteBaralho);
                Baralho[choosed].GetComponentInChildren<Button>().enabled = false;
                Baralho[choosed].transform.localPosition = new Vector3(-150 + ((i - 7) * 50), OponenteBaralho.transform.position.y, 0);
                //print($"Player02 |{Baralho[choosed].GetComponent<Peca>().ValorA}:{Baralho[choosed].GetComponent<Peca>().ValorB}|");
            }

            Baralho.RemoveAt(choosed);
        }
    }

    void SortearPecaInicial()
    {
        int choosed = UnityEngine.Random.Range(0, Baralho.Count);
        ExtremidadeEsquerda = Baralho[choosed].GetComponent<Peca>().ValorA;
        ExtremidadeDireita = Baralho[choosed].GetComponent<Peca>().ValorB;
        Baralho[choosed].transform.SetParent(PecaInicialTransform);
        Baralho[choosed].transform.localPosition = new Vector3(0, 0, 0);
        Baralho[choosed].GetComponentInChildren<Button>().enabled = false;

        gameState = GameState.PlayerTurn;

        //PecaEsquerda.position = new Vector3(-70,0,0);
        //PecaDireita.position = new Vector3(70,0,0);
    }

    public void ValidarJogada(Peca p)//int ValorA, int ValorB)
    {
        if (gameState != GameState.PlayerTurn)
        {
            print("Not your turn! Wait to play.");
        }
        else
        {
            if (p.transform.parent == BaralhoParaComprar)
            {
                p.transform.SetParent(PlayerBaralho,false);
            }
            else
            {
                if (p.ValorA == ExtremidadeEsquerda)
                {
                    JogarPeca(p, PecaEsquerda);
                    PecaEsquerda.localPosition = new Vector3(PecaEsquerda.transform.localPosition.x - p.GetComponent<RectTransform>().sizeDelta.y, 0, 0);
                    p.gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);

                    ExtremidadeEsquerda = p.ValorB;

                    gameState = GameState.OpponentTurn;
                }
                else if (p.ValorB == ExtremidadeEsquerda)
                {
                    JogarPeca(p, PecaEsquerda);
                    PecaEsquerda.localPosition = new Vector3(PecaEsquerda.transform.localPosition.x - p.GetComponent<RectTransform>().sizeDelta.y, 0, 0);
                    p.gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);

                    ExtremidadeEsquerda = p.ValorA;

                    gameState = GameState.OpponentTurn;
                }
                else if (p.ValorA == ExtremidadeDireita)
                {
                    JogarPeca(p, PecaDireita);
                    PecaDireita.localPosition = new Vector3(PecaDireita.transform.localPosition.x + p.GetComponent<RectTransform>().sizeDelta.y, 0, 0);
                    p.gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);

                    ExtremidadeDireita = p.ValorB;

                    gameState = GameState.OpponentTurn;
                }
                else if (p.ValorB == ExtremidadeDireita)
                {
                    JogarPeca(p, PecaDireita);
                    PecaDireita.localPosition = new Vector3(PecaDireita.transform.localPosition.x + p.GetComponent<RectTransform>().sizeDelta.y, 0, 0);
                    p.gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);

                    ExtremidadeDireita = p.ValorA;

                    gameState = GameState.OpponentTurn;
                }
                else
                {
                    Debug.Log("Jogada invalida");
                }
            }
        }
    }

    void JogarPeca(Peca p, Transform pecaPosition)
    {
        /*p.transform.parent = pecaPosition;
        p.transform.localPosition = Vector3.zero;
        p.transform.parent = TabuleiroDoGame;
        p.GetComponentInChildren<Button>().enabled = false;*/
        p.transform.SetParent(pecaPosition);
        p.transform.localPosition = Vector3.zero;
        p.transform.SetParent(TabuleiroDoGame);
        p.GetComponentInChildren<Button>().enabled = false;
    }


    #region network Methods

    private void GerarBaralhoDePecas()
    {
        for (int i = 0; i <= 27; i++)
        {
            Peca p = new Peca();

            GameObject g = Instantiate(PecaPrefab, BaralhoParaComprar);


            if (i <= 6)
            {
                p.ValorA = 0;
                p.ValorB = i;
            }
            else if (i > 6 && i <= 12)
            {
                p.ValorA = 1;
                p.ValorB = i - 6;
            }
            else if (i > 12 && i <= 17)
            {
                p.ValorA = 2;
                p.ValorB = i - 11;
            }
            else if (i > 17 && i <= 21)
            {
                p.ValorA = 3;
                p.ValorB = i - 15;
            }
            else if (i > 21 && i <= 24)
            {
                p.ValorA = 4;
                p.ValorB = i - 18;
            }
            else if (i > 25 && i <= 26)
            {
                p.ValorA = 5;
                p.ValorB = i - 20;
            }
            else
            {
                p.ValorA = 6;
                p.ValorB = 6;
            }

            g.GetComponent<Peca>().ValorA = p.ValorA;
            g.GetComponent<Peca>().ValorB = p.ValorB;

            Baralho.Add(g);
        }
    }


    public GamePecas GamePecasForNetwork()
    {
        GamePecas result = new GamePecas();

        DistribuirPecasEntreJogadores(result);

        SortearInicial(result);

        return result;
    }

    private void GerarPecas()
    {
        for (int i = 0; i <= 27; i++)
        {
            Peca p = new Peca();

            if (i <= 6)
            {
                p.ValorA = 0;
                p.ValorB = i;
            }
            else if (i > 6 && i <= 12)
            {
                p.ValorA = 1;
                p.ValorB = i - 6;
            }
            else if (i > 12 && i <= 17)
            {
                p.ValorA = 2;
                p.ValorB = i - 11;
            }
            else if (i > 17 && i <= 21)
            {
                p.ValorA = 3;
                p.ValorB = i - 15;
            }
            else if (i > 21 && i <= 24)
            {
                p.ValorA = 4;
                p.ValorB = i - 18;
            }
            else if (i > 25 && i <= 26)
            {
                p.ValorA = 5;
                p.ValorB = i - 20;
            }
            else
            {
                p.ValorA = 6;
                p.ValorB = 6;
            }

            pecasGeradas.Add(p);
        }
    }

    void DistribuirPecasEntreJogadores(GamePecas gp)
    {
        for (int i = 0; i < 14; i++)
        {
            int choosed = UnityEngine.Random.Range(0, pecasGeradas.Count);

            if (i < 7)//Setando baralho do player 01
            {
                gp.playerAPecas[choosed] = pecasGeradas[i];
            }
            else//Setando baralho do player 02
            {
                gp.playerBPecas[choosed] = pecasGeradas[i];
            }

            pecasGeradas.RemoveAt(choosed);
        }
    }

    void SortearInicial(GamePecas gp)
    {
        int choosed = UnityEngine.Random.Range(0, pecasGeradas.Count);
        gp.pecaInicial = pecasGeradas[choosed];
        gp.pecaInicial.GetComponentInChildren<Button>().enabled = false;
    }

     #endregion
}
