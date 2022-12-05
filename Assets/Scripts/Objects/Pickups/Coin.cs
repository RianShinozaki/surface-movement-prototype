using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject pickupFX;
    public void OnTriggerEnter(Collider other) {
        ObjectPool.Instance.Spawn(pickupFX, transform.position, Quaternion.identity);
        ObjectPool.Instance.Despawn(gameObject);
    }
}
