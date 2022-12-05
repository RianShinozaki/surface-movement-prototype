using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SpawnLevelEssentials {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void SpawnEssentials(){
        if(!Application.isPlaying){
            return;
        }

        LevelEssentialsList list = Resources.Load<LevelEssentialsList>("Essentials List");

        foreach(GameObject obj in list.Essentials){
            Object.Instantiate(obj);
        }
    }
}