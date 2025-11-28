using Photon.Voice.Unity;
using UnityEngine;

public class TalkingFace : MonoBehaviour
{
    [SerializeField] private Recorder recorder;
    private float output;

    public Material FaceMaterial;

    public Texture NotTalking;
    public Texture SlightlyTalking;
    public Texture Talking;
    public Texture LoudTalking;
    public Texture Screaming;

    void Start()
    {
        
    }

    private void Update()
    {
        if (recorder != null)
        {
            
            output = recorder.LevelMeter.CurrentPeakAmp;
        }
        else
        {
            Debug.Log("Bro you haven't put anything in the recorder");
        }

        if (output < 0.1) { FaceMaterial.mainTexture = NotTalking; }
        if (output > 0.1 && output < 0.3) { FaceMaterial.mainTexture = SlightlyTalking; }
        if (output > 0.3 && output < 0.5) { FaceMaterial.mainTexture = Talking; }
        if (output > 0.5 && output < 0.7) { FaceMaterial.mainTexture = LoudTalking; }
        if (output > 0.7) { FaceMaterial.mainTexture = Screaming; }

    }


}
