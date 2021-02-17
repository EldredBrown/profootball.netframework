using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels.FocusVMLib;
using EldredBrown.ProFootball.WpfApp.Windows;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public interface IGamesWindowViewModel : IViewModelBase
    {
        Visibility AddGameControlVisibility { get; set; }
        Visibility DeleteGameControlVisibility { get; set; }
        Visibility EditGameControlVisibility { get; set; }
        IGameFinderWindow GameFinder { get; set; }
        ReadOnlyCollection<Game> Games { get; set; }
        string GuestName { get; set; }
        double GuestScore { get; set; }
        string HostName { get; set; }
        double HostScore { get; set; }
        bool IsFindGameFilterApplied { get; set; }
        bool IsGamesReadOnly { get; set; }
        bool IsPlayoffGame { get; set; }
        bool IsPlayoffGameEnabled { get; set; }
        bool IsShowAllGamesEnabled { get; set; }
        string Notes { get; set; }
        Game SelectedGame { get; set; }
        int Week { get; set; }

        DelegateCommand AddGameCommand { get; }
        DelegateCommand DeleteGameCommand { get; }
        DelegateCommand EditGameCommand { get; }
        DelegateCommand FindGameCommand { get; }
        DelegateCommand ShowAllGamesCommand { get; }
        DelegateCommand ViewGamesCommand { get; }
    }

    /// <summary>
    /// ViewModel logic for the Games window.
    /// </summary>
    public class GamesWindowViewModel : ViewModelBase, IFocusMover, IGamesWindowViewModel
	{
        private readonly IGamesWindowService _controlService;

        /// <summary>
        /// Initializes a new instance of the GamesWindowViewModel class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="controlService"></param>
        /// <param name="gameFinder"></param>
        public GamesWindowViewModel(ISharedService sharedService, IGamesWindowService controlService,
            IGameFinderWindow gameFinder)
            : base(sharedService)
		{
            _controlService = controlService;

            GameFinder = gameFinder;
		}

        /// <summary>
        /// Gets the visibility of the AddEntity control
        /// </summary>
        private Visibility _addGameControlVisibility;
        public Visibility AddGameControlVisibility
        {
            get
            {
                return _addGameControlVisibility;
            }
            set
            {
                if (value != _addGameControlVisibility)
                {
                    _addGameControlVisibility = value;
                    OnPropertyChanged("AddGameControlVisibility");
                }
            }
        }

        /// <summary>
        /// Gets the visibility of the RemoveEntity control.
        /// </summary>
        private Visibility _deleteGameControlVisibility;
        public Visibility DeleteGameControlVisibility
        {
            get
            {
                return _deleteGameControlVisibility;
            }
            set
            {
                if (value != _deleteGameControlVisibility)
                {
                    _deleteGameControlVisibility = value;
                    OnPropertyChanged("DeleteGameControlVisibility");
                }
            }
        }

        /// <summary>
        /// Gets the visibility of the EditEntity control.
        /// </summary>
        private Visibility _editGameControlVisibility;
        public Visibility EditGameControlVisibility
        {
            get
            {
                return _editGameControlVisibility;
            }
            set
            {
                if (value != _editGameControlVisibility)
                {
                    _editGameControlVisibility = value;
                    OnPropertyChanged("EditGameControlVisibility");
                }
            }
        }

        public IGameFinderWindow GameFinder { get; set; }

        /// <summary>
        /// Gets SelectedGame window's games collection. 
        /// </summary>
        private ReadOnlyCollection<Game> _games;
        public ReadOnlyCollection<Game> Games
        {
            get
            {
                return _games;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Games");
                }
                else if (value != _games)
                {
                    _games = value;
                    OnPropertyChanged("Games");
                    RequestUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets SelectedGame window's guest value.
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
        /// Gets or sets SelectedGame window's guest score value.
        /// </summary>
        private double _guestScore;
        public double GuestScore
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
        /// Gets or sets SelectedGame window's host value.
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
        /// Gets or sets SelectedGame window's host score value.
        /// </summary>
        private double _hostScore;
        public double HostScore
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
        /// Gets or sets a value indicating whether the FindEntityFilter has been applied for SelectedGame object
        /// </summary>
        public bool IsFindGameFilterApplied { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether the Games collection is read-only.
        /// </summary>
        private bool _isGamesReadOnly;
        public bool IsGamesReadOnly
        {
            get
            {
                return _isGamesReadOnly;
            }
            set
            {
                if (value != _isGamesReadOnly)
                {
                    _isGamesReadOnly = value;
                    OnPropertyChanged("IsGamesReadOnly");
                }
            }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether SelectedGame game is a playoff game.
        /// </summary>
        private bool _isPlayoffGame;
        public bool IsPlayoffGame
        {
            get
            {
                return _isPlayoffGame;
            }
            set
            {
                if (value != _isPlayoffGame)
                {
                    _isPlayoffGame = value;
                    OnPropertyChanged("IsPlayoffGame");
                }
            }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether SelectedGame game can be a playoff game.
        /// </summary>
        private bool _isPlayoffGameEnabled;
        public bool IsPlayoffGameEnabled
        {
            get
            {
                return _isPlayoffGameEnabled;
            }
            set
            {
                if (value != _isPlayoffGameEnabled)
                {
                    _isPlayoffGameEnabled = value;
                    OnPropertyChanged("IsPlayoffGameEnabled");
                }
            }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether the ShowAllGames function is enabled.
        /// </summary>
        private bool _isShowAllGamesEnabled;
        public bool IsShowAllGamesEnabled
        {
            get
            {
                return _isShowAllGamesEnabled;
            }
            set
            {
                if (value != _isShowAllGamesEnabled)
                {
                    _isShowAllGamesEnabled = value;
                    OnPropertyChanged("IsShowAllGamesEnabled");
                }
            }
        }

        /// <summary>
        /// Gets or sets the notes entered for SelectedGame game, if any.
        /// </summary>
        private string _notes;
        public string Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                if (value != _notes)
                {
                    _notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }

        /// <summary>
        /// Gets or sets SelectedGame window's selected game
        /// </summary>
        private Game _selectedGame;
        public Game SelectedGame
        {
            get
            {
                return _selectedGame;
            }
            set
            {
                if ((value != _selectedGame) || (_selectedGame == null))
                {
                    _selectedGame = value;
                    OnPropertyChanged("SelectedGame");

                    if (_selectedGame == null)
                    {
                        ClearDataEntryControls();
                    }
                    else
                    {
                        PopulateDataEntryControls();
                    }
                }
            }
        }

        /// <summary>
        /// Populates data entry controls with data from selected game.
        /// </summary>
        private void PopulateDataEntryControls()
        {
            var selectedGame = SelectedGame;
            Week = selectedGame.Week;
            GuestName = selectedGame.GuestName;
            GuestScore = selectedGame.GuestScore;
            HostName = selectedGame.HostName;
            HostScore = selectedGame.HostScore;
            IsPlayoffGame = selectedGame.IsPlayoffGame;
            Notes = selectedGame.Notes;

            AddGameControlVisibility = Visibility.Hidden;
            EditGameControlVisibility = Visibility.Visible;
            DeleteGameControlVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Resets data in all data entry fields to their default values.
        /// </summary>
        private void ClearDataEntryControls()
        {
            GuestName = string.Empty;
            GuestScore = 0;
            HostName = string.Empty;
            HostScore = 0;
            IsPlayoffGame = false;
            Notes = string.Empty;

            AddGameControlVisibility = Visibility.Visible;
            EditGameControlVisibility = Visibility.Hidden;
            DeleteGameControlVisibility = Visibility.Hidden;

            // Set focus to GuestName field.
            MoveFocusTo("GuestName");
        }

        /// <summary>
        /// Gets/sets SelectedGame window's week value.
        /// </summary>
        private int _week;
        public int Week
        {
            get
            {
                return _week;
            }
            set
            {
                if (value != _week)
                {
                    _week = value;
                    OnPropertyChanged("Week");
                }
            }
        }

        /// <summary>
        /// Adds a new game to the database.
        /// </summary>
        private DelegateCommand _addGameCommand;
        public DelegateCommand AddGameCommand
        {
            get
            {
                if (_addGameCommand == null)
                {
                    _addGameCommand = new DelegateCommand(param => AddGame());
                }
                return _addGameCommand;
            }
        }
        private void AddGame()
        {
            try
            {
                try
                {
                    ValidateDataEntry();
                }
                catch (DataValidationException ex)
                {
                    _sharedService.ShowExceptionMessage(ex);
                    return;
                }

                var newGame = MapNewGameValues();
                _controlService.AddGame(newGame);

                Games = new ReadOnlyCollection<Game>(
                    _controlService.GetGames((int)WpfGlobals.SelectedSeason).ToList());

                SelectedGame = null;
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Removes an existing game from the database.
        /// </summary>
        private DelegateCommand _deleteGameCommand;
        public DelegateCommand DeleteGameCommand
        {
            get
            {
                if (_deleteGameCommand == null)
                {
                    _deleteGameCommand = new DelegateCommand(param => DeleteGame());
                }
                return _deleteGameCommand;
            }
        }
        private void DeleteGame()
        {
            try
            {
                // Delete matching row in the dataset.Games table
                var oldGame = MapOldGameValues();

                _controlService.DeleteGame(oldGame);

                SelectedGame = null;

                Games = new ReadOnlyCollection<Game>(_controlService.GetGames((int)WpfGlobals.SelectedSeason).ToList());
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Edits an existing game in the database.
        /// </summary>
        private DelegateCommand _editGameCommand;
        public DelegateCommand EditGameCommand
        {
            get
            {
                if (_editGameCommand == null)
                {
                    _editGameCommand = new DelegateCommand(param => EditGame());
                }
                return _editGameCommand;
            }
        }
        private void EditGame()
        {
            try
            {
                try
                {
                    ValidateDataEntry();
                }
                catch (DataValidationException ex)
                {
                    _sharedService.ShowExceptionMessage(ex);
                    return;
                }

                var seasonID = WpfGlobals.SelectedSeason;

                // Edit selected row of Games table.
                var oldGame = MapOldGameValues();
                var newGame = MapNewGameValues();
                _controlService.EditGame(oldGame, newGame);

                if (IsFindGameFilterApplied)
                {
                    ApplyFindGameFilter();
                }
                else
                {
                    Games = new ReadOnlyCollection<Game>(
                        _controlService.GetGames((int)WpfGlobals.SelectedSeason).ToList());
                }
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Validates all data entered into the data entry fields.
        /// </summary>
        private void ValidateDataEntry()
        {
            if (string.IsNullOrWhiteSpace(GuestName))
            {
                // GuestName field is left empty.
                MoveFocusTo("GuestName");
                throw new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            }
            else if (string.IsNullOrWhiteSpace(HostName))
            {
                // HostName field is left empty.
                MoveFocusTo("HostName");
                throw new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            }
            else if (GuestName == HostName)
            {
                // Guest and host are the same team.
                MoveFocusTo("GuestName");
                throw new DataValidationException(WpfGlobals.Constants.DifferentTeamsNeededErrorMessage);
            }
        }

        /// <summary>
        /// Moves the focus to the specified property
        /// </summary>
        /// <param name="focusedProperty"></param>
        private void MoveFocusTo(string focusedProperty)
        {
            OnMoveFocus(focusedProperty);
        }

        private Game MapOldGameValues()
        {
            return new Game
            {
                ID = SelectedGame.ID,
                SeasonID = (int)WpfGlobals.SelectedSeason,
                Week = SelectedGame.Week,
                GuestName = SelectedGame.GuestName,
                GuestScore = SelectedGame.GuestScore,
                HostName = SelectedGame.HostName,
                HostScore = SelectedGame.HostScore,
                IsPlayoffGame = SelectedGame.IsPlayoffGame,
                Notes = SelectedGame.Notes
            };
        }

        private Game MapNewGameValues()
        {
            return new Game
            {
                SeasonID = (int)WpfGlobals.SelectedSeason,
                Week = this.Week,
                GuestName = this.GuestName,
                GuestScore = this.GuestScore,
                HostName = this.HostName,
                HostScore = this.HostScore,
                IsPlayoffGame = this.IsPlayoffGame,
                Notes = this.Notes
            };
        }

        /// <summary>
        /// Searches for a specified game in the database.
        /// </summary>
        private DelegateCommand _FindEntityCommand;
        public DelegateCommand FindGameCommand
        {
            get
            {
                if (_FindEntityCommand == null)
                {
                    _FindEntityCommand = new DelegateCommand(param => FindEntity());
                }
                return _FindEntityCommand;
            }
        }
        private void FindEntity()
        {
            try
            {
                var dlgResult = GameFinder.ShowDialog();

                if (dlgResult == true)
                {
                    ApplyFindGameFilter();
                    IsGamesReadOnly = true;
                    IsShowAllGamesEnabled = true;

                    if (Games.Count == 0)
                    {
                        SelectedGame = null;
                    }

                    AddGameControlVisibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Applies the filter set in the GameFinderWindowViewModel
        /// </summary>
        private void ApplyFindGameFilter()
        {
            var dataContext = GameFinder.DataContext as IGameFinderWindowViewModel;

            Games = new ReadOnlyCollection<Game>(
                _controlService.GetGames((int)WpfGlobals.SelectedSeason, dataContext.GuestName, dataContext.HostName)
                .ToList());

            IsFindGameFilterApplied = true;
        }

        /// <summary>
        /// Shows all the games currently in the database.
        /// </summary>
        private DelegateCommand _showAllGamesCommand;
        public DelegateCommand ShowAllGamesCommand
        {
            get
            {
                if (_showAllGamesCommand == null)
                {
                    _showAllGamesCommand = new DelegateCommand(param => ShowAllGames());
                }
                return _showAllGamesCommand;
            }
        }
        private void ShowAllGames()
        {
            try
            {
                ViewGames();
                IsFindGameFilterApplied = false;
                IsGamesReadOnly = false;
                IsShowAllGamesEnabled = false;
                SelectedGame = null;
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Views the Games database table.
        /// </summary>
        private DelegateCommand _viewGamesCommand;
        public DelegateCommand ViewGamesCommand
        {
            get
            {
                if (_viewGamesCommand == null)
                {
                    _viewGamesCommand = new DelegateCommand(param => ViewGames());
                }
                return _viewGamesCommand;
            }
        }
        private void ViewGames()
        {
            try
            {
                // Load the DataModel's Games table.
                Games = new ReadOnlyCollection<Game>(_controlService.GetGames((int)WpfGlobals.SelectedSeason).ToList());

                SelectedGame = null;

                OnMoveFocus("GuestName");

                Week = _controlService.GetWeekCount();
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex);
            }
        }
    }
}
