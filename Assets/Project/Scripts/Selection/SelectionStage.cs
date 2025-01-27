using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectionStage : MonoBehaviour
{
    private Transform trans;
    private GameObject nodeToGenerateOn;

    public GameObject[] nodes;

    private void Awake()
    {
        trans = GameObject.Find("Transition").transform;
        nodeToGenerateOn = GameObject.Find("Node");
    }

    void Start()
    {
        trans.DOMove(trans.position - new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
        GenerateNextNode();
    }

    void GenerateNextNode()
    {
        GameObject newNode = Instantiate(nodes[Random.Range(0, nodes.Length)]);
        newNode.transform.position = nodeToGenerateOn.transform.position + new Vector3(1.5f, 0);

        Transform startNode = null;

        LineRenderer nodeLine = nodeToGenerateOn.GetComponent<LineRenderer>();
        foreach (Transform item in newNode.transform)
        {
            if (item.CompareTag("Main"))
            {
                startNode = item;
            }
        }
        nodeLine.SetPosition(0, nodeToGenerateOn.transform.position);
        nodeLine.SetPosition(1, startNode.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
