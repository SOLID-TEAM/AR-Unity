using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Color sceneColor;
    int currentBlockNum = 0;
    int currentLevelNum = 0;
    GameObject currentLevel;
    public Transform origin;
    public Gradient gradient;
    public GameObject destroyParticle;
    public int initialLevel = 1;
    public GameObject[] levels;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.EnableKeyword("_EMISSION");
        sceneColor = Color.red;
        currentLevelNum = initialLevel;
        LoadLevel(currentLevelNum);
    }

    // Update is called once per frame
    float acc = 0.0f;
    void Update()
    {
        acc += Time.deltaTime* 1.0f/6f;
        sceneColor = gradient.Evaluate(Mathf.PingPong(acc, 1.0f)) * 2.6f;
        meshRenderer.material.SetColor("_EmissionColor", sceneColor);
        if (currentLevel == null) return;
    }

    public void LoadLevel(int i)
    {
        if (i > levels.Length || i <= 0) return;
        currentLevelNum = i - 1;
        currentLevel = Instantiate(levels[currentLevelNum], origin);
        Block[] blocks = currentLevel.GetComponentsInChildren<Block>();
        currentBlockNum = 0;

        foreach ( Block block in blocks) {
            if (!block.indestructible)
                ++currentBlockNum;
        }
    }

    public void UnLoadLevel()
    {
        if (currentLevel == null) return;
        Destroy(currentLevel);
        currentLevel = null;
        currentBlockNum = 0;
    }
}
