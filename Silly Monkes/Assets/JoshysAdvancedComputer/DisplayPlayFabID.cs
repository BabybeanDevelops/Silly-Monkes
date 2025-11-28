using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class DisplayPlayFabID : MonoBehaviour
{
    public Playfablogin playfabloginscript;
    public TextMeshPro IDDisplayText;

    void Update()
    {
        IDDisplayText.text = "Your ID is: " + playfabloginscript.MyPlayFabID;
    }
}