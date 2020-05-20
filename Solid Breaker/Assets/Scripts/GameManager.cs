using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [HideInInspector]public bool targetDetected = false;
    [Header("Audio SFX")]
    [SerializeField] private AudioSource m_audio;
    public AudioClip new_round_clip;
    public AudioClip end_round_clip;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        blocksManager = FindObjectOfType<BlocksManager>();
        scoreMarker = FindObjectOfType<ScoreMarker>();
        currentRoundNum = initRound;
        StartRound();

        m_audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        
    }

    public void StartRound()
    {
        if (targetDetected == false)
        {
            Invoke("StartRound", 0.4f);
            return;
        }

        playerController.ResetPlayer();
        blocksManager.LoadRound(currentRoundNum);
        InvokeRepeating("CheckRoundState", 0f ,0.5f);

        // play new round sfx
        if (m_audio)
            m_audio.PlayOneShot(new_round_clip);
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

        // play new round sfx
        if (m_audio)
            m_audio.PlayOneShot(end_round_clip);
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

        PowerUp powerUp = FindObjectOfType<PowerUp>();
        if (powerUp != null) Destroy(powerUp.gameObject);

        if (lifes == 0)
        {
            EndGame(false);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void BeginDetection()
    {
        targetDetected = true;
    }

    public void LostDetection()
    {
        targetDetected = false;
    }
}
