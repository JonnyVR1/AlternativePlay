using AlternativePlay.Models;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;

namespace AlternativePlay.UI
{
    [HotReload]
    public class DarthMaulView : BSMLAutomaticViewController
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
        private string controllerChoice = Configuration.instance.ConfigurationData.DarthMaulControllerCount.ToString();
        [UIValue("ControllerChoiceList")]
        private List<object> controllerChoiceList = new List<object> { "One", "Two" };
        [UIAction("OnControllersChanged")]
        private void OnControllersChanged(string value)
        {
            Configuration.instance.ConfigurationData.DarthMaulControllerCount = (ControllerCountEnum)Enum.Parse(typeof(ControllerCountEnum), value);
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("UseLeftController")]
        private bool useLeftController = Configuration.instance.ConfigurationData.UseLeftController;
        [UIAction("OnUseLeftControllerChanged")]
        private void OnUseLeftControllerChanged(bool value)
        {
            Configuration.instance.ConfigurationData.UseLeftController = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("ReverseMaulDirection")]
        private bool reverseSaberDirection = Configuration.instance.ConfigurationData.ReverseMaulDirection;
        [UIAction("OnReverseMaulDirectionChanged")]
        private void OnReverseMaulDirectionChanged(bool value)
        {
            Configuration.instance.ConfigurationData.ReverseMaulDirection = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("UseTriggerToSeparate")]
        private bool useTriggerToSeparate = Configuration.instance.ConfigurationData.UseTriggerToSeparate;
        [UIAction("OnUseTriggerToSeparateChanged")]
        private void OnUseTriggerToSeparateChanged(bool value)
        {
            Configuration.instance.ConfigurationData.UseTriggerToSeparate = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("SeparationAmount")]
        private int separationAmount = Configuration.instance.ConfigurationData.MaulDistance;
        [UIAction("OnSeparationAmountChanged")]
        private void OnSeparationAmountChanged(int value)
        {
            Configuration.instance.ConfigurationData.MaulDistance = value;
            Configuration.instance.SaveConfiguration();
        }

        #region SelectTracker Modal Members

        // Text Displays for the Main View
        private string leftMaulTrackerSerial;
        [UIValue("LeftMaulTrackerSerial")]
        public string LeftMaulTrackerSerial { get => leftMaulTrackerSerial; set { leftMaulTrackerSerial = value; NotifyPropertyChanged(nameof(LeftMaulTrackerSerial)); } }

        private string leftMaulTrackerHoverHint;
        [UIValue("LeftMaulTrackerHoverHint")]
        public string LeftMaulTrackerHoverHint { get => leftMaulTrackerHoverHint; set { leftMaulTrackerHoverHint = value; NotifyPropertyChanged(nameof(LeftMaulTrackerHoverHint)); } }

        private string rightMaulTrackerSerial;
        [UIValue("RightMaulTrackerSerial")]
        public string RightMaulTrackerSerial { get => rightMaulTrackerSerial; set { rightMaulTrackerSerial = value; NotifyPropertyChanged(nameof(RightMaulTrackerSerial)); } }

        private string rightMaulTrackerHoverHint;
        [UIValue("RightMaulTrackerHoverHint")]
        public string RightMaulTrackerHoverHint { get => rightMaulTrackerHoverHint; set { rightMaulTrackerHoverHint = value; NotifyPropertyChanged(nameof(RightMaulTrackerHoverHint)); } }

        // Text Display for the Current Tracker in the Tracker Select Modal
        private string currentTrackerText;
        [UIValue("CurrentTrackerText")]
        public string CurrentTrackerText { get => currentTrackerText; set { currentTrackerText = value; NotifyPropertyChanged(nameof(CurrentTrackerText)); } }

        // Events
        [UIAction("OnShowSelectLeftTracker")]
        private void OnShowSelectLeftTracker()
        {
            mainFlowCoordinator.ShowTrackerSelect(Configuration.instance.ConfigurationData.LeftMaulTracker);
        }

        [UIAction("OnShowSelectRightTracker")]
        private void OnShowSelectRightTracker()
        {
            mainFlowCoordinator.ShowTrackerSelect(Configuration.instance.ConfigurationData.RightMaulTracker);
        }

        [UIAction("OnClearLeftTracker")]
        private void OnClearLeftTracker()
        {
            Configuration.instance.ConfigurationData.LeftMaulTracker = new TrackerConfigData();
            Configuration.instance.SaveConfiguration();
            LeftMaulTrackerSerial = TrackerConfigData.NoTrackerText;
            LeftMaulTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
        }

        [UIAction("OnClearRightTracker")]
        private void OnClearRightTracker()
        {
            Configuration.instance.ConfigurationData.RightMaulTracker = new TrackerConfigData();
            Configuration.instance.SaveConfiguration();
            RightMaulTrackerSerial = TrackerConfigData.NoTrackerText;
            RightMaulTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
        }

        /// <summary>
        /// Initializes the bound variables for the fields on this view
        /// </summary>
        private void SetTrackerText()
        {
            var config = Configuration.instance.ConfigurationData;
            if (String.IsNullOrWhiteSpace(config.LeftMaulTracker.Serial))
            {
                LeftMaulTrackerSerial = TrackerConfigData.NoTrackerText;
                LeftMaulTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
            }
            else
            {
                LeftMaulTrackerSerial = config.LeftMaulTracker.Serial;
                LeftMaulTrackerHoverHint = config.LeftMaulTracker.FullName;
            }

            if (String.IsNullOrWhiteSpace(config.RightMaulTracker.Serial))
            {
                RightMaulTrackerSerial = TrackerConfigData.NoTrackerText;
                RightMaulTrackerHoverHint = TrackerConfigData.NoTrackerHoverHint;
            }
            else
            {
                RightMaulTrackerSerial = config.RightMaulTracker.Serial;
                RightMaulTrackerHoverHint = config.RightMaulTracker.FullName;
            }
        }

        #endregion

    }
}
