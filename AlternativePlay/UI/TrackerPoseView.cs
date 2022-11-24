﻿using AlternativePlay.Models;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AlternativePlay.UI
{
    [HotReload]
    public class TrackerPoseView : BSMLAutomaticViewController
    {
        private const float positionScaling = 1000.0f;
        private const float rotationScaling = 10.0f;

        [UIParams]
#pragma warning disable CS0649 // Field 'TrackerPoseView.parserParams' is never assigned to, and will always have its default value null
        private BSMLParserParams parserParams;
#pragma warning restore CS0649 // Field 'TrackerPoseView.parserParams' is never assigned to, and will always have its default value null

        private TrackerConfigData trackerConfigData;
        private Vector3 originalPosition;
        private Vector3 originalEuler;

        public void SetSelectingTracker(TrackerConfigData trackerConfigData)
        {
            this.trackerConfigData = trackerConfigData;
            originalPosition = this.trackerConfigData.Position;
            originalEuler = this.trackerConfigData.EulerAngles;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            RefreshAllValues();
        }

        [UIValue("PositionIncrementChoice")]
        private string positionIncrement = Configuration.instance.ConfigurationData.PositionIncrement;
        [UIValue("PositionIncrementList")]
        private List<object> positionIncrementList = ConfigurationData.PositionIncrementList.Cast<object>().ToList();
        [UIAction("OnPositionIncrementChanged")]
        private void OnPositionIncrementChanged(string value)
        {
            Configuration.instance.ConfigurationData.PositionIncrement = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("RotationIncrementChoice")]
        private string rotationIncrement = Configuration.instance.ConfigurationData.RotationIncrement;
        [UIValue("RotationIncrementList")]
        private List<object> rotationIncrementList = ConfigurationData.RotationIncrementList.Cast<object>().ToList();
        [UIAction("OnRotationIncrementChanged")]
        private void OnRotationIncrementChanged(string value)
        {
            Configuration.instance.ConfigurationData.RotationIncrement = value;
            Configuration.instance.SaveConfiguration();
        }

        [UIValue("PositionX")]
        private int PositionX
        {
            get => Convert.ToInt32(trackerConfigData.Position.x * positionScaling);
            set
            {
                int incrementedValue = PositionIncrement(Convert.ToInt32(trackerConfigData.Position.x * positionScaling), value);
                trackerConfigData.Position = new Vector3(incrementedValue / positionScaling, trackerConfigData.Position.y, trackerConfigData.Position.z);

                Configuration.instance.SaveConfiguration();
                parserParams.EmitEvent("RefreshPositionXEvent");
            }
        }

        [UIValue("PositionY")]
        private int PositionY
        {
            get => Convert.ToInt32(trackerConfigData.Position.y * positionScaling);
            set
            {
                int incrementedValue = PositionIncrement(Convert.ToInt32(trackerConfigData.Position.y * positionScaling), value);
                trackerConfigData.Position = new Vector3(trackerConfigData.Position.x, incrementedValue / positionScaling, trackerConfigData.Position.z);

                Configuration.instance.SaveConfiguration();
                parserParams.EmitEvent("RefreshPositionYEvent");
            }
        }

        [UIValue("PositionZ")]
        private int PositionZ
        {
            get => Convert.ToInt32(trackerConfigData.Position.z * positionScaling);
            set
            {
                int incrementedValue = PositionIncrement(Convert.ToInt32(trackerConfigData.Position.z * positionScaling), value);
                trackerConfigData.Position = new Vector3(trackerConfigData.Position.x, trackerConfigData.Position.y, incrementedValue / positionScaling);

                Configuration.instance.SaveConfiguration();
                parserParams.EmitEvent("RefreshPositionZEvent");
            }
        }

        [UIValue("RotationX")]
        private int RotationX
        {
            get => Convert.ToInt32(trackerConfigData.EulerAngles.x * rotationScaling);
            set
            {
                int incrementedValue = RotationIncrement(Convert.ToInt32(trackerConfigData.EulerAngles.x * rotationScaling), value);
                trackerConfigData.EulerAngles = new Vector3(incrementedValue / rotationScaling, trackerConfigData.EulerAngles.y, trackerConfigData.EulerAngles.z);

                Configuration.instance.SaveConfiguration();
                parserParams.EmitEvent("RefreshRotationXEvent");
            }
        }

        [UIValue("RotationY")]
        private int RotationY
        {
            get => Convert.ToInt32(trackerConfigData.EulerAngles.y * rotationScaling);
            set
            {
                int incrementedValue = RotationIncrement(Convert.ToInt32(trackerConfigData.EulerAngles.y * rotationScaling), value);
                trackerConfigData.EulerAngles = new Vector3(trackerConfigData.EulerAngles.x, incrementedValue / rotationScaling, trackerConfigData.EulerAngles.z);

                Configuration.instance.SaveConfiguration();
                parserParams.EmitEvent("RefreshRotationYEvent");
            }
        }

        [UIValue("RotationZ")]
        private int RotationZ
        {
            get => Convert.ToInt32(trackerConfigData.EulerAngles.z * rotationScaling);
            set
            {
                int incrementedValue = RotationIncrement(Convert.ToInt32(trackerConfigData.EulerAngles.z * rotationScaling), value);
                trackerConfigData.EulerAngles = new Vector3(trackerConfigData.EulerAngles.x, trackerConfigData.EulerAngles.y, incrementedValue / rotationScaling);

                Configuration.instance.SaveConfiguration();
                parserParams.EmitEvent("RefreshRotationZEvent");
            }
        }

        [UIAction("PositionFormatter")]
        private string PositionFormatter(float value)
        {
            return String.Format("{0:0.0} cm", value / 10.0f);
        }

        [UIAction("RotationFormatter")]
        private string RotationFormatter(float value)
        {
            return string.Format("{0:0.0} deg", value / rotationScaling);
        }


        [UIAction("OnReset")]
        private void OnSelected()
        {
            trackerConfigData.Position = Vector3.zero;
            trackerConfigData.EulerAngles = Vector3.zero;
            Configuration.instance.SaveConfiguration();
            RefreshAllValues();
        }


        [UIAction("OnRevert")]
        private void OnRevert()
        {
            trackerConfigData.Position = originalPosition;
            trackerConfigData.EulerAngles = originalEuler;
            Configuration.instance.SaveConfiguration();
            RefreshAllValues();
        }

        private void RefreshAllValues()
        {
            parserParams.EmitEvent("RefreshPositionXEvent");
            parserParams.EmitEvent("RefreshPositionYEvent");
            parserParams.EmitEvent("RefreshPositionZEvent");
            parserParams.EmitEvent("RefreshRotationXEvent");
            parserParams.EmitEvent("RefreshRotationYEvent");
            parserParams.EmitEvent("RefreshRotationZEvent");
        }

        private int PositionIncrement(int currentValue, int value)
        {
            float positionIncrement = ConfigurationData.GetIncrement(Configuration.instance.ConfigurationData.PositionIncrement);

            int result = currentValue;
            result = currentValue < value
                ? result + Convert.ToInt32(positionIncrement * 10.0f)
                : result - Convert.ToInt32(positionIncrement * 10.0f);

            result = Math.Min(result, Convert.ToInt32(ConfigurationData.PositionMax * 10.0f));  // Clamps to the MAX
            result = Math.Max(result, Convert.ToInt32(ConfigurationData.PositionMax) * -10);  // Clamps to the MIN
            return result;
        }

        private int RotationIncrement(int currentValue, int value)
        {
            float rotationIncrement = ConfigurationData.GetIncrement(Configuration.instance.ConfigurationData.RotationIncrement);

            int result = currentValue;
            result = currentValue < value
                ? result + Convert.ToInt32(rotationIncrement * rotationScaling)
                : result - Convert.ToInt32(rotationIncrement * rotationScaling);

            // Go over the 0 / 360 degree point
            if (result >= Convert.ToInt32(ConfigurationData.RotationMax * rotationScaling)) result = result - 3600;
            if (result < 0) result = result + 3600;

            return result;
        }
    }
}
