using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NodePoint : MonoBehaviour
{
    private FlappyStage nodeLevel;
    private SpriteRenderer nodeHead;
    private Transform anchor;
    private Transform nodeInterface;
    private SelectionStage stageScript;
    private Pathfinding.Follower pathfinding;

    public List<NodePoint> neighbors;
    public bool Empty;
    public bool Completed;

    private void OnMouseDown()
    {
        if (pathfinding.Moving)
        {
            return;
        }
        BringUpNodeInteface();
        pathfinding.StartFollow(pathfinding.currentNode, GetComponent<Pathfinding.Node>());
        pathfinding.currentNode = GetComponent<Pathfinding.Node>();
    }

    private void BringUpNodeInteface()
    {
        if (Empty || Completed) // || pathfinding.Moving)
        {
            nodeInterface.transform.DOScale(new Vector3(1, 0, 1), 0.5f).OnComplete(() => {
                //nodeInterface.gameObject.SetActive(false);
            });
            return;
        }
        nodeInterface.gameObject.SetActive(true);
        nodeInterface.position = transform.position + new Vector3(0, 2.5f);
        nodeInterface.transform.localScale = new Vector3(1, 0, 1);
        nodeInterface.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        nodeInterface.Find("LevelName").GetComponent<TextMeshProUGUI>().text = nodeLevel.levelName;
        nodeInterface.Find("ChapterDesc").GetComponent<TextMeshProUGUI>().text = "Chapter: " + nodeLevel.chapter;
        nodeInterface.Find("Description").GetComponent<TextMeshProUGUI>().text = nodeLevel.description;
        nodeInterface.Find("LevelThumbnail").GetComponent<Image>().sprite = nodeLevel.thumbnail;
        stageScript.SetSelectedNode(nodeLevel, gameObject);

        var pos = transform.position;
        pos.z = -10;
        Camera.main.transform.DOMove(pos + new Vector3(0, 0.5f), 1);
    }

    private void Awake()
    {
        if (!Empty) {
            nodeHead = transform.GetChild(1).GetComponent<SpriteRenderer>();
        }
        anchor = transform.Find("BirdAnchor");
        nodeInterface = GameObject.Find("InfoFrame").transform;
        stageScript = FindObjectOfType<SelectionStage>();
        pathfinding = FindObjectOfType<Pathfinding.Follower>();
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
