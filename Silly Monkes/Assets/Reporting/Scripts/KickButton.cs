using UnityEngine;

public class KickButton : MonoBehaviour
{
    [SerializeField] public int ButtonNumber;
    [SerializeField] public LeaderBoard LB;
    [SerializeField] public string HandTag = "HandTag";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(HandTag))
        {
            LB.KickPress(ButtonNumber);
        }
    }

}

