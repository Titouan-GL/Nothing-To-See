using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 5;

    private Rigidbody2D myRB;
    [SerializeField] Transform endRifle;

    private float footprintDistanceMax = 1f;
    private float footprintDistance;
    private bool rightFoot = true;

    Vector2 movement;
    bool isMoving;

    public bool isGrabbing;
    Transform grabbedObject;
    [SerializeField]Footprints lowerBody;

    [SerializeField]Weapon currentWeapon;
    [SerializeField] private Animator screenAnimator;

    int life;
    int lifemax = 100;

    // Start is called before the first frame update
    void Start()
    {
        life = lifemax;
        myRB = GetComponentInChildren<Rigidbody2D>();
        footprintDistance = footprintDistanceMax;
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        Debug.DrawLine(endRifle.position,transform.position+(endRifle.position - transform.position)*2);
        int layerMask = 1 << 9;
        layerMask += 1 << 17;
        RaycastHit2D hit = Physics2D.Raycast(endRifle.position,endRifle.position - transform.position, 2f, layerMask);
        if(Input.GetButtonDown("Select")){
            if(isGrabbing){//dropping
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
                if(hit.collider.GetComponentInChildren<Interactible>() != null){//activating
                    hit.collider.GetComponentInChildren<Interactible>().ChangeActivated();
                }
                else if(hit.collider.GetComponentInChildren<Movable>() != null){//picking up
                    isGrabbing = true;
                    hit.transform.parent = transform;
                    grabbedObject = hit.transform;
                    Destroy(grabbedObject.GetComponentInChildren<Rigidbody2D>());
                }
            }
        }
        else if(!isGrabbing && hit.collider != null){
            if(hit.transform != null){
                if(hit.transform.GetComponent<Movable>() != null){
                    hit.transform.GetComponent<Movable>().IsHighlighted();
                }
            }
        }

            
        if(!isMoving || isGrabbing){
            currentWeapon.IsNotMoving();
        }
        currentWeapon.CanFire(!isGrabbing);

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
        if(Input.GetButton("Fire1") && !isGrabbing){
            currentWeapon.Fire();
        }
        if(Input.GetButton("Reload") && !isGrabbing){
            currentWeapon.Reload();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        AmmoPickup ap = collision.gameObject.GetComponent<AmmoPickup>();
        if(ap != null && currentWeapon.currentRecharges < currentWeapon.maxRecharges){
            if(ap.isWorking){//this is a nasty debug
                Destroy(ap.gameObject);
                currentWeapon.currentRecharges ++;
            }
        }
    }

    public float GetLifeFraction(){
        return (life*1.0f)/lifemax;
    }

    public void IsDamaged(int damage){
        life -= damage;
        screenAnimator.Play("DamageFade");
    }
}
