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
    private Vector3 default_localScale;
    [SerializeField] private bool started_round = false;
    [SerializeField] private bool player_ready = false;
    GameManager gameManager;
    [Header("Audio SFX")]
    [SerializeField] private AudioSource m_audio;
    public AudioClip laser_clip;
    public AudioClip generic_pickup_clip;
    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        active_powerup = PowerUpType.None;
        default_localScale = transform.localScale;

        // get audio source
        m_audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.targetDetected) return;

        if (player_ready && !started_round)
        {
            // update ball position until is shooted
            GameObject current_ball = GameObject.FindGameObjectWithTag("Ball");
            current_ball.transform.position = new Vector3(this.transform.position.x, 0.0f, -5.7f);
            current_ball.transform.position += new Vector3(0.0f, 0.0f, 0.55f);

            if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0 && Input.GetTouch(0).phase.Equals(TouchPhase.Began))
            {
                GameObject ball_go = GameObject.FindGameObjectWithTag("Ball");
                BallBehaviour ball = ball_go.GetComponent<BallBehaviour>();
                if (ball)
                {
                    ball.movement = new Vector3(1.0f, 0.0f, 1.0f);
                    started_round = true;
                }
            }

        }

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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ResetPlayer();
        }
    }

    public void ResetPlayer()
    {
        ActivatePowerUp(PowerUpType.None);
        // PLAYER
        transform.position = transform.parent.position;
        transform.position += new Vector3(0.0f, 0.0f, -5.7f);

        // DELETE all balls, just in case or for debug purposes
        GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in allBalls)
            Destroy(ball);

        // NEW BALL
        GameObject new_ball = Instantiate(Resources.Load("Ball"), this.transform.parent) as GameObject; // child of frame
        Vector3 offseted_pos = this.transform.position;
        offseted_pos += new Vector3(0.0f, 0.0f, 0.55f);
        new_ball.transform.position = offseted_pos;

        // prepare player to shot first ball of the round
        player_ready = true;
        started_round = false;
    }

    void HandleMovement()
    {
        Vector3 new_pos = transform.position;

        if (Input.touchCount > 0)
        {
            Vector2 touchPos = Input.GetTouch(0).position;
            int halfScreen = (int)((float)Screen.width * 0.5f);

            if (touchPos.x < halfScreen)
            {
                new_pos.x -= vaus_speed * Time.deltaTime;
            }
            else
            {
                new_pos.x += vaus_speed * Time.deltaTime;
            }
        }


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

            // play laser shot
            m_audio.PlayOneShot(laser_clip);
        }
    }

    void SwitchBallsSpeed(bool activate_powerup)
    {
        GameObject[] all_balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject go_ball in all_balls)
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
                            if (new_ball)
                            {
                                BallBehaviour ball = new_ball.GetComponent<BallBehaviour>();
                                if (ball)
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
                    gameManager.AddLife();
                    break;
                }
        }

        // play generic pickup clip
        if (!type.Equals(PowerUpType.None))
        {
            m_audio.PlayOneShot(generic_pickup_clip);
        }
    }

}

//    private void OnCollisionEnter(Collision collision)
//    {
//        Debug.Log("Collision with: " + collision.gameObject.name);
//    }
//}
