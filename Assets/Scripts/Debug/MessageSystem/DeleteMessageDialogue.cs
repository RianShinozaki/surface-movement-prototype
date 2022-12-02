using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMessageDialogue : MonoBehaviour {
    private void OnEnable() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Delete() => MessageHandler.Instance.SetDeletionState(MessageDialogueState.Yes);
    public void Cancel() => MessageHandler.Instance.SetDeletionState(MessageDialogueState.No);
}