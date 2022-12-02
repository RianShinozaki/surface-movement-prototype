using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageAddDialogue : MonoBehaviour {
    public TMP_InputField message;
    public TMP_Dropdown dropdown;

    private void OnEnable() {
        message.text = "";
        dropdown.value = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.Instance.enabled = false;
    }

    private void OnDisable() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.Instance.enabled = true;
    }

    public void Confirm() {
        MessageHandler.Instance.AddMessage(PlayerController.Instance.transform.position, message.text, dropdown.value);
        gameObject.SetActive(false);
    }

    public void Cancel() {
        gameObject.SetActive(false);
    }
}