using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cosmetics/Hat", fileName = "Hat")]
public class FlappyHat : ScriptableObject
{
    public string itemName;
    public Sprite iconSprite;
    public Vector3 localPos;
    public Vector3 localPosUI;
    public float uiWidth;
    public float uiHeight;
}
