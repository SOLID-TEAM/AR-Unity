using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    public Transform origin;
    public Gradient gradient;
    public GameObject destroyParticle;
    public GameObject[] rounds;
    
    int currentBlockNum = 0;
    GameObject currentRound;
    GameManager gameManager;
    MeshRenderer meshRenderer;
    Color sceneColor;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.EnableKeyword("_EMISSION");
        sceneColor = Color.red;
    }

    // Update is called once per frame
    float acc = 0.0f;
    void Update()
    {
        acc += Time.deltaTime* 1.0f/6f;
        sceneColor = gradient.Evaluate(Mathf.PingPong(acc, 1.0f)) * 2.6f;
        meshRenderer.material.SetColor("_EmissionColor", sceneColor);
        if (currentRound == null) return;
    }

    public bool LoadRound(int i)
    {
        if (i > rounds.Length || i <= 0) return false;
        currentRound = Instantiate(rounds[i - 1], origin);
        Block[] blocks = currentRound.GetComponentsInChildren<Block>();
        currentBlockNum = 0;

        foreach ( Block block in blocks) {
            if (!block.indestructible)
                ++currentBlockNum;
        }

        return true;
    }

    public void UnloadRound()
    {
        if (currentRound == null) return;
        Destroy(currentRound);
        currentRound = null;
        currentBlockNum = 0;
    }

    public bool HasBreakableBlocks()
    {
        return currentBlockNum > 0;
    }

    public void BlockDestroyed()
    {
        --currentBlockNum;
    }
}
