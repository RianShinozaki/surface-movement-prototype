using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingTarget : MonoBehaviour {
    private void Awake() {
        if(!PlayerController.Instance.GetComponent<PolyHomingSystem>()){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        other.GetComponent<PolyHomingSystem>().possibleTargets.Add(transform);
    }

    private void OnTriggerExit(Collider other) {
        other.GetComponent<PolyHomingSystem>().possibleTargets.Add(transform);
    }
}
