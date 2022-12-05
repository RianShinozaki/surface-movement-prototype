using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEmptyVFX : MonoBehaviour {
    public UnityEngine.VFX.VisualEffect VFX;
    float delay = 5;

    void Update() {
        if (delay > 0)
            delay--;
        else {

            if (VFX.aliveParticleCount == 0) {
                VFX.Stop();
                ObjectPool.Instance.Despawn(gameObject);
            }
        }
    }
    private void OnEnable() {
        VFX.Play();
    }
}
