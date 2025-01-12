using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    readonly float TERMINAL_VELOCITY = -7;
    private GameObject birdBase;
    private Rigidbody2D rb2d;

    public float FloatPower;
    public float BirdSpeed;

    private bool isFalling;

    private void Awake()
    {
        birdBase = transform.GetChild(0).Find("BirdBase").gameObject;
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    private void DoAFlap()
    {
        birdBase.transform.DOKill();
        birdBase.transform.DORotate(new Vector3(0, 0, 20), 0.1f);
        rb2d.velocity = Vector3.zero;
        rb2d.AddForce(new Vector3(0, FloatPower, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(birdBase.transform.position.x + 1.2f, 0, -10);
        if (rb2d.velocity.y <= TERMINAL_VELOCITY)
        {
            if (!isFalling)
            {
                isFalling = true;
                birdBase.transform.DORotate(new Vector3(5, 0, -90), 0.5f)   ;
            }
            rb2d.velocity = new Vector3(0, TERMINAL_VELOCITY, 0);
        } 
        var vel = rb2d.velocity;
        vel.x = BirdSpeed;
        rb2d.velocity = vel;

        if (Input.touchCount > 0) {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                isFalling = false;
                DoAFlap();
            }
        }
    }
}
