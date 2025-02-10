using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flappy/Item List", fileName = "Items")]
public class ItemList : ScriptableObject
{
    public FlappyItem[] items; 
}
