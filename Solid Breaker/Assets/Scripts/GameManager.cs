using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject text;
    public Mesh gameOverMesh;
    public Mesh youWinMesh;
    public int lifes = 5;
    public int score = 0;
    public int initRound = 1;
    int currentRoundNum = 0;
    BlocksManager blocksManager;
    ScoreMarker scoreMarker;
    void Start()
    {
        blocksManager = FindObjectOfType<BlocksManager>();
        scoreMarker = FindObjectOfType<ScoreMarker>();
        currentRoundNum = initRound;
        Invoke("StartRound", 2.0f);
    }
    void Update()
    {
        
    }

    public void StartRound()
    {
        blocksManager.LoadRound(currentRoundNum);
        InvokeRepeating("CheckRoundState", 0f ,0.5f);
    }

    public void EndRound()
    {
        ++currentRoundNum;

        if (currentRoundNum > blocksManager.rounds.Length )
        {
            // End Game Win
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
            CancelInvoke("CheckRoundState");
            EndRound();
        }
    }

    public void ResetRound()
    {

    }

    public void AddLife()
    {
        lifes = Mathf.Clamp(++lifes, 0, 5);
    }
    public void ExtractLife()
    {
        lifes = Mathf.Clamp(--lifes, 0, 5);
        scoreMarker.DestroyLife(lifes);

        if (lifes == 0)
        {
            // End Game Lose
        }
    }
}
