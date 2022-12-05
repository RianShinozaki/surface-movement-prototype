using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlkylEntity : MonoBehaviour {
    int type;
    int subType;
    int mode;
    int subMode;
    public float timeInMode;

    public int Mode {
        set {
            mode = value;
            timeInMode = 0;
        }
        get {
            return mode;
        }
    }

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

    public virtual void Update() {
        timeInMode += Time.deltaTime;
    }

    public virtual void HealthSetup() { }
    public virtual void AnimatorSetup() { }
}