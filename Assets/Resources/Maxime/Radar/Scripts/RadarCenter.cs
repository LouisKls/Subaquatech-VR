using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCenter : MonoBehaviour
{

    public float increment;
    public float maxIntensity;
    private float initialIntensity;
    private float intensity;
    Light radarLight;

    void Start()
    {
        radarLight = GetComponent<Light>();
        initialIntensity = radarLight.intensity;
    }

    void Update()
    {
        intensity += increment;
        if(intensity > maxIntensity) intensity = initialIntensity;
        radarLight.intensity = intensity;
    }
}
