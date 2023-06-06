using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprints : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField] GameObject footprint_R;
    [SerializeField] GameObject footprint_L;

    [SerializeField] Transform footprint_pos_R;
    [SerializeField] Transform footprint_pos_L;

    public Vector2 lookDir;

    private float initialTime = 1f;
    private float timeLeft = 0f;

    private float disappearTime = 1f;
    private Color colorSet;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        int layerMask = 1 << 13;
        Collider2D coll = Physics2D.OverlapCircle(transform.position, 0.3f, layerMask);
        if(coll != null){
            if(coll.gameObject.layer == 13){
                LeaveFootprints lf = coll.gameObject.GetComponent<LeaveFootprints>();
                if(lf != null){
                    initialTime = lf.timeSticking;
                    timeLeft = lf.timeSticking;
                    disappearTime = lf.disappearTime;
                    colorSet = lf.color;
                }
            }
        }
        
        if(timeLeft > 0){
            timeLeft -= Time.fixedDeltaTime;
        }
        
    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawSphere(transform.position, 0.3f);
    }

    public void RightFootPrint(){
        GameObject go = Instantiate(footprint_R, footprint_pos_R.position - new Vector3(0, 0, -0.1f), transform.rotation);
        go.GetComponent<Disappear>().colorSet = colorSet;
        if(timeLeft <= 0){
            go.GetComponent<Disappear>().disappearTime = 0;
            go.GetComponent<Disappear>().colorSet.a = 0;
        }
        else{
            go.GetComponent<Disappear>().disappearTime = disappearTime;
            go.GetComponent<Disappear>().colorSet.a = timeLeft/initialTime;
        }
    }

    public void LeftFootPrint(){
        GameObject go = Instantiate(footprint_L, footprint_pos_L.position - new Vector3(0, 0, -0.1f), transform.rotation);
        go.GetComponent<Disappear>().colorSet = colorSet;
        if(timeLeft <= 0){
            go.GetComponent<Disappear>().disappearTime = 0;
            go.GetComponent<Disappear>().colorSet.a = 0;
        }
        else{
            go.GetComponent<Disappear>().disappearTime = disappearTime;
            go.GetComponent<Disappear>().colorSet.a = timeLeft/initialTime;
        }
    }
}
