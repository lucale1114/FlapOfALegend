using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    private GameObject birdBase;
    private Rigidbody2D rb2d;

    public float floatPower;

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
        rb2d.velocity = Vector3.zero;
        rb2d.AddForce(new Vector3(0, floatPower, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                DoAFlap();
            }
        }
    }
}
