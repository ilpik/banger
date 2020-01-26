using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts;

public class PickUpSpawner : MonoBehaviour
{
    public GameObject pickUpPrefab;
    private Transform[] pointsArr ;

    //private PickUpsRespawnPoints[] etRespawnPoints;

    //private Transform[] points;
    int index;
    void Start()
    {
        PickRandomPoints();
    }

    void PickRandomPoints()
    {
        var respawnArr = transform.FindChildGameObjectsByTag("PickUpsRespawnPoints");
        index = Random.Range(0, respawnArr.Count);

        pointsArr = respawnArr[index].GetComponentsInChildren<Transform>().Where(x => x.gameObject != respawnArr[index].gameObject).ToArray();
        for(int i=0; i< pointsArr.Length; i++)
        {
            Object.Instantiate(pickUpPrefab, pointsArr[i].position, Quaternion.Euler(0, 0, 0), this.transform);

        }
    }

}