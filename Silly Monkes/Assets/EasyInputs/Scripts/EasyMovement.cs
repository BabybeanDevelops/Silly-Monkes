using UnityEngine;
using easyInputs;

public class EasyMovement : MonoBehaviour
{
    [Header("Options")]
    public bool AllowJumping;
    public bool AllowSprinting;
    public float WalkSpeed = 2;
    public float SprintSpeed = 4;
    public float JumpForce = 4;
    [Header("Hand")]
    public EasyHand Hand = EasyHand.LeftHand;
    [Header("Head")]
    public Transform Head;
    [Header("Height")]
    public float MinHeight = 0.5f;
    public float MaxHeight = 2f;
    [Header("Character Controller")]
    public CharacterController controller;
    //Hidden
    Vector2 ThumbStick2DAxis;
    float originalStepOffset;
    Quaternion HeadRotation;
    bool IsSprinting;
    float Gravity;
    Vector3 velocity;
    void Update()
    {
        CharacterController();
        ThumbStick2DAxis = EasyInputs.GetThumbStick2DAxis(Hand);
        MovePlayer();
        HeadRotation = Quaternion.Euler(0, Head.eulerAngles.y, 0);
        if (EasyInputs.GetThumbStickButtonDown(Hand))
        {
            IsSprinting = true;
        } else
        {
            IsSprinting = false;
        }
        if (!controller.isGrounded)
        {
            Gravity += Physics.gravity.y * Time.deltaTime;
            controller.stepOffset = 0;
        }
        else
        {
            Gravity = -0.5f;
            controller.stepOffset = originalStepOffset;
            if (AllowJumping)
            {
                if (EasyInputs.GetPrimaryButtonDown(EasyHand.RightHand))
                {
                    Gravity = JumpForce;
                }
            }
        }
    }
    /// <summary>
    /// Moves The Character Player
    /// </summary>
    void MovePlayer()
    {
        Vector3 Movement = HeadRotation * new Vector3(ThumbStick2DAxis.x, Gravity, ThumbStick2DAxis.y);
        float Magnitude = Mathf.Clamp01(Movement.magnitude);

            if (!IsSprinting)
            {
                velocity = Movement * Magnitude;
                controller.Move(velocity * WalkSpeed * Time.deltaTime);
            }
            else
            {
                velocity = Movement * Magnitude;
                controller.Move(velocity * SprintSpeed * Time.deltaTime);
            }
    }

    /// <summary>
    /// Updates Character Controller To Position. And Player Height
    /// </summary>
    void CharacterController()
    {
        var height = Mathf.Clamp(Head.localPosition.y, MinHeight, MaxHeight);
        Vector3 center = Head.localPosition;
        center.y = height / 2f;
        controller.height = height;
        controller.center = center;
    }
}
