using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    private GameObject levelObject;
    private Transform pointer;

    public FlappyStage ChosenStage;

    private int sizeLeft;
    void Awake()
    {
        levelObject = GameObject.Find("Level");
        pointer = levelObject.transform.Find("Pointer");
    }

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int i = 0; i < ChosenStage.length; i++)
        {
            FlappySegment segmentToCreate = ChosenStage.segments[Random.Range(0, ChosenStage.segments.Length)];
            CreateSegment(segmentToCreate);
        }
    }

    void CreateSegment(FlappySegment segment)
    {
        GameObject segmentReal = Instantiate(segment.segment, pointer.position, segment.segment.transform.rotation);
        segmentReal.transform.SetParent(levelObject.transform);
        if (segment.randomY)
        {
            float offset = (float)Random.Range(-25, 25) / 10;
            segmentReal.transform.position += new Vector3(0, offset, 0);
        }
        pointer.position += new Vector3(segment.distanceRight, 0, 0);
    }
}
