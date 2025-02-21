using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour
{
    private GameObject levelObject;
    private Transform pointer;
    private Transform bgPointer;
    [SerializeField]
    private GameObject endPipe;
    private Vector3 bgPointerPos;
    private GameObject bird;
    [SerializeField]
    private AudioClip pipeSound;
    [SerializeField]
    private bool IsMenu;
    [SerializeField]
    private FlappyStage ChosenStage;
    [SerializeField]
    private GameObject shopObject;

    private SpriteRenderer ground;
    private SpriteRenderer sky;
    private SpriteRenderer wall;
    private SpriteRenderer wallEnd;
    
    void Awake()
    {
        levelObject = GameObject.Find("Level");
        pointer = levelObject.transform.Find("Pointer");
        bgPointer = levelObject.transform.Find("BackgroundPointer");
        bird = GameObject.Find("Bird");
    }

    void Start()
    {
        GameVariables.Instance.SetInteractSelect(true);
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
        }
        if (GameVariables.Instance.GetSpecialLevel() == "Shop")
        {
            SetupShop();
            return;
        }
        if (GameVariables.Instance != null && !IsMenu)
        {
            print("set");
            ChosenStage = GameVariables.Instance.GetLevel();
        }
        bgPointerPos = bgPointer.position;
        GenerateLevel();
        AudioManager.LevelMusic = ChosenStage.music;
        if (bird.GetComponent<BirdMovement>().enabled == false)
        {
            StartCoroutine(MoveOut(bird.transform));
        }
    }

    void SetupShop()
    {
        sky = GameObject.Find("Sky").GetComponent<SpriteRenderer>();
        ground = GameObject.Find("Ground").GetComponent<SpriteRenderer>();
        wall = GameObject.Find("Brickwall11").GetComponent<SpriteRenderer>();
        wallEnd = GameObject.Find("Brickwall22").GetComponent<SpriteRenderer>();

        Destroy(wall.transform.parent.gameObject);
        Destroy(ground.gameObject);
        Destroy(bird);

        print(shopObject);
        GameObject newShop = Instantiate(shopObject);
        GameObject.Find("Lives").SetActive(false);
        GameObject.Find("ActiveItemTab").SetActive(false);

        Transform heli = newShop.transform.Find("Chinook");
        Transform fakeBird = newShop.transform.Find("Bird");
        heli.DOMoveX(0.1f, 4);
        fakeBird.DOMoveX(-0.15f, 3);
        FindObjectOfType<Shop>().Leaving += () =>
        {
            heli.DOMoveX(-8, 5);
            GameObject.Find("Helicopter").GetComponent<AudioSource>().DOFade(0, 2);
            fakeBird.DOMoveX(5, 5);
        };
    }

    IEnumerator MoveOut(Transform trans)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        {
            trans.DOMove(new Vector3(trans.position.x + 0.2f, 0, 0), 0.15f);
            AudioManager.PlaySound(pipeSound, 1, 0.8f);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.8f);
        StartLevel();
    }

    void DecorateLevel()
    {
        sky = GameObject.Find("Sky").GetComponent<SpriteRenderer>();
        ground = GameObject.Find("Ground").GetComponent<SpriteRenderer>();
        wall = GameObject.Find("Brickwall11").GetComponent<SpriteRenderer>();
        wallEnd = GameObject.Find("Brickwall22").GetComponent<SpriteRenderer>();

        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (var pipe in pipes)
        {
            pipe.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ChosenStage.pipeColor;
            pipe.transform.GetChild(1).GetComponent<SpriteRenderer>().color = ChosenStage.pipeColor;
            try
            {
                if (pipe.transform.GetChild(2))
                {
                    pipe.transform.GetChild(2).GetComponent<SpriteRenderer>().color = ChosenStage.pipeColor;
                }
            }
            catch(System.Exception e) { };  
        }

        sky.sprite = ChosenStage.sky;
        ground.sprite = ChosenStage.ground;
        wall.sprite = ChosenStage.wallSprite;
        wallEnd.sprite = ChosenStage.wallEndSprite;


        GameObject.Find("Brickwall").GetComponent<SpriteRenderer>().sprite = ChosenStage.wallSprite;
        GameObject.Find("Brickwall2").GetComponent<SpriteRenderer>().sprite = ChosenStage.wallEndSprite;
    }

    void StartLevel()
    {
        StartCoroutine(AudioManager.FadeOutAndPlayNew());
        bird.GetComponent<BirdMovement>().enabled = true;
    }

    void GenerateLevel()
    {
        
        for (int i = 0; i < ChosenStage.length; i++)
        {
            FlappySegment segmentToCreate = null;
            while (segmentToCreate == null)
            {
                segmentToCreate = PickASegment();
            }
            CreateSegment(segmentToCreate);
        }

        pointer.position += new Vector3(10, 0, 0);
        CreateBackgroundProps();

        GameObject newEnd = Instantiate(endPipe, new Vector3(pointer.position.x, 0, 0), endPipe.transform.rotation);
        newEnd.transform.SetParent(levelObject.transform);
        newEnd.transform.localPosition = new Vector3(pointer.position.x, 0, 0);

        newEnd.transform.Find("Brickbreak").GetComponent<ParticleSystem>().startColor = ChosenStage.brickbreakColor;

        if (!IsMenu)
        {
            DecorateLevel();
        }
    }

    FlappySegment PickASegment()
    {
        int randomChance = Random.Range(1, 100);
        FlappySegment segmentChosen = ChosenStage.segments[Random.Range(0, ChosenStage.segments.Length)];
        if (segmentChosen.commonFactor < randomChance)
        {
            return null;
        }
        return segmentChosen;
    }

    void CreateSegment(FlappySegment segment)
    {
        pointer.position += new Vector3(segment.distanceLeft, 0, 0);
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

        foreach (var item in ChosenStage.backgroundElements)
        {
            GameObject newObj = Instantiate(item);
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
