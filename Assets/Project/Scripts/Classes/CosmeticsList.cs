using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flappy/Cosmetic List", fileName = "CosmeticList")]
public class CosmeticsList : ScriptableObject
{
    public FlappyBody[] bodies; 
    public FlappyHat[] hats; 
    public FlappyEyes[] eyes; 
    public FlappyBeak[] beaks; 
    public FlappyWings[] wings; 
}
