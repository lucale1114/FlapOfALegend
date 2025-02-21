using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cosmetics/Body", fileName = "Body")]
public class FlappyBody : ScriptableObject
{
    public string itemName;
    public Sprite iconSprite;
    public AnimationClip animation;
    public Sprite blinkSprite;
    public Sprite openEyes;
    public Sprite sleepSprite;
}
