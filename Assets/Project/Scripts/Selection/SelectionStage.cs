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
    private Vector3 transPos;

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
        transPos = trans.position;
        trans.DOMove(trans.position - new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
        GenerateNextNode();
        AudioManager.LevelMusic = music[Random.Range(0, music.Length)];
        StartCoroutine(AudioManager.FadeOutAndPlayNew());
    }

    void GenerateNextNode()
    {
        alreadyUsed.Clear();
        nodeToGenerateOn.GetComponent<NodePoint>().Completed = true;

        GameObject newNode = Instantiate(nodes[Random.Range(0, nodes.Length)]);
        Vector3 normalPos = nodeToGenerateOn.transform.position + new Vector3(1.5f, 0);
        newNode.transform.position = normalPos + new Vector3(0, 50);
        newNode.transform.DOMove(normalPos, 2.5f);

        Transform startNode = null;

        LineRenderer nodeLine = nodeToGenerateOn.GetComponent<LineRenderer>();
        foreach (Transform item in newNode.transform)
        {
            if (item.name == "LineThing")
            {
                continue;
            }
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
        DOTween.To(() => nodeLine.GetPosition(0), (x) => nodeLine.SetPosition(1, x), normalPos, 2.25f).Play();
        NodePoint oldNode = nodeToGenerateOn.GetComponent<NodePoint>();

        oldNode.neighbors.Add(startNode.GetComponent<NodePoint>());
        oldNode.GetComponent<Pathfinding.Node>().m_Connections.Add(oldNode.neighbors[oldNode.neighbors.Count - 1].GetComponent<Pathfinding.Node>());

        startNode.GetComponent<NodePoint>().neighbors.Add(oldNode);
        startNode.GetComponent<Pathfinding.Node>().m_Connections.Add(startNode.GetComponent<NodePoint>().neighbors[startNode.GetComponent<NodePoint>().neighbors.Count - 1].GetComponent<Pathfinding.Node>());
    }

    public void SetSelectedNode(FlappyStage nodeSelected, GameObject newNode)
    {
        nodeToGenerateOn = newNode;
        levelToPlay = nodeSelected;
        GameVariables.Instance.SetLevel(levelToPlay);
    }

    public void StartTheGame()
    {
        AudioManager.SimpleFadeOut();
        SwitchScene(false);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    private void SpawnNext() {
        GenerateNextNode();
    }
    public void LevelCleared()
    {
        SwitchScene(true);
        trans.position = transPos;
        GameObject.Find("InfoFrame").transform.position -= new Vector3(100, 0);
        FindObjectOfType<AudioManager>().SetAudioSource();
        AudioManager.LevelMusic = music[Random.Range(0, music.Length)];
        StartCoroutine(AudioManager.FadeOutAndPlayNew());
        trans.DOMove(trans.position - new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
        Invoke("SpawnNext", 2);
    }

    void SwitchScene(bool state)
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
