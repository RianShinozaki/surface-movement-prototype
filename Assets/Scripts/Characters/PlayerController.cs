using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum MovementType {
    Simple,
    Momentum
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
    #endregion

    PolyAnimator animator;

    public override void Awake() {
        if (Instance) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        base.Awake();
        animator = GetComponent<PolyAnimator>();
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

                    if (!grounded || ( !(Speed > haltSpeed && Mathf.Abs(Vector2.Angle(groundSpeed, inputVec)) > haltAngle))) {
                        float steer = Speed * (grounded ? steeringCoefficient : steeringCoefficientinAir) * Mathf.Sin(Mathf.Deg2Rad * Vector2.Angle(groundSpeed, inputVec));
                        Debug.Log(steer);
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

                if (Input.GetKeyDown(KeyCode.LeftShift)) {
                    groundSpeed = new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * boostSpeed;
                }
                break;
        }
    }

    public void Spring() => animator.Spring();

    public void SetRelativeCameraMovement(bool set) {
        relativeToCamera = set;
    }
}
