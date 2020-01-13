using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isOnGround;
    public float jumpHeight; 
    private Rigidbody rb;

    private static PlayerController _instance;
    public static PlayerController Instance => _instance;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Jump(isOnGround);
    }

    void Jump(bool isOnGround)
    {
        if (isOnGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpHeight);
            }
        }
    }
   
}
