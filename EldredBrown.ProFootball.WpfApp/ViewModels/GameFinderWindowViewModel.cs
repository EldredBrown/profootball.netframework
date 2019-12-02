using System;
using EldredBrown.ProFootball.WpfApp.Services;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public interface IGameFinderWindowViewModel
    {
        string GuestName { get; set; }
        string HostName { get; set; }
        DelegateCommand WindowLoadedCommand { get; }

        void ValidateDataEntry();
    }

    /// <summary>
    /// ViewModel logic for the GameFinderWindow.
    /// </summary>
    public class GameFinderWindowViewModel : ViewModelBase, IGameFinderWindowViewModel
    {
        #region Constructors & Finalizers

        public GameFinderWindowViewModel(ISharedService sharedService)
            : base(sharedService)
        {}

        #endregion Constructors & Finalizers

        #region Properties

        /// <summary>
        /// Gets or sets this window's guest value.
        /// </summary>
        private string _GuestName;
        public string GuestName
        {
            get
            {
                return _GuestName;
            }
            set
            {
                if (value != _GuestName)
                {
                    _GuestName = value;
                    OnPropertyChanged("GuestName");
                }
            }
        }

        /// <summary>
        /// Gets or sets this window's host value.
        /// </summary>
        private string _HostName;
        public string HostName
        {
            get
            {
                return _HostName;
            }
            set
            {
                if (value != _HostName)
                {
                    _HostName = value;
                    OnPropertyChanged("HostName");
                }
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Views the Games database table.
        /// </summary>
        private DelegateCommand _windowLoadedCommand;
        public DelegateCommand WindowLoadedCommand
        {
            get
            {
                if (_windowLoadedCommand == null)
                {
                    _windowLoadedCommand = new DelegateCommand(param => WindowLoaded());
                }
                return _windowLoadedCommand;
            }
        }
        private void WindowLoaded()
        {
            MoveFocusTo("GuestName");
        }

        #endregion Commands

        #region Methods

        /// <summary>
        /// Validates data entered into the data entry controls.
        /// </summary>
        public void ValidateDataEntry()
        {
            if (String.IsNullOrWhiteSpace(GuestName) || String.IsNullOrWhiteSpace(HostName))
            {
                throw new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            }
            else if (GuestName == HostName)
            {
                throw new DataValidationException(WpfGlobals.Constants.DifferentTeamsNeededErrorMessage);
            }
        }

        #region Helpers

        /// <summary>
        /// Moves the focus to the specified property
        /// </summary>
        /// <param name="focusedProperty"></param>
        private void MoveFocusTo(string focusedProperty)
        {
            OnMoveFocus(focusedProperty);
        }

        #endregion Helpers

        #endregion Methods
    }
}
