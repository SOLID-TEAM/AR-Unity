using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{

    public float radius = 0.5f;
    public Vector3 movement = Vector3.zero;
    public float speed = 5.0f;
    [SerializeField] private float ray_length = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        movement = new Vector3(Random.Range(1, 3), 0.0f, Random.Range(1, 3));
        Debug.Log(movement);
    }

    // Update is called once per frame
    void Update()
    {
        HandleCollision();
        transform.position += movement.normalized * speed * Time.deltaTime;
    }

    void HandleCollision()
    {
        // first shoot little ray
        RaycastHit hit;
        if(Physics.SphereCast(transform.position, radius, movement.normalized, out hit, ray_length))
        {
            Debug.Log("Ball hit with: " + hit.collider.gameObject.name);

            movement = Vector3.Reflect(movement, hit.normal);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius * 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + movement.normalized * ray_length);
    }
}
