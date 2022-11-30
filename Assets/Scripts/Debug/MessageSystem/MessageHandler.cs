using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageHandler : MonoBehaviour {
    private void Start() {
        GameObject messageBox = Resources.Load<GameObject>("Message Box");
        ServerMessage[] messages = MessageSystem.GetMessages(SceneManager.GetActiveScene().buildIndex);

        foreach (ServerMessage msg in messages) {
            GameObject obj = Instantiate(messageBox, msg.Position, Quaternion.identity);
            obj.GetComponent<MessageBox>().Message = msg.Message;
            obj.GetComponent<MessageBox>().Level = msg.SeverityLevel;
        }
    }
}