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
    #region movement parameters
    [FoldoutGroup("Movement Params")] public float topSpeed;
    [FoldoutGroup("Movement Params")] public float boostSpeed;
    [FoldoutGroup("Movement Params")] public float accelSpeed;
    [FoldoutGroup("Movement Params")] public float decelSpeed;
    [FoldoutGroup("Movement Params")] public float steeringCoefficient;
    [FoldoutGroup("Movement Params")] public float fastSpeedKeepCoefficient;        //How well you get to keep your speed if you're running faster than topspeed;
    [FoldoutGroup("Movement Params")] public float fastSpeedDecel;                  //How well you get to keep your speed if you're running faster than topspeed; NOT USED CURRENTLY
    [FoldoutGroup("Movement Params")] public MovementType movementType;             //How fast you lose speed if past topspeed
    #endregion

    void Start()
    {
        
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

                Debug.Log(Speed);
                if (inputMagnitude > 0) {
                    float inputDirection = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                    float correctedInputDirection = inputDirection + Mathf.Deg2Rad * cam.transform.localRotation.eulerAngles.y;

                    groundSpeed = Vector2.MoveTowards(groundSpeed, new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * 999, Mathf.Clamp((Speed * steeringCoefficient + accelSpeed), 0, fastSpeedDecel-1) * Time.deltaTime);
                    if(Speed > topSpeed) {
                        groundSpeed = Vector2.MoveTowards(groundSpeed, Vector2.zero, fastSpeedDecel * Time.deltaTime);

                    }

                    flatDirection = correctedInputDirection;
                }
                else {
                    Debug.Log("Stop");
                    groundSpeed = Vector2.MoveTowards(groundSpeed, Vector2.zero, decelSpeed * Time.deltaTime);
                }
                if (grounded && Input.GetButtonDown("Fire1")) {
                    grounded = false;
                    verticalSpeed = 5;
                }
                if (Input.GetButtonDown("Fire2")) {
                    groundSpeed = new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * boostSpeed;
                }
                break;
        }
    }
}
