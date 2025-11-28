using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerBack : MonoBehaviour
{
    [Header("THIS SCRIPT WAS MADE BY FLIMCYVR. IT IS NOT YOURS.")]
    [Header("Distributing This Script Will Lead To A Permanent Ban and MORE!")]
    [Header("If you make a video on this script")]
    [Header("credit me with my discord and youtube")]
    public JoinRoom JoinRoom;
    public string HandTag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == HandTag)
        {
            JoinRoom.name = JoinRoom.name.Remove(JoinRoom.name.Length - 1);
        }
    }
}