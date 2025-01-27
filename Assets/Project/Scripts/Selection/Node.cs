using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private FlappyStage nodeLevel;
    private SpriteRenderer nodeHead;

    public bool Completed;

    void Start()
    {
        nodeHead = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
