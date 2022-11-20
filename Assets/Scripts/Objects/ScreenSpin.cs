using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpin : MonoBehaviour {
    public float Speed;
    public AnimationCurve SpinCurve;

    Camera cam;
    WrappedValue spinTime;

    private void Awake() {
        cam = Camera.main;
        spinTime = new WrappedValue(0f, 1f);
    }

    private void Update() {
        Vector3 camRot = cam.transform.eulerAngles;
        spinTime += Time.deltaTime * Speed;

        transform.rotation = Quaternion.Euler(-90f, camRot.y + (SpinCurve.Evaluate(spinTime) * 360f), 0f);
    }
}