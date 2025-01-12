using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flappy/Flappy Segment", fileName = "Segment")]
public class FlappySegment : ScriptableObject
{
    public GameObject segment;
    public float distanceRight;
    public float distanceLeft;
    public float commonFactor;
    public bool randomY;
}
