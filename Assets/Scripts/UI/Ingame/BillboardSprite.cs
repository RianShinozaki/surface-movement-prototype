using UnityEngine;

public class BillboardSprite : MonoBehaviour {
    float angle;
    void Update() {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * 4, 64 * Time.deltaTime);
    }

    private void OnEnable() {
        transform.localScale = Vector3.one * 8;
    }

}