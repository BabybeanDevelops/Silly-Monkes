using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JoinRoom : MonoBehaviourPunCallbacks
{
    [Header("THIS SCRIPT WAS MADE BY FLIMCYVR. IT IS NOT YOURS.")]
    [Header("Distributing This Script Will Lead To A Permanent Ban and MORE!")]
    [Header("If you make a video on this script")]
    [Header("credit me with my discord and youtube")]
    public string name = "";
    public bool pressed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("farded");
                PhotonNetwork.LeaveRoom();
            }
            Debug.Log(name);
            pressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (pressed)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                pressed = false;
                OnLeftRoom(); //should work hehehhaw
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        if (pressed)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                pressed = false;
                OnLeftRoom(); //should work hehehhaw
            }
        }
    }

    void OnLeftRoom() //not sure if you meant "public override void OnLeftRoom()" but ok
    {
        string roomName = name;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;
        options.IsVisible = false;
        options.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom(roomName, options, null); //should work lol
    }
}
