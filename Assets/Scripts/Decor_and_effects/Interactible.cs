using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public abstract void SetActivated(bool activated);
    public abstract void ChangeActivated();
}
