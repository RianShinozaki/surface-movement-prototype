using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Comment : MonoBehaviour {
    [HideLabel, Multiline(10), DisableIf("Lock")]
    public string text;
    public bool Lock;
}