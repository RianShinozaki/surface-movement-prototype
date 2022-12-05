using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCompareTest : MonoBehaviour {
    public GameObject Obj1, Obj2;

    void Update(){
        Debug.Log(Obj1 == Obj2);
    }
}