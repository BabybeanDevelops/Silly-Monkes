using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MuteButton : MonoBehaviour
{
    [SerializeField] public int ButtonNumber;
    [SerializeField] public LeaderBoard LB;
    [SerializeField] public string HandTag = "HandTag";

    private bool Muted = false;
    public Material MutedMaterial;
    public AudioClip ClickSound;
    private Material UnMutedMaterial;
    private Renderer rend;

    private Player MutedUser;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        UnMutedMaterial = rend.material;
    }

    private void Update()
    {
        if (ButtonNumber > 0 && ButtonNumber <= PhotonNetwork.PlayerList.Length)
        {
            if (PhotonNetwork.PlayerList[ButtonNumber - 1] != MutedUser && Muted)
            {
                Muted = false;
                rend.material = UnMutedMaterial;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ButtonNumber > 0 && ButtonNumber <= PhotonNetwork.PlayerList.Length)
        {
            if (other.CompareTag(HandTag))
            {
                AudioSource.PlayClipAtPoint(ClickSound, transform.position);
                LB.MutePress(ButtonNumber);

                Muted = !Muted;
                MutedUser = PhotonNetwork.PlayerList[ButtonNumber - 1];
                if (Muted)
                {
                    rend.material = MutedMaterial;
                }
                else
                {
                    rend.material = UnMutedMaterial;
                }
            }
        }
    }
}
