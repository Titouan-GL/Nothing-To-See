using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FluidSim : MonoBehaviour
{
    [SerializeField] GameObject points;
    private int nbrOfPoints = 30;
    private List<GameObject> pointList = new List<GameObject>();
    private float distance = 1.8f;

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
        for(int i = 0; i < nbrOfPoints; i ++){
            pointList[i].transform.position += pointList[i].transform.right * 1f * Time.deltaTime;
            
            shape.spline.SetPosition(i, pointList[i].transform.localPosition);

            Vector2 _newLt = (pointList[ContinuousIndexes(i-1, nbrOfPoints)].transform.position - pointList[ContinuousIndexes(i+1, nbrOfPoints)].transform.position) * 0.2f;
            Vector2 _newRt = - _newLt;

            shape.spline.SetRightTangent(i, _newRt);
            shape.spline.SetLeftTangent(i, _newLt);
            
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
}
