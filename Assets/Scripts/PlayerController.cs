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
    public float moveSpeed;
    public float rotationSpeed = 5;

    private Rigidbody rb;
    private static PlayerController _instance;
    public static PlayerController Instance => _instance;

    private GameObject rotDirectionPointer;
    public Transform ball;
    public Transform ballController;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _instance = this;
        rotDirectionPointer = GameObject.FindGameObjectWithTag("DirectionPointer");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
    }

    void Movement()
    {
        
        float rotY = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            rotY = -45;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            rotY = 45;
        }

        ballController.rotation = Quaternion.Euler(0, rotY, 0);

        
        ball.Rotate(new Vector3(1, 0, 0), rotationSpeed * Time.deltaTime, Space.Self);
    }
    void Jump()
    {
        if (isOnGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpHeight);
                isOnGround = false;
            }
        }
    }

    

}
