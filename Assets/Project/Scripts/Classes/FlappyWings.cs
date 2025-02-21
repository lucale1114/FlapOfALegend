using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cosmetics/Wings", fileName = "Wings")]
public class FlappyWings : ScriptableObject
{
    public string itemName;
    public Sprite iconSprite;
    public AnimationClip animation;
}
