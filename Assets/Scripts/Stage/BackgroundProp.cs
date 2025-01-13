using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundProp : MonoBehaviour
{
    private SpriteRenderer renderer;
    private Rigidbody2D birdBody;

    public float ObjectMoveMultiplier;
    public bool Forced;
    public bool RandomSize;
    public bool RandomTransparency;
    public Sprite[] Variations;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        birdBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        if (Variations.Length > 0)
        {
            renderer.sprite = Variations[Random.Range(0,Variations.Length)];
        }
        if (RandomSize)
        {
            transform.localScale *= (float)Random.Range(80, 120) / 100;
        }
        if (RandomTransparency)
        {
            var color = renderer.color;
            color.a = (float)Random.Range(30, 100)/100;
            renderer.color = color;
        }
        if (!Forced)
        {
            ObjectMoveMultiplier *= (float)Random.Range(95, 105) / 100;
        }
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        transform.Translate(Vector3.left * ObjectMoveMultiplier * -birdBody.velocity.x * 0.7f * Time.deltaTime); 
    }
}
