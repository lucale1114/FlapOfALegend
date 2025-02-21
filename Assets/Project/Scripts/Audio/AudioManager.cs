using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    private static AudioSource source;
    private BirdMovement bm;
    [SerializeField]
    private bool InSelection;
    private static float volumeMod = 0.5f;
    
    public static AudioClip LevelMusic;

    
    void Awake()
    {
        bm = FindObjectOfType<BirdMovement>();
    }

    public void SetAudioSource()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = 0.5f;
        if (!InSelection) { 
            bm.WokeUp += () => {
                StartCoroutine(FadeOutAndPlayNew());
            };
        }
    }

    public static void SimpleFadeOut()
    {
        source.DOFade(0, 2).SetUpdate(true);
    }

    public static IEnumerator FadeOutAndPlayNew()
    {
        source.DOFade(0, 1);
        yield return new WaitForSeconds(1.5f);
        source.clip = LevelMusic;
        source.Play();
        source.DOFade(1 * volumeMod, 2);
    }

    public static void StopMusic() {
        source.Stop();
    }

    public static void PlaySound(AudioClip sound, float volume, float pitch)
    {
        GameObject soundObject = new GameObject();
        soundObject.tag = "Sound";
        AudioSource source = soundObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.name = sound.name;
        source.volume = volume;
        source.pitch = pitch;
        if (pitch < 0)
        {
            source.time = source.clip.length - 0.05f;
        }
        source.Play();
        Destroy(soundObject, sound.length + 0.5f);
    }

    public static void PlaySound(AudioClip sound, float volume, float pitch, bool loop)
    {
        GameObject soundObject = new GameObject();
        soundObject.tag = "Sound";
        AudioSource source = soundObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.name = sound.name;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = true;
        if (pitch < 0)
        {
            source.time = source.clip.length - 0.05f;
        }
        source.Play();
        if (!loop)
        {
            Destroy(soundObject, sound.length + 0.5f);
        }
    }
}
