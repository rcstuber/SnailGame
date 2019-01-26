using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public GameObject groundMesh;

    public Weather weather;

    public float angularSpeed = 30.0f;


    private float _lastLightningTime = -1;

    void Start()
    {
        
    }

    void Update()
    {
        if(!Game.Current.isRunning)
            return;
            
        var angles = groundMesh.transform.eulerAngles;
        angles.z += Time.deltaTime * angularSpeed;
        groundMesh.transform.eulerAngles = angles;

        if(Time.time - _lastLightningTime > 5)
        {
            weather?.StartLightning();
            _lastLightningTime = Time.time;
        }

    }
}
