using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.Rotate(Vector3.up, Random.value * 360);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
