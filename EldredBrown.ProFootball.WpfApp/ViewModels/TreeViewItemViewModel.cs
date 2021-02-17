using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public interface ITreeViewItemViewModel
    {
        string AssociationName { get; }
        ObservableCollection<ITreeViewItemViewModel> Children { get; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        ReadOnlyCollection<GetSeasonStandingsForAssociation_Result> Standings { get; set; }
    }

    /// <summary>
    /// Base class for all TreeViewItem ViewModels.
    /// </summary>
    public class TreeViewItemViewModel : ViewModelBase, ITreeViewItemViewModel
    {
        protected static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();
        protected readonly ISeasonStandingsControlService _controlService;

        /// <summary>
        /// Initializes a new instance of the TreeViewItemViewModel class.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="controlService"></param>
        /// <param name="sharedService"></param>
        /// <param name="lazyLoadChildren"></param>
        protected TreeViewItemViewModel(ITreeViewItemViewModel parent, ISeasonStandingsControlService controlService,
            ISharedService sharedService = null, bool lazyLoadChildren = true)
            : base(sharedService)
		{
            Parent = parent;
            _controlService = controlService;

            try
            {
				Children = new ObservableCollection<ITreeViewItemViewModel>();
				if (lazyLoadChildren)
				{
					Children.Add(DummyChild);
				}
			}
			catch (Exception ex)
			{
                _sharedService.ShowExceptionMessage(ex);
			}
		}

		// This is used to create the DummyChild instance.
		private TreeViewItemViewModel()
			: this(parent: null, controlService: null, lazyLoadChildren: true)
		{
		}

        /// <summary>
        /// Gets the name of the association (league, conference, or division) represented by this object
        /// </summary>
        public virtual string AssociationName { get; }

        /// <summary>
        /// Gets the children for this TreeViewItem
        /// </summary>
        protected ObservableCollection<ITreeViewItemViewModel> _children;
        public ObservableCollection<ITreeViewItemViewModel> Children
        {
            get
            {
                return _children;
            }
            protected set
            {
                if (value != _children)
                {
                    _children = value;
                    OnPropertyChanged("Children");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TreeViewItem associated with this object is expanded.
        /// </summary>
        protected bool _isExpanded;
        public virtual bool IsExpanded
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
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TreeViewItem associated with this object is selected.
        /// </summary>
        protected bool _isSelected;
        public virtual bool IsSelected
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
            }
        }

        /// <summary>
        /// Gets or sets the standings table for display
        /// </summary>
        public virtual ReadOnlyCollection<GetSeasonStandingsForAssociation_Result> Standings { get; set; }

        /// <summary>
        /// Gets the parent for this TreeViewItem
        /// </summary>
        internal ITreeViewItemViewModel Parent { get; }

        /// <summary>
        /// Gets a value indicating whether this control's children have been populated yet.
        /// </summary>
        protected bool HasDummyChild()
        {
            return ((Children.Count == 1) && (Children[0] == DummyChild));
        }

        protected internal virtual void LoadChildren(ObservableCollection<ITreeViewItemViewModel> children = null)
        {
            // To be implemented in subclass
        }

        protected internal virtual void ShowStandings(
            IList<GetSeasonStandingsForAssociation_Result> associationSeasonStandingsViewModels = null)
        {
            // To be implemented in subclass
        }
    }
}
