using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPM/Message System/ Message System Data")]
public class MessageSystemData : ScriptableObject {
    public Color[] SeverityColors;
    public float MessageActivationDistance;
    public string ReferenceObjectOverride;
}