using AlternativePlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace AlternativePlay
{
    public class ShowTrackersBehavior : MonoBehaviour
    {
        private MainSettingsModelSO mainSettingsModel;

        private bool showTrackers;
        private TrackerConfigData selectedTracker;


        private List<TrackerInstance> trackerInstances;
        private GameObject saberInstance;

        /// <summary>
        /// Begins showing the tracked devices
        /// </summary>
        public void EnableShowTrackers()
        {
            RemoveAllInstances();

            trackerInstances = TrackedDeviceManager.instance.TrackedDevices.Select((t) => new TrackerInstance
            {
                Instance = Instantiate(BehaviorCatalog.instance.AssetLoaderBehavior.TrackerPrefab),
                InputDevice = t,
                Serial = t.serialNumber,
            }).ToList();

            trackerInstances.ForEach(t => t.Instance.SetActive(true));
            saberInstance = Instantiate(BehaviorCatalog.instance.AssetLoaderBehavior.SaberPrefab);
            showTrackers = true;
            enabled = true;
        }

        /// <summary>
        /// Hides all trackers and prevents rendering
        /// </summary>
        public void DisableShowTrackers()
        {
            showTrackers = false;
            RemoveAllInstances();

            enabled = false;
        }

        /// <summary>
        /// Sets the given serial of the tracker as the currently selected tracker.
        /// The tracker will be drawn with a saber.  Send null or a non-existant
        /// serial to set nothing to be selected.
        /// </summary>
        /// <param name="serial">The serial of the tracker to set as selected</param>
        public void SetSelectedSerial(TrackerConfigData tracker)
        {
            selectedTracker = tracker;
        }

        private void Awake()
        {
            mainSettingsModel = Resources.FindObjectsOfTypeAll<MainSettingsModelSO>().FirstOrDefault();

        }

        private void Update()
        {
            if (!showTrackers || trackerInstances == null || trackerInstances.Count == 0) return;

            foreach (var tracker in trackerInstances)
            {
                // Update all the tracker poses
                Pose trackerPose = TrackedDeviceManager.GetDevicePose(tracker.InputDevice) ?? new Pose();
                trackerPose = AdjustForRoomRotation(trackerPose);

                tracker.Instance.transform.position = trackerPose.position;
                tracker.Instance.transform.rotation = trackerPose.rotation;
            }

            var selectedTrackerInstance = trackerInstances.Find(t => t.Serial == selectedTracker.Serial);
            if (selectedTracker == null || String.IsNullOrWhiteSpace(selectedTracker.Serial))
            {
                // No selected tracker so disable the saber
                saberInstance.SetActive(false);
                return;
            }

            // Transform the Saber according to the Tracker Config Data
            Pose selectedTrackerPose = new Pose(
                selectedTrackerInstance.Instance.transform.position,
                selectedTrackerInstance.Instance.transform.rotation);

            Pose pose = Utilities.CalculatePoseFromTrackerData(selectedTracker, selectedTrackerPose);
            saberInstance.transform.position = pose.position;
            saberInstance.transform.rotation = pose.rotation;

            saberInstance.SetActive(true);
        }

        /// <summary>
        /// Given any pose this method returns a new pose that is adjusted for the Beat Saber
        /// Room Rotation.
        /// </summary>
        private Pose AdjustForRoomRotation(Pose pose)
        {
            var roomCenter = mainSettingsModel.roomCenter;
            var roomRotation = Quaternion.Euler(0, mainSettingsModel.roomRotation, 0);

            Pose result = pose;
            result.position = roomRotation * pose.position;
            result.position += roomCenter;
            result.rotation = roomRotation * pose.rotation;
            return result;
        }

        // Deletes all the instances and all the locally stored tracker instances data
        private void RemoveAllInstances()
        {
            if (trackerInstances != null) trackerInstances.ForEach(t => Destroy(t.Instance));
            trackerInstances = null;

            if (saberInstance != null) Destroy(saberInstance);
            saberInstance = null;
        }


        private class TrackerInstance
        {
            public GameObject Instance { get; set; }
            public bool Selected { get; set; }
            public string Serial { get; set; }
            public InputDevice InputDevice { get; internal set; }
        }

    }
}
