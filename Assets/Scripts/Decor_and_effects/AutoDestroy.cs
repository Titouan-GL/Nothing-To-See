using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 2f;

    // Update is called once per frame
    void Update()
    {
        if(lifetime <= 0){
            Destroy(gameObject);
        }
        else{
            lifetime -= Time.deltaTime;
        }
    }
}
