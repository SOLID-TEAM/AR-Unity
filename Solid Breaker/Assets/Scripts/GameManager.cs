using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int initRound = 1;
    int currentRoundNum = 0;
    BlocksManager blocksManager;

    void Start()
    {
        blocksManager = FindObjectOfType<BlocksManager>();
        currentRoundNum = initRound;
        Invoke("StartRound", 2.0f);
    }
    void Update()
    {
        
    }

    public void StartRound()
    {
        blocksManager.LoadRound(currentRoundNum);
        Invoke("CheckRoundState", 0.5f);
    }

    public void EndRound()
    {
        ++currentRoundNum;

        if (currentRoundNum > blocksManager.rounds.Length )
        {
            // End Game
        }
        else
        {
            Invoke("StartRound", 2.0f);
        }

    }

    public void CheckRoundState()
    {
        if (!blocksManager.HasBreakableBlocks())
        {
            EndRound();
        }
        else
        {
            Invoke("CheckRoundState", 0.5f);
        }
    }

    public void ResetRound()
    {

    }

}
