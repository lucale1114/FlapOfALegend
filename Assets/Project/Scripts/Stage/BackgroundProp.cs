using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundProp : MonoBehaviour
{
    private SpriteRenderer render;
    private Rigidbody2D birdBody;

    public float ObjectMoveMultiplier;
    public bool Forced;
    public bool RandomSize;
    public bool RandomTransparency;
    public bool MoveAuto;
    public Sprite[] Variations;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Rigidbody2D t))
            {
                birdBody = t;
            }
        }
    }
    private void Start()
    {
        if (Variations.Length > 0)
        {
            render.sprite = Variations[Random.Range(0,Variations.Length)];
        }
        if (RandomSize)
        {
            transform.localScale *= (float)Random.Range(80, 120) / 100;
        }
        if (RandomTransparency)
        {
            var color = render.color;
            color.a = (float)Random.Range(30, 100)/100;
            render.color = color;
        }
        if (!Forced)
        {
            ObjectMoveMultiplier *= (float)Random.Range(95, 105) / 100;
        }
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        if (birdBody)
        {
            transform.Translate(Vector3.left * ObjectMoveMultiplier * -birdBody.velocity.x * 0.7f * Time.deltaTime);
        }
        if (MoveAuto) { 
            transform.Translate(Vector3.left * ObjectMoveMultiplier / 10 * Time.deltaTime); 
        }
    }
}
