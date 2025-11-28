
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.VR;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;

public class ExecuteButtonName : MonoBehaviour
{
    public NameScript nameScript;
    public string Handtag;

    private void OnTriggerEnter(Collider other)
    {
        if (nameScript.NameVar.Length < 1)
        {
            PhotonVRManager.SetUsername("steve" + Random.Range(0, 1000).ToString());
        }
        if (other.transform.tag == Handtag)
        {
            PhotonVRManager.SetUsername(nameScript.NameVar);
            PlayerPrefs.SetString("username", nameScript.NameVar);
            PhotonNetwork.LocalPlayer.NickName = nameScript.NameVar;

            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
                {
                    DisplayName = PlayerPrefs.GetString("username")
                }, delegate (UpdateUserTitleDisplayNameResult result)
                {
                    Debug.Log("Display Name Changed!");
                }, delegate (PlayFabError error)
                {
                    Debug.Log("Error");
                    if (error.Error == PlayFabErrorCode.AccountBanned)
                    {
                        Application.Quit();
                    }
                });
            }

        }
    }
}