using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DisplayCollider : MonoBehaviour {
    public bool AlwaysDraw;
    public bool DrawWire;
    public Color Color = Color.white;

    BoxCollider[] boxes;
    SphereCollider[] spheres;
    bool drawCache;

    private void Awake() {
        PopulateColliderArrays();
    }

    private void OnDrawGizmos() {
        if (!AlwaysDraw) {
            return;
        }

        foreach (BoxCollider collider in boxes) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawBox(collider);
        }
        foreach (SphereCollider collider in spheres) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawSphere(collider);
        }
    }

    private void OnDrawGizmosSelected() {
        if (AlwaysDraw) {
            return;
        }

        foreach (BoxCollider collider in boxes) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawBox(collider);
        }
        foreach (SphereCollider collider in spheres) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawSphere(collider);
        }
    }

    void PopulateColliderArrays() {
        boxes = GetComponents<BoxCollider>();
        spheres = GetComponents<SphereCollider>();
    }

    void DrawBox(BoxCollider collider) {
        Gizmos.matrix = collider.transform.localToWorldMatrix;
        Gizmos.color = Color;
        if (DrawWire) {
            Gizmos.DrawWireCube(collider.center, collider.size);
        }
        else {
            Gizmos.DrawCube(collider.center, collider.size);
        }
    }

    void DrawSphere(SphereCollider collider) {
        Gizmos.matrix = collider.transform.localToWorldMatrix;
        Gizmos.color = Color;
        if (DrawWire) {
            Gizmos.DrawWireSphere(collider.center, collider.radius);
        }
        else {
            Gizmos.DrawSphere(collider.center, collider.radius);
        }
    }
}