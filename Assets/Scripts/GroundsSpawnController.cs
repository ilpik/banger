using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts;
using UnityEditor.Rendering;
using Random = UnityEngine.Random;

public class GroundsSpawnController : MonoBehaviour
{
    public List<GameObject> grounds;

    private Vector3 _groundSpawnPoint;
    private Vector3 _nextPlatformPosition;
    private GameObject[] _existedPlatforms;

    private bool hasSpawned;
    public float spawnDistance = 8;


    void FixedUpdate()
    {
        var groundIndex = Random.Range(0, grounds.Count);

        _groundSpawnPoint = grounds[groundIndex].transform.Find("GroundSpawnPoint").position;
        _nextPlatformPosition = grounds[groundIndex].transform.Find("NextPlatformPosition").position;
        _existedPlatforms = GameObject.FindGameObjectsWithTag("Ground");
    }
}
