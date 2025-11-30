namespace easyInputs
{
    using System;
    using System.Collections;
    using Unity.XR.Oculus;
    using UnityEngine;
    using UnityEngine.XR;
    public static partial class EasyInputs
    {

        /// <summary>
        /// Get's If The Grip Button Is Down.
        /// </summary>
        public static bool GetGripButtonDown(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.gripButton, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's If The Primary Button Is Down.
        /// </summary>
        public static bool GetTriggerButtonDown(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.triggerButton, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's If The Trigger Button Is Touched. Only Works On Quest And You Need To Build For It To Work
        /// </summary>
        public static bool GetTriggerButtonTouched(EasyHand easyHand)
        {
            GetInputBool(easyHand, OculusUsages.indexTouch, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's If The Grip Button Float.
        /// </summary>
        public static float GetGripButtonFloat(EasyHand easyHand)
        {
            GetInputFloat(easyHand, CommonUsages.grip, out float GripFloat);
            return GripFloat;
        }

        /// <summary>
        /// Get's The Trigger Float.
        /// </summary>
        public static float GetTriggerButtonFloat(EasyHand easyHand)
        {
            GetInputFloat(easyHand, CommonUsages.trigger, out float TriggerFloat);
            return TriggerFloat;
        }

        /// <summary>
        /// Get's If The Primary Button Is Down. Example. A & X Button On Quest
        /// </summary>
        public static bool GetPrimaryButtonDown(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.primaryButton, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's If The Primary Button Is Touched. Example. A & X Button On Quest
        /// </summary>
        public static bool GetPrimaryButtonTouched(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.primaryTouch, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's If The Secondary Button Is Down. Example. B & Y Button On Quest
        /// </summary>
        public static bool GetSecondaryButtonDown(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.secondaryButton, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's If The Secondary Button Is Touched. Example. B & Y Button On Quest
        /// </summary>
        public static bool GetSecondaryButtonTouched(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.secondaryTouch, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's If The Thumbstick Is Down.
        /// </summary>
        public static bool GetThumbStickButtonDown(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.primary2DAxisClick, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's If The Thumbstick Is Touched.
        /// </summary>
        public static bool GetThumbStickButtonTouched(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.primary2DAxisTouch, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's The Thumbsticks Vector2, This Can Be Used For Movement And Turning
        /// </summary>
        public static Vector2 GetThumbStick2DAxis(EasyHand easyHand)
        {
            GetInputVector2(easyHand, CommonUsages.primary2DAxis, out Vector2 ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Get's The Thumbsticks Vector2, This Can Be Used For Movement And Turning
        /// </summary>
        public static Vector3 GetDeviceVelocity(EasyHand easyHand)
        {
            GetInputVector3(easyHand, CommonUsages.deviceVelocity, out Vector3 ButtonDown);
            return ButtonDown;
        }



        /// <summary>
        /// Get's The Inputs
        /// </summary>
        static bool GetInputBool(EasyHand hand, InputFeatureUsage<bool> usage, out bool value)
        {
            if (hand == EasyHand.LeftHand)
            {
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
                return value;
            }
            else
            {
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
                return value;
            }
        }

        static float GetInputFloat(EasyHand hand, InputFeatureUsage<float> usage, out float value)
        {
            if (hand == EasyHand.LeftHand)
            {
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
                return value;
            }
            else
            {
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
                return value;
            }
        }

        static Vector2 GetInputVector2(EasyHand hand, InputFeatureUsage<Vector2> usage, out Vector2 value)
        {
            if (hand == EasyHand.LeftHand)
            {
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
                return value;
            }
            else
            {
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
                return value;
            }
        }

        static Vector3 GetInputVector3(EasyHand hand, InputFeatureUsage<Vector3> usage, out Vector3 value)
        {
            if (hand == EasyHand.LeftHand)
            {
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
                return value;
            }
            else
            {
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
                return value;
            }
        }

        public static IEnumerator Vibration(EasyHand hand, float amplitude, float duration)
        {
            float startTime = Time.time;
            uint channel = 0U;
            InputDevice device;
            if (hand == EasyHand.LeftHand)
            {
                device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            }
            else
            {
                device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            }
            while (Time.time < startTime + duration)
            {
                device.SendHapticImpulse(channel, amplitude, duration);
                yield return new WaitForSeconds(duration * 0.9f);
            }
            yield break;
        }

    }
    public enum EasyHand
    {
        LeftHand,
        RightHand
    }
}

