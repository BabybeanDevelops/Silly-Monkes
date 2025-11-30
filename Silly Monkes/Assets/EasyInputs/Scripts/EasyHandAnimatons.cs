using easyInputs;
using UnityEngine;

public class EasyHandAnimatons : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Hand")]
    public EasyHand Hand;
    [Header("Animator")]
    public Animator animator;
    public string Trigger_Parameter = "Trigger";
    public string Grip_Parameter = "Grip";
    void Update()
    {
        //Get's Trigger Value
        float TriggerValue = EasyInputs.GetTriggerButtonFloat(Hand);
        animator.SetFloat(Trigger_Parameter, TriggerValue);

        //Get's Grip Value
        float GripValue = EasyInputs.GetGripButtonFloat(Hand);
        animator.SetFloat(Grip_Parameter, GripValue);
    }
}
