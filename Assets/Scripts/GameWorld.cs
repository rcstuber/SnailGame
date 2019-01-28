using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public GameObject groundMesh;

    public Weather weather;

    public float angularSpeed = 30.0f;

    public Vector2 randomLightningTime = new Vector2(5, 10);

    private float _untilNextLightning;

    private float _lastLightningTime = -1;

    void Start()
    {
        _untilNextLightning = Random.Range(randomLightningTime.x, randomLightningTime.y);
    }

    void Update()
    {
        if(!Game.Current.isRunning)
            return;
            
        var angles = groundMesh.transform.eulerAngles;
        angles.z += Time.deltaTime * angularSpeed;
        groundMesh.transform.eulerAngles = angles;

        if(Time.time - _lastLightningTime > _untilNextLightning)
        {
            weather?.StartLightning();
            _lastLightningTime = Time.time;
            _untilNextLightning = Random.Range(randomLightningTime.x, randomLightningTime.y);
        }
    }
}
