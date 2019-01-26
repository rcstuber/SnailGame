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


    [Header("Sounds")]

    public AudioClip soundCrawl;

    public AudioClip soundHide;

    public AudioClip soundSmashed;

    public AudioClip soundSlideOff;

    public AudioClip soundElectrocute;


    [Header("Sprites")]

    public Sprite spriteShelled;

    public Sprite spriteElectrocuted;


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

    private SpriteRenderer renderer;

    public ParticleSystem slimePS;

    public Animator animatorSnail;

    public Animator animatorHouse;


    void Start()
    {
        _initialHealth = health;
        _startTime = Time.time;
        renderer = GetComponentInChildren<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        slimePS.Stop();
        animatorSnail.enabled = false;
        animatorHouse.enabled = false;

        defaultSprite = renderer?.sprite;
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
            renderer.sprite = _isInShell ? spriteShelled : defaultSprite;

            /* 
            audio.pitch = 1;
            audio.PlayOneShot(soundHide);
            audio.clip = soundSlideOff;
            audio.PlayDelayed(soundHide.length);
            */

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
                //audio.PlayOneShot(soundCrawl);
                //audio.pitch = Mathf.Max(1f, forwardSpeed / world.angularSpeed * 0.8f);

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
            //audio.PlayOneShot(soundSmashed);
            SoundManager.instance.PlaySound(SoundManager.instance.soundSmashed, 1, 1);
            renderer.sprite = null;
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
        renderer.sprite = defaultSprite;
        transform.rotation = Quaternion.identity;
    }

    public void OnHitByLightning() 
    {
        renderer.sprite = spriteElectrocuted;
        //audio.pitch = 1f;
        //audio.PlayOneShot(soundElectrocute);
        SoundManager.instance.PlaySound(SoundManager.instance.soundElectrocute, 1, 1);
        slimePS.Stop();
        animatorSnail.enabled = false;
        animatorHouse.enabled = false;

        health = 0;
    }
}
