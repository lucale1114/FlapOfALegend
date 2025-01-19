using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdMovement : MonoBehaviour
{
    readonly float TERMINAL_VELOCITY = -7;

    public event Action WokeUp;
    public Sprite GlideSprite;
    public float FloatPower;
    public float BirdSpeed;
    public bool Sleeping;

    private bool isFalling;
    private GameObject birdBase;
    private Rigidbody2D rb2d;
    private Animator wingAnimator;
    private Blinking blinking;
    private BirdHealth birdHealth;
    private bool died;


    private void Awake()
    {
        Application.targetFrameRate = 120;
        birdBase = transform.Find("BirdObject").gameObject;
        rb2d = GetComponent<Rigidbody2D>();
        blinking = FindObjectOfType<Blinking>();
        birdHealth = FindObjectOfType<BirdHealth>();
        wingAnimator = birdBase.transform.Find("Wings").GetComponent<Animator>();
    }

    void Start()
    {
        if (Sleeping)
        {
            rb2d.bodyType = RigidbodyType2D.Static;
        }
        birdHealth.Died += () =>
        {
            BirdSpeed = 0;
            died = true;
            birdBase.transform.DORotate(new Vector3(5, 0, -90), 0.5f);

        };
    }

    private void DoAFlap()
    {
        if (died)
        {
            return;
        }
        birdBase.transform.DOKill();
        birdBase.transform.DORotate(new Vector3(0, 0, 20), 0.1f);
        rb2d.velocity = Vector3.zero;
        wingAnimator.Play("Wings1", 0, 0);
        rb2d.AddForce(new Vector3(0, FloatPower, 0));
    }

    // Update is called once per frame

    private void Update()
    {
        //Debug.Log((int)1.0f / Time.smoothDeltaTime + " FPS");
        if (Input.touchCount > 0)
        {
            if (Sleeping)
            {
                if (blinking.CanStart)
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    float savedSpeed = BirdSpeed;
                    BirdSpeed = 0;
                    DOTween.To(() => BirdSpeed, x => BirdSpeed = x, savedSpeed, 1);
                    Sleeping = false;
                    blinking.Sleeping = false;
                    WokeUp?.Invoke();
                }
                else
                {
                    StartCoroutine(blinking.WakeUp());
                    Camera.main.transform.DOMove(new Vector3(birdBase.transform.position.x + 1.2f, 0, -10), 1f);
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Began && !Sleeping)
            {
                isFalling = false;
                DoAFlap();
            }
        }
    }

    void FixedUpdate()
    {
        if (Sleeping)
        {
            return;
        }
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
    }
}
