using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHomingSensor : MonoBehaviour {
    public List<HomingTarget> homingTargets;
    private void OnTriggerEnter(Collider other) {
        Debug.Log("foundEntity?");
        HomingTarget ent = other.GetComponent<HomingTarget>();
        if (ent != null) {
            homingTargets.Add(ent);
        }
    }
    private void OnTriggerExit(Collider other) {
        HomingTarget ent = other.GetComponent<HomingTarget>();
        if (ent != null) {
            homingTargets.Remove(ent);
        }
    }

    void UpdateEntities() {
        foreach (HomingTarget ent in homingTargets) {
            if (!ent || !ent.isActiveAndEnabled) {
                homingTargets.Remove(ent);
            }
        }
    }
    public HomingTarget GetNearest(Vector3 pos, Vector2 Direction, float MaxAngle) {
        UpdateEntities();
        HomingTarget NearestEnt = null;
        float NearestDist = 9999f;
        foreach (HomingTarget ent in homingTargets) {
            float dist = (ent.transform.position - pos).magnitude;
            if (dist < NearestDist) {
                float angle = Vector2.SignedAngle(Direction, (new Vector2(ent.transform.position.x, ent.transform.position.z) - new Vector2(pos.x, pos.z)).normalized);
                Debug.Log("ANGLE TOWARDS ENEMY: " + (angle.ToString()));
                if (Mathf.Abs(angle) <= MaxAngle) {
                    NearestEnt = ent;
                    NearestDist = dist;
                }

            }
        }
        return NearestEnt;
    }
}
