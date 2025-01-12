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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bird.TakeDamage();
        }
    }
}
