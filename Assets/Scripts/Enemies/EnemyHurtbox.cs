using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    BaseEnemy ent;
    
    public void Awake() {
        ent = GetComponentInParent<BaseEnemy>();

    }
    public void OnTriggerEnter(Collider other) {
        Debug.Log("ENEMYHIT");
        ent.OnHurt(other.GetComponentInParent<PlayerController>(), other.GetComponent<PlayerHitbox>(), other.ClosestPoint(transform.position));
        other.GetComponentInParent<PlayerController>().OnStruckTarget(ent);
    }
}
