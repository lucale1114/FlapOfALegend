using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flappy/Flappy Item", fileName = "Item")]
public class FlappyItem : ScriptableObject
{
    public string ItemName;
    [TextArea]
    public string Description;
    public string[] Positives;
    public Sprite Icon;
    public bool Active;
}
