using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Core vars")]
    public float vaus_speed = 9.0f;
    public float max_pos_limits = 5.0f;
    [SerializeField] bool can_move = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        HandleMovement();

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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);


    }
}
