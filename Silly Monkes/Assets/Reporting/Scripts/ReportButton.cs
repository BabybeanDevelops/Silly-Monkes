using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ReportButton : MonoBehaviour
{
    [SerializeField] public int ButtonNumber;
    [SerializeField] public LeaderBoard LB;
    [SerializeField] public string HandTag = "HandTag";
    public Material PressedMaterial;
    public AudioClip ClickSound;
    private Material UnPressedMaterial;
    private Renderer rend;

    private Player ReportedPlayer;

    private bool reported;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        UnPressedMaterial = rend.material;
    }

    private void Update()
    {
        if (ButtonNumber > 0 && ButtonNumber <= PhotonNetwork.PlayerList.Length)
        {
            if (PhotonNetwork.PlayerList[ButtonNumber - 1] != ReportedPlayer && reported)
            {
                reported = false;
                rend.material = UnPressedMaterial;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (ButtonNumber > 0 && ButtonNumber <= PhotonNetwork.PlayerList.Length)
        {
            if (other.CompareTag(HandTag) && !reported)
            {
                AudioSource.PlayClipAtPoint(ClickSound, transform.position);
                LB.Report(ButtonNumber);
                rend.material = PressedMaterial;
                ReportedPlayer = PhotonNetwork.PlayerList[ButtonNumber - 1];
                reported = true;
            }
        }
    }
}