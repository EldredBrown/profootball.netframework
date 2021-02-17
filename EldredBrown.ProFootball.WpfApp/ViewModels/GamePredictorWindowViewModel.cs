using System;
using System.Collections.ObjectModel;
using System.Linq;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels.FocusVMLib;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public interface IGamePredictorWindowViewModel : IViewModelBase
    {
        string GuestName { get; set; }
        int? GuestScore { get; set; }
        ReadOnlyCollection<int> GuestSeasons { get; set; }
        int GuestSelectedSeason { get; set; }

        string HostName { get; set; }
        int? HostScore { get; set; }
        ReadOnlyCollection<int> HostSeasons { get; set; }
        int HostSelectedSeason { get; set; }

        DelegateCommand CalculatePredictionCommand { get; }
        DelegateCommand ViewSeasonsCommand { get; }
    }

    public class GamePredictorWindowViewModel : ViewModelBase, IFocusMover, IGamePredictorWindowViewModel
    {
        private readonly IGamePredictorWindowService _windowService;

        /// <summary>
        /// Initializes a new instance of the GamePredictorWindowViewModel class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="windowService"></param>
        public GamePredictorWindowViewModel(ISharedService sharedService, IGamePredictorWindowService windowService)
            : base(sharedService)
        {
            _windowService = windowService;
        }

        /// <summary>
        /// Gets or sets this window's guest value.
        /// </summary>
        private string _guestName;
        public string GuestName
        {
            get
            {
                return _guestName;
            }
            set
            {
                if (value != _guestName)
                {
                    _guestName = value;
                    OnPropertyChanged("GuestName");
                }
            }
        }

        /// <summary>
        /// Gets or sets this window's predicted guest score value
        /// </summary>
        private int? _guestScore;
        public int? GuestScore
        {
            get
            {
                return _guestScore;
            }
            set
            {
                if (value != _guestScore)
                {
                    _guestScore = value;
                    OnPropertyChanged("GuestScore");
                }
            }
        }

        /// <summary>
        /// Gets or sets this window's seasons collection.
        /// </summary>
        private ReadOnlyCollection<int> _guestSeasons;
        public ReadOnlyCollection<int> GuestSeasons
        {
            get
            {
                return _guestSeasons;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("GuestSeasons");
                }
                else if (value != _guestSeasons)
                {
                    _guestSeasons = value;
                    OnPropertyChanged("GuestSeasons");
                    RequestUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets this window's selected season.
        /// </summary>
        private int _guestSelectedSeason;
        public int GuestSelectedSeason
        {
            get
            {
                return _guestSelectedSeason;
            }
            set
            {
                if (value != _guestSelectedSeason)
                {
                    _guestSelectedSeason = value;
                    OnPropertyChanged("GuestSelectedSeason");
                }
            }
        }

        /// <summary>
        /// Gets or sets this window's host value.
        /// </summary>
        private string _hostName;
        public string HostName
        {
            get
            {
                return _hostName;
            }
            set
            {
                if (value != _hostName)
                {
                    _hostName = value;
                    OnPropertyChanged("HostName");
                }
            }
        }

        /// <summary>
        /// Gets or sets this window's predicted guest score value
        /// </summary>
        private int? _hostScore;
        public int? HostScore
        {
            get
            {
                return _hostScore;
            }
            set
            {
                if (value != _hostScore)
                {
                    _hostScore = value;
                    OnPropertyChanged("HostScore");
                }
            }
        }

        /// <summary>
        /// Gets or sets this window's seasons collection.
        /// </summary>
        private ReadOnlyCollection<int> _hostSeasons;
        public ReadOnlyCollection<int> HostSeasons
        {
            get
            {
                return _hostSeasons;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("HostSeasons");
                }
                else if (value != _hostSeasons)
                {
                    _hostSeasons = value;
                    OnPropertyChanged("HostSeasons");
                    RequestUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets this window's selected season.
        /// </summary>
        private int _hostSelectedSeason;
        public int HostSelectedSeason
        {
            get
            {
                return _hostSelectedSeason;
            }
            set
            {
                if (value != _hostSelectedSeason)
                {
                    _hostSelectedSeason = value;
                    OnPropertyChanged("HostSelectedSeason");
                }
            }
        }

        /// <summary>
        /// Loads the DataModel's Seasons table.
        /// </summary>
        private DelegateCommand _viewSeasonsCommand;
        public DelegateCommand ViewSeasonsCommand
        {
            get
            {
                if (_viewSeasonsCommand == null)
                {
                    _viewSeasonsCommand = new DelegateCommand(param => ViewSeasons());
                }
                return _viewSeasonsCommand;
            }
        }
        private void ViewSeasons()
        {
            try
            {
                var seasonIDs = _windowService.GetSeasonIDs();

                GuestSeasons = new ReadOnlyCollection<int>(seasonIDs.ToList());
                HostSeasons = new ReadOnlyCollection<int>(seasonIDs.ToList());

                GuestSelectedSeason = 1920;
                HostSelectedSeason = 1920;
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Calculates the predicted score of a future or hypothetical game.
        /// </summary>
        private DelegateCommand _calculatePredictionCommand;
        public DelegateCommand CalculatePredictionCommand
        {
            get
            {
                if (_calculatePredictionCommand == null)
                {
                    _calculatePredictionCommand = new DelegateCommand(param => CalculatePrediction());
                }
                return _calculatePredictionCommand;
            }
        }
        private void CalculatePrediction()
        {
            try
            {
                var matchup = ValidateDataEntry();

                var guestSeason = matchup.GuestSeason;
                var hostSeason = matchup.HostSeason;

                GuestScore = (int)((guestSeason.OffensiveFactor * hostSeason.DefensiveAverage +
                    hostSeason.DefensiveFactor * guestSeason.OffensiveAverage) / 2d);
                HostScore = (int)((hostSeason.OffensiveFactor * guestSeason.DefensiveAverage +
                    guestSeason.DefensiveFactor * hostSeason.OffensiveAverage) / 2d);
            }
            catch (DataValidationException ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Validates the data entered via the context window
        /// </summary>
        /// <returns></returns>
        public Matchup ValidateDataEntry()
        {
            var guestSeason = _sharedService.FindTeamSeason(GuestName, GuestSelectedSeason);
            var hostSeason = _sharedService.FindTeamSeason(HostName, HostSelectedSeason);

            if (string.IsNullOrWhiteSpace(GuestName))
            {
                // GuestName not in database (name probably misspelled)
                MoveFocusTo("GuestName");
                throw new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            }
            else if (string.IsNullOrWhiteSpace(HostName))
            {
                // GuestName not in database (name probably misspelled)
                MoveFocusTo("HostName");
                throw new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            }
            else if (guestSeason == null)
            {
                // GuestName not in database (name probably misspelled)
                MoveFocusTo("GuestName");
                throw new DataValidationException(WpfGlobals.Constants.TeamNotInDatabaseMessage);
            }
            else if (hostSeason == null)
            {
                // GuestName not in database (name probably misspelled)
                MoveFocusTo("HostName");
                throw new DataValidationException(WpfGlobals.Constants.TeamNotInDatabaseMessage);
            }

            return new Matchup(guestSeason, hostSeason);
        }

        /// <summary>
        /// Moves the focus to the specified property
        /// </summary>
        /// <param name="focusedProperty"></param>
        private void MoveFocusTo(string focusedProperty)
        {
            OnMoveFocus(focusedProperty);
        }
    }
}
