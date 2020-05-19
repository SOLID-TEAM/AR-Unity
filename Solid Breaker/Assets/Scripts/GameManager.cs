using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    PlayerController playerController;
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
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
            EndGame(true);
        }
        else
        {
            Invoke("StartRound", 2.0f);
        }

    }

    public void EndGame(bool isWinner)
    {
        text.SetActive(true);
        text.GetComponent<MeshFilter>().mesh = (isWinner) ? youWinMesh : gameOverMesh;
        Invoke("ReturnToMainMenu", 3.0f);
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
        scoreMarker.AddLife(lifes);
    }
    public void ExtractLife()
    {
        lifes = Mathf.Clamp(--lifes, 0, 5);
        scoreMarker.DestroyLife(lifes);
        playerController.ResetPlayer();

        if (lifes == 0)
        {
            EndGame(false);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
