using AlternativePlay.Models;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;

namespace AlternativePlay.UI
{
    [HotReload]
    public class BeatSpearView : BSMLAutomaticViewController
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

        [UIValue("ControllerChoice")]
        private string controllerChoice = Configuration.instance.ConfigurationData.SpearControllerCount.ToString();
        [UIValue("ControllerChoiceList")]
        private List<object> controllerChoiceList = new List<object> { "One", "Two" };
        [UIAction("OnControllersChanged")]
        private void OnControllersChanged(string value)
        {
            Configuration.instance.ConfigurationData.SpearControllerCount = (ControllerCountEnum)Enum.Parse(typeof(ControllerCountEnum), value);
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("UseLeftSpear")]
        private bool useLeftSpear = Configuration.instance.ConfigurationData.UseLeftSpear;
        [UIAction("OnUseLeftSpearChanged")]
        private void OnUseLeftSpearChanged(bool value)
        {
            Configuration.instance.ConfigurationData.UseLeftSpear = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("UseTriggerToSwitchHands")]
        private bool useTriggerToSwitchHands = Configuration.instance.ConfigurationData.UseTriggerToSwitchHands;
        [UIAction("OnUseTriggerToSwitchHands")]
        private void OnUseTriggerToSwitchHands(bool value)
        {
            Configuration.instance.ConfigurationData.UseTriggerToSwitchHands = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("ReverseSpearDirection")]
        private bool reverseSpearDirection = Configuration.instance.ConfigurationData.ReverseSpearDirection;
        [UIAction("OnReverseSpearDirectionChanged")]
        private void OnReverseSpearDirectionChanged(bool value)
        {
            Configuration.instance.ConfigurationData.ReverseSpearDirection = value;
            Configuration.instance.SaveConfiguration();
        }

        #region SelectTracker Modal Members

        // Text Displays for the Main View
        private string leftSpearTrackerSerial;
        [UIValue("LeftSpearTrackerSerial")]
        public string LeftSpearTrackerSerial { get => leftSpearTrackerSerial; set { leftSpearTrackerSerial = value; NotifyPropertyChanged(nameof(LeftSpearTrackerSerial)); } }

        private string leftSpearTrackerHoverHint;
        [UIValue("LeftSpearTrackerHoverHint")]
        public string LeftSpearTrackerHoverHint { get => leftSpearTrackerHoverHint; set { leftSpearTrackerHoverHint = value; NotifyPropertyChanged(nameof(LeftSpearTrackerHoverHint)); } }

        private string rightSpearTrackerSerial;
        [UIValue("RightSpearTrackerSerial")]
        public string RightSpearTrackerSerial { get => rightSpearTrackerSerial; set { rightSpearTrackerSerial = value; NotifyPropertyChanged(nameof(RightSpearTrackerSerial)); } }

        private string rightSpearTrackerHoverHint;
        [UIValue("RightSpearTrackerHoverHint")]
        public string RightSpearTrackerHoverHint { get => rightSpearTrackerHoverHint; set { rightSpearTrackerHoverHint = value; NotifyPropertyChanged(nameof(RightSpearTrackerHoverHint)); } }

        // Text Display for the Current Tracker in the Tracker Select Modal
        private string currentTrackerText;
        [UIValue("CurrentTrackerText")]
        public string CurrentTrackerText { get => currentTrackerText; set { currentTrackerText = value; NotifyPropertyChanged(nameof(CurrentTrackerText)); } }

        // Events
        [UIAction("OnShowSelectLeftTracker")]
        private void OnShowSelectLeftTracker()
        {
            mainFlowCoordinator.ShowTrackerSelect(Configuration.instance.ConfigurationData.LeftSpearTracker);
        }

        [UIAction("OnShowSelectRightTracker")]
        private void OnShowSelectRightTracker()
        {
            mainFlowCoordinator.ShowTrackerSelect(Configuration.instance.ConfigurationData.RightSpearTracker);
        }

        [UIAction("OnClearLeftTracker")]
        private void OnClearLeftTracker()
        {
            Configuration.instance.ConfigurationData.LeftSpearTracker = new TrackerConfigData();
            Configuration.instance.SaveConfiguration();
            LeftSpearTrackerSerial = TrackerConfigData.NoTrackerText;
            LeftSpearTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
        }

        [UIAction("OnClearRightTracker")]
        private void OnClearRightTracker()
        {
            Configuration.instance.ConfigurationData.RightSpearTracker = new TrackerConfigData();
            Configuration.instance.SaveConfiguration();
            RightSpearTrackerSerial = TrackerConfigData.NoTrackerText;
            RightSpearTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
        }

        /// <summary>
        /// Initializes the bound variables for the fields on this view
        /// </summary>
        private void SetTrackerText()
        {
            var config = Configuration.instance.ConfigurationData;
            if (String.IsNullOrWhiteSpace(config.LeftSpearTracker.Serial))
            {
                LeftSpearTrackerSerial = TrackerConfigData.NoTrackerText;
                LeftSpearTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
            }
            else
            {
                LeftSpearTrackerSerial = config.LeftSpearTracker.Serial;
                LeftSpearTrackerHoverHint = config.LeftSpearTracker.FullName;
            }

            if (String.IsNullOrWhiteSpace(config.RightSpearTracker.Serial))
            {
                RightSpearTrackerSerial = TrackerConfigData.NoTrackerText;
                RightSpearTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
            }
            else
            {
                RightSpearTrackerSerial = config.RightSpearTracker.Serial;
                RightSpearTrackerHoverHint = config.RightSpearTracker.FullName;
            }
        }

        #endregion

    }
}
