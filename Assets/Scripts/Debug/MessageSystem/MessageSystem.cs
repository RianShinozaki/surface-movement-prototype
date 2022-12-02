using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageSystem {
    public static MessageSystemData Data {
        get {
            if (!data) {
                data = Resources.Load<MessageSystemData>("Message System Data");
            }
            return data;
        }
    }
    static MessageSystemData data;

    static int gameID => Application.productName.GetHashCode();

    public static ServerMessage[] GetMessages(int sceneID) {
        return new ServerMessage[] { new ServerMessage() { Position = Vector3.zero, Message = "Hello World!" } };
    }

    public static void AddMessage(int sceneID, Vector3 position, string message, int level) {

    }

    public static void DeleteMessage(int sceneID, int messageID) {

    }
}

public class ServerMessage {
    public Vector3 Position;
    public int SeverityLevel;
    public int Type;
    public string Message;
}