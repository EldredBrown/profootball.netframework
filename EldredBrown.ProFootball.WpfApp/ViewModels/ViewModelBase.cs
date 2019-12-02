using System;
using System.ComponentModel;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels.FocusVMLib;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    /// <summary>
    /// Base class for all the ViewModel classes used in this project.
    /// </summary>
    public abstract class ViewModelBase : IFocusMover, INotifyPropertyChanged, IViewModelBase
    {
        #region Member Fields

        protected readonly ISharedService _sharedService;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the ViewModelBase class.
        /// </summary>
        protected ViewModelBase(ISharedService sharedService)
		{
            _sharedService = sharedService;
		}

        #endregion Constructors & Finalizers

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this view model object needs to be updated
        /// </summary>
        private bool _requestUpdate;
		public bool RequestUpdate
		{
			get
            {
                return _requestUpdate;
            }
			set
			{
				_requestUpdate = value;
				OnPropertyChanged("RequestUpdate");
				_requestUpdate = false;
			}
		}

        #endregion Properties

        #region Commands

        public DelegateCommand UpdateCommand
		{
			get
			{
				return new DelegateCommand(param => { RequestUpdate = true; });
			}
		}

        #endregion Commands

        #region Event Raisers

        /// <summary>
        /// Raises MoveFocus event when focus is moved from one control to another.
        /// </summary>
        /// <param name="focusedProperty"></param>
        protected virtual void OnMoveFocus(string focusedProperty)
        {
            // Validate focusedProperty argument.
            if (String.IsNullOrEmpty(focusedProperty))
            {
                throw new ArgumentException("focusedProperty");
            }

            // Raise MoveFocus event.
            MoveFocus?.Invoke(this, new MoveFocusEventArgs(focusedProperty));
        }

        /// <summary>
        /// Raise the PropertyChanged event.
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
		{
			// Validate name argument.
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentException("name");
			}

			// Raise event to all subscribed event handlers.
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

        #endregion Event Raisers

        #region IFocusMover Members

        public event EventHandler<MoveFocusEventArgs> MoveFocus;

        #endregion IFocusMover Members

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Event triggered when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members
    }
}
