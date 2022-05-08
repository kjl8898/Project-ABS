using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private GameObject explosionGO;
    [SerializeField] private int damage;
    private Rigidbody rb;
    private Vector3 previousPos;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    private void Start()
    {
        previousPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward.normalized * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionGO, transform.position, Quaternion.identity);

        if(collision.gameObject.tag == "Unit")
        {
            collision.gameObject.GetComponent<Unit>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
