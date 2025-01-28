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
        AudioManager.LevelMusic = music[Random.Range(0, music.Length)];
        StartCoroutine(AudioManager.FadeOutAndPlayNew());
    }

    void GenerateNextNode()
    {
        GameObject newNode = Instantiate(nodes[Random.Range(0, nodes.Length)]);
        newNode.transform.position = nodeToGenerateOn.transform.position + new Vector3(1.5f, 0);

        Transform startNode = null;

        LineRenderer nodeLine = nodeToGenerateOn.GetComponent<LineRenderer>();
        foreach (Transform item in newNode.transform)
        {
            Node nodeScript = item.GetComponent<Node>();
            if (item.CompareTag("Main"))
            {
                startNode = item;
            }
            if (!nodeScript.Empty)
            {
                nodeScript.StartNode(GrabRandomStage());
            }
        }
        nodeLine.SetPosition(0, nodeToGenerateOn.transform.position);
        nodeLine.SetPosition(1, startNode.position);
    }

    public void SetSelectedNode(FlappyStage nodeSelected)
    {
        levelToPlay = nodeSelected;
    }

    public void StartTheGame() 
    { 
        GameVariables.Instance.SetLevel(levelToPlay);
        SceneManager.LoadScene(2);
    }
    private FlappyStage GrabRandomStage()
    {
        int chapter = GameVariables.Instance.GetChapter();
        FlappyStage flappyStage = null;
        while (flappyStage == null) {
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
                if (selected != null) { 
                    //alreadyUsed.Add(selected);
                    flappyStage = selected;
                }
            }
        }
        print(flappyStage);
        return flappyStage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
