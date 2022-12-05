using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnLevelEssentials {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void SpawnEssentials(){
        if(!Application.isPlaying){
            return;
        }

        LevelEssentialsList list = Resources.Load<LevelEssentialsList>("Essentials List");

        foreach(GameObject obj in list.Essentials){
            Object.Instantiate(obj);
        }
    }
}