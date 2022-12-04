using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum MovementType {
    Simple,
    Momentum,
    Homing
}

public class PlayerController : CustomPhysicsObject
{
    public static PlayerController Instance;
    #region movement parameters
    [FoldoutGroup("Movement Params")] public float topSpeed;
    [FoldoutGroup("Movement Params")] public float startSpeed;                      //Initial speed when moving from below startspeed velocity
    [FoldoutGroup("Movement Params")] public float boostSpeed;
    [FoldoutGroup("Movement Params")] public float accelSpeed;                      
    [FoldoutGroup("Movement Params")] public float accelSpeedLowSpdThresh;          //Definition of low speed
    [FoldoutGroup("Movement Params")] public float accelSpeedLowSpd;                //Accel force in low speed
    [FoldoutGroup("Movement Params")] public float decelSpeed;
    [FoldoutGroup("Movement Params")] public float decelSpeedLowSpd;
    [FoldoutGroup("Movement Params")] public float accelSpeedinAir;
    [FoldoutGroup("Movement Params")] public float decelSpeedinAir;
    [FoldoutGroup("Movement Params")] public float haltSpeed;                       //The minimum speed needed to trigger a halt
    [FoldoutGroup("Movement Params")] public float haltAccel;                       //The force that you halt with
    [FoldoutGroup("Movement Params")] public float haltAngle;                       //The input angle relative to direction needed to trigger a halt


    [FoldoutGroup("Movement Params")] public float steeringCoefficient;
    [FoldoutGroup("Movement Params")] public float steeringCoefficientinAir;
    [FoldoutGroup("Movement Params")] public float fastSpeedKeepCoefficient;        //How well you get to keep your speed if you're running faster than topspeed //NOT USED
    [FoldoutGroup("Movement Params")] public float fastSpeedDecel;                  //How fast you lose speed if past topspeed
    [FoldoutGroup("Movement Params")] public float jumpForce;
    [FoldoutGroup("Movement Params")] public MovementType movementType;
    [FoldoutGroup("Movement Params")] public bool relativeToCamera = true;
    [FoldoutGroup("Movement Params")] public float homingSpeed;
    [FoldoutGroup("Movement Params")] public InputPath currentPath;

    [FoldoutGroup("Movement Params")] public float attackSpeedBoost;
    [FoldoutGroup("Movement Params")] public float attackVertBoost;
    [FoldoutGroup("Movement Params")] public float attackCooldownMax;

    #endregion

    [FoldoutGroup("Movement Variables")] public HomingTarget currentTarget;
    [FoldoutGroup("Movement Variables")] public bool canAirAttackBoost;
    [FoldoutGroup("Movement Variables")] public float attackCooldown;

    [FoldoutGroup("Components")] PolyAnimator animator;
    [FoldoutGroup("Components")] PlayerHomingSensor homingSensor;
    [FoldoutGroup("Components")] TrailRenderer trail;
    [FoldoutGroup("Components")] public UnityEngine.VFX.VisualEffect SpinVFX;
    [FoldoutGroup("Components")] public UnityEngine.VFX.VisualEffect HomingVFX;

    public override void Awake() {
        if (Instance) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        base.Awake();
        animator = GetComponent<PolyAnimator>();
        homingSensor = GetComponentInChildren<PlayerHomingSensor>();
        trail = GetComponentInChildren<TrailRenderer>();
    }


