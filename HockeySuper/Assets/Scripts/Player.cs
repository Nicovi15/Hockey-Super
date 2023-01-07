using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
enum Controller
{
    Keyboard = 0,
    Mouse = 1,
    Webcam = 2
}

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField]
    Controller controller;
    [Header("Keyboard Settings")]
    [SerializeField]
    KeyCode Left;
    [SerializeField]
    KeyCode Right;
    [SerializeField]
    KeyCode Up;
    [SerializeField]
    KeyCode Down;
    [Header("Mouse Settings")]
    [SerializeField]
    Camera cam;
    [SerializeField]
    LayerMask boardLayer;

    [Header("Movement Settings")]
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    Vector3 bounds;
    [SerializeField]
    Vector3 ballDir;

    [Header("UI Settings")]
    [SerializeField]
    TextMeshProUGUI scoreText;

    Vector3 newMousePos;
    Vector3 moveDirection;
    Rigidbody rb;
    GameManager GM;
    public int score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        score = 0;
        scoreText.text = score.ToString();
    }

    void Update()
    {
        moveDirection = Vector3.zero;
        if (Input.GetKey(Left))
            moveDirection += Vector3.left;
        if (Input.GetKey(Right))
            moveDirection += Vector3.right;
        if (Input.GetKey(Up))
            moveDirection += Vector3.forward;
        if (Input.GetKey(Down))
            moveDirection += Vector3.back;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 500.0f, boardLayer)){
            newMousePos = ray.GetPoint(hit.distance);
        }
    }

    private void FixedUpdate()
    {
        Vector3 newPos = Vector3.zero;
        if(controller == Controller.Keyboard)
        {
            //newPos = Vector3.Lerp(rb.position, rb.position + moveDirection.normalized * moveSpeed, Time.deltaTime * moveSpeed);
            newPos = Vector3.MoveTowards(transform.position, rb.position + moveDirection.normalized * moveSpeed, Time.deltaTime * moveSpeed);
        }
        else if(controller == Controller.Mouse)
        {
            //newPos = Vector3.Lerp(rb.position, newMousePos, Time.deltaTime * moveSpeed);
            newPos = Vector3.MoveTowards(transform.position, newMousePos, Time.deltaTime * moveSpeed);
        }
        else
        {

        }
        if (newPos.x < -bounds.x)
            newPos.x = -bounds.x;
        else if (newPos.x > bounds.x)
            newPos.x = bounds.x;

        if (newPos.z < bounds.z)
            newPos.z = bounds.z;
        else if (newPos.z > bounds.y)
            newPos.z = bounds.y;

        rb.MovePosition(newPos);
    }

    public void Goal(int point)
    {
        score += point;
        scoreText.text = score.ToString();
        StartCoroutine(GM.startNewRound(ballDir));
    } 
}
