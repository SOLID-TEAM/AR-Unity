﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int hits = 1;
    public int points = 0;
    public bool indestructible = false;
    private bool iluminated = false;
    private float blend = 0.0f;
    private Color originalColor;
    private MeshRenderer meshRenderer;
    private BlocksManager blocksManager;

    public void BlockHit()
    {
        if (!indestructible && --hits  == 0)
        {
            StopCoroutine("HitIlumination");
            BlockDestroyed();
        }
        else
        {
            iluminated = true;
            StopCoroutine("HitIlumination");
            StartCoroutine("HitIlumination");
        }
    }
    IEnumerator HitIlumination()
    {
        while (!De())
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield break;
    }

    private bool De()
    {
        if (iluminated)
        {
            blend = Mathf.Clamp(blend + Time.deltaTime * 5.0f, 0.0f, 1.0f);
            if (blend == 1.0f) iluminated = false;
        }
        else
        {
            blend = Mathf.Clamp(blend - Time.deltaTime * 5.0f, 0.0f ,1.0f);
            if (blend == 0.0f)
                return true;
        }

        meshRenderer.material.SetColor("_EmissionColor", Color.Lerp(originalColor, new Color(1, 1, 1), blend) * Mathf.Lerp(0.0f, 3.0f, blend));
        return false;
    }

    void BlockDestroyed()
    {
        // Rest block manager
        // Spawn particle
        Destroy(gameObject);
    }

    void Start()
    {
        blocksManager = FindObjectOfType<BlocksManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.EnableKeyword("_EMISSION");
        originalColor = meshRenderer.material.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}