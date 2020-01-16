using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickUpSpawner : MonoBehaviour
{
    public GameObject pickUpPrefab;
    private GameObject[] respawnArr;
    private Transform[] pointsArr ;
    private PickUpsRespawnPoints[] GetRespawnPoints;

    //private Transform[] points;
    int index;
    void Start()
    {
        PickRandomPoints();
    }

    void PickRandomPoints()
    {
        respawnArr = GameObject.FindGameObjectsWithTag("PickUpsRespawnPoints");
        index = Random.Range(0, respawnArr.Length);
        pointsArr = respawnArr[index].GetComponentsInChildren<Transform>().Where(x => x.gameObject != gameObject).ToArray();
        for(int i=0; i<= pointsArr.Length; i++)
        {
            Object.Instantiate(pickUpPrefab, pointsArr[i].position, Quaternion.Euler(0, 0, 0), this.transform);

        }
       
        //points = this.GetComponentsInChildren<Transform>().Where(x => x.gameObject != gameObject).ToArray();

        //Debug.Log(points);

        //GameObject pointsA = respawnArr[index];
        //points = pointsArr.GetComponentsInChildren<Transform>().Where(x => x.gameObject != gameObject).ToArray();
        //Debug.Log(points.Length);
        //for (int i = 1; i <= points.Length; i++)
        //{
        //    Object.Instantiate(pickUpPrefab, points[i].position, Quaternion.Euler(0, 0, 0), this.transform);
        //}
        //foreach (Transform point in points)
        //{
        //    Object.Instantiate(pickUpPrefab, point.position, Quaternion.Euler(0, 0, 0), this.transform);
        //}
    }

}