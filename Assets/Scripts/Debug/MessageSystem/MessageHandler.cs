using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageHandler : MonoBehaviour {
    public static MessageHandler Instance;

    ServerMessage[] Messages;
    GameObject[] objectMessages;
    GameObject MessageViewer;
    GameObject MessageDeleteDialogue;
    GameObject MessageAddDialogue;

    public int sceneID => SceneManager.GetActiveScene().buildIndex;

    MessageDialogueState deletionState = MessageDialogueState.Waiting;
    int messageIDChache = -1;
    IEnumerator DeleteMessage() {
        MessageViewer.SetActive(false);
        MessageDeleteDialogue.SetActive(true);

        while (deletionState == MessageDialogueState.Waiting) {
            yield return null;
        }

        if (deletionState == MessageDialogueState.Yes) {
            MessageSystem.DeleteMessage(sceneID, messageIDChache);
        }

        Destroy(objectMessages[messageIDChache]);
        Destroy(MessageViewer.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(messageIDChache).gameObject);
        MessageSystem.DeleteMessage(sceneID, messageIDChache);

        List<ServerMessage> newMessages = new List<ServerMessage>();
        List<GameObject> newGOs = new List<GameObject>();

        for (int i = 0; i < Messages.Length; i++) {
            if (i == messageIDChache) {
                continue;
            }

            newMessages.Add(Messages[i]);
            newGOs.Add(objectMessages[i]);
        }

        Messages = newMessages.ToArray();
        objectMessages = newGOs.ToArray();

        MessageDeleteDialogue.SetActive(false);
        messageIDChache = -1;
        deletionState = MessageDialogueState.Waiting;
    }

    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        GameObject messageBox = Resources.Load<GameObject>("Message Box");
        MessageViewer = Instantiate(Resources.Load<GameObject>("Message Viewer"));
        MessageDeleteDialogue = Instantiate(Resources.Load<GameObject>("Delete Message Dialogue"));
        MessageAddDialogue = Instantiate(Resources.Load<GameObject>("Message Add Dialogue"));

        Messages = MessageSystem.GetMessages(sceneID);

        //I hate my life
        Transform viewerContent = MessageViewer.transform.GetChild(0).GetChild(0).GetChild(0);
        GameObject messageItem = Resources.Load<GameObject>("Message Item");
        objectMessages = new GameObject[Messages.Length];
        for (int i = 0; i < Messages.Length; i++) {
            //Instantiate messages
            ServerMessage msg = Messages[i];
            GameObject obj = Instantiate(messageBox, msg.Position, Quaternion.identity);
            obj.GetComponent<MessageBox>().Message = msg.Message;
            obj.GetComponent<MessageBox>().Level = msg.SeverityLevel;
            objectMessages[i] = obj;

            //Create Message Viewer item list
            MessageUIItem item = Instantiate(messageItem).GetComponent<MessageUIItem>();
            item.transform.SetParent(viewerContent);
            item.MessageText.text = msg.Message;
            item.SeverityLevel.color = MessageSystem.Data.SeverityColors[msg.SeverityLevel];
            item.ID = i;
            item.transform.localScale = Vector3.one;
        }
    }

    private void Update() {
        if (messageIDChache != -1) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F3)) {
            MessageAddDialogue.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            MessageViewer.SetActive(!MessageViewer.activeSelf);
        }
    }

    public void AddMessage(Vector3 position, string message, int level) {
        //Create Message Viewer item list
        MessageUIItem item = Instantiate(Resources.Load<GameObject>("Message Item")).GetComponent<MessageUIItem>();
        item.transform.SetParent(MessageViewer.transform.GetChild(0).GetChild(0).GetChild(0));
        item.MessageText.text = message;
        item.SeverityLevel.color = MessageSystem.Data.SeverityColors[level];
        item.ID = Messages.Length;
        item.transform.localScale = Vector3.one;

        GameObject obj = Instantiate(Resources.Load<GameObject>("Message Box"), position, Quaternion.identity);
        obj.GetComponent<MessageBox>().Message = message;
        obj.GetComponent<MessageBox>().Level = level;

        GameObject[] objs = new GameObject[objectMessages.Length + 1];
        objectMessages.CopyTo(objs, 0);
        objs[objs.Length - 1] = obj;

        objectMessages = objs;

        MessageSystem.AddMessage(sceneID, position, message, level);
    }

    public void DeleteMessage(int messageID) {
        messageIDChache = messageID;
        StartCoroutine(DeleteMessage());
    }

    public void SetDeletionState(MessageDialogueState deletionState) => this.deletionState = deletionState;

    public void TeleportPlayer(int messageID) {
        PlayerController.Instance.transform.position = Messages[messageID].Position + (Vector3.up * 0.2f);
        PlayerController.Instance.groundSpeed = Vector2.zero;
    }
}

public enum MessageDialogueState {
    Waiting,
    Yes,
    No
}