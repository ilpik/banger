using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public Object treePrefab;

    private Transform[] points;

    // Start is called before the first frame update
    void Start()
    {
        points = this.GetComponentsInChildren<Transform>().Where(x => x.gameObject != gameObject).ToArray();
        var rightPoint = GetRightPoint();
        var leftPoint = GetLeftPoint();
        float rotation = Random.value * 360;
        Object.Instantiate(treePrefab, rightPoint, Quaternion.Euler(0, rotation, 0), this.transform);
        Object.Instantiate(treePrefab, leftPoint, Quaternion.Euler(0, -rotation, 0), this.transform);
    }

    private Vector3 GetRightPoint()
    {
        return points[Random.Range(0, points.Length)].position;
    }

    private Vector3 GetLeftPoint()
    {
        var pt = GetRightPoint();
        return new Vector3(-pt.x, pt.y, pt.z);
    }
}
