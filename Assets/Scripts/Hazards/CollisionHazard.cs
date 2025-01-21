using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHazard : MonoBehaviour
{
    private BirdHealth bird;

    void Awake()
    {
        bird = FindObjectOfType<BirdHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && bird.CanBeHit())
        {
            bird.TakeDamage();
        }
    }
}
