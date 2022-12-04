using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : AlkylEntity
{
    Rigidbody rb;
    CustomPhysicsObject phys;
    SphereCollider sColl;
    public GameObject hurtbox;
    public GameObject homingTarget;
    public override void Awake() {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        phys = GetComponent<CustomPhysicsObject>();
        sColl = GetComponentInParent<SphereCollider>();
    }
    public void OnHurt(PlayerController attacker, PlayerHitbox hb, Vector3 pos) {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        phys.enabled = false;
        Vector3 hitDirection;
        float hitForce;

        if(hb.knockbackDir == Vector3.zero) {
            hitDirection = (transform.position - attacker.transform.position).normalized;
        } else {
            hitDirection = hb.knockbackDir.normalized;
        }

        hitDirection += attacker.rb.velocity;
        hitDirection += Vector3.up*3;
        hitDirection = hitDirection.normalized;
        hitForce = hb.knockbackForce + attacker.rb.velocity.magnitude;

        rb.AddForceAtPosition(hitDirection * hitForce, pos, ForceMode.Impulse);
        hurtbox.SetActive(false);
        homingTarget.SetActive(false);
    }
}
