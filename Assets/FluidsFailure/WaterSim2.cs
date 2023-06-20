using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSim2 : MonoBehaviour
{
    [SerializeField] GameObject droplet;

    // Update is called once per frame
    void Update()
    {
        Instantiate(droplet, transform.position+ Random.insideUnitSphere*0.1f, Quaternion.identity);
    }
}
