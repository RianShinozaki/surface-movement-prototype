using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class PrefabPickInPlace : PickInPlace {
    [AssetsOnly]
    public GameObject[] Prefabs;
    public float PositionRandomFactor;
    public Vector3 PositionOffset;
    [FoldoutGroup("Use Position")]
    public bool PosX, PosY, PosZ;
    public float RotationRandomFactor;
    public bool RotX, RotY, RotZ;
    public Vector3 RotationOffset;

    public override void OnRandomize() {
        if (Application.isPlaying) {
            return;
        }
        base.OnRandomize();
        if (Prefabs.Length <= 0) {
            return;
        }

        if (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        Vector3 randomRot = Random.insideUnitSphere * RotationRandomFactor;
        randomRot += transform.eulerAngles;
        randomRot += RotationOffset;

        Vector3 randomPos = Random.insideUnitSphere * PositionRandomFactor;
        randomPos.x *= PosX.ConvertToInt();
        randomPos.y *= PosY.ConvertToInt();
        randomPos.z *= PosZ.ConvertToInt();
        randomPos += transform.position;
        randomPos += PositionOffset;

        Instantiate(Prefabs[GetRandom(Prefabs.Length)], randomPos, Quaternion.Euler(randomRot.x, randomRot.y, randomRot.z), transform);
    }
}