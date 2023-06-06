using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    bool highlight = false;
    [SerializeField] private Material highlightMat;
    [SerializeField] private Material notHighlightMat;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!highlight){
            spriteRenderer.material = notHighlightMat;
        }
        else{
            highlight = false;
        }
    }

    public void IsHighlighted(){
        highlight = true;
        spriteRenderer.material = highlightMat;

    }


}
