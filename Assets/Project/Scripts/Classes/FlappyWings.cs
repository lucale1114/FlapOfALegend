using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cosmetics/Wings", fileName = "Wings")]
public class FlappyWings : ScriptableObject
{
    public string itemName;
    public int itemId;
    public Sprite glide;
    public Sprite iconSprite;
    public AnimationClip animation;
    public AnimationClip animationUI;
}
