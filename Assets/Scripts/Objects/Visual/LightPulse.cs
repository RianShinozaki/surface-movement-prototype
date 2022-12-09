using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightPulse : MonoBehaviour {
    public float MidLevel;
    public float Amplitude;
    public float Speed;

    new Light light;

    private void Awake() {
        light = GetComponent<Light>();
    }

    private void Update() {
        light.range = MidLevel + (Mathf.Sin(Time.time * Speed) * Amplitude);
    }
}
