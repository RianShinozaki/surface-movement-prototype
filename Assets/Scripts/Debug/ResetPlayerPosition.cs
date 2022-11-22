using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour {
#if UNITY_EDITOR
    Vector3 pos;

    private void Awake() {
        pos = transform.position;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            transform.position = pos;
        }
    }
#endif
}