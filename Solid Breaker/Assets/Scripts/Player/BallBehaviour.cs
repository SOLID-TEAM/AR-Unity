using System.Collections;
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
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        blocksManager = FindObjectOfType<BlocksManager>();
        speed = defaultSpeed;
    }

    void Update()
    {
        //if(!started_round)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //        movement = new Vector3(1.0f, 0.0f, 1.0f);
        //}

        HandleCollision();
        //transform.position += movement.normalized * speed * Time.deltaTime;
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
                // Spawn dead particle
                // Extract player life
                // Reset player pos
                if (FindObjectsOfType<BallBehaviour>().Length == 1)
                {
                    gameManager.ExtractLife();
                }

                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.CompareTag("Wall"))
            {
                // Spawn hit wall particle in hit point
            }
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

