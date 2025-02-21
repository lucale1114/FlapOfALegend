using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class GameInterface : MonoBehaviour
{
    [SerializeField]
    private Transform trans;
    private BirdHealth health;
    private PipeExit winCondition;
    private static Image flash;
    [SerializeField]
    private bool isInGame;
    private Transform deathscreen;
    private Image deathFade;
    private Slider realProgress;
    private TextMeshProUGUI meterText;
    private float distance;
    private float displayMeters;
    private Vector3 transSaved;
    [SerializeField]
    private AudioClip tickSound;
    [SerializeField]
    private AudioClip woosh;
    private float tickMod = 0.8f;
    private int displayNumberOld = 0;
    private bool exiting;
    private GameObject pauseScreen;

    public Image[] HeartSprites;
    public Sprite FullHeart;
    public Sprite EmptyHeart;

    private void Awake()
    {
        
        health = FindObjectOfType<BirdHealth>();
        flash = GameObject.Find("Flash").GetComponent<Image>();
        deathscreen = GameObject.Find("DeathScreen").transform;
        deathFade = GameObject.Find("Darkening").GetComponent<Image>();
        realProgress = GameObject.Find("RealProgress").GetComponent<Slider>();
        meterText = GameObject.Find("Meter").GetComponent<TextMeshProUGUI>();
        pauseScreen = GameObject.Find("PauseScreen");
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseScreen.SetActive(false);
        transSaved = trans.position;
        deathscreen.transform.position -= new Vector3(0, 2000, 0);
        trans.DOMove(trans.position + new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
        if (GameVariables.Instance.GetSpecialLevel() == "Shop")
        {
            return;
        }
        DisplayHearts();
        health.HealthChanged += () =>
        {
            DisplayHearts();
        };
        health.Died += () =>
        {
            Invoke("LostGame", 2);
        };
        Invoke("FindDelay", 1);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    void LostGame()
    {
        deathFade.DOFade(0.5f, 0.25f).OnComplete(() =>
        {
            deathFade.raycastTarget = true;
            AudioManager.PlaySound(woosh, 1f, 0.6f);
            deathscreen.DOMove(deathscreen.transform.position + new Vector3(0, 2000, 0), 1).OnComplete(() => {
                float meters = (health.transform.position - winCondition.transform.position).magnitude;
                float calculatedDistance = 1 - (meters / distance);
                displayMeters = 0;
                distance = Mathf.RoundToInt(distance);
                DOTween.To(() => displayMeters, x => displayMeters = x, distance - meters, 2);
                InvokeRepeating("TweenNumberText", 0, 0.05f);
                realProgress.DOValue(calculatedDistance, 2);
            });
        });
    }

    void TweenNumberText()
    {
        int display = Mathf.RoundToInt(displayMeters);
        if (display > displayNumberOld)
        {
            tickMod += 0.02f;
            displayNumberOld = display;
            AudioManager.PlaySound(tickSound, 0.7f, tickMod);
        }
        meterText.text = display + "m/" + distance + "m";
    }

    public void ExitToMenu()
    {
        if (exiting)
        {
            return;
        }
        exiting = true;
        AudioManager.SimpleFadeOut();
        trans.DOMove(transSaved, 2).SetUpdate(true).OnComplete(() => {
            if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
            {
                SceneManager.UnloadSceneAsync(1);
            }
            GameVariables.Instance.ResetVariables();
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        });
    }

    void FindDelay()
    {
        winCondition = FindObjectOfType<PipeExit>();
        distance = (health.transform.position - winCondition.transform.position).magnitude;
        winCondition.WinState += () =>
        {
            AudioManager.SimpleFadeOut();
            Invoke("Transition", 1f);
        };
    }

    private void Transition()
    {
        if (health.Health == 0) { return; }
        trans.DOMove(trans.position - new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
    }

    private void DisplayHearts()
    {
        for (int i = 0; i < HeartSprites.Length; i++)
        {
            if (i < health.Health)
            {
                HeartSprites[i].sprite = FullHeart;
            }
            else
            {
                HeartSprites[i].sprite = EmptyHeart;
            }

            if (i < health.Containers)
            {
                HeartSprites[i].enabled = true;
            }
            else
            {
                HeartSprites[i].enabled = false;
            }
        }
    }

    public static void FlashImage(float colorStart, float fTime)
    {
        flash.color = new Color(1, 1, 1, colorStart);
        flash.DOColor(new Color(1, 1, 1, 0), fTime).SetEase(Ease.Linear).SetUpdate(true);
    }
}
