using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingTarget : MonoBehaviour
{
    SpriteRenderer target;
    public bool displayReticle;

    public void Start() {
        target = GetComponentInChildren<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (displayReticle) {
            target.gameObject.SetActive(true);
        }
        else target.gameObject.SetActive(false);

        displayReticle = false;
    }
}
