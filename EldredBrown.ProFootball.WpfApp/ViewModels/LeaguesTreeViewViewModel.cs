using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public interface ILeaguesTreeViewViewModel
    {
        ReadOnlyCollection<ITreeViewItemViewModel> LeagueNodes { get; }
        ReadOnlyCollection<GetSeasonStandingsForAssociation_Result> Standings { get; set; }
    }

    /// <summary>
    /// ViewModel logic for the Leagues TreeView control.
    /// </summary>
    public class LeaguesTreeViewViewModel : ViewModelBase, ILeaguesTreeViewViewModel
    {
        private readonly ISeasonStandingsControlViewModel _parentControl;
        private readonly ISeasonStandingsControlService _controlService;

        /// <summary>
        /// Initializes a new instance of the LeaguesTreeViewViewModel class
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="parentControl"></param>
        /// <param name="leagues"></param>
        /// <param name="WpfGlobals"></param>
        /// <param name="leagueNodes"></param>
        public LeaguesTreeViewViewModel(ISeasonStandingsControlViewModel parentControl,
            ISeasonStandingsControlService controlService, IEnumerable<League> leagues,
            IRepository<Conference> conferenceRepository, ReadOnlyCollection<ITreeViewItemViewModel> leagueNodes = null)
            :base(null)
        {
            _parentControl = parentControl ?? throw new ArgumentNullException("parentControl");
            _controlService = controlService ?? throw new ArgumentNullException("service");

            try
            {
                LeagueNodes = leagueNodes ?? new ReadOnlyCollection<ITreeViewItemViewModel>((
                    from league in leagues
                    select new LeagueNodeViewModel(this, _controlService, league, conferenceRepository))
                    .ToList<ITreeViewItemViewModel>());
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Gets this control's leagues collection.
        /// </summary>
        private ReadOnlyCollection<ITreeViewItemViewModel> _leagueNodes;
        public ReadOnlyCollection<ITreeViewItemViewModel> LeagueNodes
        {
            get
            {
                return _leagueNodes;
            }
            internal set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("LeagueNodes");
                }
                else if (value != _leagueNodes)
                {
                    _leagueNodes = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets this control's standings collection.
        /// </summary>
        public ReadOnlyCollection<GetSeasonStandingsForAssociation_Result> Standings
        {
            get
            {
                return _parentControl.Standings;
            }
            set
            {
                _parentControl.Standings = value;
            }
        }
    }
}
