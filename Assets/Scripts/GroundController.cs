using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public float groundSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back* groundSpeed*Time.deltaTime); 
    }

}
