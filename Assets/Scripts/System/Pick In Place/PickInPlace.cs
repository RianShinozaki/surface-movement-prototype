using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickInPlace : MonoBehaviour {
    public bool Lock;
    public int GetRandom(int MaxValue) => Random.Range(0, MaxValue);

    Vector3 lastPos;
    Quaternion lastRot;

    private void Awake() {
        if (Application.isPlaying) {
            Destroy(this);
        }
    }

    private void Update() {
        if ((transform.position == lastPos && transform.rotation == lastRot) || Lock) {
            return;
        }

        OnRandomize();
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    public virtual void OnRandomize() { }
}