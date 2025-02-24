using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;

public enum SpecialStage {
    None,
    Shop,
    Sleep,
}

public class SelectionStage : MonoBehaviour
{
    private Transform trans;
    private GameObject nodeToGenerateOn;
    private FlappyStage levelToPlay;
    [SerializeField]
    private StageList stageList;
    [SerializeField]
    private AudioClip[] music;
    [SerializeField]
    private FlappyStage shopStage;
    [SerializeField]
    private Material[] materials;
    private List<FlappyStage> alreadyUsed = new List<FlappyStage>();
    private Pathfinding.Graph graph;
    private Vector3 transPos;
    private GameObject transCircle;
    private bool starting;
    private TextMeshProUGUI coinsAmount;
    private GameObject[] backgroundObjects;

    public event Action StartingNew;
    public event Action ClearedLevel;
    public AudioClip Beep;
    public AudioClip NodeAppearSound;
    public List<GameObject> everything;
    public GameObject[] nodes;

    public Image[] HeartSprites;
    public Sprite FullHeart;
    public Sprite EmptyHeart;

    private void Awake()
    {
        transCircle = GameObject.Find("Circle");
        trans = GameObject.Find("Transition").transform;
        nodeToGenerateOn = GameObject.Find("StartNode");
        graph = FindObjectOfType<Pathfinding.Graph>();
        coinsAmount = GameObject.Find("CoinsAmount").GetComponent<TextMeshProUGUI>();
        backgroundObjects = GameObject.FindGameObjectsWithTag("Aggregation");
    }

    void Start()
    {
        transPos = trans.position;
        coinsAmount.text = GameVariables.Instance.GetCoins() + "x";
        trans.DOMove(trans.position - new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
        GenerateNextNode();
        FindObjectOfType<AudioManager>().SetAudioSource();
        AudioManager.LevelMusic = music[UnityEngine.Random.Range(0, music.Length)];
        DisplayHearts();
        SetBackgroundImage();
        StartCoroutine(AudioManager.FadeOutAndPlayNew());
    }

    void GenerateNextNode()
    {
        alreadyUsed.Clear();
        nodeToGenerateOn.GetComponent<NodePoint>().Completed = true;

        GameObject newNode = Instantiate(nodes[UnityEngine.Random.Range(0, nodes.Length)]);
        Vector3 normalPos = nodeToGenerateOn.transform.position + new Vector3(1.5f, 0);
        newNode.transform.position = normalPos + new Vector3(0, 50);
        newNode.transform.DOMove(normalPos, 2.5f).OnComplete(() => {
            GameVariables.Instance.SetInteractSelect(true);
        });

        Transform startNode = null;

        LineRenderer nodeLine = nodeToGenerateOn.GetComponent<LineRenderer>();
        foreach (Transform item in newNode.transform)
        {
            if (item.name == "LineThing")
            {
                continue;
            }
            NodePoint nodeScript = item.GetComponent<NodePoint>();
            if (nodeScript.special == SpecialStage.Shop)
            {
                nodeScript.shopIndex = GameVariables.Instance.GetShops().Count;
                GameVariables.Instance.GenerateAShop();
                nodeScript.StartNode(shopStage);
            }
            graph.nodes.Add(item.GetComponent<Pathfinding.Node>());
            if (item.CompareTag("Main"))
            {
                startNode = item;
            }
            if (!nodeScript.Empty && nodeScript.special == SpecialStage.None)
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

    void SetBackgroundImage()
    {
        Material chosen = materials[UnityEngine.Random.Range(0, materials.Length)];
        foreach (GameObject item in backgroundObjects)
        {
            item.GetComponent<MeshRenderer>().material = chosen;
        }
    }

    public void SetSelectedNode(FlappyStage nodeSelected, GameObject newNode)
    {
        nodeToGenerateOn = newNode;
        levelToPlay = nodeSelected;
        GameVariables.Instance.SetLevel(levelToPlay);
    }

    public void StartTheGame()
    {
        GameVariables.Instance.SetSpecialLevel("");
        if (levelToPlay.levelName == "Flappycopter")
        {
            GameVariables.Instance.SetSpecialLevel("Shop");
        }
        GameVariables.Instance.SetInteractSelect(false);
        GameObject.Find("Lives").GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        GameObject.Find("Coin").GetComponent<Image>().DOFade(0, 0.5f);
        StartingNew?.Invoke();
        coinsAmount.DOFade(0, 0.5f);
        if (starting)
        {
            return;
        }

        AudioManager.SimpleFadeOut();

        //transCircle.transform.position = GameObject.Find("SelectionBird").transform.position;
        starting = true;
        transCircle.transform.DOScale(new Vector3(0, 0, 0), 2).OnComplete(() => {
            SwitchScene(false);
            Invoke("GoNext", 1);
        });
    }

    private void GoNext()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    private void SpawnNext() {
        GenerateNextNode();
    }

    private void DisplayHearts()
    {
        for (int i = 0; i < HeartSprites.Length; i++)
        {
            if (i < GameVariables.Instance.GetHealth())
            {
                HeartSprites[i].sprite = FullHeart;
            }
            else
            {
                HeartSprites[i].sprite = EmptyHeart;
            }

            if (i < GameVariables.Instance.GetContainers())
            {
                HeartSprites[i].enabled = true;
            }
            else
            {
                HeartSprites[i].enabled = false;
            }
        }
    }

    public void LevelCleared()
    {
        GameVariables.Instance.SetInteractSelect(false);
        coinsAmount.text = GameVariables.Instance.GetCoins() + "x";
        SwitchScene(true);
        starting = false;
        GameObject.Find("Circle").transform.localScale = new Vector3(3.2f, 2.75f, 1.35f);
        trans.position = transPos;
        GameObject.Find("InfoFrame").transform.position -= new Vector3(100, 0);
        AudioManager.LevelMusic = music[UnityEngine.Random.Range(0, music.Length)];
        GameObject.FindGameObjectWithTag("Useless").GetComponent<AudioManager>().SetAudioSource();
        StartCoroutine(AudioManager.FadeOutAndPlayNew());
        trans.DOMove(trans.position - new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
        ClearedLevel?.Invoke();
        SetBackgroundImage();
        if (GameVariables.Instance.GetSpecialLevel() == "")
        {
            Invoke("SpawnNext", 2);
            return;
        }
        GameVariables.Instance.SetInteractSelect(true);
    }

    void SwitchScene(bool state)
    {
        if (!state)
        {
            everything = new List<GameObject>(FindObjectsOfType<GameObject>());
            foreach (GameObject e in everything)
            {
                if (e.CompareTag("Sound"))
                {
                    everything.Remove(e);
                }
            }
        }
        foreach (GameObject obj in everything)
        {
            if (obj == gameObject || obj.name == "[DOTween]" || obj.name == "Firebase Services")
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
            FlappyStage selected = stageList.stages[UnityEngine.Random.Range(0, stageList.stages.Length)];
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
}
