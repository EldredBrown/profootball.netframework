using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public interface IRankingsControlViewModel
    {
        ReadOnlyCollection<GetRankingsOffensive_Result> OffensiveRankings { get; set; }
        ReadOnlyCollection<GetRankingsDefensive_Result> DefensiveRankings { get; set; }
        ReadOnlyCollection<GetRankingsTotal_Result> TotalRankings { get; set; }

        DelegateCommand ViewRankingsCommand { get; }
    }

    /// <summary>
    /// ViewModel logic for the Rankings control.
    /// </summary>
    public class RankingsControlViewModel : ViewModelBase, IRankingsControlViewModel
    {
        private readonly IRankingsControlService _controlService;

        /// <summary>
        /// Initializes a new instance of the RankingsControlViewModel class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="controlService"></param>
        public RankingsControlViewModel(ISharedService sharedService, IRankingsControlService controlService)
            :base(sharedService)
        {
            _controlService = controlService;
        }

        /// <summary>
        /// Gets or sets this control's offensive rankings collection.
        /// </summary>
        private ReadOnlyCollection<GetRankingsOffensive_Result> _offensiveRankings;
        public ReadOnlyCollection<GetRankingsOffensive_Result> OffensiveRankings
        {
            get
            {
                return _offensiveRankings;
            }
            set
            {
                Debug.Assert(value != null);

                if (value == null)
                {
                    throw new ArgumentNullException("OffensiveRankings");
                }
                else if (value != _offensiveRankings)
                {
                    _offensiveRankings = value;
                    OnPropertyChanged("OffensiveRankings");
                }
            }
        }

        /// <summary>
        /// Gets or sets this control's defensive rankings collection.
        /// </summary>
        private ReadOnlyCollection<GetRankingsDefensive_Result> _defensiveRankings;
        public ReadOnlyCollection<GetRankingsDefensive_Result> DefensiveRankings
        {
            get
            {
                return _defensiveRankings;
            }
            set
            {
                Debug.Assert(value != null);

                if (value == null)
                {
                    throw new ArgumentNullException("DefensiveRankings");
                }
                else if (value != _defensiveRankings)
                {
                    _defensiveRankings = value;
                    OnPropertyChanged("DefensiveRankings");
                }
            }
        }

        /// <summary>
        /// Gets or sets this control's total rankings collection.
        /// </summary>
        private ReadOnlyCollection<GetRankingsTotal_Result> _totalRankings;
        public ReadOnlyCollection<GetRankingsTotal_Result> TotalRankings
        {
            get
            {
                return _totalRankings;
            }
            set
            {
                Debug.Assert(value != null);

                if (value == null)
                {
                    throw new ArgumentNullException("TotalRankings");
                }
                else if (value != _totalRankings)
                {
                    _totalRankings = value;
                    OnPropertyChanged("TotalRankings");
                }
            }
        }

        /// <summary>
        /// Loads the DataModel's Teams table.
        /// </summary>
        private DelegateCommand _viewRankingsCommand;
        public DelegateCommand ViewRankingsCommand
        {
            get
            {
                if (_viewRankingsCommand == null)
                {
                    _viewRankingsCommand = new DelegateCommand(param => ViewRankings());
                }
                return _viewRankingsCommand;
            }
        }
        private void ViewRankings()
        {
            try
            {
                // Load the rankings grids.
                var seasonID = (int)WpfGlobals.SelectedSeason;

                TotalRankings = new ReadOnlyCollection<GetRankingsTotal_Result>(
                    _controlService.GetRankingsTotalBySeason(seasonID).ToList());

                OffensiveRankings = new ReadOnlyCollection<GetRankingsOffensive_Result>(
                    _controlService.GetRankingsOffensiveBySeason(seasonID).ToList());

                DefensiveRankings = new ReadOnlyCollection<GetRankingsDefensive_Result>(
                    _controlService.GetRankingsDefensiveBySeason(seasonID).ToList());
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }
    }
}
