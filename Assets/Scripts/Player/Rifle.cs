using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    float reloadTime;
    float reloadTimeMax;
    private Camera cam;
    [SerializeField] Transform endRifle;

    [SerializeField]Transform shellPoint;
    [SerializeField]GameObject shellObject;
    [SerializeField]GameObject bulletObject;
    [SerializeField]GameObject fireLight;
    float lightTime = 0f;

    private GameObject fireAngleR;
    private GameObject fireAngleL;
    private GameObject fireAngle2R;
    private GameObject fireAngle2L;
    private GameObject fireAngle3R;
    private GameObject fireAngle3L;
    [SerializeField] private LineRenderer fireLineR;
    [SerializeField] private LineRenderer fireLineL;

    float fireAngleMin = 4f;
    float fireAngleMax = 15f;
    float currentFireAngle;
    float recalibration = 5f;
    float decalibration = 2f;
    float pushbackDuration;

    public AnimationCurve curve;
    [SerializeField] private Transform rifleModel;
    Vector3 riflePosition;

    public void Start(){
        reloadTime = 0f;
        reloadTimeMax = 0.1f;
        pushbackDuration = 0.2f;
        cam = Camera.main;
        fireLight.SetActive(false);
        currentFireAngle = fireAngleMin;

        riflePosition = rifleModel.localPosition;
        CreateFireAngle();

    }

    public void Update(){
        fireLineR.SetPosition(0, fireAngle2R.transform.position);
        fireLineR.SetPosition(1, fireAngle3R.transform.position);
        fireLineL.SetPosition(0, fireAngle2L.transform.position);
        fireLineL.SetPosition(1, fireAngle3L.transform.position);

        fireAngleR.transform.rotation = Quaternion.Euler(new Vector3(0, 0, endRifle.rotation.eulerAngles.z + currentFireAngle));
        fireAngleL.transform.rotation = Quaternion.Euler(new Vector3(0, 0, endRifle.rotation.eulerAngles.z - currentFireAngle));

        if(currentFireAngle > fireAngleMin){
            currentFireAngle -= recalibration * Time.deltaTime;
        }
        else{
            currentFireAngle = fireAngleMin;
        }


        if(lightTime < 0){
            lightTime = 0;
            fireLight.SetActive(false);
        }
        else if(lightTime > 0){

            lightTime -= Time.deltaTime;
        }
    }

    public void FixedUpdate(){
        if(reloadTime > 0){
            reloadTime -= Time.fixedDeltaTime;
        }
    }

    public override void IsNotMoving(){
        currentFireAngle -= recalibration * Time.deltaTime;
    }

    public override void CanFire(bool canfire){
        fireLineL.gameObject.SetActive(canfire);
        fireLineR.gameObject.SetActive(canfire);
    }

    public override void Fire(){
        if(reloadTime <= 0){
            fireLight.SetActive(true);
            lightTime = 0.05f;
            GameObject go = Instantiate(shellObject, shellPoint.position, Quaternion.Euler(shellPoint.rotation.eulerAngles - new Vector3(0, 0, 90f))); 
            GameObject go2 = Instantiate(bulletObject, endRifle.position, Quaternion.Euler(endRifle.rotation.eulerAngles - new Vector3(0, 0, Random.Range(-currentFireAngle, currentFireAngle))));
            cam.GetComponent<CameraScript>().Shake();
            reloadTime = reloadTimeMax;
            if(currentFireAngle < fireAngleMax){
                currentFireAngle += decalibration;
                if(currentFireAngle > fireAngleMax){
                    currentFireAngle = fireAngleMax;
                }
            }
            StartCoroutine(PushBack());
        }
    }

    void CreateFireAngle(){
        fireAngleR = new GameObject("fireAngleR");
        fireAngleL = new GameObject("fireAngleL");
        fireAngle2R = new GameObject("fireAngle2R");
        fireAngle2L = new GameObject("fireAngle2L");
        fireAngle3R = new GameObject("fireAngle3R");
        fireAngle3L = new GameObject("fireAngle3L");

        fireAngleR.transform.parent = endRifle;
        fireAngleL.transform.parent = endRifle;
        fireAngleR.transform.localPosition = new Vector3(0, 0, 0);
        fireAngleL.transform.localPosition = new Vector3(0, 0, 0);

        fireAngle2R.transform.parent = fireAngleR.transform;
        fireAngle2L.transform.parent = fireAngleL.transform;
        fireAngle2R.transform.localPosition = new Vector3(0, 1, 0);
        fireAngle2L.transform.localPosition = new Vector3(0, 1, 0);

        fireAngle3R.transform.parent = fireAngle2R.transform;
        fireAngle3L.transform.parent = fireAngle2L.transform;
        fireAngle3R.transform.localPosition = new Vector3(0, 3, 0);
        fireAngle3L.transform.localPosition = new Vector3(0, 3, 0);
    }

    IEnumerator PushBack(){
        Vector3 startPosition = riflePosition;
        float elapsedTime = 0f;

        while(elapsedTime < pushbackDuration){
            elapsedTime += Time.deltaTime;
            float distance = curve.Evaluate(elapsedTime/pushbackDuration);
            rifleModel.localPosition = startPosition + new Vector3(0, -distance, 0);
            yield return null; 
        }
        
        rifleModel.localPosition = riflePosition;
    }
}
