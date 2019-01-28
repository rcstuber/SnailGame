using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public MeshRenderer rainRenderer;

    public Light lightning;

    public float rainSpeed = 0.05f;

    public float lightningIntensity = 10f;
    public float lightningDuration = 1.5f;


    // Tmp vars

    private Vector2 rainOffset = Vector2.zero;

    private float _lightingStartTime = -1;


    void Start()
    {
        lightning.intensity = 0;
    }

    void Update()
    {
        rainOffset.y += rainSpeed;
        var mat = rainRenderer.material;
        mat.mainTextureOffset = rainOffset;

        float lightningInProgress = Time.time - _lightingStartTime;
        if(_lightingStartTime > 0 && lightningInProgress < lightningDuration) {
            lightning.intensity = lightningIntensity * Mathf.Sin( (lightningInProgress / lightningDuration) * Mathf.PI );
        } else {
            lightning.intensity = 0;
            _lightingStartTime = -1;
        }
    }

    public void StartLightning()
    {
        _lightingStartTime = Time.time;

        SoundManager.instance.PlaySound(SoundManager.instance.soundThunder, 1f, 1);
    }
}
