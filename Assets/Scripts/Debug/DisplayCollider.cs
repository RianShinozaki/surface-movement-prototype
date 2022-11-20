using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DisplayCollider : MonoBehaviour {
    public bool AlwaysDraw;
    public bool DrawWire;
    public Color Color = Color.white;

    BoxCollider coll;
    bool drawCache;

    private void Awake() {
        coll = GetComponent<BoxCollider>();
    }

    private void OnDrawGizmos() {
        if (!coll || !AlwaysDraw) {
            return;
        }

        DrawBox();
    }

    private void OnDrawGizmosSelected() {
        if (!coll || AlwaysDraw) {
            return;
        }
        DrawBox();
    }

    void DrawBox() {
        Gizmos.matrix = coll.transform.localToWorldMatrix;
        Gizmos.color = Color;
        if (DrawWire) {
            Gizmos.DrawWireCube(coll.center, coll.size);
        }
        else {
            Gizmos.DrawCube(coll.center, coll.size);
        }
    }
}