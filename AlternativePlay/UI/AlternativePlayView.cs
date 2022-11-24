using AlternativePlay.Models;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace AlternativePlay.UI
{
    [HotReload]
    public class AlternativePlayView : BSMLAutomaticViewController
    {
        public ModMainFlowCoordinator MainFlowCoordinator { get; set; }

        private const string Grey = "#4F4F4F";
        private const string White = "#FFFFFF";

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetPlayModeColor(Configuration.instance.ConfigurationData.PlayMode);
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
        }

        private string beatSaberColor = White;
        [UIValue("BeatSaberColor")]
        public string BeatSaberColor { get => beatSaberColor; set { beatSaberColor = value; NotifyPropertyChanged(nameof(BeatSaberColor)); } }
        [UIValue("BeatSaberDefaultColor")]
        public string BeatSaberDefaultColor { get => beatSaberColor; set { beatSaberColor = value; NotifyPropertyChanged(nameof(BeatSaberColor)); } }
        [UIValue("BeatSaberHighLightColor")]
        public string BeatSaberHighLightColor { get => beatSaberColor; set { beatSaberColor = value; NotifyPropertyChanged(nameof(BeatSaberColor)); } }

        private string beatSaberIcon;
        [UIValue("BeatSaberIcon")]
        public string BeatSaberIcon { get => beatSaberIcon; set { beatSaberIcon = value; NotifyPropertyChanged(nameof(BeatSaberIcon)); } }

        private string darthMaulColor = Grey;
        [UIValue("DarthMaulColor")]
        public string DarthMaulColor { get => darthMaulColor; set { darthMaulColor = value; NotifyPropertyChanged(nameof(DarthMaulColor)); } }
        [UIValue("DarthMaulDefaultColor")]
        public string DarthMaulDefaultColor { get => darthMaulColor; set { darthMaulColor = value; NotifyPropertyChanged(nameof(DarthMaulColor)); } }
        [UIValue("DarthMaulHightLightColor")]
        public string DarthMaulHightLightColor { get => darthMaulColor; set { darthMaulColor = value; NotifyPropertyChanged(nameof(DarthMaulColor)); } }

        private string darthMaulIcon;
        [UIValue("DarthMaulIcon")]
        public string DarthMaulIcon { get => darthMaulIcon; set { darthMaulIcon = value; NotifyPropertyChanged(nameof(DarthMaulIcon)); } }

        private string beatSpearColor = Grey;
        [UIValue("BeatSpearColor")]
        public string BeatSpearColor { get => beatSpearColor; set { beatSpearColor = value; NotifyPropertyChanged(nameof(BeatSpearColor)); } }
        [UIValue("BeatSpearDefaultColor")]
        public string BeatSpearDefaultColor { get => beatSpearColor; set { beatSpearColor = value; NotifyPropertyChanged(nameof(BeatSpearColor)); } }
        [UIValue("BeatSpearHighLightColor")]
        public string BeatSpearHighLightColor { get => beatSpearColor; set { beatSpearColor = value; NotifyPropertyChanged(nameof(BeatSpearColor)); } }

        private string beatSpearIcon;
        [UIValue("BeatSpearIcon")]
        public string BeatSpearIcon { get => beatSpearIcon; set { beatSpearIcon = value; NotifyPropertyChanged(nameof(BeatSpearIcon)); } }

        private string nunchakuColor = Grey;
        [UIValue("NunchakuColor")]
        public string NunchakuColor { get => nunchakuColor; set { nunchakuColor = value; NotifyPropertyChanged(nameof(NunchakuColor)); } }
        [UIValue("NunchakuDefaultColor")]
        public string NunchakuDefaultColor { get => nunchakuColor; set { nunchakuColor = value; NotifyPropertyChanged(nameof(NunchakuDefaultColor)); } }
        [UIValue("NunchakuHighLightColor")]
        public string NunchakuHighLightColor { get => nunchakuColor; set { nunchakuColor = value; NotifyPropertyChanged(nameof(NunchakuColor)); } }

        private string nunchakuIcon;
        [UIValue("NunchakuIcon")]
        public string NunchakuIcon { get => nunchakuIcon; set { nunchakuIcon = value; NotifyPropertyChanged(nameof(NunchakuIcon)); } }

        private string flailColor = Grey;
        [UIValue("FlailColor")]
        public string FlailColor { get => flailColor; set { flailColor = value; NotifyPropertyChanged(nameof(FlailColor)); } }
        [UIValue("FlailDefaultColor")]
        public string FlailDefaultColor { get => flailColor; set { flailColor = value; NotifyPropertyChanged(nameof(FlailColor)); } }
        [UIValue("FlailHighLightColor")]
        public string FlailHighLightColor { get => flailColor; set { flailColor = value; NotifyPropertyChanged(nameof(FlailColor)); } }

        private string flailIcon;
        [UIValue("FlailIcon")]
        public string FlailIcon { get => flailIcon; set { flailIcon = value; NotifyPropertyChanged(nameof(FlailIcon)); } }

        [UIAction("BeatSaberClick")]
        private void OnBeatSaberClick()
        {
            Configuration.instance.ConfigurationData.PlayMode = PlayMode.BeatSaber;
            Configuration.instance.SaveConfiguration();

            SetPlayModeColor(Configuration.instance.ConfigurationData.PlayMode);
            MainFlowCoordinator.ShowBeatSaber();
        }

        [UIAction("DarthMaulClick")]
        private void OnDarthMaulClick()
        {
            Configuration.instance.ConfigurationData.PlayMode = PlayMode.DarthMaul;
            Configuration.instance.SaveConfiguration();

            SetPlayModeColor(Configuration.instance.ConfigurationData.PlayMode);
            MainFlowCoordinator.ShowDarthMaul();
        }

        [UIAction("BeatSpearClick")]
        private void OnBeatSpearClick()
        {
            Configuration.instance.ConfigurationData.PlayMode = PlayMode.BeatSpear;
            Configuration.instance.SaveConfiguration();

            SetPlayModeColor(Configuration.instance.ConfigurationData.PlayMode);
            MainFlowCoordinator.ShowBeatSpear();
        }

        [UIAction("NunchakuClick")]
        private void OnNunchakuClick()
        {
            Configuration.instance.ConfigurationData.PlayMode = PlayMode.Nunchaku;
            Configuration.instance.SaveConfiguration();

            SetPlayModeColor(Configuration.instance.ConfigurationData.PlayMode);
            MainFlowCoordinator.ShowNunchaku();
        }

        [UIAction("FlailClick")]
        private void OnFlailClick()
        {
            Configuration.instance.ConfigurationData.PlayMode = PlayMode.BeatFlail;
            Configuration.instance.SaveConfiguration();

            SetPlayModeColor(Configuration.instance.ConfigurationData.PlayMode);
            MainFlowCoordinator.ShowBeatFlail();
        }

        private void SetPlayModeColor(PlayMode playMode)
        {
            // Set everything to grey first
            BeatSaberColor = Grey;
            BeatSaberIcon = "AlternativePlay.Public.BeatSaberGrey.png";
            DarthMaulColor = Grey;
            DarthMaulIcon = "AlternativePlay.Public.DarthMaulGrey.png";
            BeatSpearColor = Grey;
            BeatSpearIcon = "AlternativePlay.Public.BeatSpearGrey.png";
            NunchakuColor = Grey;
            NunchakuIcon = "AlternativePlay.Public.NoArrows.png";
            FlailColor = Grey;
            FlailIcon = "AlternativePlay.Public.NoArrows.png";

            // Set only the item we selected to white
            switch (playMode)
            {
                case PlayMode.DarthMaul:
                    DarthMaulColor = White;
                    DarthMaulIcon = "AlternativePlay.Public.DarthMaul.png";
                    break;

                case PlayMode.BeatSpear:
                    BeatSpearColor = White;
                    BeatSpearIcon = "AlternativePlay.Public.BeatSpear.png";
                    break;

                case PlayMode.Nunchaku:
                    NunchakuColor = White;
                    NunchakuIcon = "AlternativePlay.Public.NoArrows.png";
                    break;

                case PlayMode.BeatFlail:
                    FlailColor = White;
                    FlailIcon = "AlternativePlay.Public.NoArrows.png";
                    break;

                case PlayMode.BeatSaber:
                default:
                    BeatSaberColor = White;
                    BeatSaberIcon = "AlternativePlay.Public.BeatSaber.png";
                    break;
            }
        }
    }
}
