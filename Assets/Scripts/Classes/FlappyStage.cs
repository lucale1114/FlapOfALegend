using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flappy/Flappy Stage", fileName = "Stage")]
public class FlappyStage : ScriptableObject
{
    public FlappySegment[] segments;
    public string levelName;
    public int chapter;
    public int length;
}
