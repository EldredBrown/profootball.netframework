using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    /// <summary>
    /// ViewModel logic for the Division tree view item.
    /// </summary>
    public class DivisionNodeViewModel : TreeViewItemViewModel
	{
        private readonly Division _division;

        /// <summary>
        /// Initializes a new instance of the DivisionViewModel class.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="controlService"></param>
        /// <param name="division"></param>
        /// <param name="sharedService"></param>
        /// <param name="lazyLoadChildren"></param>
        public DivisionNodeViewModel(ITreeViewItemViewModel parent, ISeasonStandingsControlService controlService,
            Division division, ISharedService sharedService = null, bool lazyLoadChildren = false)
            : base(parent, controlService, sharedService, lazyLoadChildren)
        {
            // Assign argument values to member fields.
            _division = division ?? throw new ArgumentNullException("division");
		}

        /// <summary>
        /// Gets the name of the Division instance wrapped inside this ViewModel.
        /// </summary>
        public override string AssociationName
        {
            get
            {
                return _division.Name;
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
        /// Shows the standings for the selected division.
        /// </summary>
        /// <param name="associationSeasonStandings">Enumerable collection of 
        /// GetSeasonStandingsForAssociation_Result objects used only for unit test dependency injection</param>
        protected internal override void ShowStandings(
            IList<GetSeasonStandingsForAssociation_Result> associationSeasonStandings = null)
        {
            try
            {
                // Load standings data for the Division object wrapped inside this ViewModel.
                Parent.Standings = new ReadOnlyCollection<GetSeasonStandingsForAssociation_Result>(
                    associationSeasonStandings ??
                    _controlService.GetSeasonStandingsForDivision((int)WpfGlobals.SelectedSeason, _division.Name)
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
