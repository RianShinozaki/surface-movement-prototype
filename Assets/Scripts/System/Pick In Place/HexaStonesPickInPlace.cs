using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class HexaStonesPickInPlace : PickInPlace {
    [AssetsOnly]
    public GameObject[] Prefabs;
    public float YRandomizationFactor;

    public override void OnRandomize() {
        base.OnRandomize();

        if (Prefabs == null || Prefabs.Length <= 0) {
            return;
        }

        if (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        Instantiate(Prefabs[GetRandom(Prefabs.Length)], transform.position + new Vector3(0f, Random.Range(-YRandomizationFactor, YRandomizationFactor), 0f), Quaternion.Euler(-90f, 0f, GetRandom(6) * 60f), transform);
    }
}