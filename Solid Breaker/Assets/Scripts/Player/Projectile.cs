using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5.0f;
    public float radius = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleCollision();
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    void HandleCollision()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider col in cols)
        {
            if (col.gameObject == this.gameObject || col.gameObject.CompareTag("Ball") || col.gameObject.CompareTag("Player"))
                continue;

            Debug.Log("Collision laser shot with: " + col.gameObject.name);

            if(col.gameObject.CompareTag("Block")) // assumes only possible collisions is with one block
            {
                col.gameObject.GetComponent<Block>().BlockHit();
            }

            Destroy(this.gameObject);
            break; // only destroys first hitted block
        }
    }
}
