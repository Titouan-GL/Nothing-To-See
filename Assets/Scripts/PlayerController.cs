using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 5;

    private Rigidbody2D myRB;
    private Camera cam;
    [SerializeField] Transform endRifle;

    private float footprintDistanceMax = 1f;
    private float footprintDistance;
    private bool rightFoot = true;

    Vector2 movement;
    bool isMoving;

    public bool isGrabbing;
    Transform grabbedObject;
    [SerializeField]Footprints lowerBody;

    [SerializeField]Transform shellPoint;
    [SerializeField]GameObject shellObject;
    [SerializeField]GameObject bulletObject;
    [SerializeField]GameObject fireLight;
    float lightTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponentInChildren<Rigidbody2D>();
        footprintDistance = footprintDistanceMax;
        cam = Camera.main;
        fireLight.SetActive(false);
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        Debug.DrawLine(endRifle.position,transform.position+(endRifle.position - transform.position)*2);
        if(Input.GetButtonDown("Select")){
            int layerMask = 1 << 9;
            RaycastHit2D hit = Physics2D.Raycast(endRifle.position,endRifle.position - transform.position, 2f, layerMask);
            if(isGrabbing){
                grabbedObject.parent = null;
                grabbedObject.gameObject.AddComponent<Rigidbody2D>();
                grabbedObject.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
                grabbedObject.GetComponentInChildren<Rigidbody2D>().drag = 1000;
                grabbedObject.GetComponentInChildren<Rigidbody2D>().angularDrag = 1000;
                grabbedObject.GetComponentInChildren<Rigidbody2D>().mass = 10;
                isGrabbing = false;
            }
            else if(hit.collider != null)
            {
                isGrabbing = true;
                hit.transform.parent = transform;
                grabbedObject = hit.transform;
                Destroy(grabbedObject.GetComponentInChildren<Rigidbody2D>());
            }
        }

        HandleShooting();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float multiplierSpeed = isGrabbing ? 0.3f : 1f;
        myRB.MovePosition(myRB.position + movement * moveSpeed* multiplierSpeed * Time.fixedDeltaTime);
        footprintDistance -= (movement * moveSpeed* multiplierSpeed * Time.fixedDeltaTime).magnitude;

        if(footprintDistance <= 0 && rightFoot){
            footprintDistance = footprintDistanceMax;
            lowerBody.RightFootPrint();
            rightFoot = false;
        }
        if(footprintDistance <= 0 && !rightFoot){
            footprintDistance = footprintDistanceMax;
            lowerBody.LeftFootPrint();
            rightFoot = true;
        }
        if(isMoving){
            lowerBody.lookDir = Vector2.Lerp(lowerBody.lookDir, movement, 0.2f);
        }
        lowerBody.GetComponent<Rigidbody2D>().MovePosition(myRB.position);
    }

    private void HandleShooting(){
        if(lightTime < 0){
            lightTime = 0;
            fireLight.SetActive(false);
        }
        else if(lightTime > 0){
            lightTime -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Fire1")){
            fireLight.SetActive(true);
            lightTime = 0.05f;
            GameObject go = Instantiate(shellObject, shellPoint.position, Quaternion.Euler(shellPoint.rotation.eulerAngles - new Vector3(0, 0, 90f))); 
            GameObject go2 = Instantiate(bulletObject, endRifle.position, endRifle.rotation); 
            cam.GetComponent<CameraScript>().Shake();
        }
    }
}
