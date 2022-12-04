using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PolyAnimator : MonoBehaviour {
    public Animator anim;
    public float ReferenceTopSpeed;

    PlayerController controller;

    private void Awake() {
        controller = GetComponent<PlayerController>();
    }

    private void Update() {
        Vector3 velocity = controller.groundSpeed.ConvertTo3D(controller.verticalSpeed);

        anim.SetFloat("Y Speed", velocity.y);
        anim.SetFloat("Speed", controller.groundSpeed.magnitude / ReferenceTopSpeed);
        anim.SetBool("In Air", !controller.grounded);

    }

    public void Jump() {
        anim.SetTrigger("Jump");
    }
    public void Attack() {
        anim.SetTrigger("Attack");
    }
    public void Spring() {
        anim.SetTrigger("Spring");
    }
    public void Homing(bool set) {
        anim.SetBool("Homing", set);
    }
    public void CancelHoming() {
        anim.SetTrigger("Cancel Homing");
    }
}