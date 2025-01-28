using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flappy/Stage List", fileName = "Stage List")]
public class StageList : ScriptableObject
{
    public FlappyStage[] stages;
}
