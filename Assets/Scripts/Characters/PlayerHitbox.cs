using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public Vector3 knockbackDir;
    public float knockbackForce;
    public PlayerController player;
    public GameObject HitFX;

    public void PlayHitFX(Vector3 pos) {
        Instantiate(HitFX, pos, Quaternion.identity);
    }
}
