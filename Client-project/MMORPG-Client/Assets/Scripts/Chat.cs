using System;
using System.Collections;
using System.Collections.Generic;
using PhotonMMORPG.Common.CustomEventArgs;
using UnityEngine;

public class Chat : MonoBehaviour {

    private string message = "";
    private string chatLog = "";
    // Use this for initialization
    void Start () {
        PhotonServer.Instance.OnReceiveChatMessage += OnReceiveChatMessage;
        PhotonServer.Instance.GetRecentChatMessage();
    }

    private void OnDestroy()
    {
        PhotonServer.Instance.OnReceiveChatMessage -= OnReceiveChatMessage;
    }
    private void OnReceiveChatMessage(object sender, ChatMessageEventArgs e)
    {
        chatLog = e.Message+ "\r\n" + chatLog;
        Debug.Log("Mesaj nou " + chatLog);
    }

    // Update is called once per frame
    void Update () {
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0,0,100,20),(String.Format("{0} RTT   {1} VAR", PhotonServer.Instance.PhotonPeer.RoundTripTime, PhotonServer.Instance.PhotonPeer.RoundTripTimeVariance)));

        GUI.Label(new Rect(5, 25, 300, 300), chatLog);

        message = GUI.TextField(new Rect(5, 320, 200, 20), message);

        if (GUI.Button(new Rect(210, 320, 80, 20), "Send")) { 
            if (message.Length == 0)
                return;
            PhotonServer.Instance.SendChatMessage(message);
            message = "";
        }
    }
}
