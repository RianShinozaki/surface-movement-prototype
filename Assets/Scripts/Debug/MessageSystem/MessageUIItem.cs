using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageUIItem : MonoBehaviour {
    public TMP_Text MessageText;
    public RawImage SeverityLevel;
    public int ID;

    public void Teleport() => MessageHandler.Instance.TeleportPlayer(ID);
    public void Delete() => MessageHandler.Instance.DeleteMessage(ID);
}