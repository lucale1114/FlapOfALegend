using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    private GameObject levelObject;
    private Transform pointer;
    private Transform bgPointer;
    [SerializeField]
    private GameObject endPipe;
    private Vector3 bgPointerPos;

    public FlappyStage ChosenStage;

    void Awake()
    {
        levelObject = GameObject.Find("Level");
        pointer = levelObject.transform.Find("Pointer");
        bgPointer = levelObject.transform.Find("BackgroundPointer");
    }

    void Start()
    {
        bgPointerPos = bgPointer.position;
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int i = 0; i < ChosenStage.length; i++)
        {
            FlappySegment segmentToCreate = ChosenStage.segments[Random.Range(0, ChosenStage.segments.Length)];
            CreateSegment(segmentToCreate);
        }

        pointer.position += new Vector3(10, 0, 0);
        CreateBackgroundProps();

        GameObject newEnd = Instantiate(endPipe, new Vector3(pointer.position.x, 0, 0), endPipe.transform.rotation);
        newEnd.transform.SetParent(levelObject.transform);
        newEnd.transform.localPosition = new Vector3(pointer.position.x, 0, 0);
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
    void CreateBackgroundProps()
    {
        if (ChosenStage.cloud != null)
        {
            PaintClouds(ChosenStage.cloud);
            bgPointer.position = bgPointerPos;
        }
    }

    void PaintClouds(GameObject cloud)
    {
        for (int i = 0; i < ChosenStage.length * 6; i++) {
            GameObject floatingCloud = Instantiate(cloud);
            floatingCloud.transform.position = bgPointer.position + new Vector3(0, 2 + (float)Random.Range(-25, 25)/10);
            bgPointer.position += new Vector3((float)Random.Range(10, 40)/10, 0);
        }
    }
}
