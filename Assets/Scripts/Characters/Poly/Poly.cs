using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolyHomingSystem))]
public class Poly : PlayerController {
    PolyHomingSystem system;

    public override void Awake() {
        base.Awake();

        system = GetComponent<PolyHomingSystem>();
    }
}