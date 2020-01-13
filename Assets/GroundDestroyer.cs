using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDestroyer : MonoBehaviour
{
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
