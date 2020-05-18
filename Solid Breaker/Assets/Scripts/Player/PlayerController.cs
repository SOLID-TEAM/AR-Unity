using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Core vars")]
    public float max_pos_limits = 5.0f;
    [Header("Gameplay vars")]
    public float vaus_speed = 9.0f;
    [SerializeField] PowerUpType active_powerup;
    [SerializeField]private PowerUpType last_powerup;
    public int lifes = 5;
    

    // Start is called before the first frame update
    void Start()
    {
        active_powerup = PowerUpType.none;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandlePowerUpActive();

        HandleGodKeys();
    }

    void HandleGodKeys()
    {
        for(KeyCode i = KeyCode.Alpha0, n = 0; i < KeyCode.Alpha9; ++i, ++n)
        {
            if (Input.GetKeyDown(i))
            {
                Instantiate(Resources.Load(System.Enum.GetName(typeof(PowerUpType), n)), new Vector3(0, 0.5f, 0), Quaternion.Euler(0.0f, 0.0f, 90.0f));
            }
        }
    }

    void HandleMovement()
    {
        Vector3 new_pos = transform.position;

        if (Input.GetKey(KeyCode.A))
        {
            new_pos.x -= vaus_speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            new_pos.x += vaus_speed * Time.deltaTime;
        }

        new_pos.x = Mathf.Clamp(new_pos.x, -max_pos_limits, max_pos_limits);
        transform.position = new_pos;
    }

    void HandlePowerUpActive()
    {
        // handle dynamic powerup behaviours
        switch (active_powerup)
        {
            case PowerUpType.Laser:
                {
                    HandleLaserShot();
                    break;
                }
        }
    }

    void HandleLaserShot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    public void ActivePowerUp(PowerUpType type)
    {
        last_powerup = active_powerup;
        active_powerup = type;

        // handle one shot behaviours
        switch (active_powerup)
        {
            case PowerUpType.Enlarge:
                {
                    break;
                }
            case PowerUpType.Slow:
                {
                    break;
                }
            case PowerUpType.Disruption:
                {
                    // instantiate 2 new balls
                    break;
                }
            case PowerUpType.ExtraLife:
                {
                    ++lifes;
                    break;
                }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);
    }
}
