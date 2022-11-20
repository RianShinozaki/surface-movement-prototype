using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveMove : MonoBehaviour {
    public Transform Object;
    public Vector3 Offset;
    public Vector3 Direction;
    public float Speed;
    public float Amplitude;

    private void Update() {
        Object.localPosition = (Direction.normalized * Mathf.Sin(Time.time * Speed) * Amplitude) + Offset;
    }
}