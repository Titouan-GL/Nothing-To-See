using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Rigidbody2D myRB;
    private Camera cam;
    private PlayerController playerController;
    Vector2 mousePos;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInChildren<PlayerController>();
        myRB = GetComponentInChildren<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate(){
        transform.position = transform.position;

        
        float multiplierRotation = playerController.isGrabbing ? 0.1f : 1f;

        Vector2 lookDir = mousePos - myRB.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        myRB.rotation = Quaternion.Lerp(Quaternion.Euler(new Vector3(0, 0, myRB.rotation)), Quaternion.Euler(new Vector3(0, 0, angle)), 0.2f * multiplierRotation).eulerAngles.z;
    }
}
