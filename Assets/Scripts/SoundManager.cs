using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    public AudioSource musicSource;

    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;
    public AudioSource audioSource5;


    [Header("Music")]
    [Space]
    public AudioClip music01;

    [Header("Sound")]
    [Space]
    public AudioClip soundCrawl;
    public AudioClip soundHide;
    public AudioClip soundSmashed;
    public AudioClip soundSlideOff;
    public AudioClip soundElectrocute;
    public AudioClip soundThunder;

    public AudioClip soundGameOver;
    public AudioClip soundGameVictory;
    public AudioClip soundLevelProgression;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        PlayMusic(music01, 0.8f);
    }

    //--------------------- Music ---------------------------

    public void PlayMusic(AudioClip musicClip, float volume = 1) {
        musicSource.loop = true;
        musicSource.clip = musicClip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic() {
        musicSource.Stop();
        musicSource.loop = false;
    }

    //--------------------- Sound ---------------------------

    public void PlaySound(AudioClip Clip, float volume = 1, float pitch = 1) {
        if (!audioSource1.isPlaying) {
            audioSource1.loop = false;
            audioSource1.clip = Clip;
            audioSource1.volume = volume;
            audioSource1.pitch = pitch;
            audioSource1.Play();
        }
        else if (!audioSource2.isPlaying) {
            audioSource2.loop = false;
            audioSource2.clip = Clip;
            audioSource2.volume = volume;
            audioSource2.pitch = pitch;
            audioSource2.Play();
        }
        else if (!audioSource3.isPlaying) {
            audioSource3.loop = false;
            audioSource3.clip = Clip;
            audioSource3.volume = volume;
            audioSource3.pitch = pitch;
            audioSource3.Play();
        }
        else if (!audioSource4.isPlaying) {
            audioSource4.loop = false;
            audioSource4.clip = Clip;
            audioSource4.volume = volume;
            audioSource4.pitch = pitch;
            audioSource4.Play();
        }
        else {
            audioSource5.loop = false;
            audioSource5.clip = Clip;
            audioSource5.volume = volume;
            audioSource5.pitch = pitch;
            audioSource5.Play();
        }
    }
}
