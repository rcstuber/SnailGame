using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenery : MonoBehaviour
{
    public GameWorld world;

    public float parallaxFactor = 0.3f;

    void Start()
    {
        
    }

    void Update()
    {
        if(Game.Current.isRunning)
        {
            transform.Rotate(Vector3.forward, world.angularSpeed * parallaxFactor * Time.deltaTime);
        }
    }
}
