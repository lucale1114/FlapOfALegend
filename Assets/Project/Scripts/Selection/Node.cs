using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    private FlappyStage nodeLevel;
    private SpriteRenderer nodeHead;
    private Transform anchor;
    private Transform nodeInterface;
    private SelectionStage stageScript;

    public bool Empty;
    public bool Completed;

    private void OnMouseDown()
    {
        BringUpNodeInteface();
    }

    private void BringUpNodeInteface()
    {
        if (Empty)
        {
            return;
        }
        nodeInterface.position = transform.position + new Vector3(0, 2f);
        nodeInterface.Find("LevelName").GetComponent<TextMeshProUGUI>().text = nodeLevel.levelName;
        nodeInterface.Find("ChapterDesc").GetComponent<TextMeshProUGUI>().text = "Chapter: " + nodeLevel.chapter;
        nodeInterface.Find("Description").GetComponent<TextMeshProUGUI>().text = nodeLevel.description;
        nodeInterface.Find("LevelThumbnail").GetComponent<Image>().sprite = nodeLevel.thumbnail;
        stageScript.SetSelectedNode(nodeLevel);
    }

    private void Awake()
    {
        if (!Empty) {
            nodeHead = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        anchor = transform.Find("BirdAnchor");
        nodeInterface = GameObject.Find("InfoFrame").transform;
        stageScript = FindObjectOfType<SelectionStage>();
    }

    private void Start()
    {

    }

    public void StartNode(FlappyStage stage)
    {
        nodeLevel = stage;
        nodeHead.color = nodeLevel.pipeColor;
    }
}
