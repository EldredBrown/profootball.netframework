using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    /// <summary>
    /// ViewModel logic for the Conference tree view item.
    /// </summary>
    public class ConferenceNodeViewModel : TreeViewItemViewModel
	{
        private readonly Conference _conference;

        /// <summary>
        /// Initializes a new instance of the Conference class.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="controlService"></param>
        /// <param name="conference"></param>
        /// <param name="sharedService"></param>
        /// <param name="lazyLoadChildren"></param>
        public ConferenceNodeViewModel(ITreeViewItemViewModel parent, ISeasonStandingsControlService controlService,
            Conference conference, ISharedService sharedService = null, bool lazyLoadChildren = true)
			: base(parent, controlService, sharedService, lazyLoadChildren)
		{
            // Assign argument values to member fields.
            _conference = conference ?? throw new ArgumentNullException("conference");
        }

        /// <summary>
        /// Gets the name of the Conference instance wrapped inside this ViewModel.
        /// </summary>
        public override string AssociationName
        {
            get
            {
                return _conference.Name;
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
                return Parent.Standings;
            }
            set
            {
                Parent.Standings = value;
            }
        }

        /// <summary>
        /// Loads this object's children into the TreeView.
        /// </summary>
        protected internal override void LoadChildren(ObservableCollection<ITreeViewItemViewModel> children = null)
        {
            try
            {
                // Load collection of Division objects for the League object wrapped inside this ViewModel.
                var divisions = _controlService.GetDivisionsByConferenceAndSeason(_conference.Name,
                    (int)WpfGlobals.SelectedSeason);

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
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Shows the standings for the selected conference.
        /// </summary>
        protected internal override void ShowStandings(
            IList<GetSeasonStandingsForAssociation_Result> associationSeasonStandings = null)
        {
            try
            {
                // Load standings data for the Conference object wrapped inside this ViewModel.
                //var conferenceName = _division.ConferenceName;
                Parent.Standings = new ReadOnlyCollection<GetSeasonStandingsForAssociation_Result>(
                    associationSeasonStandings ?? 
                    _controlService.GetSeasonStandingsForConference((int)WpfGlobals.SelectedSeason, _conference.Name)
                    .ToList<GetSeasonStandingsForAssociation_Result>());
            }
            catch (Exception ex)
            {
                Parent.Standings = null;
                _sharedService.ShowExceptionMessage(ex.InnerException);
            }
        }
    }
}
