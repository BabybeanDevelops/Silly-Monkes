using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class InRoom : MonoBehaviour
{
    [Header("THIS SCRIPT WAS MADE BY FLIMCYVR. IT IS NOT YOURS.")]
    [Header("Distributing This Script Will Lead To A Permanent Ban and MORE!")]
    [Header("If you make a video on this script")]
    [Header("credit me with my discord and youtube")]
    public TextMeshPro Text;
    public void FixedUpdate()
    {
        if (PhotonNetwork.InRoom)
        {
            Text.text = "In Room: " + PhotonNetwork.CurrentRoom.Name;
        }
        else if (!PhotonNetwork.InRoom)
        {
            Text.text = "In Room: NOT CONNECTED";
        }
    }
}