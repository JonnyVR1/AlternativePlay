using AlternativePlay.HarmonyPatches;
using AlternativePlay.Models;
using System;
using UnityEngine;
using UnityEngine.XR;

namespace AlternativePlay
{
    /// <summary>
    /// Responsible for maintaining and tracking the sabers to the controller or tracker
    /// positions which is necessary after Beat Saber 1.13.2 where the SaberManager no 
    /// longer resets the saber position on each Update() frame.
    /// </summary>
    public class SaberDeviceManager : MonoBehaviour
    {
        private SaberManager saberManager;
        private GameObject playerOrigin;
        private InputDevice leftController;
        private InputDevice rightController;

        private Pose savedLeftController;
        private Pose savedLeftSaber;
        private Pose savedRightController;
        private Pose savedRightSaber;
        private bool calibrated;

        public void BeginGameCoreScene()
        {
            calibrated = false;
        }

        /// <summary>
        /// Gets the pose for the given tracker config data or else it falls back to the
        /// left controller pose. This method accounts for Room Adjust and Noodle Extensions
        /// changing viewpoint functionality.
        /// </summary>
        public Pose GetLeftSaberPose(TrackerConfigData configData)
        {
            Pose? trackerPose;
            if (!String.IsNullOrWhiteSpace(configData?.Serial) &&
                (trackerPose = TrackedDeviceManager.instance.GetPoseFromSerial(configData.Serial)) != null)
            {
                // Return adjusted position from the tracker
                Pose adjustedPose = AdjustForPlayerOrigin(trackerPose.Value);
                return Utilities.CalculatePoseFromTrackerData(configData, adjustedPose);
            }
            else
            {
                if (!calibrated || !leftController.isValid) return new Pose();

                // Return adjusted position from the saber
                Pose controllerPose = TrackedDeviceManager.GetDevicePose(leftController) ?? new Pose();
                Pose adjustedControllerPose = AdjustForPlayerOrigin(controllerPose);
                return TrackedDeviceManager.GetTrackedObjectPose(savedLeftSaber, savedLeftController, adjustedControllerPose);
            }
        }

        /// <summary>
        /// Gets the pose for the given tracker config data or else it falls back to the
        /// right controller pose. This method accounts for Room Adjust and Noodle Extensions
        /// changing viewpoint functionality.
        /// </summary>
        public Pose GetRightSaberPose(TrackerConfigData configData)
        {
            Pose? trackerPose;
            if (!String.IsNullOrWhiteSpace(configData?.Serial) &&
                (trackerPose = TrackedDeviceManager.instance.GetPoseFromSerial(configData.Serial)) != null)
            {
                // Return adjusted position from the tracker
                Pose adjustedPose = AdjustForPlayerOrigin(trackerPose.Value);
                return Utilities.CalculatePoseFromTrackerData(configData, adjustedPose);
            }
            else
            {
                if (!calibrated || !rightController.isValid) return new Pose();

                // Return adjusted position from the saber
                Pose controllerPose = TrackedDeviceManager.GetDevicePose(rightController) ?? new Pose();
                Pose adjustedControllerPose = AdjustForPlayerOrigin(controllerPose);
                return TrackedDeviceManager.GetTrackedObjectPose(savedRightSaber, savedRightController, adjustedControllerPose);
            }
        }

        /// <summary>
        /// Moves the left saber to the given <see cref="Pose"/>
        /// </summary>
        public void SetLeftSaberPose(Pose pose)
        {
            if (saberManager == null) return;

            saberManager.leftSaber.transform.position = pose.position;
            saberManager.leftSaber.transform.rotation = pose.rotation;

            if (MultiplayerLocalActivePlayerGameplayManagerPatch.multiplayerSaberManager)
                MultiplayerSyncStateManagerPatch.SetMultiplayerLeftSaberPose(pose);
        }

