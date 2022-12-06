using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolyHomingSystem))]
public class Poly : PlayerController {
    PolyHomingSystem system;

    public override void Awake() {
        base.Awake();

        system = GetComponent<PolyHomingSystem>();
    }

    public override void Mode1() {
        Homing(false);

        base.Mode1();

        #region jump
        if (grounded && Input.GetKeyDown(KeyCode.Space)) {
            grounded = false;

            Vector3 jumpVector = new Vector3(Vector3.Dot(upDirection, Vector3.right), Vector3.Dot(upDirection, Vector3.up), Vector3.Dot(upDirection, Vector3.forward));
            groundSpeed = new Vector2(jumpVector.x, jumpVector.z) * jumpForce;
            verticalSpeed = jumpVector.y * jumpForce;
            keepSpeedCache = true;
            animator.Jump();
        }
        #endregion

        #region boost
        if (Input.GetMouseButtonDown(1)) {
            groundSpeed = new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * boostSpeed;
        }
        #endregion

        #region attack
        HomingVFX.Stop();
        if (Input.GetKey(KeyCode.LeftShift)) {
            //These get components are temp for testing until you unhook all this
            if (GetComponent<PolyHomingSystem>().HasTarget) {
                Transform nearestTarget = GetComponent<PolyHomingSystem>().ActiveTarget;
                if (Input.GetMouseButtonDown(0)) {
                    ent.Mode = 1;
                    timeInHoming = 0;
                    currentTarget = nearestTarget;
                    hitbox.SetActive(true);
                    Homing(true);
                    HomingVFX.Play();
                    return;
                }
            }
        }

        if (grounded)
            canAirAttackBoost = true;

        if (attackCooldown != 0)
            attackCooldown = Mathf.MoveTowards(attackCooldown, 0, Time.deltaTime);
        else {
            SpinVFX.Stop();
            hitbox.SetActive(false);
            if (Input.GetMouseButtonDown(0)) {
                if (!grounded && canAirAttackBoost) {
                    canAirAttackBoost = false;
                    verticalSpeed = attackVertBoost;
                }
                if (grounded) {
                    groundSpeed += new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * attackSpeedBoost;
                }
                SpinVFX.Play();
                Attack();
                hitbox.SetActive(true);
                attackCooldown = attackCooldownMax;
            }
        }
        #endregion
        
    }
    public override void Mode2() {
        base.Mode2();
        trail.startColor = new Color(1, 1, 1, 1);
        trail.endColor = new Color(1, 1, 1, 0);
        Vector3 totalSpd = (currentTarget.transform.position - transform.position).normalized * homingSpeed;
        grounded = false;
        upDirectionLast = Vector3.up;
        groundedLast = false;
        groundSpeed = new Vector2(totalSpd.x, totalSpd.z);
        verticalSpeed = totalSpd.y;
        keepSpeedCache = false;
        transform.LookAt(currentTarget.transform);
        hitbox.SetActive(true);
        if (ent.timeInMode > maxHomingTime) {
            CancelHoming();
            Homing(false);
            ent.Mode = 1;
            hitbox.SetActive(false);
        }
    }

    public override void OnStruckTarget(BaseEnemy ent) {
        switch (ent.Mode) {
            case 1:
                groundSpeed += new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * attackStruckTargetBoost;
                if (!grounded) {
                    verticalSpeed = homingHitBounce;
                }
                break;
            case 2:
                transform.position = transform.position + transform.up;
                grounded = false;
                upDirectionLast = Vector3.up;
                groundedLast = false;
                groundSpeed = Vector2.zero;
                keepSpeedCache = true;
                verticalSpeed = homingHitBounce;
                ent.Mode = 1;
                canAirAttackBoost = true;
                hitbox.SetActive(false);
                Homing(false);
                break;
        }
    }

    public void Attack() => animator.Attack();
    public void Homing(bool set) => animator.Homing(set);
    public void CancelHoming() => animator.CancelHoming();
}