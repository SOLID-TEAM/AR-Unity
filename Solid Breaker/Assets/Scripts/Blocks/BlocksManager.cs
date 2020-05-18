using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    int currentBlockNum = 0;
    int currentLevelNum = 0;
    GameObject currentLevel;
    public Transform origin;
    public GameObject destroyParticle;
    public int initialLevel = 1;
    public GameObject[] levels;
    void Start()
    {
        currentLevelNum = initialLevel;
        LoadLevel(currentLevelNum);
    }

    // Update is called once per frame
    void Update()
    {
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
            if (!block.gameObject.isStatic)
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
