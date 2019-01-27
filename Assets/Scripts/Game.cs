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

        public float lightningCount;
        public Vector2 lightningSpawnTime;

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

    public BlitzSpawn lightningSpawner;


    [Header("Gameplay Settings")]

    public List<Stage> stages;


    [Header("Sounds")]

    public AudioClip soundGameOver;

    public AudioClip soundGameVictory;

    public AudioClip soundLevelProgression;


    [Header("Managers")]

    public HighscoreManager highscoreManager;

    public bool isRunning = false;


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

        menu.SetActive(true);
        gameOverMenu.SetActive(false);
        gameVictoryMenu.SetActive(false);
        hud.gameObject.SetActive(false);
    }

    void Update()
    {
        if(isRunning)
        {
            if(player.worldAmplitude > 90f || player.health <= 0) {
                StartCoroutine(GameOver());
            } else {
                UpdateStageDifficulty(activeStage);

                if(player.totalDistance > activeStage.distanceGoal)
                {
                    var stageIndex = stages.IndexOf(activeStage);
                    if(stageIndex + 1 < stages.Count) {
                        activeStage = stages[stageIndex + 1];
                        player.totalDistance = 0;

                        audio.PlayOneShot(soundLevelProgression);
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
        if( highscoreManager.AddHighscore(player.totalDistance) )
        {
            //highscoreManager.Show(true);
            //yield break;
        }

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
        highscoreManager.AddHighscore(player.totalDistance);


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


    public void UpdateStageDifficulty(Stage stage)
    {
        world.angularSpeed = _defaultWorldRotationSpeed * stage.difficulty;
        player.mashMultiplier = stage.difficulty;

        lightningSpawner.atSameTime = stage.lightningCount;
        lightningSpawner.waitMinTime = stage.lightningSpawnTime.x;
        lightningSpawner.WaitMaxTime = stage.lightningSpawnTime.y;
    }
}
