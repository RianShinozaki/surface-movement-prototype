using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingTarget : MonoBehaviour {
    public Vector3 Offset;

    PolyHomingSystem homingSystem;
    private void Start() {
        if(!PlayerController.Instance){
            Debug.Log("[Homing Target] No player controller found!");
            return;
        }
        if(!PlayerController.Instance.GetComponent<PolyHomingSystem>()){
            Destroy(gameObject);
        }

        homingSystem = PlayerController.Instance.GetComponent<PolyHomingSystem>();
    }

    private void OnTriggerEnter(Collider other) {
        if(!homingSystem){
            return;
        }
        homingSystem.possibleTargets.Add(this);
    }

    private void OnTriggerExit(Collider other) {
        if(!homingSystem){
            return;
        }
        homingSystem.possibleTargets.Remove(this);
    }

    private void OnDestroy() {
        if(!homingSystem){
            return;
        }
        homingSystem.possibleTargets.Remove(this);
        homingSystem.ActiveTarget = null;
    }
}
