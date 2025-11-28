using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServerScript : MonoBehaviour
{
    [Header("THIS SCRIPT WAS MADE BY FLIMCYVR. IT IS NOT YOURS.")]
    [Header("Distributing This Script Will Lead To A Permanent Ban and MORE!")]
    [Header("If you make a video on this script")]
    [Header("credit me with my discord and youtube")]
    public JoinRoom joinRoom;
    public string Letter;
    public TextMeshPro ServerText; //add REFERENCE TO THIS

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "HandTag")
        {
            if (joinRoom.name.Length < 12) 
            {
                joinRoom.name += Letter;
                ServerText.text = joinRoom.name; //check if there's errors :( penis penis penis penis penis Licker
            }
        }
    }

    private void FixedUpdate() 
    {
        ServerText.text = joinRoom.name; //check if there's errors :( penis penis penis penis penis Licker
    }
}