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

[RequireComponent(typeof(PhotonView))]
public class LeaderBoard : MonoBehaviour
{
    [SerializeField] public TMP_Text[] displaySpot;
    [SerializeField] public Renderer[] ColorSpot;
    [SerializeField] public string WebHookURL;
    [SerializeField] public Playfablogin playfablogin;

    private bool hashed;
    private bool Kicked = false;
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();

        if (view.OwnershipTransfer != OwnershipOption.Takeover)
            view.OwnershipTransfer = OwnershipOption.Takeover;

        // Start updating leaderboard every second
        StartCoroutine(UpdateLeaderboardLoop());
    }

    private IEnumerator UpdateLeaderboardLoop()
    {
        while (true)
        {
            if (PhotonNetwork.IsConnected)
            {
                if (!hashed)
                {
                    ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
                    hash["PlayfabID"] = playfablogin.MyPlayFabID;
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                    hashed = true;
                }

                UpdateLeaderboardDisplay();
            }

            yield return new WaitForSeconds(1f); // Wait 1 second between updates
        }
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateLeaderboardLoop());
    }   

    private void UpdateLeaderboardDisplay()
    {
        if (Kicked)
        {
            HandleKickDisplay();
            return;
        }

        // Update leaderboard text and colors
        for (int i = 0; i < PhotonNetwork.PlayerList.Length && i < displaySpot.Length; i++)
        {
            displaySpot[i].text = PhotonNetwork.PlayerList[i].NickName;

            foreach (PhotonVRPlayer PVRP in FindObjectsOfType<PhotonVRPlayer>())
            {
                if (PVRP.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[i])
                {
                    ColorSpot[i].material.color = JsonUtility.FromJson<Color>(
                        (string)PVRP.GetComponent<PhotonView>().Owner.CustomProperties["Colour"]
                    );
                }
            }
        }

        // Clear unused slots
        for (int i = PhotonNetwork.PlayerList.Length; i < displaySpot.Length; i++)
        {
            displaySpot[i].text = string.Empty;
            ColorSpot[i].material.color = Color.white;
        }
    }

    private void HandleKickDisplay()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();

        foreach (TMP_Text spot in displaySpot)
        {
            spot.color = Color.red;
            spot.text = "You have been Kicked";
        }
    }

    public void MutePress(int ButtonNumber)
    {
        if (PhotonNetwork.PlayerList.Length >= ButtonNumber - 1)
        {
            foreach (PhotonVRPlayer PVRP in FindObjectsOfType<PhotonVRPlayer>())
            {
                if (PVRP.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[ButtonNumber - 1])
                {
                    var audioSource = PVRP.GetComponent<PhotonVoiceView>().SpeakerInUse.gameObject.GetComponent<AudioSource>();
                    audioSource.mute = !audioSource.mute;
                    break;
                }
            }
        }
    }

    public void KickPress(int ButtonNumber)
    {
        if (PhotonNetwork.PlayerList.Length >= ButtonNumber - 1)
        {
            foreach (PhotonVRPlayer PVRP in FindObjectsOfType<PhotonVRPlayer>())
            {
                if (PVRP.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[ButtonNumber - 1])
                {
                    view.RequestOwnership();
                    view.RPC("KickPlayer", PVRP.GetComponent<PhotonView>().Owner);
                }
            }
        }
    }

    [PunRPC]
    void KickPlayer()
    {
        Kicked = true;
    }

    public void Report(int ButtonNumber)
    {
        string playfabid = playfablogin.MyPlayFabID;
        if (PhotonNetwork.PlayerList.Length >= ButtonNumber - 1)
        {
            foreach (PhotonVRPlayer PVRP in FindObjectsOfType<PhotonVRPlayer>())
            {
                if (PVRP.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[ButtonNumber - 1])
                {
                    SendtoWebhook(
                        PhotonNetwork.PlayerList[ButtonNumber - 1].NickName + " " +
                        (string)PVRP.GetComponent<PhotonView>().Owner.CustomProperties["PlayfabID"] +
                        " was reported by " + PlayerPrefs.GetString("Username", null) + " " + playfablogin.MyPlayFabID
                    );
                }
            }
        }
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
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Reporting Webhook Error: " + www.error);
        }
    }
}
