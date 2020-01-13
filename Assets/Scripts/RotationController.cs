using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public float rotationSpeed;

    // Start is called before the first frame update
    void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed);
    }

}
