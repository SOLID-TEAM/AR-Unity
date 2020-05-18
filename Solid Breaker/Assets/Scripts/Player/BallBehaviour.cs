﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{

    public float radius = 0.5f;
    public Vector3 movement = Vector3.zero;
    public Vector3 direction = Vector3.zero;
    public float speed = 5.0f;
    //[SerializeField] private float ray_length = 0.5f;
    [SerializeField] private float defaultSpeed = 13.0f;
    [SerializeField] private float powerupSpeed = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
        movement = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, 1.0f);
        speed = defaultSpeed;
    }

    void Update()
    {
        HandleCollision();
        transform.position += movement.normalized * speed * Time.deltaTime;
    }

    void HandleCollision()
    {
        // first shoot little ray
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, movement.normalized, out hit, speed * Time.deltaTime))
        {
            if (!hit.collider.gameObject.CompareTag("PowerUp") || !hit.collider.gameObject.CompareTag("Ball"))
            {
                //Debug.Log("Ball hit with: " + hit.collider.gameObject.name);
                Vector3 dNormal = hit.normal;
                dNormal.y = 0.0f;
                movement = Vector3.Reflect(movement, dNormal.normalized).normalized;
                movement.y = 0.0f;
            }
        }
        //else // TODO: if we ignore the collision using compare tag and this workaround, never moves until collisions is finished
        //{
        //    transform.position += movement.normalized * speed * Time.deltaTime;
        //}
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

