using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class OnlinePlayerCounter : MonoBehaviour
{
    private TMP_Text playerCountText;

    private void Start()
    {
        playerCountText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            int playerCount = PhotonNetwork.CountOfPlayers;
            playerCountText.text = "Online Players: " + playerCount.ToString();
        }
    }
}
