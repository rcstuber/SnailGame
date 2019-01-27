using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameWorld world;

    public Transform character;


    public float mashsPerMinute = 15;

    [HideInInspector]
    public float mashMultiplier = 1f;

    public float mashGain = 5;

    public bool isGainExponential = true;

    public float shellSlither = 2f;

    public float health = 100;

    [HideInInspector]
    public float totalDistance = 0;


    [Header("Sprites")]

    public GameObject root;

    public GameObject[] playerStates;

    private void SetState(int state)
    {
        for(int i = 0; i<playerStates.Length; i++)
        {
            playerStates[i].SetActive(i == state);
        }
    }


    public float worldAmplitude {
        get => amplitude;
    }

    // Temp vars

    private float _totalMashNum = 0;

    private float _startTime = 0;

    private float _initialHealth = 100;

    private Sprite defaultSprite;


    // Components

    private AudioSource audio;


    public ParticleSystem slimePS;

    public Animator animatorSnail;

    public Animator animatorHouse;


    void Start()
    {
        _initialHealth = health;
        _startTime = Time.time;
        audio = GetComponent<AudioSource>();
        slimePS.Stop();
        animatorSnail.enabled = false;
        animatorHouse.enabled = false;

        SetState(0);
    }

    private float amplitude {
        get {
            return Vector3.Angle(Vector3.up, character.position);
        }
    }

    private float requiredMashFrequency {
        get {
            var gain = mashGain * (isGainExponential ? (amplitude*amplitude / 8100f) : (amplitude / 90f));
            return (mashMultiplier * mashsPerMinute) + gain;
        }
    }

    private float forwardSpeed = 10.0f;

    private bool hasDied = false;


    private float _lastMashTime = 0;

    private bool _isInShell = false;

    private bool _isFirstLoop = true;


    void Update()
    {
        if(!Game.Current.isRunning)
            return;

        // Hide in shell
        if(Input.GetButtonDown("Jump") && !_isFirstLoop && health > 0)
        {
            _isInShell = !_isInShell;
            SetState(_isInShell ? 1 : 0);

            SoundManager.instance.PlaySound(SoundManager.instance.soundHide, 1, 1);
        }

        // Crawl forward
        if(!_isInShell && Input.GetButtonDown("Fire1") && health > 0)
        {
            _totalMashNum += 1;
            var curMashsPerMinute = 60.0f / (Time.time - _lastMashTime);

            if(!float.IsNaN(curMashsPerMinute))
            {
                forwardSpeed = world.angularSpeed * (curMashsPerMinute / requiredMashFrequency);
            }
            _lastMashTime = Time.time;

            if(!audio.isPlaying) {
                SoundManager.instance.PlaySound(SoundManager.instance.soundCrawl, 1, Mathf.Max(1f, forwardSpeed / world.angularSpeed * 0.8f));
            }

            slimePS.Play();
        }
        else {
            if(Time.time - _lastMashTime > 0.2)
            {
                forwardSpeed = 0;
                slimePS.Stop();
                animatorSnail.enabled = false;
                animatorHouse.enabled = false;
            }
        }

        if(health == 0) {
            forwardSpeed = 0;
        }

        // Move snail with world
        var deg = (_isInShell ? shellSlither : 1f) * world.angularSpeed - forwardSpeed;
        transform.Rotate(Vector3.forward * deg * Time.deltaTime);

        if(amplitude > 90.0)
        {
            SoundManager.instance.PlaySound(SoundManager.instance.soundSmashed, 1, 1);
            root.SetActive(false);
        }

        // Count distance
        if(forwardSpeed > 0)
        {
            totalDistance += (forwardSpeed * Time.deltaTime) / 1000f;
            animatorSnail.enabled = true;
            animatorHouse.enabled = true;
        }

        _isFirstLoop = false;
    }

    public void ResetPlayer()
    {
        _isInShell = false;
        _isFirstLoop = true;

        health = _initialHealth;
        totalDistance = 0;
        forwardSpeed = 0;
        transform.rotation = Quaternion.identity;

        root.SetActive(true);
        SetState(0);
    }

    public void OnHitByLightning() 
    {
        if(_isInShell)
            return;
            
        SetState(2);

        SoundManager.instance.PlaySound(SoundManager.instance.soundElectrocute, 0.6f, 1);
        slimePS.Stop();
        animatorSnail.enabled = false;
        animatorHouse.enabled = false;

        health = 0;
    }
}
