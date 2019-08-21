using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockchainInstancer : MonoBehaviour
{
    public static Blockchain bc = new Blockchain();//Singleton of the blockchain

    public int LastChainSize;
    public int CurrentChainSize;

    void Start()
    {
        LastChainSize = bc.Chain.Count;
    }

    void Update()
    {
        CurrentChainSize = bc.Chain.Count;

        if (CurrentChainSize != LastChainSize)
        {
            LastChainSize = CurrentChainSize;
            Debug.Log("CHAIN SIZE CHANGED FROM " + LastChainSize + " TO " + CurrentChainSize);
        }
    }

    void CreateTestTRansactionsAndBlocks()
    {
        bc.AddBlock(new Block(DateTime.Now, bc.GetLastBlock().BlockHash, "PA / PB / valor - 100"));        
        bc.AddBlock(new Block(DateTime.Now, bc.GetLastBlock().BlockHash, "PC / PD / valor - 50"));        
        bc.AddBlock(new Block(DateTime.Now, bc.GetLastBlock().BlockHash, "PE / PF / valor - 150"));

        bc.CreateTransaction(new Transaction("PA", "PB", 100));
        bc.CreateTransaction(new Transaction("PC", "PD", 200));
        bc.CreateTransaction(new Transaction("PE", "PF", 300));
        bc.ProcessTransactionPool("PE");
    }
}
