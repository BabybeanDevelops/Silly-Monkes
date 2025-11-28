using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made By conspeed55#0540 On Discord
public class SaveColor : MonoBehaviour
{
    private float RedKey = 0;
    private float GreenKey = 0;
    private float BlueKey = 0;
    public ColorScript ColorScript;
    // Start is called before the first frame update
    void Start()
    {
        RedKey = PlayerPrefs.GetFloat("RedKey");
        GreenKey = PlayerPrefs.GetFloat("GreenKey");
        BlueKey = PlayerPrefs.GetFloat("BlueKey");
        ColorScript.Red = RedKey;
        ColorScript.Green = GreenKey;
        ColorScript.Blue = BlueKey;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("HandTag"))
        {
            RedKey = ColorScript.Red;
            PlayerPrefs.SetFloat("RedKey", RedKey);
            GreenKey = ColorScript.Green;
            PlayerPrefs.SetFloat("GreenKey", GreenKey);
            BlueKey = ColorScript.Blue;
            PlayerPrefs.SetFloat("BlueKey", BlueKey);
        }
    }
}