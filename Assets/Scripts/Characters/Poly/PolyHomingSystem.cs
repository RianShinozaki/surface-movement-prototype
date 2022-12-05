using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyHomingSystem : MonoBehaviour {
    public float DotLimit;
    public float ReticleAnimationTime;

    [HideInInspector]
    public List<Transform> possibleTargets = new List<Transform>();
    public bool HasTarget => ActiveTarget;
    public Transform ActiveTarget{
        get{
            return target;
        }
        set{
            if(value != null && value != target){
                ReticleAnimation();
            }
            targetRect.gameObject.SetActive(value != null);
            target = value;
        }
    }
    Transform target;
    RectTransform targetRect;
    LTDescr reticleTween;
    Camera cam;

    void Awake() {
        cam = Camera.main;
        targetRect = Instantiate(Resources.Load<GameObject>("Characters/Poly/Homing Recticle")).transform.GetChild(0).GetComponent<RectTransform>();
    }

    void Update(){
        //Check for positive target hits
        //This fucking AI chat bot has got me feeling bad about not commenting code
        float lastDot = 0f;
        int lastDotIndex = -1;

        for(int i = 0; i < possibleTargets.Count; i++){
            float dot = GameMath.Dot01(transform.forward, (possibleTargets[i].position - transform.position).normalized);
            if(dot > lastDot){
                lastDot = dot;
                lastDotIndex = i;
            }
        }

        //quick catch for resetting
        if(lastDotIndex == -1){
            //Gross but I don't want to be setting this every frame
            if(ActiveTarget){
                ActiveTarget = null;
            }
            return;
        }

        ActiveTarget = possibleTargets[lastDotIndex];
        UpdateTarget();
    }

    void ReticleAnimation(){
        if(reticleTween != null){
            LeanTween.cancel(reticleTween.id);
        }

        reticleTween = LeanTween.value(0f, 1f, ReticleAnimationTime).setOnUpdate((float val) => {
            targetRect.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, val);
        }).setOnComplete(() => {
            targetRect.localScale = Vector3.one;
            reticleTween = null;
        }).setEaseOutBack();
    }

    void UpdateTarget() {
        if(!ActiveTarget){
            return;
        }
        Vector2 position = cam.WorldToScreenPoint(ActiveTarget.position);
        targetRect.anchoredPosition = position;
    }
}