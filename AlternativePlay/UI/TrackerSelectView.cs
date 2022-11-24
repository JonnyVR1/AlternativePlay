using AlternativePlay.Models;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlternativePlay.UI
{
    [HotReload]
    public class TrackerSelectView : BSMLAutomaticViewController
    {
        // Internal tracker selection members
        private List<TrackerDisplayText> LoadedTrackers;
        private TrackerConfigData trackerConfigData;
        private TrackerConfigData originalTrackerData;
        private ModMainFlowCoordinator mainFlowCoordinator;

        public void SetMainFlowCoordinator(ModMainFlowCoordinator mainFlowCoordinator)
        {
            this.mainFlowCoordinator = mainFlowCoordinator;
        }

        public void SetSelectingTracker(TrackerConfigData trackerConfigData)
        {
            this.trackerConfigData = trackerConfigData;
            originalTrackerData = TrackerConfigData.Clone(trackerConfigData);
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

            if (trackerConfigData == null)
            {
                // Calls to this method should never pass in null
                AlternativePlay.Logger.Error($"TrackerSelectView.DidActivate() Error null tracker was given at {Environment.StackTrace}");
                trackerConfigData = new TrackerConfigData();
            }

            InitializeTrackerList();
            BehaviorCatalog.instance.ShowTrackersBehavior.EnableShowTrackers();
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            BehaviorCatalog.instance.ShowTrackersBehavior.DisableShowTrackers();
        }

        // Components
        [UIComponent("SelectTrackerList")]
        public CustomListTableData trackerList;

        private string currentTrackerText;
        [UIValue("CurrentTrackerText")]
        public string CurrentTrackerText { get => currentTrackerText; set { currentTrackerText = value; NotifyPropertyChanged(nameof(CurrentTrackerText)); } }

        // Events
        [UIAction("OnTrackerListCellSelected")]
        private void OnTrackerListCellSelected(TableView _, int row)
        {
            var tracker = LoadedTrackers[row];
            trackerConfigData.Serial = tracker.Serial;
            trackerConfigData.FullName = tracker.HoverHint;

            BehaviorCatalog.instance.ShowTrackersBehavior.SetSelectedSerial(trackerConfigData);
        }

        [UIAction("OnSelected")]
        private void OnSelected()
        {
            Configuration.instance.SaveConfiguration();
            mainFlowCoordinator.DismissTrackerSelect();
        }

        [UIAction("OnCancelled")]
        private void OnCancelled()
        {
            TrackerConfigData.Copy(originalTrackerData, trackerConfigData);
            Configuration.instance.SaveConfiguration();
            mainFlowCoordinator.DismissTrackerSelect();
        }

        /// <summary>
        /// Initializes the state and the bound variables for the Tracker Select list
        /// </summary>
        /// <param name="selectingLeft">Whether to initialize for the Left or the Right tracker</param>
        private void InitializeTrackerList()
        {
            trackerList.tableView.ClearSelection();
            trackerList.data.Clear();

            // Set the currently used tracker text
            CurrentTrackerText = String.IsNullOrWhiteSpace(trackerConfigData.FullName) ? TrackerConfigData.NoTrackerHoverHint : trackerConfigData.FullName;

            // Load the currently found trackers
            TrackedDeviceManager.instance.LoadTrackedDevices();
            TrackedDeviceManager.instance.TrackedDevices.ForEach(t =>
            {
                var customCellInfo = new CustomListTableData.CustomCellInfo(TrackerConfigData.FormatTrackerHoverHint(t));
                trackerList.data.Add(customCellInfo);
            });

            // Save the list of serials for later reference
            LoadedTrackers = TrackedDeviceManager.instance.TrackedDevices
                .Select(t => new TrackerDisplayText
                {
                    Serial = t.serialNumber,
                    HoverHint = TrackerConfigData.FormatTrackerHoverHint(t),
                }).ToList();

            // Reload all the data for display
            trackerList.tableView.ReloadData();

            // Find the cell to select
            int index = 0;
            if (!String.IsNullOrWhiteSpace(trackerConfigData.Serial))
            {
                index = LoadedTrackers.FindIndex(t => t.Serial == trackerConfigData.Serial);
            }

            if (index != -1 && trackerList.data.Count > 0)
            {
                trackerList.tableView.SelectCellWithIdx(index);
            }

            // Set the Tracker Renderer to show trackers
            BehaviorCatalog.instance.ShowTrackersBehavior.SetSelectedSerial(trackerConfigData);
        }
    }
}
