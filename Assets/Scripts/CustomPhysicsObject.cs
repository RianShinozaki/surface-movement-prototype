using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CustomPhysicsObject : MonoBehaviour
{
    public Rigidbody rb;
    public SphereCollider coll;
    public Camera cam;

    public bool grounded;

    #region physics parameters
    [FoldoutGroup("Physics Params")] public float gravity;
    [FoldoutGroup("Physics Params")] public float slopeInfluence;
    [FoldoutGroup("Physics Params")] public float rayCastRadius;                    //How far around the player origin to create raycasts
    [FoldoutGroup("Physics Params")] public float airRaycastDist;                   //How far down to check for ground collisions in air
    [FoldoutGroup("Physics Params")] public float groundRaycastDist;                //How far down to check for ground collisions on the ground
    [FoldoutGroup("Physics Params")] public LayerMask environmentMask;
    #endregion

    #region physics variables
    [FoldoutGroup("Physics Vars")] public Vector2 groundSpeed;                      //Speed along the normal
    [FoldoutGroup("Physics Vars")] public float verticalSpeed;                      //Speed up and down
    [FoldoutGroup("Physics Vars")] public float flatDirection;                      //Direction along the normal
    [FoldoutGroup("Physics Vars")] public Vector3 upDirection;                      //The current normal (or straight up if in the air)
    [FoldoutGroup("Physics Vars")] public float groundSlopeAngle;                   //The angle from upwards vector of the ground in degrees
    [FoldoutGroup("Physics Vars")] public float groundSlopePoint;                   //The angle the ground points in radians
    [FoldoutGroup("Physics Vars")] public Vector3 groundSlopeDir;                   //A vector of the direction the ground points
    [FoldoutGroup("Physics Vars")] public float normalLerpSpeed;                    //Speed of the smoothening when changing normals
    [FoldoutGroup("Physics Vars")] public float slopeResistFromSpeedCoefficient;    //Being faster makes slopes affect you this much less
    #endregion

    public float Speed {
        get {
            return groundSpeed.magnitude;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        RaycastHit hit;
        List<RaycastHit> hits = new List<RaycastHit>();

        //Central ray, this tells the object if it's on the ground or not
        Ray r = new Ray(transform.TransformPoint(coll.center), -transform.up);
        bool foundGround = false;
        if ( verticalSpeed <= 0 && Physics.Raycast(r, out hit, grounded ? groundRaycastDist : airRaycastDist, environmentMask)) {
            hits.Add(hit);
            foundGround = true;
            transform.position = hit.point;
        }
        else {
            foundGround = false;
        }

        if (foundGround) {
            r = new Ray(transform.TransformPoint(coll.center) + transform.forward * rayCastRadius, -transform.up);
            if (Physics.Raycast(r, out hit, grounded ? groundRaycastDist : airRaycastDist, environmentMask)) {
                hits.Add(hit);
            }
            r = new Ray(transform.TransformPoint(coll.center) - transform.forward * rayCastRadius, -transform.up);
            if (Physics.Raycast(r, out hit, grounded ? groundRaycastDist : airRaycastDist, environmentMask)) {
                hits.Add(hit);
            }
            r = new Ray(transform.TransformPoint(coll.center) + transform.right * rayCastRadius, -transform.up);
            if (Physics.Raycast(r, out hit, grounded ? groundRaycastDist : airRaycastDist, environmentMask)) {
                hits.Add(hit);
            }
            r = new Ray(transform.TransformPoint(coll.center) - transform.right * rayCastRadius, -transform.up);
            if (Physics.Raycast(r, out hit, grounded ? groundRaycastDist : airRaycastDist, environmentMask)) {
                hits.Add(hit);
            }
            r = new Ray(transform.TransformPoint(coll.center) + transform.TransformDirection(new Vector3(0.71f, 0, 0.71f)) * rayCastRadius, -transform.up);
            if (Physics.Raycast(r, out hit, grounded ? groundRaycastDist : airRaycastDist, environmentMask)) {
                hits.Add(hit);
            }
            r = new Ray(transform.TransformPoint(coll.center) + transform.TransformDirection(new Vector3(-0.71f, 0, 0.71f)) * rayCastRadius, -transform.up);
            if (Physics.Raycast(r, out hit, grounded ? groundRaycastDist : airRaycastDist, environmentMask)) {
                hits.Add(hit);
            }
            r = new Ray(transform.TransformPoint(coll.center) + transform.TransformDirection(new Vector3(-0.71f, 0, -0.71f)) * rayCastRadius, -transform.up);
            if (Physics.Raycast(r, out hit, grounded ? groundRaycastDist : airRaycastDist, environmentMask)) {
                hits.Add(hit);
            }
            r = new Ray(transform.TransformPoint(coll.center) + transform.TransformDirection(new Vector3(0.71f, 0, -0.71f)) * rayCastRadius, -transform.up);
            if (Physics.Raycast(r, out hit, grounded ? groundRaycastDist : airRaycastDist, environmentMask)) {
                hits.Add(hit);
            }
        }
        

        Debug.Log(foundGround);
        grounded = foundGround;

        if (grounded) {
            verticalSpeed = 0;

            Vector3 avgNormal = Vector3.zero;
            int hitNum = 0;
            foreach(RaycastHit thisHit in hits) {
                avgNormal += thisHit.normal;
                hitNum++;
            }
            avgNormal /= hitNum;
            upDirection = Vector3.Slerp(upDirection, avgNormal, normalLerpSpeed);

            groundSlopeAngle = Vector3.Angle(avgNormal, Vector3.up);
            Vector3 temp = Vector3.Cross(avgNormal, Vector3.down);
            groundSlopeDir = Vector3.Cross(temp, avgNormal);
            groundSlopePoint = Mathf.Atan2(groundSlopeDir.x, groundSlopeDir.z);
            groundSpeed += new Vector2( Mathf.Sin( groundSlopePoint), Mathf.Cos(groundSlopePoint)) * Mathf.Sin(groundSlopeAngle * Mathf.Deg2Rad) * Time.deltaTime * slopeInfluence / (Speed * slopeResistFromSpeedCoefficient + 1);

        }
        else {
            if (foundGround) grounded = true;
            else {
                groundSlopeDir = Vector3.forward;
                verticalSpeed += gravity * Time.fixedDeltaTime;
                upDirection = Vector3.Slerp(upDirection, Vector3.up, normalLerpSpeed);
            }
        }
        

        transform.rotation = Quaternion.Euler(0, flatDirection * Mathf.Deg2Rad, 0);
        transform.rotation = Quaternion.FromToRotation(transform.up, upDirection) * transform.rotation;
        transform.position += transform.TransformDirection(new Vector3(groundSpeed.x, verticalSpeed, groundSpeed.y)) * Time.fixedDeltaTime ;
    }

    public void OnDrawGizmos() {
        Ray r = new Ray(transform.position, transform.forward);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(r);

        Gizmos.color= Color.green;
        r = new Ray(transform.TransformPoint(coll.center), -transform.up);
        Gizmos.DrawRay(r);
        r = new Ray(transform.TransformPoint(coll.center) + transform.forward * rayCastRadius, -transform.up);
        Gizmos.DrawRay(r);
        r = new Ray(transform.TransformPoint(coll.center) - transform.forward * rayCastRadius, -transform.up);
        Gizmos.DrawRay(r);
        r = new Ray(transform.TransformPoint(coll.center) + transform.right * rayCastRadius, -transform.up);
        Gizmos.DrawRay(r);
        r = new Ray(transform.TransformPoint(coll.center) - transform.right * rayCastRadius, -transform.up);
        Gizmos.DrawRay(r);
        r = new Ray(transform.TransformPoint(coll.center) + transform.TransformDirection(new Vector3(0.71f, 0, 0.71f)) * rayCastRadius, -transform.up);
        Gizmos.DrawRay(r);
        r = new Ray(transform.TransformPoint(coll.center) + transform.TransformDirection(new Vector3(-0.71f, 0, 0.71f)) * rayCastRadius, -transform.up);
        Gizmos.DrawRay(r);
        r = new Ray(transform.TransformPoint(coll.center) + transform.TransformDirection(new Vector3(-0.71f, 0, -0.71f)) * rayCastRadius, -transform.up);
        Gizmos.DrawRay(r);
        r = new Ray(transform.TransformPoint(coll.center) + transform.TransformDirection(new Vector3(0.71f, 0, -0.71f)) * rayCastRadius, -transform.up);
        Gizmos.DrawRay(r);



    }
}
