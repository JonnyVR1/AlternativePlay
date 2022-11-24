using AlternativePlay.Models;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;

namespace AlternativePlay.UI
{
    [HotReload]
    public class BeatSaberView : BSMLAutomaticViewController
    {
        private ModMainFlowCoordinator mainFlowCoordinator;

        public void SetMainFlowCoordinator(ModMainFlowCoordinator mainFlowCoordinator)
        {
            this.mainFlowCoordinator = mainFlowCoordinator;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            SetTrackerText();
        }

        [UIValue("ReverseLeftSaber")]
        private bool reverseLeftSaber = Configuration.instance.ConfigurationData.ReverseLeftSaber;
        [UIAction("OnReverseLeftSaberChanged")]
        private void OnReverseLeftSaberChanged(bool value)
        {
            Configuration.instance.ConfigurationData.ReverseLeftSaber = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("ReverseRightSaber")]
        private bool reverseRightSaber = Configuration.instance.ConfigurationData.ReverseRightSaber;
        [UIAction("OnReverseRightSaberChanged")]
        private void OnReverseRightSaberChanged(bool value)
        {
            Configuration.instance.ConfigurationData.ReverseRightSaber = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("RemoveOtherSaber")]
        private bool removeOtherSaber = Configuration.instance.ConfigurationData.RemoveOtherSaber;
        [UIAction("OnRemoveOtherSaberChanged")]
        private void OnRemoveOtherSaberChanged(bool value)
        {
            Configuration.instance.ConfigurationData.RemoveOtherSaber = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("UseLeftSaber")]
        private bool useLeftSaber = Configuration.instance.ConfigurationData.UseLeftSaber;
        [UIAction("OnUseLeftSaberChanged")]
        private void OnUseLeftSaberChanged(bool value)
        {
            Configuration.instance.ConfigurationData.UseLeftSaber = value;
            Configuration.instance.SaveConfiguration();
        }

        #region SelectTracker Modal Members

        // Text Displays for the Main View
        private string leftSaberTrackerSerial;
        [UIValue("LeftSaberTrackerSerial")]
        public string LeftSaberTrackerSerial { get => leftSaberTrackerSerial; set { leftSaberTrackerSerial = value; NotifyPropertyChanged(nameof(LeftSaberTrackerSerial)); } }

        private string leftSaberTrackerHoverHint;
        [UIValue("LeftSaberTrackerHoverHint")]
        public string LeftSaberTrackerHoverHint { get => leftSaberTrackerHoverHint; set { leftSaberTrackerHoverHint = value; NotifyPropertyChanged(nameof(LeftSaberTrackerHoverHint)); } }

        private string rightSaberTrackerSerial;
        [UIValue("RightSaberTrackerSerial")]
        public string RightSaberTrackerSerial { get => rightSaberTrackerSerial; set { rightSaberTrackerSerial = value; NotifyPropertyChanged(nameof(RightSaberTrackerSerial)); } }

        private string rightSaberTrackerHoverHint;
        [UIValue("RightSaberTrackerHoverHint")]
        public string RightSaberTrackerHoverHint { get => rightSaberTrackerHoverHint; set { rightSaberTrackerHoverHint = value; NotifyPropertyChanged(nameof(RightSaberTrackerHoverHint)); } }

        // Text Display for the Current Tracker in the Tracker Select Modal
        private string currentTrackerText;
        [UIValue("CurrentTrackerText")]
        public string CurrentTrackerText { get => currentTrackerText; set { currentTrackerText = value; NotifyPropertyChanged(nameof(CurrentTrackerText)); } }

        // Events

        [UIAction("OnShowSelectLeftTracker")]
        private void OnShowSelectLeftTracker()
        {
            mainFlowCoordinator.ShowTrackerSelect(Configuration.instance.ConfigurationData.LeftSaberTracker);
        }

        [UIAction("OnShowSelectRightTracker")]
        private void OnShowSelectRightTracker()
        {
            mainFlowCoordinator.ShowTrackerSelect(Configuration.instance.ConfigurationData.RightSaberTracker);
        }

        [UIAction("OnClearLeftTracker")]
        private void OnClearLeftTracker()
        {
            Configuration.instance.ConfigurationData.LeftSaberTracker = new TrackerConfigData();
            Configuration.instance.SaveConfiguration();
            LeftSaberTrackerSerial = TrackerConfigData.NoTrackerText;
            LeftSaberTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
        }

        [UIAction("OnClearRightTracker")]
        private void OnClearRightTracker()
        {
            Configuration.instance.ConfigurationData.RightSaberTracker = new TrackerConfigData();
            Configuration.instance.SaveConfiguration();
            RightSaberTrackerSerial = TrackerConfigData.NoTrackerText;
            RightSaberTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
        }

        /// <summary>
        /// Initializes the bound variables for the fields on this view
        /// </summary>
        private void SetTrackerText()
        {
            var config = Configuration.instance.ConfigurationData;
            if (String.IsNullOrWhiteSpace(config.LeftSaberTracker.Serial))
            {
                LeftSaberTrackerSerial = TrackerConfigData.NoTrackerText;
                LeftSaberTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
            }
            else
            {
                LeftSaberTrackerSerial = config.LeftSaberTracker.Serial;
                LeftSaberTrackerHoverHint = config.LeftSaberTracker.FullName;
            }

            if (String.IsNullOrWhiteSpace(config.RightSaberTracker.Serial))
            {
                RightSaberTrackerSerial = TrackerConfigData.NoTrackerText;
                RightSaberTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
            }
            else
            {
                RightSaberTrackerSerial = config.RightSaberTracker.Serial;
                RightSaberTrackerHoverHint = config.RightSaberTracker.FullName;
            }
        }

        #endregion

    }
}
