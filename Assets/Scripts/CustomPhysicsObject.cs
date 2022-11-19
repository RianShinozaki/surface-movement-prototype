using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsObject : MonoBehaviour
{
    public Rigidbody rb;
    public SphereCollider coll;
    public Camera cam;

    public bool grounded;

    #region physics parameters
    public float gravity;
    public float rayCastRadius;         //How far around the player origin to create raycasts
    public float airRaycastDist;        //How far down to check for ground collisions in air
    public float groundRaycastDist;     //How far down to check for ground collisions on the ground
    public LayerMask environmentMask;
    #endregion

    #region physics variables
    public Vector2 groundSpeed;
    public float verticalSpeed;
    public float flatDirection;
    public Vector3 upDirection;
    float groundSlopeAngle;
    Vector3 groundSlopeDir;
    public float normalLerpSpeed;
    #endregion

    #region movement parameters

    #endregion

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

        }
        else {
            if (foundGround) grounded = true;
            else {
                groundSlopeDir = Vector3.forward;
                verticalSpeed += gravity * Time.fixedDeltaTime;
                upDirection = Vector3.Slerp(upDirection, Vector3.up, normalLerpSpeed);
            }
        }
        float inputMagnitude = Mathf.Clamp(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude, 0, 1);
        if (inputMagnitude > 0) {
            float inputDirection = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            float correctedInputDirection = inputDirection + Mathf.Deg2Rad * cam.transform.localRotation.eulerAngles.y;
            flatDirection = correctedInputDirection;

            groundSpeed = new Vector2(Mathf.Sin(flatDirection), Mathf.Cos(flatDirection)) * 3;
        } else {
            groundSpeed = Vector2.zero;
        }
        if(grounded && Input.GetButtonDown("Fire1")) {
            grounded = false;
            verticalSpeed = 5;
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
