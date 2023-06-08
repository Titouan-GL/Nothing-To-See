using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactible
{
    [SerializeField] private List<UnityEngine.Rendering.Universal.Light2D> lights = new List<UnityEngine.Rendering.Universal.Light2D>();
    [SerializeField] private bool activated = false;

    public void Start(){
        SetActivated(activated);
    }

    public override void SetActivated(bool activated){
        this.activated = activated;
        foreach(UnityEngine.Rendering.Universal.Light2D light in lights){
            light.gameObject.SetActive(activated);
        }
    }

    public override void ChangeActivated(){
        SetActivated(!activated);
    }
}
