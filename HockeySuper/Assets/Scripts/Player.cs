using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Controller
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
    public Controller controller;
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

    [Header("Camera Settings")]
    [SerializeField]
    RecoPlayer RP;
    [SerializeField]
    bool isGreen;
    Vector3 camPos = Vector3.zero;
    [SerializeField] 
    float minDist;

    [Header("UI Settings")]
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    public string pseudo;
    [SerializeField]
    ParticleSystem PS;

    Vector3 newMousePos;
    Vector3 moveDirection;
    Rigidbody rb;
    GameManager GM;
    public int score;
    [HideInInspector]
    public CameraPlayer CP;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        score = 0;
        scoreText.text = score.ToString();
        CP = cam.GetComponent<CameraPlayer>();
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

        if(controller == Controller.Webcam)
        {
            if (isGreen)
            {
                camPos.x = -2.6f + 5.2f * ((630.0f - (float)RP.greenPos.x) / 310.0f);
                camPos.z = -7.1f + 1.4f * (((float)RP.greenPos.y) - 150.0f) / 170.0f;
            }
            else
            {
                camPos.x = 2.6f - 5.2f * ((320.0f - (float)RP.bluePos.x) / 310.0f);
                camPos.z = 7.1f - 1.4f * (((float)RP.bluePos.y) - 150.0f) / 170.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 newPos = Vector3.zero;
        bool isValid = true;
        if (controller == Controller.Keyboard)
        {
            //newPos = Vector3.Lerp(rb.position, rb.position + moveDirection.normalized * moveSpeed, Time.deltaTime * moveSpeed);
            newPos = Vector3.MoveTowards(transform.position, rb.position + moveDirection.normalized * moveSpeed, Time.deltaTime * moveSpeed);
        }
        else if (controller == Controller.Mouse)
        {
            //newPos = Vector3.Lerp(rb.position, newMousePos, Time.deltaTime * moveSpeed);
            newPos = Vector3.MoveTowards(transform.position, newMousePos, Time.deltaTime * moveSpeed);
        }
        else
        {
            if (camPos.x < -bounds.x)
                isValid = false;
            else if (camPos.x > bounds.x)
                isValid = false;

            if (camPos.z < bounds.z)
                isValid = false;
            else if (camPos.z > bounds.y)
                isValid = false;

            if (isValid)
                if (Vector3.Distance(transform.position, camPos) < minDist)
                    isValid = false;
            newPos = camPos;
        }

        if (newPos.x < -bounds.x)
            newPos.x = -bounds.x;
        else if (newPos.x > bounds.x)
            newPos.x = bounds.x;

        if (newPos.z < bounds.z)
            newPos.z = bounds.z;
        else if (newPos.z > bounds.y)
            newPos.z = bounds.y;

        if(isValid)
            rb.MovePosition(newPos);
    }

    public void Goal(int point)
    {
        score += point;
        scoreText.text = score.ToString();
        StartCoroutine(GM.Goal(this, ballDir));
        var em = PS.emission;
        em.rateOverTime = 500 + 300 * (point - 1);
        PS.Play();
    }

    public void Reset()
    {
        scoreText.text = score.ToString();
    }
}
