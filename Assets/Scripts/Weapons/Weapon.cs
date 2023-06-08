using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject ammoSpriteForUI;
    public GameObject ammoEmptySpriteForUI;
    public GameObject rechargeSpriteForUI;
    public int maxAmmo;
    public int currentAmmo;
    // Start is called before the first frame update
    public abstract void Fire();
    public abstract void IsNotMoving();
    public abstract void CanFire(bool canfire);
    public abstract void Reload();

    public int currentRecharges;
    public int maxRecharges;
}
