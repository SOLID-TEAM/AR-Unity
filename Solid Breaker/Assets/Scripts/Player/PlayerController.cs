using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{

    [Header("Core vars")]
    public float max_pos_limits = 5.0f;
    [Header("Gameplay vars")]
    public float vaus_speed = 9.0f;
    [SerializeField] PowerUpType active_powerup;
    [SerializeField] private PowerUpType last_powerup;
    public int lifes = 5;
    private Vector3 default_localScale;

    // Start is called before the first frame update
    void Start()
    {
        active_powerup = PowerUpType.none;
        default_localScale = transform.localScale;
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
        for (KeyCode i = KeyCode.Alpha0, n = 0; i < KeyCode.Alpha9; ++i, ++n)
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
            Instantiate(Resources.Load("projectile01"), transform.position + transform.forward * 0.5f, Quaternion.identity);
        }
    }

    void SwitchBallsSpeed(bool activate_powerup)
    {
        GameObject[] all_balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach(GameObject go_ball in all_balls)
        {
            BallBehaviour ball = go_ball.GetComponent<BallBehaviour>();
            if (ball)
                ball.SwitchPowerupSpeed(activate_powerup);
        }
    }

    public void ActivatePowerUp(PowerUpType type)
    {
        last_powerup = active_powerup;
        active_powerup = type;

        // check switch off conditions
        if (last_powerup == PowerUpType.Enlarge)
            transform.localScale = default_localScale;
        if (last_powerup == PowerUpType.Slow)
            SwitchBallsSpeed(false);

        // handle one shot behaviours
        switch (active_powerup)
        {
            case PowerUpType.Enlarge:
                {
                    Vector3 localScale = new Vector3(transform.localScale.x, 1.2f, transform.localScale.z);
                    transform.localScale = localScale;
                    break;
                }
            case PowerUpType.Slow:
                {
                    SwitchBallsSpeed(true);
                    break;
                }
            case PowerUpType.Disruption:
                {
                    // instantiate 2 new balls
                    // find current ball position
                    GameObject any_current_ball = GameObject.FindGameObjectWithTag("Ball");
                    if (any_current_ball)
                    {
                        for (int i = 0; i < 2; ++i)
                        {
                            // TODO: check why this doesnt work if we instantiate directly as ballbehaviour
                            GameObject new_ball = Instantiate(Resources.Load("Ball"), any_current_ball.transform.position, Quaternion.identity) as GameObject;
                            if(new_ball)
                            { 
                                BallBehaviour ball = new_ball.GetComponent<BallBehaviour>();
                                if(ball)
                                    ball.movement = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
                            }
                           
                        }

                    }
                    else
                    {
                        Debug.LogError("not found any ball ingame");
                    }
                    break;
                }
            case PowerUpType.ExtraLife:
                {
                    ++lifes;
                    break;
                }
        }

        Debug.Log("basersf");
    }

}

//    private void OnCollisionEnter(Collision collision)
//    {
//        Debug.Log("Collision with: " + collision.gameObject.name);
//    }
//}
