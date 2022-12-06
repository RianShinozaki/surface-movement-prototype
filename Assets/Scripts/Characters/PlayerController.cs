using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[RequireComponent(typeof(AlkylEntity))]
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
    [FoldoutGroup("Movement Params")] public bool relativeToCamera = true;
    [FoldoutGroup("Movement Params")] public float homingSpeed;
    [FoldoutGroup("Movement Params")] public float maxHomingTime;
    [FoldoutGroup("Movement Params")] public float homingHitBounce;
    [FoldoutGroup("Movement Params")] public InputPath currentPath;

    [FoldoutGroup("Movement Params")] public float attackSpeedBoost;
    [FoldoutGroup("Movement Params")] public float attackStruckTargetBoost;
    [FoldoutGroup("Movement Params")] public float attackVertBoost;
    [FoldoutGroup("Movement Params")] public float attackCooldownMax;

    #endregion

    [FoldoutGroup("Movement Variables")] public Transform currentTarget;
    [FoldoutGroup("Movement Variables")] public bool canAirAttackBoost;
    [FoldoutGroup("Movement Variables")] public float attackCooldown;
    [FoldoutGroup("Movement Variables")] public float timeInHoming;

    [FoldoutGroup("Components")] public PolyAnimator animator;
    [FoldoutGroup("Components")] public PlayerHomingSensor homingSensor;
    [FoldoutGroup("Components")] public TrailRenderer trail;
    [FoldoutGroup("Components")] public UnityEngine.VFX.VisualEffect SpinVFX;
    [FoldoutGroup("Components")] public UnityEngine.VFX.VisualEffect HomingVFX;
    [FoldoutGroup("Components")] public GameObject hitbox;
    [FoldoutGroup("Components")] public AlkylEntity ent;

    public virtual void Awake() {
        if (Instance) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        animator = GetComponent<PolyAnimator>();
        homingSensor = GetComponentInChildren<PlayerHomingSensor>();
        trail = GetComponentInChildren<TrailRenderer>();
        ent = GetComponent<AlkylEntity>();
        ent.Mode = 1;
    }


    public virtual void Update() {
        
        switch (ent.Mode) {
            case 0:
                Mode0();
                break;
            case 1:
                Mode1();
                break;
            case 2:
                Mode2();
                break;
        }
    }

    public virtual void Mode0() {
        #region debug  ground movement
        float inputMagnitude = Mathf.Clamp(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude, 0, 1);
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
        #endregion
    }

    public virtual void Mode1() {
        #region momentum-based ground movement
        

        float inputMagnitude = Mathf.Clamp(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude, 0, 1);
        
        if (inputMagnitude > 0) {
            float inputDirection = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            float correctedInputDirection = inputDirection + Mathf.Deg2Rad * cam.transform.localRotation.eulerAngles.y;

            Vector2 inputVec = new Vector2(Mathf.Sin(correctedInputDirection), Mathf.Cos(correctedInputDirection));

            if (currentPath != null) {
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

            if (grounded && Speed < 2) {
                groundSpeed = inputVec * startSpeed;
            }

            if (!grounded || (!(Speed > haltSpeed && Mathf.Abs(Vector2.Angle(groundSpeed, inputVec)) > haltAngle))) {
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

            if (Speed > topSpeed) {
                groundSpeed = Vector2.MoveTowards(groundSpeed, Vector2.zero, fastSpeedDecel * Time.deltaTime);

            }

            if (grounded)
                flatDirection = Mathf.Atan2(groundSpeed.x, groundSpeed.y);
            else {
                flatDirection = Mathf.LerpAngle(flatDirection * Mathf.Rad2Deg, correctedInputDirection * Mathf.Rad2Deg, 20 * Time.deltaTime) * Mathf.Deg2Rad;
            }
        }
        else {
            groundSpeed = Vector2.MoveTowards(groundSpeed, Vector2.zero, (grounded ? (Speed < 2 ? decelSpeedLowSpd : decelSpeed) : decelSpeedinAir) * Time.deltaTime);
        }
        trail.startColor = Color.Lerp(trail.startColor, new Color(1, 1, 1, Mathf.Clamp(((Speed - 30) / 60), 0, 1)), Time.deltaTime * 4);
        #endregion
    }

    public virtual void Mode2() {}

    public virtual void OnStruckTarget(BaseEnemy ent) {}

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
    

    public void SetRelativeCameraMovement(bool set) {
        relativeToCamera = set;
    }
}
