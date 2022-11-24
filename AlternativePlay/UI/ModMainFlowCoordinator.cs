using AlternativePlay.Models;
using BeatSaberMarkupLanguage;
using HMUI;

namespace AlternativePlay.UI
{
    public class ModMainFlowCoordinator : FlowCoordinator
    {
        private const string titleString = "Alternative Play";
        private AlternativePlayView alternativePlayView;
        private GameModifiersView gameModifiersView;

        private BeatSaberView beatSaberSettingsView;
        private DarthMaulView darthMaulSettingsView;
        private BeatSpearView beatSpearSettingsView;
        private NunchakuView nunchakuView;
        private BeatFlailView beatFlailView;

        private TrackerSelectView trackerSelectView;
        private TrackerPoseView trackerPoseView;

        public bool IsBusy { get; set; }

        public void ShowBeatSaber()
        {
            IsBusy = true;
            SetLeftScreenViewController(beatSaberSettingsView, ViewController.AnimationType.In);
            IsBusy = false;
        }

        public void ShowDarthMaul()
        {
            IsBusy = true;
            SetLeftScreenViewController(darthMaulSettingsView, ViewController.AnimationType.In);
            IsBusy = false;
        }

        public void ShowBeatSpear()
        {
            IsBusy = true;
            SetLeftScreenViewController(beatSpearSettingsView, ViewController.AnimationType.In);
            IsBusy = false;
        }

        public void ShowNunchaku()
        {
            IsBusy = true;
            SetLeftScreenViewController(nunchakuView, ViewController.AnimationType.In);
            IsBusy = false;
        }

        public void ShowBeatFlail()
        {
            IsBusy = true;
            SetLeftScreenViewController(beatFlailView, ViewController.AnimationType.In);
            IsBusy = false;
        }

        public void ShowTrackerSelect(TrackerConfigData trackerConfigData)
        {
            IsBusy = true;
            SetTitle("Select Tracker");

            trackerSelectView.SetSelectingTracker(trackerConfigData);
            trackerPoseView.SetSelectingTracker(trackerConfigData);

            ReplaceTopViewController(trackerSelectView);
            SetLeftScreenViewController(null, ViewController.AnimationType.In);
            SetRightScreenViewController(trackerPoseView, ViewController.AnimationType.In);

            IsBusy = false;

        }

        public void DismissTrackerSelect()
        {
            IsBusy = true;
            SetTitle(titleString);

            ReplaceTopViewController(alternativePlayView);
            var viewToDisplay = DecideLeftMainView();
            SetLeftScreenViewController(viewToDisplay, ViewController.AnimationType.Out);
            SetRightScreenViewController(gameModifiersView, ViewController.AnimationType.Out);
            IsBusy = false;
        }

        private void Awake()
        {
            alternativePlayView = BeatSaberUI.CreateViewController<AlternativePlayView>();
            alternativePlayView.MainFlowCoordinator = this;
            gameModifiersView = BeatSaberUI.CreateViewController<GameModifiersView>();

            beatSaberSettingsView = BeatSaberUI.CreateViewController<BeatSaberView>();
            beatSaberSettingsView.SetMainFlowCoordinator(this);
            darthMaulSettingsView = BeatSaberUI.CreateViewController<DarthMaulView>();
            darthMaulSettingsView.SetMainFlowCoordinator(this);
            beatSpearSettingsView = BeatSaberUI.CreateViewController<BeatSpearView>();
            beatSpearSettingsView.SetMainFlowCoordinator(this);
            nunchakuView = BeatSaberUI.CreateViewController<NunchakuView>();
            nunchakuView.SetMainFlowCoordinator(this);
            beatFlailView = BeatSaberUI.CreateViewController<BeatFlailView>();
            beatFlailView.SetMainFlowCoordinator(this);

            trackerSelectView = BeatSaberUI.CreateViewController<TrackerSelectView>();
            trackerSelectView.SetMainFlowCoordinator(this);
            trackerPoseView = BeatSaberUI.CreateViewController<TrackerPoseView>();
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetTitle(titleString);
            showBackButton = true;

            var viewToDisplay = DecideLeftMainView();

            IsBusy = true;
            trackerSelectView.SetSelectingTracker(new TrackerConfigData());
            ProvideInitialViewControllers(alternativePlayView, viewToDisplay, gameModifiersView);
            IsBusy = false;
        }

        private ViewController DecideLeftMainView()
        {
            ViewController viewToDisplay;

            switch (Configuration.instance.ConfigurationData.PlayMode)
            {
                case PlayMode.DarthMaul:
                    viewToDisplay = darthMaulSettingsView;
                    break;

                case PlayMode.BeatSpear:
                    viewToDisplay = beatSpearSettingsView;
                    break;

                case PlayMode.Nunchaku:
                    viewToDisplay = nunchakuView;
                    break;

                case PlayMode.BeatFlail:
                    viewToDisplay = beatFlailView;
                    break;

                case PlayMode.BeatSaber:
                default:
                    viewToDisplay = beatSaberSettingsView;
                    break;
            }

            return viewToDisplay;
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            if (IsBusy) return;

            if (topViewController == trackerSelectView)
            {
                DismissTrackerSelect();
                return;
            }

            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}
