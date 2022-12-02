using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshPickInPlace : PickInPlace {
    public Mesh[] Meshes;

    MeshFilter filter;

    private void Awake() {
        filter = GetComponent<MeshFilter>();
    }

    public override void OnRandomize() {
        base.OnRandomize();
        if (Meshes.Length <= 0 || !filter) {
            return;
        }
        filter.mesh = Meshes[GetRandom(Meshes.Length)];
    }
}