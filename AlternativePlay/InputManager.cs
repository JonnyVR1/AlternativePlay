using UnityEngine;
using UnityEngine.XR;

namespace AlternativePlay
{
    /// <summary>
    /// This class manages the input system, most importantly helping to gate button presses
    /// on the controller, basically debouncing buttons.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        private InputDevice leftController;
        private InputDevice rightController;
        private bool leftTriggerCanClick;
        private bool rightTriggerCanClick;
        private bool bothTriggerCanClick;
        private bool isPolling;

        #region Button Polling Methods

        public bool LeftTriggerDown { get; private set; }
        public bool RightTriggerDown { get; private set; }
        public bool BothTriggerDown { get; private set; }

        public bool GetLeftTriggerClicked()
        {
            bool returnValue = false;
            if (leftTriggerCanClick && LeftTriggerDown)
            {
                returnValue = true;
                leftTriggerCanClick = false;
            }
            return returnValue;
        }

        public bool GetRightTriggerClicked()
        {
            bool returnValue = false;
            if (rightTriggerCanClick && RightTriggerDown)
            {
                returnValue = true;
                rightTriggerCanClick = false;
            }

            return returnValue;
        }

        public bool GetBothTriggerClicked()
        {
            bool returnValue = false;
            if (bothTriggerCanClick && BothTriggerDown)
            {
                returnValue = true;
                bothTriggerCanClick = false;
            }

            return returnValue;
        }

        #endregion

        #region MonoBehavior Methods

        public void BeginPolling()
        {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            leftTriggerCanClick = true;
            rightTriggerCanClick = true;
            bothTriggerCanClick = true;
            isPolling = true;
        }

        internal void EndPolling()
        {
            isPolling = false;
        }

        private void Update()
        {
            const float pulled = 0.75f;
            const float released = 0.2f;

            if (!isPolling) return;

            leftController.TryGetFeatureValue(CommonUsages.trigger, out float leftTriggerValue);
            rightController.TryGetFeatureValue(CommonUsages.trigger, out float rightTriggerValue);

            LeftTriggerDown = leftTriggerValue > pulled;
            RightTriggerDown = rightTriggerValue > pulled;
            BothTriggerDown = LeftTriggerDown && RightTriggerDown;

            if (leftTriggerValue < released) { leftTriggerCanClick = true; }
            if (rightTriggerValue < released) { rightTriggerCanClick = true; }
            if (leftTriggerValue < released && rightTriggerValue < released) { bothTriggerCanClick = true; }
        }

        #endregion
    }
}
