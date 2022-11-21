using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PolyAnimator : MonoBehaviour {
    public Animator anim;

    PlayerController controller;

    private void Awake() {
        controller = GetComponent<PlayerController>();
    }

    private void Update() {
        Vector3 velocity = controller.groundSpeed.ConvertTo3D(controller.verticalSpeed);

        anim.SetFloat("XSpeed", velocity.x);
        anim.SetFloat("YSpeed", velocity.y);
        anim.SetFloat("ZSpeed", velocity.z);
        anim.SetFloat("Speed", velocity.magnitude / controller.topSpeed);
        anim.SetBool("InAir", !controller.grounded);
    }
}