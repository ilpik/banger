using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Vector3 fromPoint;
    private Vector3 toPoint;
    private int speed = 5;

     void Start()
     {
         fromPoint = transform.position;
         toPoint = new Vector3(transform.position.x *(-1), fromPoint.y, fromPoint.z);
         PlatformMovement();
    }

     void FixedUpdate()
     {

    }
    void PlatformMovement()
     {
         if (transform.position.x == fromPoint.x)
         {
             MoveLeft();
        }
        //else if (transform.position.x == toPoint.x)
        //{
        //    MoveRight();
        //}
    }

    void MoveLeft()
         {
             Debug.Log("move left ");
            transform.Translate(Vector3.left * Time.deltaTime);
        }
         void MoveRight()
         {
             Debug.Log("move right ");
             transform.Translate(Vector3.right * Time.deltaTime);

         }

}
