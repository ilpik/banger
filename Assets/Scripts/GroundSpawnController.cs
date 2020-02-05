using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawnController : MonoBehaviour
{
    //public GameObject groundSpawner;
    public List<GameObject> grounds;

    private Vector3 playerPosition;
    private Vector3 spawnPosition;
    private bool hasSpawned;
    private int groundIndex;


    public Transform NextPlatformPosition;

    public float spawnDistance = 8;


    void Start()
    {
    }

    void FixedUpdate()
    {
        //clonePosition = groundSpawner.transform.position;
        playerPosition = PlayerController.Instance.transform.position;
        spawnPosition = transform.position;
        var distance = GetDistance(playerPosition, spawnPosition);

        if (!hasSpawned && distance <= spawnDistance)
        {
            var position = GetNextPlatformPosition();
            var ground = NewGround();
            var newGround = Instantiate(ground, position, Quaternion.identity, transform.parent);
            newGround.name = this.name;
            hasSpawned = true;
        }

    }

    float GetDistance(Vector3 playerPos, Vector3 spawnPos)
    {
        return Vector3.Distance(playerPos, spawnPos);
    }

    Vector3 GetNextPlatformPosition()
    {
        return NextPlatformPosition.position;
    }

    GameObject NewGround()
    {
        groundIndex = Random.Range(0, grounds.Count);
        return grounds[groundIndex];
    }
}
