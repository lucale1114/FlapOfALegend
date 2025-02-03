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
        source.DOFade(0, 2);
    }

    public static IEnumerator FadeOutAndPlayNew()
    {
        source.DOFade(0, 1);
        yield return new WaitForSeconds(1.5f);
        source.clip = LevelMusic;
        source.Play();
        source.DOFade(1, 2);
    }

    public static void StopMusic() {
        source.Stop();
    }

    public static void PlaySound(AudioClip sound, float volume, float pitch)
    {
        GameObject soundObject = Instantiate(new GameObject());
        AudioSource source = soundObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(soundObject, sound.length + 0.5f);
    }
}
