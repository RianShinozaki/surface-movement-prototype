using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Sirenix.OdinInspector;

public class InputPath : MonoBehaviour
{
    public PathCreator myPath;
    [Range(0, 1)] public float pathInfluence;
    public AnimationCurve displacementInfluenceFalloff;
    public float displacementDistMax;

    private void Start() {
        myPath = GetComponent<PathCreator>();
    }

    private void OnDrawGizmos() {
        if(myPath == null) { myPath = GetComponent<PathCreator>(); }
        Gizmos.DrawWireSphere(myPath.path.GetPointAtDistance(0), displacementDistMax);
    }
}
