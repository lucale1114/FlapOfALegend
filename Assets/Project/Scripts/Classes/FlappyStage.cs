using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flappy/Flappy Stage", fileName = "Stage")]
public class FlappyStage : ScriptableObject
{
    public FlappySegment[] segments;
    public string levelName;
    public string description;
    public AudioClip music;
    public int chapter;
    public int length;
    public GameObject cloud;
    public GameObject[] backgroundElements;
    public Sprite sky;
    public Sprite ground;
    public Sprite wallSprite;
    public Sprite wallEndSprite;
    public Sprite thumbnail;
    public Color pipeColor;
}
