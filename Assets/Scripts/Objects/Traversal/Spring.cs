using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float springPower;
    Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }
    public void OnTriggerEnter(Collider other) {
       
        PlayerController player = other.GetComponent<PlayerController>();
        if(player != null) {
            player.transform.position = transform.position + transform.up;
            player.grounded = false;
            player.upDirectionLast = Vector3.up;
            player.groundedLast = false;
            player.groundSpeed = new Vector2(transform.up.x, transform.up.z) * springPower;
            player.keepSpeedCache = true;
            player.verticalSpeed = transform.up.y * springPower;
            anim.SetTrigger("Bounce");
        }
    }

    private void OnDrawGizmos() {

        Gizmos.DrawLine(transform.position, transform.position + transform.up * springPower);
    }
}