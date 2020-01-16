using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isOnGround;
    public float jumpHeight;
    public float moveSpeed;
    public float rotationSpeed;
    public Transform ball;
    public Transform ballController;

    public Text textScoreCount;
    private int scoreCount;

    private Rigidbody rb;
    private static PlayerController _instance;
    public static PlayerController Instance => _instance;

    private GameObject rotDirectionPointer;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _instance = this;
        rotDirectionPointer = GameObject.FindGameObjectWithTag("DirectionPointer");
    }
    void Start()
    {
        scoreCount = 0;
        SetCountText();
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            scoreCount = scoreCount+10;
            SetCountText();
            Debug.Log(scoreCount);
        }
    }
    void SetCountText()
    {
        textScoreCount.text = "Count: " + scoreCount.ToString();

    }


}
