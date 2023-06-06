using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float speed;
    public float decelerationFactor;
    public float rotationSpeed;
    [SerializeField]Transform model;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        decelerationFactor = Random.Range(1.8f, 2f);
        speed = Random.Range(2.3f, 3.5f);
        rotationSpeed = Random.Range(200f, 400f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(speed > 0.1f){
            rb.velocity = transform.up * speed;
            model.rotation = Quaternion.Euler(model.rotation.eulerAngles + new Vector3(0f, 0f, rotationSpeed * Time.fixedDeltaTime));
            speed -= speed * decelerationFactor * Time.fixedDeltaTime;
            rotationSpeed -= rotationSpeed * decelerationFactor * Time.fixedDeltaTime;
        }
        else{

        }
    }
}
