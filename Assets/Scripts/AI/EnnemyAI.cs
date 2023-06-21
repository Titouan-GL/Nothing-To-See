using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class EnnemyAI : MonoBehaviour
{
    public AIPath aiPath;
    public Seeker seeker;

    private Vector3 back_position;
    private int life = 10;

    private bool isded = false;

    SpriteRenderer rend;
    private float appearTime = 1f;
    private float screamerTime = 0.2f;

    public Transform playerTransform;

    private float reloadTime;
    private float reloadTimeMax = 1f;

    private int damage = 10;
    bool visible = false;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();

        rend = GetComponent<SpriteRenderer>();
        Color c = rend.material.color;
        c.a = 0;
        rend.material.color = c;

        playerTransform = GameObject.FindWithTag("Player").transform;

        reloadTime = reloadTimeMax;
    }

    // Update is called once per frame
    void Update()
    {
        if(life > 0){
            GraphNode node1 = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
            GraphNode node2 = AstarPath.active.GetNearest(aiPath.destination, NNConstraint.Default).node;

            if(PathUtilities.IsPathPossible(node1, node2)){
                seeker.graphMask = 1 << 0;
            }
            else{
                seeker.graphMask = 1 << 1;
            }

            RaycastHit2D hit;
            Vector3 dir = playerTransform.position - transform.position;
            int layerMask = 1 << 10;
            hit = Physics2D.Raycast(transform.position, dir, dir.magnitude, layerMask);
            if(hit.collider != null && reloadTime >= reloadTimeMax){
                if(!visible && hit.distance < 1.5f){
                    StartCoroutine("AttackAppear");
                }
                if(hit.distance < 1f){
                    hit.collider.GetComponent<PlayerController>().IsDamaged(damage);
                    reloadTime = 0f;
                }
            }
            else{
                if(visible){
                    StartCoroutine("AttackDisappear");
                }
            }
            if(reloadTime < reloadTimeMax){
                reloadTime += Time.deltaTime;
            }
        }
        else{
            Dies();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 15 ){
            life -= 1;
        }
    }

    void Dies(){
        if(!isded){
            Destroy(aiPath);
            Destroy(seeker);
            StartCoroutine("FadeIn");
            isded = true;
        }
    }


    IEnumerator FadeIn(){
        visible = true;
        for(float f = 0; f <= appearTime; f += Time.fixedDeltaTime){
            Color c = rend.material.color;
            c.a = f/appearTime;
            rend.material.color = c;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    IEnumerator AttackAppear(){
        visible = true;
        for(float f = 0; f <= screamerTime; f += Time.fixedDeltaTime){
            Color c = rend.material.color;
            c.a = f/screamerTime;
            rend.material.color = c;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    IEnumerator AttackDisappear(){
        visible = false;
        for(float f = screamerTime; f >= 0; f -= Time.fixedDeltaTime){
            Color c = rend.material.color;
            c.a = f/screamerTime;
            rend.material.color = c;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}
