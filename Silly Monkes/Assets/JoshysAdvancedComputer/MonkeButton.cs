using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeButton : MonoBehaviour
{
    public GameObject Object;
    public Material red;
    public Material white;
    public AudioSource sound;

    void OnTriggerEnter()
    {
        Object.GetComponent<Renderer>().material = red;
        sound.Play();
    }
    void OnTriggerExit()
    {
        Object.GetComponent<Renderer>().material = white;
        sound.Stop();
    }


}