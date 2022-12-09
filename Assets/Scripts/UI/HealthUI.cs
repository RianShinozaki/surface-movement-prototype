using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour {
    public static HealthUI Instance;
    public GameObject HealthPip;
    public int MaxPips;
    public Transform PipHolder;

    int pipCount => PipHolder.childCount;

    private void Awake() {
        if(Instance){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddPip() {
        if(pipCount >= MaxPips){
            return;
        }

        Instantiate(HealthPip, PipHolder);
    }

    public void RemovePip() {
        if(pipCount <= 0){
            return;
        }

        Destroy(PipHolder.GetChild(0).gameObject);
    }
}