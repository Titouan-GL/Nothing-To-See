using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FluidSim : MonoBehaviour
{
    [SerializeField] GameObject points;
    [SerializeField]private int nbrOfPoints = 30;
    private List<GameObject> pointList = new List<GameObject>();
    [SerializeField]private float distance = 1.8f;
    [SerializeField]private float evolution = 1f;
    [SerializeField]private float evolutionPerSeconds = 1f;

    private SpriteShapeController shape;
    // Start is called before the first frame update
    void Start()
    {
        shape = GetComponent<SpriteShapeController>();
        SpringJoint2D[] springJoint;
        for(int i = 0; i < nbrOfPoints; i++){
            GameObject go = Instantiate(points, transform.position + GetPosFromHypothenuse((360/nbrOfPoints)*i, distance), Quaternion.Euler(new Vector3(0, 0, (360/nbrOfPoints)*i)),  transform);
            pointList.Add(go);

            if(i > 1){

                springJoint = pointList[i-1].GetComponents<SpringJoint2D>();


                springJoint[0].connectedBody = pointList[i-2].GetComponent<Rigidbody2D>();
                springJoint[1].connectedBody = go.GetComponent<Rigidbody2D>();
                springJoint[2].connectedBody = GetComponent<Rigidbody2D>();

                pointList[i-1].GetComponent<DistanceJoint2D>().connectedBody = pointList[i-2].GetComponent<Rigidbody2D>();
            }
        }
        springJoint = pointList[0].GetComponents<SpringJoint2D>();
        springJoint[0].connectedBody = pointList[nbrOfPoints-1].GetComponent<Rigidbody2D>();
        springJoint[1].connectedBody = pointList[1].GetComponent<Rigidbody2D>();
        springJoint[2].connectedBody = GetComponent<Rigidbody2D>();
        pointList[0].GetComponent<DistanceJoint2D>().connectedBody = pointList[nbrOfPoints-1].GetComponent<Rigidbody2D>();



        springJoint = pointList[nbrOfPoints-1].GetComponents<SpringJoint2D>();
        springJoint[0].connectedBody = pointList[nbrOfPoints-2].GetComponent<Rigidbody2D>();
        springJoint[1].connectedBody = pointList[0].GetComponent<Rigidbody2D>();
        springJoint[2].connectedBody = GetComponent<Rigidbody2D>();
        pointList[nbrOfPoints-1].GetComponent<DistanceJoint2D>().connectedBody = pointList[nbrOfPoints-2].GetComponent<Rigidbody2D>();

        //Shape
        for(int i = 0; i < nbrOfPoints; i ++){
            if(i < 4){
                shape.spline.SetPosition(i, pointList[i].transform.localPosition);
            }
            else{
                shape.spline.InsertPointAt(i, pointList[i].transform.localPosition);
            }
            shape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }

    }

    private Vector3 GetPosFromHypothenuse(float angle, float hypo){
        return new Vector3(Mathf.Cos(angle* Mathf.Deg2Rad) * hypo, Mathf.Sin(angle* Mathf.Deg2Rad) * hypo, 0);
    }

    void Update(){
        /*int layerMask2 = 1 << 13;
        Vector2 direction3 = pointList[12].transform.position - pointList[8].transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(pointList[8].transform.position, direction3, direction3.magnitude, layerMask2);
        Debug.DrawRay(pointList[8].transform.position, direction3, Color.blue);
        Debug.Log(hits.Length);*/

        Vector2[] allDirections = new Vector2[nbrOfPoints];
        for(int i = 0; i < nbrOfPoints; i ++){
            
            shape.spline.SetPosition(i, pointList[i].transform.localPosition);

            Vector2 _newLt = (pointList[ContinuousIndexes(i-1, nbrOfPoints)].transform.position - pointList[ContinuousIndexes(i+1, nbrOfPoints)].transform.position) * 0.2f;
            Vector2 _newRt = - _newLt;

            shape.spline.SetRightTangent(i, _newRt);
            shape.spline.SetLeftTangent(i, _newLt);

            Vector2 defaultDirection = Vector2.Perpendicular(_newLt).normalized;
            int layerMask = 1 << 13;
            int j = 0;
            bool isInRange = true;
            while(j < nbrOfPoints && isInRange){
                Vector2 direction2 = pointList[ContinuousIndexes(j+i, nbrOfPoints)].transform.position - pointList[i].transform.position;
                RaycastHit2D[] hits = Physics2D.RaycastAll(pointList[i].transform.position + V2toV3(direction2).normalized*0.1f, direction2- direction2.normalized*0.2f, direction2.magnitude, layerMask);

                Debug.DrawRay(pointList[i].transform.position + V2toV3(direction2).normalized*0.1f, direction2- direction2.normalized*0.2f, Color.green);
                if(hits.Length == 0/*hit.collider == null*/){
                    Debug.Log(hits.Length);
                    allDirections[i] += V3toV2(direction2);
                    allDirections[j] -= V3toV2(direction2);
                }
                j++;
            }
        }
        for(int i = 0; i < nbrOfPoints; i ++){
            Debug.DrawRay(pointList[i].transform.position, -allDirections[i].normalized, Color.red);
            pointList[i].transform.position += V2toV3(-allDirections[i].normalized)*Time.deltaTime;
        }
    }

    private int ContinuousIndexes(int currentIndex, int maxIndex){
        while(currentIndex < 0){
            currentIndex += maxIndex;
        }
        while(currentIndex >= maxIndex){
            currentIndex -= maxIndex;
        }
        return currentIndex;
    }

    void FixedUpdate(){
        evolution *= Mathf.Pow(evolutionPerSeconds, Time.fixedDeltaTime);
    }

    Vector2 V3toV2 (Vector3 vec){
        return new Vector2(vec.x, vec.y);
    }
    Vector3 V2toV3 (Vector2 vec){
        return new Vector3(vec.x, vec.y, 0);
    }
}
