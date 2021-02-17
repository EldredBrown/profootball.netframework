using System;
using System.Collections.ObjectModel;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public interface ISeasonStandingsControlViewModel
    {
        ReadOnlyCollection<ITreeViewItemViewModel> LeagueNodes { get; }
        ReadOnlyCollection<GetSeasonStandingsForAssociation_Result> Standings { get; set; }

        DelegateCommand ViewStandingsCommand { get; }
    }

    /// <summary>
    /// ViewModel logic for the Standings control.
    /// </summary>
    public class SeasonStandingsControlViewModel : ViewModelBase, ISeasonStandingsControlViewModel
    {
        private readonly ISeasonStandingsControlService _controlService;
        private readonly IRepository<Conference> _conferenceRepository;

        private ILeaguesTreeViewViewModel _leaguesTreeViewViewModel;

        /// <summary>
        /// Initializes a new instance of the StandingsControlViewModel class
        /// </summary>
        /// <param name="controlService"></param>
        public SeasonStandingsControlViewModel(ISharedService sharedService, 
            ISeasonStandingsControlService controlService, IRepository<Conference> conferenceRepository)
            : base(sharedService)
        {
            _controlService = controlService;
            _conferenceRepository = conferenceRepository;

            _leaguesTreeViewViewModel = null;
        }

        /// <summary>
        /// Gets the leagues collection for the TreeView control contained inside this control.
        /// </summary>
        private ReadOnlyCollection<ITreeViewItemViewModel> _leagueNodes;
        public ReadOnlyCollection<ITreeViewItemViewModel> LeagueNodes
        {
            get
            {
                return _leagueNodes;
            }
            private set
            {
                if (value != _leagueNodes)
                {
                    _leagueNodes = value;
                    OnPropertyChanged("LeagueNodes");
                }
            }
        }

        /// <summary>
        /// Gets or sets this control's standings collection.
        /// </summary>
        private ReadOnlyCollection<GetSeasonStandingsForAssociation_Result> _standings;
        public ReadOnlyCollection<GetSeasonStandingsForAssociation_Result> Standings
        {
            get
            {
                return _standings;
            }
            set
            {
                if (value != _standings)
                {
                    _standings = value;
                    OnPropertyChanged("Standings");
                }
            }
        }

        /// <summary>
        /// Loads the control.
        /// </summary>
        private DelegateCommand _viewStandingsCommand;
        public DelegateCommand ViewStandingsCommand
        {
            get
            {
                if (_viewStandingsCommand == null)
                {
                    _viewStandingsCommand = new DelegateCommand(param => ViewStandings());
                }
                return _viewStandingsCommand;
            }
        }
        private void ViewStandings()
        {
            try
            {
                Standings = null;

                var leagues = _controlService.GetLeaguesBySeason((int)WpfGlobals.SelectedSeason);

                _leaguesTreeViewViewModel = new LeaguesTreeViewViewModel(this, _controlService, leagues,
                    _conferenceRepository);
                LeagueNodes = _leaguesTreeViewViewModel.LeagueNodes;
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }
    }
}
