using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject pickupFX;
    public void OnTriggerEnter(Collider other) {
        Instantiate(pickupFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
