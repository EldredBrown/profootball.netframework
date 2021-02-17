using System;
using System.Collections.ObjectModel;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public interface ITeamSeasonsControlViewModel
    {
        TeamSeason SelectedTeam { get; set; }

        ReadOnlyCollection<TeamSeason> Teams { get; set; }
        ReadOnlyCollection<GetTeamSeasonScheduleProfile_Result> TeamSeasonScheduleProfile { get; }
        ReadOnlyCollection<GetTeamSeasonScheduleTotals_Result> TeamSeasonScheduleTotals { get; }
        ReadOnlyCollection<GetTeamSeasonScheduleAverages_Result> TeamSeasonScheduleAverages { get; }

        DelegateCommand ViewTeamScheduleCommand { get; }
        DelegateCommand ViewTeamsCommand { get; }
    }

    /// <summary>
    /// ViewModel logic for the Teams control.
    /// </summary>
    public class TeamSeasonsControlViewModel : ViewModelBase, ITeamSeasonsControlViewModel
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITeamSeasonsControlService _controlService;

        /// <summary>
        /// Initializes a new instance of the TeamSeasonsControlViewModel class 
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="controlService"></param>
        public TeamSeasonsControlViewModel(ISharedService sharedService, ITeamSeasonsControlService controlService)
            : base(sharedService)
        {
            _controlService = controlService;
        }

        /// <summary>
        /// Gets or sets this control's selected team.
        /// </summary>
        private TeamSeason _selectedTeam;
        public TeamSeason SelectedTeam
        {
            get
            {
                return _selectedTeam;
            }
            set
            {
                if (value != _selectedTeam)
                {
                    _selectedTeam = value;
                    OnPropertyChanged("SelectedTeam");
                }
            }
        }

        /// <summary>
        /// Gets this control's teams collection.
        /// </summary>
        private ReadOnlyCollection<TeamSeason> _teams;
        public ReadOnlyCollection<TeamSeason> Teams
        {
            get
            {
                return _teams;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Teams");
                }
                else if (value != _teams)
                {
                    _teams = value;
                    OnPropertyChanged("Teams");
                    RequestUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets this control's team schedule profile collection.
        /// </summary>
        private ReadOnlyCollection<GetTeamSeasonScheduleProfile_Result> _teamSeasonScheduleProfile;
        public ReadOnlyCollection<GetTeamSeasonScheduleProfile_Result> TeamSeasonScheduleProfile
        {
            get
            {
                return _teamSeasonScheduleProfile;
            }
            private set
            {
                if (value != _teamSeasonScheduleProfile)
                {
                    _teamSeasonScheduleProfile = value;
                    OnPropertyChanged("TeamSeasonScheduleProfile");
                }
            }
        }

        /// <summary>
        /// Gets this controls's collection of team schedule totals.
        /// </summary>
        private ReadOnlyCollection<GetTeamSeasonScheduleTotals_Result> _teamSeasonScheduleTotals;
        public ReadOnlyCollection<GetTeamSeasonScheduleTotals_Result> TeamSeasonScheduleTotals
        {
            get
            {
                return _teamSeasonScheduleTotals;
            }
            private set
            {
                if (value != _teamSeasonScheduleTotals)
                {
                    _teamSeasonScheduleTotals = value;
                    OnPropertyChanged("TeamSeasonScheduleTotals");
                }
            }
        }

        /// <summary>
        /// Gets this control's collection of team schedule averages.
        /// </summary>
        private ReadOnlyCollection<GetTeamSeasonScheduleAverages_Result> _teamSeasonScheduleAverages;
        public ReadOnlyCollection<GetTeamSeasonScheduleAverages_Result> TeamSeasonScheduleAverages
        {
            get
            {
                return _teamSeasonScheduleAverages;
            }
            private set
            {
                if (value != _teamSeasonScheduleAverages)
                {
                    _teamSeasonScheduleAverages = value;
                    OnPropertyChanged("TeamSeasonScheduleAverages");
                }
            }
        }

        /// <summary>
        /// Views the team schedule profile, totals, and averages for the selected team.
        /// </summary>
        private DelegateCommand _viewTeamScheduleCommand;
        public DelegateCommand ViewTeamScheduleCommand
        {
            get
            {
                if (_viewTeamScheduleCommand == null)
                {
                    _viewTeamScheduleCommand = new DelegateCommand(param => ViewTeamSchedule());
                }
                return _viewTeamScheduleCommand;
            }
        }
        private void ViewTeamSchedule()
        {
            try
            {
                if (SelectedTeam == null)
                {
                    TeamSeasonScheduleProfile = null;
                    TeamSeasonScheduleTotals = null;
                    TeamSeasonScheduleAverages = null;
                }
                else
                {
                    // Load data from TeamScheduleProfile, TeamScheduleTotals, and TeamScheduleAverages for selected team.
                    var teamName = SelectedTeam.TeamName;
                    var seasonID = (int)WpfGlobals.SelectedSeason;

                    TeamSeasonScheduleProfile = new ReadOnlyCollection<GetTeamSeasonScheduleProfile_Result>(
                        _controlService.GetTeamSeasonScheduleProfile(teamName, seasonID).ToList());

                    TeamSeasonScheduleTotals = new ReadOnlyCollection<GetTeamSeasonScheduleTotals_Result>(
                        _controlService.GetTeamSeasonScheduleTotals(teamName, seasonID).ToList());

                    TeamSeasonScheduleAverages = new ReadOnlyCollection<GetTeamSeasonScheduleAverages_Result>(
                        _controlService.GetTeamSeasonScheduleAverages(teamName, seasonID).ToList());
                }
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Loads the DataModel's Teams table.
        /// </summary>
        private DelegateCommand _viewTeamsCommand;
        public DelegateCommand ViewTeamsCommand
        {
            get
            {
                if (_viewTeamsCommand == null)
                {
                    _viewTeamsCommand = new DelegateCommand(param => ViewTeams());
                }
                return _viewTeamsCommand;
            }
        }
        private void ViewTeams()
        {
            try
            {
                Teams = new ReadOnlyCollection<TeamSeason>(
                    _controlService.GetEntities((int)WpfGlobals.SelectedSeason).ToList());
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
