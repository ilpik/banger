using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickUpsRespawnPoints : MonoBehaviour
{
    private Transform[] points;
    private void Start()
    {

    }
    public Transform[] GetRespawnPoints()
    {
        points = this.GetComponentsInChildren<Transform>().Where(x => x.gameObject != gameObject).ToArray();
        Debug.Log(points.Length) ;
        return points;
    }
}
