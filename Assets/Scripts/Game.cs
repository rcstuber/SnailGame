using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Game : MonoBehaviour
{
    public static Game Current {
        get => _instance;
    }
    private static Game _instance;


    public GameWorld world;

    public Player player;

    public GameObject menu;


    [Header("Sounds")]

    public AudioClip soundGameOver;

    public bool isRunning = false;

    public float difficulty = 1f;

    private float _defaultWorldRotationSpeed = 0;

    // Components

    private AudioSource audio;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        _defaultWorldRotationSpeed = world.angularSpeed;
    }

    void Update()
    {
        if(isRunning)
        {
            SetDifficulty(difficulty);

            if(player.worldAmplitude > 90f) {
                audio.PlayOneShot(soundGameOver);
                
                player.ResetPlayer();
                menu.SetActive(true);
                isRunning = false;
            }

        } else {
            if(Input.anyKeyDown)
            {
                player.ResetPlayer();
                menu.SetActive(false);
                isRunning = true;
            }
        }
    }

    public void SetDifficulty(float difficulty)
    {
        world.angularSpeed = _defaultWorldRotationSpeed * difficulty;
        player.mashMultiplier = difficulty;
    }
}
