﻿using AlternativePlay.Models;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using Zenject;

namespace AlternativePlay.UI
{
    [HotReload]
    public class GameModifiersView : BSMLAutomaticViewController
    {
#pragma warning disable CS0649
        [Inject]
        private Configuration configuration;
#pragma warning restore CS0649

        private PlayModeSettings settings;

        public void SetPlayModeSettings(PlayModeSettings Settings)
        {
            this.settings = Settings;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            this.UpdateAllValues();
        }

        [UIValue(nameof(NoArrowsRandomIcon))]
        public string NoArrowsRandomIcon => IconNames.NoArrowsRandom;

        [UIValue(nameof(NoArrowsRandom))]
        private bool NoArrowsRandom
        {
            get => this.settings.NoArrowsRandom;
            set
            {
                this.settings.NoArrowsRandom = value;
                this.configuration.SaveConfiguration();
            }
        }

        [UIValue(nameof(OneColorIcon))]
        public string OneColorIcon => IconNames.OneColor;

        [UIValue(nameof(OneColor))]
        private bool OneColor
        {
            get => this.settings.OneColor;
            set
            {
                this.settings.OneColor = value;
                this.configuration.SaveConfiguration();
            }
        }

        [UIValue(nameof(NoSlidersIcon))]
        public string NoSlidersIcon => IconNames.NoSliders;

        [UIValue(nameof(NoSliders))]
        private bool NoSliders
        {
            get => this.settings.NoSliders;
            set
            {
                this.settings.NoSliders = value;
                this.configuration.SaveConfiguration();
            }
        }

        [UIValue(nameof(NoArrowsIcon))]
        public string NoArrowsIcon => IconNames.NoArrows;

        [UIValue(nameof(NoArrows))]
        private bool NoArrows
        {
            get => this.settings.NoArrows;
            set
            {
                this.settings.NoArrows = value;
                this.configuration.SaveConfiguration();
            }
        }

        [UIValue(nameof(TouchNotesIcon))]
        public string TouchNotesIcon => IconNames.TouchNotes;

        [UIValue(nameof(TouchNotes))]
        private bool TouchNotes
        {
            get => this.settings.TouchNotes;
            set
            {
                this.settings.TouchNotes = value;
                this.configuration.SaveConfiguration();
            }
        }

        private void UpdateAllValues()
        {
            this.NotifyPropertyChanged(nameof(this.NoArrowsRandom));
            this.NotifyPropertyChanged(nameof(this.OneColor));
            this.NotifyPropertyChanged(nameof(this.NoSliders));
            this.NotifyPropertyChanged(nameof(this.NoArrows));
            this.NotifyPropertyChanged(nameof(this.TouchNotes));
        }
    }
}
