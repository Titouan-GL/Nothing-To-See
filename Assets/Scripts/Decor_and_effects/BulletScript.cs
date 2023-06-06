using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float speed = 200f;
    Rigidbody2D rb;
    [SerializeField] GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go2 = Instantiate(Explosion, collision.GetContact(0).point, Quaternion.identity); 
        Destroy(gameObject);
    }
}
