using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatEvents {
    public delegate void OnCoinPickupEvent();

    public static event OnCoinPickupEvent OnCoinPickup;

    public static void CoinPickup(){
        OnCoinPickup?.Invoke();
    }
}