using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    None = 0, Laser, Enlarge, Slow, Disruption, ExtraLife, Max 
}
public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float rotateAngleSpeed = 180.0f;
    public float fallSpeed = 3.0f;
    [SerializeField] private float radius = 0.25f; // capsule radius
    [SerializeField] private float width = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        radius = GetComponent<CapsuleCollider>().radius;
        width = GetComponent<CapsuleCollider>().height;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCollision();
        transform.position += new Vector3(0.0f, 0.0f, -fallSpeed * Time.deltaTime);
        transform.Rotate(0.0f, rotateAngleSpeed * Time.deltaTime, 0.0f);
    }

    void HandleCollision()
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, new Vector3(width * transform.localScale.y, radius * 0.5f, radius * 0.5f), -Vector3.forward, out hit, Quaternion.identity, 0.1f))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                // get player controller script and active power up type
                PlayerController pctrl = hit.collider.GetComponent<PlayerController>();
                if (pctrl)
                    pctrl.ActivatePowerUp(type);

                Destroy(this.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + -Vector3.forward * (radius * 0.5f));
        //Gizmos.DrawWireSphere(transform.position, (width * 0.5f) * transform.localScale.y);
        Gizmos.DrawWireCube(transform.position, new Vector3(width * transform.localScale.y, radius, radius));
    }
}