    void Update() {
        float inputMagnitude = Mathf.Clamp(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude, 0, 1);
        switch (movementType) {
            case MovementType.Simple:
                
                if (inputMagnitude > 0) {
                    float inputDirection = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                    float correctedInputDirection = inputDirection + Mathf.Deg2Rad * cam.transform.localRotation.eulerAngles.y;
                    flatDirection = correctedInputDirection;
                    groundSpeed = new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * 3;
                }
                else {
                    groundSpeed = Vector2.zero;
                }
                if (grounded && Input.GetButtonDown("Fire1")) {
                    grounded = false;
                    verticalSpeed = 5;
                }
                break;

            case MovementType.Momentum:

                if (inputMagnitude > 0) {
                    float inputDirection = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                    float correctedInputDirection = inputDirection + Mathf.Deg2Rad * cam.transform.localRotation.eulerAngles.y;

                    Vector2 inputVec    = new Vector2(Mathf.Sin(correctedInputDirection) , Mathf.Cos(correctedInputDirection));

                    if(currentPath != null) {
                        float t = currentPath.myPath.path.GetClosestDistanceAlongPath(transform.position);
                        Vector3 dirAlongPath = currentPath.myPath.path.GetDirectionAtDistance(t);
                        Vector3 dirToPath = (currentPath.myPath.path.GetClosestPointOnPath(transform.position) - transform.position).normalized;
                        float distTopath = (currentPath.myPath.path.GetClosestPointOnPath(transform.position) - transform.position).magnitude;
                        float distToPathClamped = Mathf.Clamp(distTopath / currentPath.displacementDistMax, 0, 1);

                        Vector3 moveDir = dirAlongPath * (1 - currentPath.displacementInfluenceFalloff.Evaluate(distToPathClamped)) + dirToPath * currentPath.displacementInfluenceFalloff.Evaluate(distToPathClamped);

                        Quaternion rot = transform.rotation;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        transform.rotation = Quaternion.FromToRotation(transform.up, upDirectionLast) * transform.rotation;
                        Vector3 correctMoveDir = transform.InverseTransformDirection(moveDir);

                        inputVec = Vector2.Lerp(inputVec, new Vector2(correctMoveDir.x, correctMoveDir.z).normalized, currentPath.pathInfluence);

                        transform.rotation = rot;


                    }

                    if(grounded && Speed < 2) {
                        groundSpeed = inputVec * startSpeed;
                    }

                    if (!grounded || ( !(Speed > haltSpeed && Mathf.Abs(Vector2.Angle(groundSpeed, inputVec)) > haltAngle))) {
                        float steer = Speed * (grounded ? steeringCoefficient : steeringCoefficientinAir) * Mathf.Sin(Mathf.Deg2Rad * Vector2.Angle(groundSpeed, inputVec));
                        //Debug.Log(steer);
                        float accelForce = (grounded ? (Speed < accelSpeedLowSpdThresh ? accelSpeedLowSpd : accelSpeed) : accelSpeedinAir);
                        float accelCap = fastSpeedDecel - 1;
                        float accelTotal = Mathf.Clamp((steer + accelForce), 0, accelCap);
                        groundSpeed = Vector2.MoveTowards(groundSpeed, inputVec * 999, accelTotal * Time.deltaTime);
                    }
                    else {
                        groundSpeed = Vector2.MoveTowards(groundSpeed, Vector2.zero, haltAccel * Time.deltaTime);
                    }

                    if(Speed > topSpeed) {
                        groundSpeed = Vector2.MoveTowards(groundSpeed, Vector2.zero, fastSpeedDecel * Time.deltaTime);

                    }

                    if (grounded)
                        flatDirection = Mathf.Atan2(groundSpeed.x, groundSpeed.y);
                    else {
                        flatDirection = Mathf.LerpAngle(flatDirection * Mathf.Rad2Deg, correctedInputDirection * Mathf.Rad2Deg, 20 * Time.deltaTime) * Mathf.Deg2Rad;
                    }
                }
                else {
                    groundSpeed = Vector2.MoveTowards(groundSpeed, Vector2.zero, (grounded ? (Speed < 2 ? decelSpeedLowSpd : decelSpeed) : decelSpeedinAir ) * Time.deltaTime);
                }
                
                if (grounded && Input.GetKeyDown(KeyCode.Space)) {
                    grounded = false;
                    
                    Vector3 jumpVector = new Vector3(Vector3.Dot(upDirection, Vector3.right), Vector3.Dot(upDirection, Vector3.up), Vector3.Dot(upDirection, Vector3.forward));
                    groundSpeed = new Vector2(jumpVector.x, jumpVector.z) * jumpForce;
                    verticalSpeed = jumpVector.y * jumpForce;
                    keepSpeedCache = true;
                    animator.Jump();
                }

                if (Input.GetMouseButtonDown(1)) {
                    groundSpeed = new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * boostSpeed;
                }

                HomingVFX.Stop();
                if (Input.GetKey(KeyCode.LeftShift)) { 
                    HomingTarget nearestTarget = homingSensor.GetNearest(transform.position, new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)), 60f);

                    if (nearestTarget != null) {
                        nearestTarget.displayReticle = true;

                        if (Input.GetMouseButtonDown(0)) {
                            movementType = MovementType.Homing;
                            currentTarget = nearestTarget;
                            Homing(true);
                            HomingVFX.Play();
                            break;
                        }
                    }
                }

                if (grounded)
                    canAirAttackBoost = true;

                if (attackCooldown != 0)
                    attackCooldown = Mathf.MoveTowards(attackCooldown, 0, Time.deltaTime);
                else {
                    SpinVFX.Stop();
                    if (Input.GetMouseButtonDown(0)) {
                        if (!grounded && canAirAttackBoost) {
                            canAirAttackBoost = false;
                            verticalSpeed = attackVertBoost;
                        }
                        if(grounded) {
                            groundSpeed += new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * attackSpeedBoost;
                        }
                        SpinVFX.Play();
                        Attack();
                        attackCooldown = attackCooldownMax;
                    }
                }
                trail.startColor = Color.Lerp(trail.startColor, new Color(1, 1, 1, Mathf.Clamp( ((Speed-30)/60) , 0, 1)), Time.deltaTime * 4);

                break;
            case MovementType.Homing:
                trail.startColor = new Color(1, 1, 1, 1);
                trail.endColor = new Color(1, 1, 1, 0);
                Vector3 totalSpd = (currentTarget.transform.position - transform.position).normalized * homingSpeed;
                grounded = false;
                grounded = false;
                upDirectionLast = Vector3.up;
                groundedLast = false;
                groundSpeed = new Vector2(totalSpd.x, totalSpd.z);
                verticalSpeed = totalSpd.y;
                keepSpeedCache = false;
                transform.LookAt(currentTarget.transform);
                break;
        }
    }

    public override void OnDrawGizmos() {
        if(currentPath != null) {
            Gizmos.DrawSphere(currentPath.myPath.path.GetClosestPointOnPath(transform.position), 1);

            float t = currentPath.myPath.path.GetClosestDistanceAlongPath(transform.position);
            Vector3 dirAlongPath = currentPath.myPath.path.GetDirectionAtDistance(t);
            Vector3 dirToPath = (currentPath.myPath.path.GetClosestPointOnPath(transform.position) - transform.position).normalized;

            Vector3 moveDir = (dirAlongPath + dirToPath).normalized;

            Ray r = new Ray(transform.position, moveDir);
            Gizmos.DrawRay(r);
        }
    }

    public void SetInputPath(InputPath ip) {
        currentPath = ip;
    }
    public void UnsetInputPath(InputPath ip) {
        if (currentPath == ip) currentPath = null;
    }

    public void Spring() => animator.Spring();
    public void Attack() => animator.Attack();
    public void Homing(bool set) => animator.Homing(set);
    public void CancelHoming() => animator.CancelHoming();

    public void SetRelativeCameraMovement(bool set) {
        relativeToCamera = set;
    }
}