        /// <summary>
        /// Moves the right saber to the given <see cref="Pose"/>
        /// </summary>
        public void SetRightSaberPose(Pose pose)
        {
            if (saberManager == null) return;

            saberManager.rightSaber.transform.position = pose.position;
            saberManager.rightSaber.transform.rotation = pose.rotation;

            if (MultiplayerLocalActivePlayerGameplayManagerPatch.multiplayerSaberManager)
                MultiplayerSyncStateManagerPatch.SetMultiplayerRightSaberPose(pose);
        }

        /// <summary>
        /// Gets the pose for the given tracker config data or else it falls back to the
        /// left controller and then moves the left saber
        /// </summary>
        public void SetLeftSaber(TrackerConfigData trackerData)
        {
            Pose pose = GetLeftSaberPose(trackerData);
            SetLeftSaberPose(pose);
        }

        /// <summary>
        /// Gets the pose for the given tracker config data or else it falls back to the
        /// right controller and then moves the right saber
        /// </summary>
        public void SetRightSaber(TrackerConfigData trackerData)
        {
            Pose pose = GetRightSaberPose(trackerData);
            SetRightSaberPose(pose);
        }

        /// <summary>
        /// Disables all the renders in the left saber.  You may have to await or 
        /// yield return for 0.1f seconds before this method will work properly
        /// </summary>
        public void DisableLeftSaberMesh()
        {
            var saberRenderers = saberManager.leftSaber.gameObject.GetComponentsInChildren<Renderer>();
            foreach (var r in saberRenderers) { r.enabled = false; }
        }

        /// <summary>
        /// Disables all the renders in the right saber.  You may have to await or 
        /// yield return for 0.1f seconds before this method will work properly
        /// </summary>
        public void DisableRightSaberMesh()
        {
            var saberRenderers = saberManager.rightSaber.gameObject.GetComponentsInChildren<Renderer>();
            foreach (var r in saberRenderers) { r.enabled = false; }
        }

        private void Awake()
        {
            saberManager = MultiplayerLocalActivePlayerGameplayManagerPatch.multiplayerSaberManager ?? FindObjectOfType<SaberManager>();
            playerOrigin = GameObject.Find("LocalPlayerGameCore/Origin");
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        private void Update()
        {
            if (!calibrated) CalibrateSaberPositions();
        }

        /// <summary>
        /// Save the initial saber positions relative to the VRControllers 
        /// </summary>
        private void CalibrateSaberPositions()
        {
            calibrated = true;

            // Save current controller position
            savedLeftController = TrackedDeviceManager.GetDevicePose(leftController) ?? new Pose();
            savedLeftController = AdjustForPlayerOrigin(savedLeftController);

            savedRightController = TrackedDeviceManager.GetDevicePose(rightController) ?? new Pose();
            savedRightController = AdjustForPlayerOrigin(savedRightController);

            // Save current game saber positions
            savedLeftSaber.position = saberManager.leftSaber.transform.position;
            savedLeftSaber.rotation = saberManager.leftSaber.transform.rotation;

            savedRightSaber.position = saberManager.rightSaber.transform.position;
            savedRightSaber.rotation = saberManager.rightSaber.transform.rotation;
        }

        /// <summary>
        /// Adjusts any pose according to the player's position in Beat Saber into 
        /// the return pose. This accounts for Beat Saber's Room Adjust and Noodle Extensions
        /// viewpoint change functionality.
        /// </summary>
        private Pose AdjustForPlayerOrigin(Pose pose)
        {
            Pose newDevicePose = pose;
            if (playerOrigin != null)
            {
                // Adjust for room rotation and noodle extensions player movement as well
                newDevicePose.position = playerOrigin.transform.rotation * pose.position;
                newDevicePose.position += playerOrigin.transform.position;
                newDevicePose.position.x *= playerOrigin.transform.localScale.x;
                newDevicePose.position.y *= playerOrigin.transform.localScale.y;
                newDevicePose.position.z *= playerOrigin.transform.localScale.z;
                newDevicePose.rotation = playerOrigin.transform.rotation * pose.rotation;
            }

            return newDevicePose;
        }
    }
}
