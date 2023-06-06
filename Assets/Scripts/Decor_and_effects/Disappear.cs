using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    SpriteRenderer rend;
    public float disappearTime = 1f;
    public Color colorSet;
    private float initialAlpha;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Color c = colorSet;
        initialAlpha = colorSet.a;
        rend.material.color = c;
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut(){
        for(float f = disappearTime; f >= 0; f -= Time.fixedDeltaTime){
            Color c = rend.material.color;
            c.a = initialAlpha * (f/disappearTime);
            rend.material.color = c;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        Destroy(gameObject);
    }
}
