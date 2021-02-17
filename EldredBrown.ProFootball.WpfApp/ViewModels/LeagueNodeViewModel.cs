using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    /// <summary>
    /// ViewModel logic for a node of the Leagues TreeView control.
    /// </summary>
    public class LeagueNodeViewModel : TreeViewItemViewModel
    {
        private readonly ILeaguesTreeViewViewModel _parentControl;
        private readonly League _league;
        private readonly IRepository<Conference> _conferenceRepository;

        /// <summary>
        /// Initializes a new instance of the LeagueNodeViewModel class.
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="controlService"></param>
        /// <param name="league"></param>
        /// <param name="conferenceRepository"></param>
        /// <param name="sharedService"></param>
        /// <param name="lazyLoadChildren"></param>
        public LeagueNodeViewModel(ILeaguesTreeViewViewModel parentControl,
            ISeasonStandingsControlService controlService, League league,
            IRepository<Conference> conferenceRepository, ISharedService sharedService = null,
            bool lazyLoadChildren = true)
            : base(null, controlService, sharedService, lazyLoadChildren)
        {
            _parentControl = parentControl ?? throw new ArgumentNullException("parentControl");
            _league = league ?? throw new ArgumentNullException("league");

            _conferenceRepository = conferenceRepository;
        }

        /// <summary>
        /// Gets the name of the League instance wrapped inside this ViewModel.
        /// </summary>
        public override string AssociationName
        {
            get
            {
                return _league.Name;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TreeViewItem associated with this object is expanded.
        /// </summary>
        public override bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && Parent != null)
                {
                    Parent.IsExpanded = true;
                }

                // Lazy load the child items, if necessary.
                if (HasDummyChild())
                {
                    Children.Remove(DummyChild);
                    LoadChildren();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TreeViewItem associated with this object is selected.
        /// </summary>
        public override bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }

                ShowStandings();
            }
        }

        /// <summary>
        /// Gets or sets this control's standings collection.
        /// </summary>
        public override ReadOnlyCollection<GetSeasonStandingsForAssociation_Result> Standings
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

        /// <summary>
        /// Loads this object's children into the TreeView.
        /// </summary>
        protected internal override void LoadChildren(ObservableCollection<ITreeViewItemViewModel> children = null)
        {
            var seasonID = (int)WpfGlobals.SelectedSeason;

            try
            {
                // Load collection of Conference objects for the League object wrapped inside this ViewModel.
                var conferences = _controlService.GetConferencesByLeagueAndSeason(_league.Name, seasonID);

                if (conferences.Count() > 0)
                {
                    Children = children ?? new ObservableCollection<ITreeViewItemViewModel>((
                        from conference in conferences
                        select new ConferenceNodeViewModel(this, _controlService, conference))
                        .ToList<TreeViewItemViewModel>());
                }
                else
                {
                    // Load collection of Division objects for the League object wrapped inside this ViewModel.
                    var divisions = _controlService.GetDivisionsByLeagueAndSeason(_league.Name, seasonID);

                    if (divisions.Count() > 0)
                    {
                        Children = children ?? new ObservableCollection<ITreeViewItemViewModel>((
                            from division in divisions
                            select new DivisionNodeViewModel(this, _controlService, division))
                            .ToList<TreeViewItemViewModel>());
                    }
                    else
                    {
                        Children = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Shows the standings for the selected league.
        /// </summary>
        protected internal override void ShowStandings(
            IList<GetSeasonStandingsForAssociation_Result> associationSeasonStandings = null)
        {
            try
            {
                // Load standings data for the League object wrapped inside this ViewModel.
                Standings = new ReadOnlyCollection<GetSeasonStandingsForAssociation_Result>(
                    associationSeasonStandings ??
                    _controlService.GetSeasonStandingsForLeague((int)WpfGlobals.SelectedSeason, _league.Name)
                        .ToList<GetSeasonStandingsForAssociation_Result>());
            }
            catch (Exception ex)
            {
                Standings = null;
                _sharedService.ShowExceptionMessage(ex.InnerException);
            }
        }
    }
}
