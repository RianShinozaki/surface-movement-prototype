using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageBox : MonoBehaviour {
    public string Message;
    public int Level;
    public TMP_Text text;

    Camera cam;
    Transform referenceObject;
    Animator anim;
    int animationID;
    bool activated;

    bool inRange => Vector3.Distance(transform.position, referenceObject.position) <= MessageSystem.Data.MessageActivationDistance;

    private void Awake() {
        cam = Camera.main;
        referenceObject = cam.transform;

        if (!string.IsNullOrEmpty(MessageSystem.Data.ReferenceObjectOverride)) {
            referenceObject = GameObject.Find(MessageSystem.Data.ReferenceObjectOverride).transform;
        }

        anim = GetComponent<Animator>();
        animationID = Animator.StringToHash("In Range");
    }

    private void Start() {
        SetText(Message);
        GetComponent<SpriteRenderer>().color = MessageSystem.Data.SeverityColors[Mathf.Clamp(Level, 0, 2)];
    }

    private void Update() {
        transform.rotation = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f);

        if (!string.IsNullOrEmpty(MessageSystem.Data.ReferenceObjectOverride) && !referenceObject) {
            Debug.Log("Re-grabbing refernce object...");
            Awake();
            return;
        }

        if (activated != inRange) {
            activated = inRange;
            anim.SetBool(animationID, activated);
        }
    }

    public void SetText(string message) {
        text.text = message;
        Message = message;
    }
}