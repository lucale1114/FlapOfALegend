using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHazard : MonoBehaviour
{
    private BirdHealth bird;
    [SerializeField]
    private AudioClip[] pipeHitSounds; 

    void Awake()
    {
        bird = FindObjectOfType<BirdHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && bird.CanBeHit())
        {
            AudioManager.PlaySound(pipeHitSounds[Random.Range(0,pipeHitSounds.Length)], 1, Random.Range(0.8f,1.2f));
            bird.TakeDamage();
        }
    }
}
