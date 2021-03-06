﻿using System.Collections;
using System.Collections.Generic;
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
    private GameManager gameManager;
    [Header("Audio SFX")]
    [SerializeField] private AudioSource m_audio;
    public AudioClip[] hit_clips;

    public void BlockHit()
    {
        if (!indestructible && --hits  == 0)
        {
            StopCoroutine("HitIlumination");
            BlockDestroyed();
            // instante sound explosion prefab
            Instantiate(Resources.Load("BlockAudioClipExplosion"), transform.parent);
        }
        else
        {
            iluminated = true;
            StopCoroutine("HitIlumination");
            StartCoroutine("HitIlumination");

            // play bounce/hit sound
            if(m_audio)
                m_audio.PlayOneShot(hit_clips[Random.Range(0, hit_clips.Length)]);
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
        // Manager ------------------
        blocksManager.BlockDestroyed();
        gameManager.score += points;

        // Random PowerUp
        if (Random.Range(0, 5) == 0 && FindObjectOfType<PowerUp>() == null)
        {
            int randomType = (int)Random.Range((int)PowerUpType.None + 1, (int)PowerUpType.Max);
            Object powerUp = Resources.Load(System.Enum.GetName(typeof(PowerUpType), (PowerUpType)randomType));
            Instantiate(powerUp, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.Euler(0.0f, 0.0f, 90.0f) );
        }

        // Destroy Particle -----------------------
        GameObject particle = Instantiate(blocksManager.destroyParticle, transform.position, blocksManager.destroyParticle.transform.rotation);
        ParticleSystem ps = particle.GetComponent<ParticleSystem>();
        ParticleSystemRenderer psRender = particle.GetComponent<ParticleSystemRenderer>();
        psRender.trailMaterial = meshRenderer.material;
        ps.Play();
        Destroy(particle, 3.1f);
        Destroy(gameObject);
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        blocksManager = FindObjectOfType<BlocksManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.EnableKeyword("_EMISSION");
        originalColor = meshRenderer.material.GetColor("_EmissionColor");

        m_audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
