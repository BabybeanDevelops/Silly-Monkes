using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.VR;

public class ColorScript : MonoBehaviour
{


    public float Red;
    public float Blue;
    public float Green;
    float TrueRed;
    float TrueBlue;
    float TrueGreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        TrueRed = Red / 10;
        TrueBlue = Blue / 10;
        TrueGreen = Green / 10;

        Color myColour = new Color(TrueRed, TrueBlue, TrueGreen);
        PhotonVRManager.SetColour(myColour);
    }

}
