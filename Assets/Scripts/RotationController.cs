using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject directionPointer;
    public Vector3 rotationVec;

    void Update()
    {
        //transform.Rotate(new Vector3(1,0,1), rotationSpeed);
        //Debug.Log(Vector3.right);
        //RotationCntrl();
    }

    void RotationCntrl()
    {
        //Vector3 directionPointerPosition = directionPointer.transform.position;
        //transform.Rotate(rotationVec, rotationSpeed);
    }

}


