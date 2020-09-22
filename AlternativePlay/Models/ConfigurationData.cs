﻿using System;

namespace AlternativePlay.Models
{
    public enum PlayMode
    {
        BeatSaber = 0,
        DarthMaul = 1,
        BeatSpear = 2
    }

    public enum ControllerCountEnum
    {
        One = 0,
        Two
    };

    [Serializable]
    public class ConfigurationData
    {
        public PlayMode PlayMode { get; set; } = PlayMode.BeatSaber;

        // Beat Saber Options
        public bool UseLeftSaber { get; set; }
        public bool ReverseLeftSaber { get; set; }
        public bool ReverseRightSaber { get; set; }
        public string LeftSaberTracker { get; set; }
        public string RightSaberTracker { get; set; }
        public string LeftSaberTrackerFullName { get; set; }
        public string RightSaberTrackerFullName { get; set; }

        // Darth Maul Options
        public ControllerCountEnum DarthMaulControllerCount { get; set; } = ControllerCountEnum.One;
        public bool UseLeftController { get; set; }
        public bool ReverseMaulDirection { get; set; }
        public bool UseTriggerToSeparate { get; set; }
        public int MaulDistance { get; set; } = 15;
        public string LeftMaulTracker { get; set; }
        public string LeftMaulTrackerFullName { get; set; }
        public string RightMaulTracker { get; set; }
        public string RightMaulTrackerFullName { get; set; }

        // Spear Options
        public ControllerCountEnum SpearControllerCount { get; set; } = ControllerCountEnum.One;
        public bool UseLeftSpear { get; set; }
        public bool UseTriggerToSwitchHands { get; set; }
        public bool ReverseSpearDirection { get; set; }
        public string LeftSpearTracker { get; set; }
        public string LeftSpearTrackerFullName { get; set; }
        public string RightSpearTracker { get; set; }
        public string RightSpearTrackerFullName { get; set; }

        // Gameplay Changes Options
        public bool NoArrowsRandom { get; set; }
        public bool OneColor { get; set; }
        public bool RemoveOtherSaber { get; set; }
        public bool NoArrows { get; set; }
        public bool TouchNotes { get; set; }
    }
}