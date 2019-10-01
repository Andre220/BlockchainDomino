using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    void Start()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    } 

    #region network Methods

    public GamePecas GamePecasForNetwork()
    {
        GamePecas result = new GamePecas();

        GerarPecas();

        DistribuirPecasEntreJogadores(result);

        SortearInicial(result);

        result.pecasParaComprar = pecasGeradas;

        return result;
    }

    private void GerarPecas()
    {
        pecasGeradas.Clear();

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

    private void DistribuirPecasEntreJogadores(GamePecas gp)
    {
        for (int i = 0; i < 14; i++)
        {
            int choosed = UnityEngine.Random.Range(0, pecasGeradas.Count);

            if (i < 7)//Setando baralho do player 01
            {
                gp.playerAPecas.Add(pecasGeradas[choosed]);
            }
            else//Setando baralho do player 02
            {
                gp.playerBPecas.Add(pecasGeradas[choosed]);
            }

            pecasGeradas.RemoveAt(choosed);
        }
    }

    void SortearInicial(GamePecas gp)
    {
        int choosed = UnityEngine.Random.Range(0, pecasGeradas.Count);
        gp.pecaInicial = pecasGeradas[choosed];
        //gp.pecaInicial.GetComponentInChildren<Button>().enabled = false;
    }

    void EmbaralharPecasParaComprar()
    {
        Debug.LogError("Embaralhar pecas n'ao implementado!");
    }

    public void DominoPrint(GamePecas GamePecas, int playerID)
    {
        if (playerID == 0)
        {
            foreach (Peca p in GamePecas.playerAPecas)
            {
                print($"* Player {playerID} pecas: A = {p.ValorA} | B = {p.ValorB}");
            }

            foreach (Peca p in GamePecas.pecasParaComprar)
            {
                print($"*** Peca para compra: A = {p.ValorA} | B = {p.ValorB}");
            }

            print($"Peca inicial: A = {GamePecas.pecaInicial.ValorA} | {GamePecas.pecaInicial.ValorB}");
        }
        else
        {
            foreach (Peca p in GamePecas.playerBPecas)
            {
                print($"* Player {playerID} pecas: A = {p.ValorA} | B = {p.ValorB}");
            }

            foreach (Peca p in GamePecas.pecasParaComprar)
            {
                print($"*** Peca para compra: A = {p.ValorA} | B = {p.ValorB}");
            }

            print($"Peca inicial: A = {GamePecas.pecaInicial.ValorA} | {GamePecas.pecaInicial.ValorB}");
        }
    }

    #endregion

    public void ValidarJogada(Peca p)
    {
        Debug.LogError("GameManager.ValidarJogada not implemented");

        //if (gameState != GameState.PlayerTurn)
        //{
        //    print("Not your turn! Wait to play.");
        //}
        //else
        //{
        //    if (p.transform.parent == BaralhoParaComprar)
        //    {
        //        p.transform.SetParent(PlayerBaralho, false);
        //    }
        //    else
        //    {
        //        if (p.ValorA == ExtremidadeEsquerda)
        //        {
        //            JogarPeca(p, PecaEsquerda);
        //            PecaEsquerda.localPosition = new Vector3(PecaEsquerda.transform.localPosition.x - p.GetComponent<RectTransform>().sizeDelta.y, 0, 0);
        //            p.gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);

        //            ExtremidadeEsquerda = p.ValorB;

        //            gameState = GameState.OpponentTurn;
        //        }
        //        else if (p.ValorB == ExtremidadeEsquerda)
        //        {
        //            JogarPeca(p, PecaEsquerda);
        //            PecaEsquerda.localPosition = new Vector3(PecaEsquerda.transform.localPosition.x - p.GetComponent<RectTransform>().sizeDelta.y, 0, 0);
        //            p.gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);

        //            ExtremidadeEsquerda = p.ValorA;

        //            gameState = GameState.OpponentTurn;
        //        }
        //        else if (p.ValorA == ExtremidadeDireita)
        //        {
        //            JogarPeca(p, PecaDireita);
        //            PecaDireita.localPosition = new Vector3(PecaDireita.transform.localPosition.x + p.GetComponent<RectTransform>().sizeDelta.y, 0, 0);
        //            p.gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);

        //            ExtremidadeDireita = p.ValorB;

        //            gameState = GameState.OpponentTurn;
        //        }
        //        else if (p.ValorB == ExtremidadeDireita)
        //        {
        //            JogarPeca(p, PecaDireita);
        //            PecaDireita.localPosition = new Vector3(PecaDireita.transform.localPosition.x + p.GetComponent<RectTransform>().sizeDelta.y, 0, 0);
        //            p.gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);

        //            ExtremidadeDireita = p.ValorA;

        //            gameState = GameState.OpponentTurn;
        //        }
        //        else
        //        {
        //            Debug.Log("Jogada invalida");
        //        }
        //    }
        //}
    }

    void JogarPeca(Peca p, Transform pecaPosition)
    {
        /*p.transform.parent = pecaPosition;
        p.transform.localPosition = Vector3.zero;
        p.transform.parent = TabuleiroDoGame;
        p.GetComponentInChildren<Button>().enabled = false;*/

        Debug.LogError("GameManager.JogarPeca not implemented");

        ////p.transform.SetParent(pecaPosition);
        ////p.transform.localPosition = Vector3.zero;
        ////p.transform.SetParent(TabuleiroDoGame);
        ////p.GetComponentInChildren<Button>().enabled = false;
    }
}
