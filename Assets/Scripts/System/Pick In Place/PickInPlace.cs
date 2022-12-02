using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PickInPlace : MonoBehaviour {
    public int GetRandom(int MaxValue) => Random.Range(0, MaxValue);

    Vector3 lastPos;
    Quaternion lastRot;

    private void Update() {
        if (transform.position == lastPos && transform.rotation == lastRot) {
            return;
        }

        OnRandomize();
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    public virtual void OnRandomize() { }
}