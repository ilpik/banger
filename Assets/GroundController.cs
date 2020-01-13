using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public float groundSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, (-1) * groundSpeed)*Time.deltaTime);
    }

}
