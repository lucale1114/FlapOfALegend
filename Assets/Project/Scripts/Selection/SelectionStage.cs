using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SelectionStage : MonoBehaviour
{
    private Transform trans;
    private GameObject nodeToGenerateOn;
    private FlappyStage levelToPlay;
    [SerializeField]
    private StageList stageList;
    [SerializeField]
    private AudioClip[] music;
    private List<FlappyStage> alreadyUsed = new List<FlappyStage>();
    private Pathfinding.Graph graph;

    public GameObject[] everything;
    public GameObject[] nodes;

    private void Awake()
    {
        trans = GameObject.Find("Transition").transform;
        nodeToGenerateOn = GameObject.Find("StartNode");
        graph = FindObjectOfType<Pathfinding.Graph>();
    }

    void Start()
    {
        trans.DOMove(trans.position - new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
        GenerateNextNode();
        AudioManager.LevelMusic = music[Random.Range(0, music.Length)];
        StartCoroutine(AudioManager.FadeOutAndPlayNew());
        for (int i = 0; i < 5; i++)
        {
            GenerateNextNode();
        }
    }

    void GenerateNextNode()
    {
        alreadyUsed.Clear();
        GameObject newNode = Instantiate(nodes[Random.Range(0, nodes.Length)]);
        newNode.transform.position = nodeToGenerateOn.transform.position + new Vector3(1.5f, 0);

        Transform startNode = null;

        LineRenderer nodeLine = nodeToGenerateOn.GetComponent<LineRenderer>();
        foreach (Transform item in newNode.transform)
        {
            NodePoint nodeScript = item.GetComponent<NodePoint>();
            graph.nodes.Add(item.GetComponent<Pathfinding.Node>());
            if (item.CompareTag("Main"))
            {
                startNode = item;
            }
            if (!nodeScript.Empty)
            {
                nodeScript.StartNode(GrabRandomStage());
            }
            foreach (NodePoint n in item.GetComponent<NodePoint>().neighbors)
            {
                item.GetComponent<Pathfinding.Node>().m_Connections.Add(n.GetComponent<Pathfinding.Node>());
            }
        }
        nodeLine.SetPosition(0, nodeToGenerateOn.transform.position);
        nodeLine.SetPosition(1, startNode.position);
        NodePoint oldNode = nodeToGenerateOn.GetComponent<NodePoint>();

        nodeToGenerateOn = newNode.transform.Find("Node").gameObject;

        oldNode.neighbors.Add(startNode.GetComponent<NodePoint>());
        oldNode.GetComponent<Pathfinding.Node>().m_Connections.Add(oldNode.neighbors[oldNode.neighbors.Count - 1].GetComponent<Pathfinding.Node>());

        startNode.GetComponent<NodePoint>().neighbors.Add(oldNode);
        startNode.GetComponent<Pathfinding.Node>().m_Connections.Add(startNode.GetComponent<NodePoint>().neighbors[startNode.GetComponent<NodePoint>().neighbors.Count - 1].GetComponent<Pathfinding.Node>());
    }

    public void SetSelectedNode(FlappyStage nodeSelected)
    {
        levelToPlay = nodeSelected;
        GameVariables.Instance.SetLevel(levelToPlay);
    }

    public void StartTheGame()
    {
        AudioManager.SimpleFadeOut();
        SwitchScene(false);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    public void SwitchScene(bool state)
    {
        if (!state)
        {
            everything = FindObjectsOfType<GameObject>();
        }
        foreach (GameObject obj in everything)
        {
            if (obj == gameObject || obj.name == "[DOTween]")
            {
                continue;
            }
            obj.SetActive(state);
        }
    }

    private FlappyStage GrabRandomStage()
    {
        int chapter = GameVariables.Instance.GetChapter();
        FlappyStage flappyStage = null;
        while (flappyStage == null)
        {
            FlappyStage selected = stageList.stages[Random.Range(0, stageList.stages.Length)];
            if (selected.chapter == chapter)
            {
                foreach (FlappyStage checkUsed in alreadyUsed)
                {
                    if (checkUsed == selected)
                    {
                        selected = null;
                        break;
                    }
                }
                if (selected != null)
                {
                    alreadyUsed.Add(selected);
                    flappyStage = selected;
                }
            }
        }
        return flappyStage;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
