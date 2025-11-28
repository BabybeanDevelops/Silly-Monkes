using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using TMPro;
using Photon.VR;
using Photon.Voice.PUN;
using Photon.VR.Player;
using System.Text;
using System.Collections;
using System;
using System.Collections.Generic;

public class discordwebhook : MonoBehaviour
{
    [SerializeField] public string WebHookURL;
    [SerializeField] public string Message;

    void Start()
    {
        SendtoWebhook(Message);
    }
    
    public void SendtoWebhook(string message)
    {
        StartCoroutine(PostToDiscord(message));
    }

    IEnumerator PostToDiscord(string message)
    {
        string jsonPayload = "{\"content\": \"" + message + "\"}";

        UnityWebRequest www = new UnityWebRequest(WebHookURL, "POST");
        byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonPayload);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
    }
}