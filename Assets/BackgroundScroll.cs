using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float horizontal = 0.2f;
    public float vertical = 0.2f;
    private Renderer render;

    void Awake()
    {
        render = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * horizontal, Time.time * vertical);
        render.material.mainTextureOffset = offset;
    }
}
