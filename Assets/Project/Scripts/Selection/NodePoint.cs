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
    private SpriteRenderer selectionSprite;

    public List<NodePoint> neighbors;
    public bool Empty;
    public bool Completed;

    private void OnMouseDown()
    {
        if (!GameVariables.Instance.CanInteractSelect())
        {
            return;
        }
        AudioManager.PlaySound(stageScript.Beep, 0.3f, 0.9f);
        if (pathfinding.Moving)
        {
            return;
        }
        pathfinding.StartFollow(pathfinding.currentNode, GetComponent<Pathfinding.Node>());
        pathfinding.currentNode = GetComponent<Pathfinding.Node>();
        BringUpNodeInteface();
    }

    private void BringUpNodeInteface()
    {
        if (Empty || Completed) // || pathfinding.Moving)
        {
            if (Completed)
            {
                selectionSprite.enabled = true;
            }
            nodeInterface.transform.DOScale(new Vector3(1, 0, 1), 0.5f).OnComplete(() => {
                //nodeInterface.gameObject.SetActive(false);
            });
            return;
        }
        selectionSprite.enabled = true;
        AudioManager.PlaySound(stageScript.NodeAppearSound, 0.5f, 0.7f);
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
            selectionSprite = transform.GetChild(2).GetComponent<SpriteRenderer>();
        }
        anchor = transform.Find("BirdAnchor");
        nodeInterface = GameObject.Find("InfoFrame").transform;
        stageScript = FindObjectOfType<SelectionStage>();
        pathfinding = FindObjectOfType<Pathfinding.Follower>();
    }

    private void Start()
    {
        if (!Empty)
        {
            if (gameObject.name != "StartNode")
            {
                selectionSprite.enabled = false;
            }
            pathfinding.Travelling += () => {
                if (pathfinding.currentNode != gameObject)
                {
                    selectionSprite.enabled = false;
                }
            };
        }
    }

    public void StartNode(FlappyStage stage)
    {
        nodeLevel = stage;
        nodeHead.color = nodeLevel.pipeColor;
    }
}
