using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveFootprints : MonoBehaviour
{
    public float timeSticking = 3f;
    public float disappearTime = 5f;
    public Color color;

    void Start(){
        color = GetComponent<SpriteRenderer>().color;
    }  
}
