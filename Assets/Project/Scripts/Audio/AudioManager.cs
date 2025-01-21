using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    private AudioSource source;
    private BirdMovement bm;

    public AudioClip LevelMusic;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        bm = FindObjectOfType<BirdMovement>();
    }

    void Start()
    {
        bm.WokeUp += () => {
            StartCoroutine(FadeOutAndPlayNew());
        };
    }

    public IEnumerator FadeOutAndPlayNew()
    {
        source.DOFade(0, 1);
        yield return new WaitForSeconds(1.5f);
        source.clip = LevelMusic;
        source.Play();
        source.DOFade(1, 1);
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
