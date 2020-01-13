using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawnController : MonoBehaviour
{
    public GameObject groundSpawner;

    private Vector3 playerPosition;
    private Vector3 clonePosition;
    private Vector3 spawnPosition;
    private bool hasSpawned;

    public Transform NextPlatformPosition;

    public float spawnDistance = 8;

    // Update is called once per frame
    void Start()
    {
    }

    void Update()
    {
        //    clonePosition = groundSpawner.transform.position;
        playerPosition = PlayerController.Instance.transform.position;
        spawnPosition = transform.position;
        var distance = GetDistance(playerPosition, spawnPosition);

        if (!hasSpawned && distance <= spawnDistance)
        {
            var position = GetNextPlatformPosition();
            var ground = Resources.Load("Ground");
            var newGround = Instantiate(ground, position, Quaternion.identity, this.transform.parent);
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

}
