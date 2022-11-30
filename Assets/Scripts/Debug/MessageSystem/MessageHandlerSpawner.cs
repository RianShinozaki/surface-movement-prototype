using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandlerSpawner {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnSceneLoad() {
#if UNITY_EDITOR
        if (Application.isPlaying) {
            Object.Instantiate(Resources.Load<GameObject>("Message Handler"));
        }
#endif
    }
}