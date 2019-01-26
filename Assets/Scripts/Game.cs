using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Game : MonoBehaviour
{

    [Serializable]
    public class Stage
    {
        public float distanceGoal;
        public float difficulty;

        public Color stageColor;
    }
    
    public static Game Current {
        get => _instance;
    }
    private static Game _instance;


    public GameWorld world;

    public Player player;

    public GameObject menu;

    public GameObject gameOverMenu;

    public GameObject gameVictoryMenu;

    public HUD hud;


    [Header("Gameplay Settings")]

    public List<Stage> stages;


    [Header("Sounds")]

    public AudioClip soundGameOver;

    public AudioClip soundGameVictory;


    public bool isRunning = false;

    public float difficulty = 1f;

    private float _defaultWorldRotationSpeed = 0;

    private Stage activeStage;

    // Components

    private AudioSource audio;

    void Awake()
    {
        _instance = this;
        activeStage = stages[0];
    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        _defaultWorldRotationSpeed = world.angularSpeed;

        gameOverMenu.SetActive(false);
        gameVictoryMenu.SetActive(false);
        hud.gameObject.SetActive(false);
    }

    void Update()
    {
        if(isRunning)
        {
            SetDifficulty(activeStage.difficulty);

            if(player.worldAmplitude > 90f || player.health <= 0) {
                StartCoroutine(GameOver());
            } else {
                if(player.totalDistance > activeStage.distanceGoal)
                {
                    var stageIndex = stages.IndexOf(activeStage);
                    if(stageIndex + 1 < stages.Count) {
                        activeStage = stages[stageIndex + 1];
                        player.totalDistance = 0;
                    } else {
                        StartCoroutine(GameVictory());
                        return;
                    }
                }

                hud.distanceGoal = activeStage.distanceGoal;
                hud.distanceMeter = player.totalDistance;
                hud.distanceMeterColor = activeStage.stageColor;
            }

        } else if(menu.activeSelf) {
            if(Input.anyKeyDown)
            {
                player.ResetPlayer();
                menu.SetActive(false);
                hud.gameObject.SetActive(true);
                isRunning = true;
            }
        }
    }

    IEnumerator GameOver()
    {
        isRunning = false;
        audio.PlayOneShot(soundGameOver);

        hud.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);

        yield return new WaitForSeconds(2.5f);
                
        player.ResetPlayer();
        activeStage = stages[0];

        gameOverMenu.SetActive(false);
        menu.SetActive(true);
    }

    IEnumerator GameVictory()
    {
        isRunning = false;
        audio.PlayOneShot(soundGameVictory);

        hud.gameObject.SetActive(false);
        gameVictoryMenu.SetActive(true);

        yield return new WaitForSeconds(10f);
                
        player.ResetPlayer();
        activeStage = stages[0];

        gameVictoryMenu.SetActive(false);
        menu.SetActive(true);
    }


    public void SetDifficulty(float difficulty)
    {
        world.angularSpeed = _defaultWorldRotationSpeed * difficulty;
        player.mashMultiplier = difficulty;
    }
}
