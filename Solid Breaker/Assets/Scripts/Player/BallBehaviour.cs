using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PredictedCollision
{
    public GameObject col_obj;
    public Vector3 col_point;
    public Vector3 current_movement;
    public Vector3 col_normal;
    public bool predicted = false;

    public void FillCollisionData(Vector3 movement, RaycastHit hit)
    {
        current_movement = movement;
        col_point = hit.point;
        col_normal = hit.normal;
        col_obj = hit.collider.gameObject;
        predicted = true;
    }
}
public class BallBehaviour : MonoBehaviour
{

    public float radius = 0.25f;
    public Vector3 movement = Vector3.zero;
    public float speed = 5.0f;
    [SerializeField] private float ray_length = 0.5f;

    [SerializeField] private PredictedCollision predicted_collision;
    //[SerializeField] private bool dynamic_col = false;

    // Start is called before the first frame update
    void Start()
    {
        movement = new Vector3(Random.Range(1, 3), 0.0f, Random.Range(1, 3));
        Debug.Log(movement);
        predicted_collision = new PredictedCollision();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCollision();
        transform.position += movement.normalized * speed * Time.deltaTime;
    }

    void HandleCollision()
    {
        // check collision prediction
        SetPrediction();
        // if we have predicted collision pos, check if we passed and reflect
        if(predicted_collision.predicted)
        {
            float offset_min = 0.2f;
            float dti = (transform.position - predicted_collision.col_point).magnitude;
            Debug.Log("Distance to impact: " + dti);
            if(dti <= radius + offset_min)
            {
                movement = Vector3.Reflect(movement, predicted_collision.col_normal);
                predicted_collision.predicted = false;
            }
        }
    }

    void SetPrediction()
    {
        // first shoot little ray
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, movement.normalized, out hit, Mathf.Infinity))
        {
            Debug.Log("Ball prediction hit with: " + hit.collider.gameObject.name);
            predicted_collision.FillCollisionData(movement, hit);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // DRAW PREDICTION
        if(predicted_collision != null)
            Gizmos.DrawWireSphere(predicted_collision.col_point, radius);

        // DRAW DYNAMIC COLLISION WITH PLAYER
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, movement.normalized, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Player")
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(predicted_collision.col_point, radius);
            }
        }
    }
}
