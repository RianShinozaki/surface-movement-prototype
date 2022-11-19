using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlkylEntity : MonoBehaviour {
    public int Type;
    public int SubType;
    public int Mode;
    public int SubMode;

    public AlkylHealth Health { get; private set; }
    public AnimatorPassthrough Passthrough { get; private set; }

    public virtual void Awake() {
        Health = GetComponent<AlkylHealth>();
        Passthrough = GetComponent<AnimatorPassthrough>();

        if (Health) {
            HealthSetup();
        }

        if (Passthrough) {
            AnimatorSetup();
        }
    }

    public virtual void HealthSetup() { }
    public virtual void AnimatorSetup() { }
}