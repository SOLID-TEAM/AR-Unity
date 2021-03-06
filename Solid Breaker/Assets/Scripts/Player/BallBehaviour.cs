﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class BallBehaviour : MonoBehaviour
{

    public float radius = 0.5f;
    public Vector3 movement = Vector3.zero;
    public Vector3 direction = Vector3.zero;
    public float speed = 5.0f;
    //[SerializeField] private float ray_length = 0.5f;
    [SerializeField] private float defaultSpeed = 13.0f;
    [SerializeField] private float powerupSpeed = 7.0f;
    BlocksManager blocksManager;
    GameManager gameManager;
    MeshRenderer meshRenderer;
    TrailRenderer trailRenderer;
    [Header("Audio SFX")]
    [SerializeField] private AudioSource m_audio;
    public AudioClip bounce_clip;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        blocksManager = FindObjectOfType<BlocksManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.EnableKeyword("_EMISSION");
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.material.EnableKeyword("_EMISSION");
        speed = defaultSpeed;

        m_audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameManager.targetDetected)
        {
            meshRenderer.material.SetColor("_EmissionColor", blocksManager.sceneColor); ;
            trailRenderer.material.SetColor("_EmissionColor", blocksManager.sceneColor);
            HandleCollision();
        }
    }

    void HandleCollision()
    {
        // first shoot little ray
        int reflectLayer = 1 << LayerMask.NameToLayer("Reflectable");
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, movement.normalized, out hit, speed * Time.deltaTime, reflectLayer))
        {
            Vector3 dNormal = hit.normal;
            dNormal.y = 0.0f;
            movement = Vector3.Reflect(movement, dNormal.normalized).normalized;
            movement.y = 0.0f;

            if (hit.collider.gameObject.CompareTag("Block"))
            {
                Block block = hit.collider.gameObject.GetComponent<Block>();
                block.BlockHit();
            }
            else if (hit.collider.gameObject.CompareTag("Killer"))
            {
                // TODO: Spawn dead particle
                // Extract player life
                if (FindObjectsOfType<BallBehaviour>().Length == 1)
                {
                    gameManager.ExtractLife();
                }
                // Reset player pos

                // play fire burned ball clip
                Instantiate(Resources.Load("BallDestroyedClip"), transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.CompareTag("Wall"))
            {
                // Spawn hit wall particle in hit point
            }

            // play bounce sfx
            if (m_audio)
                m_audio.PlayOneShot(bounce_clip);
        }
        else // TODO: if we ignore the collision using compare tag and this workaround, never moves until collisions is finished
        {
            transform.position += movement.normalized * speed * Time.deltaTime;
        }
    }

    public void SwitchPowerupSpeed(bool activate_powerup)
    {
        speed = activate_powerup ? powerupSpeed : defaultSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius * 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + movement.normalized * speed * Time.deltaTime);
    }
}

