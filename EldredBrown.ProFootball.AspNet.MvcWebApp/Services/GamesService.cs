using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;
using log4net;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Services
{
    public interface IGamesService
    {
        Task AddEntity(GameViewModel newGameViewModel, ProFootballEntities dbContextInjected = null);
        Task DeleteGame(int id, ProFootballEntities dbContextInjected = null);

        Task EditGame(GameViewModel oldGameViewModel, GameViewModel newGameViewModel,
            ProFootballEntities dbContextInjected = null);

        Task<GameViewModel> FindEntityAsync(int id, ProFootballEntities dbContextInjected = null);

        Task<IEnumerable<GameViewModel>> GetGamesAsync(int seasonID, string selectedWeek, string guestSearchString,
            string hostSearchString, ProFootballEntities dbContextInjected = null);

        Task<IEnumerable<TeamViewModel>> GetTeamsAsync(ProFootballEntities dbContextInjected = null);
        Task<IEnumerable<WeekViewModel>> GetWeeksAsync(int seasonID, ProFootballEntities dbContextInjected = null);
        void SetSelectedSeason(IEnumerable<SeasonViewModel> seasons, int? seasonID);
        void SetSelectedWeek(int? week);
    }

    /// <summary>
    /// Service class used by the GamesController
    /// </summary>
    public class GamesService : IGamesService
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISharedService _sharedService;
        private readonly IDataMapper _dataMapper;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Team> _teamRepository;
        private readonly IRepository<TeamSeason> _teamSeasonRepository;
        private readonly ICalculator _calculator;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the GamesService class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="dataMapper"></param>
        /// <param name="gameRepository"></param>
        /// <param name="teamRepository"></param>
        /// <param name="teamSeasonRepository"></param>
        /// <param name="calculator"></param>
        public GamesService(ISharedService sharedService, IDataMapper dataMapper,
            IRepository<Game> gameRepository, IRepository<Team> teamRepository,
            IRepository<TeamSeason> teamSeasonRepository, ICalculator calculator)
        {
            _sharedService = sharedService;
            _dataMapper = dataMapper;
            _gameRepository = gameRepository;
            _teamRepository = teamRepository;
            _teamSeasonRepository = teamSeasonRepository;
            _calculator = calculator;
        }

        #endregion Constructors & Finalizers

        #region Properties

        public static int SelectedSeason;
        public static WeekViewModel SelectedWeek;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a game to the underlying data store
        /// </summary>
        /// <param name="newGameViewModel">GameViewModel containing data for the game to be added</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        public async Task AddEntity(GameViewModel newGameViewModel, ProFootballEntities dbContextInjected = null)
        {
            var newGame = _dataMapper.MapToGame(newGameViewModel);

            DecideWinnerAndLoser(newGame);

            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                _gameRepository.AddEntity(dbContext, newGame);

                await AddEntityToTeams(dbContext, newGame);

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Log.Error("GamesService.AddEntity could not save changes to database: " + ex.Message);
                }
            }
        }

        ///// <summary>
        ///// Applies the filter set in the GameFinderWindowViewModel
        ///// </summary>
        //public void ApplyFindEntityFilter()
        //{
        //    try
        //    {
        //        var dataContext = _context.GameFinder.DataContext as GameFinderWindowViewModel;
        //        var guest = dataContext.GuestName;
        //        var host = dataContext.HostName;
        //        var games = (from game in _context.DbContextContext.Games
        //                     where ((game.GuestName == guest) && (game.HostName == host) && (game.SeasonID == Globals.SelectedSeason))
        //                     select game);

        //        LoadEntities(games);

        //        _context.IsFindEntityFilterApplied = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // _globals.ShowExceptionMessage(ex);
        //    }
        //}

        ///// <summary>
        ///// Resets data in all data entry fields to their default values.
        ///// </summary>
        //public void ClearDataEntryControls()
        //{
        //    try
        //    {
        //        _context.GuestName = String.Empty;
        //        _context.GuestScore = 0;
        //        _context.HostName = String.Empty;
        //        _context.HostScore = 0;
        //        _context.IsPlayoffGame = false;
        //        _context.Notes = String.Empty;

        //        _context.AddEntityControlVisibility = Visibility.Visible;
        //        _context.EditGameControlVisibility = Visibility.Hidden;
        //        _context.RemoveEntityControlVisibility = Visibility.Hidden;

        //        // Set focus to GuestName field.
        //        _context.MoveFocusTo("GuestName");
        //    }
        //    catch (Exception ex)
        //    {
        //        // _globals.ShowExceptionMessage(ex);
        //    }
        //}

        /////// <summary>
        /////// Edits each seasonTeam's data (games, points for, points against) from playoff games.
        /////// </summary>
        /////// <param name = "winner"></param>
        /////// <param name = "loser"></param>
        /////// <param name = "operation"></param>
        /////// <param name = "game"></param>
        /////// <param name = "winnerScore"></param>
        /////// <param name = "loserScore"></param>
        ////public static void EditDataFromPlayoffGames(Team winner, Team loser, Operation operation, Game game)
        ////{
        ////	try
        ////	{
        ////		EditWinLossDataFromPlayoffGames(winner, loser, operation, game);
        ////	}
        ////	catch ( Exception ex )
        ////	{
        ////		// _globals.ShowExceptionMessage(ex);
        ////	}
        ////}

        /// <summary>
        /// Removes a game from the underlying data store
        /// </summary>
        /// <param name="id">The ID of the game to remove</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        public async Task DeleteGame(int id, ProFootballEntities dbContextInjected = null)
        {
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                var oldGame = await _gameRepository.FindEntityAsync(dbContext, id);
                DecideWinnerAndLoser(oldGame);

                await DeleteGameFromTeams(dbContext, oldGame);
                _gameRepository.RemoveEntity(dbContext, oldGame);

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Log.Error("GamesService.DeleteGame could not save changes to database: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Edits a game in the underlying data store
        /// </summary>
        /// <param name="oldGameViewModel">GameViewModel object containing the old data for the game to be modified</param>
        /// <param name="newGameViewModel">GameViewModel object containing the new data for the game to be modified</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        public async Task EditGame(GameViewModel oldGameViewModel, GameViewModel newGameViewModel,
            ProFootballEntities dbContextInjected = null)
        {
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                // Replace property values of Game instance selected in DataGrid with values from newGame.
                var selectedGame = await _gameRepository.FindEntityAsync(dbContext, oldGameViewModel.ID);
                selectedGame.Week = newGameViewModel.Week;
                selectedGame.GuestName = newGameViewModel.GuestName;
                selectedGame.GuestScore = newGameViewModel.GuestScore;
                selectedGame.HostName = newGameViewModel.HostName;
                selectedGame.HostScore = newGameViewModel.HostScore;
                selectedGame.IsPlayoffGame = newGameViewModel.IsPlayoffGame;
                selectedGame.Notes = newGameViewModel.Notes;
                DecideWinnerAndLoser(selectedGame);
                _gameRepository.EditEntity(dbContext, selectedGame);

                // Edit pertinent data in Teams table.
                var oldGame = _dataMapper.MapToGame(oldGameViewModel);
                DecideWinnerAndLoser(oldGame);

                var newGame = _dataMapper.MapToGame(newGameViewModel);
                DecideWinnerAndLoser(newGame);

                await EditGameInTeams(dbContext, oldGame, newGame);

                try
                {
                    // Save changes to db.
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Log.Error("GamesService.EditEntity could not save changes to database: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Finds a game in the underlying data store asynchronously
        /// </summary>
        /// <param name="id">The ID of the desired game</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        /// <returns>A GameViewModel representing the game with the matching ID</returns>
        public async Task<GameViewModel> FindEntityAsync(int id, ProFootballEntities dbContextInjected = null)
        {
            Game game;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                game = await _gameRepository.FindEntityAsync(dbContext, id);
            }
            return _dataMapper.MapToGameViewModel(game);
        }

        /// <summary>
        /// Gets an enumerable collection of games asynchronously
        /// </summary>
        /// <param name="seasonID">The ID of the season to search for the desired game</param>
        /// <param name="selectedWeek">The week of the desired game within the desired season</param>
        /// <param name="guestSearchString">The name of the game's Guest</param>
        /// <param name="hostSearchString">The name of the game's Host</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        /// <returns>An enumerable collection of GameViewModels representing all matching games</returns>
        public async Task<IEnumerable<GameViewModel>> GetGamesAsync(int seasonID, string selectedWeek,
            string guestSearchString, string hostSearchString, ProFootballEntities dbContextInjected = null)
        {
            var gameViewModels = new List<GameViewModel>();

            IEnumerable<Game> games = null;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                try
                {
                    games = (await _gameRepository.GetEntitiesAsync(dbContext)).Where(g => g.SeasonID == seasonID);

                    if (!String.IsNullOrEmpty(selectedWeek))
                    {
                        games = games.Where(g => g.Week.ToString() == selectedWeek);
                    }
                    if (!String.IsNullOrEmpty(guestSearchString))
                    {
                        games = games.Where(g => g.GuestName.Contains(guestSearchString));
                    }
                    if (!String.IsNullOrEmpty(hostSearchString))
                    {
                        games = games.Where(g => g.HostName.Contains(hostSearchString));
                    }
                }
                catch (ArgumentNullException ex)
                {
                    Log.Error("ArgumentNullException in GamesService.GetEntitiesAsync: " + ex.Message);

                    return null;
                }
            }

            foreach (var game in games)
            {
                var gameViewModel = _dataMapper.MapToGameViewModel(game);
                gameViewModels.Add(gameViewModel);
            }

            return gameViewModels;
        }

        /// <summary>
        /// Gets an enumerable collection of teams
        /// </summary>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        /// <returns>An enumerable collection of teams</returns>
        public async Task<IEnumerable<TeamViewModel>> GetTeamsAsync(ProFootballEntities dbContextInjected = null)
        {
            var teamViewModels = new List<TeamViewModel>();

            IEnumerable<Team> teams;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                teams = await _teamRepository.GetEntitiesAsync(dbContext);
            }

            foreach (var team in teams)
            {
                var teamViewModel = _dataMapper.MapToTeamViewModel(team);
                teamViewModels.Add(teamViewModel);
            }

            return teamViewModels;
        }

        /// <summary>
        /// Gets the enumerable collection of weeks for the specified season
        /// </summary>
        /// <param name="seasonID">The ID of the season from which weeks will be returned</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        /// <returns>An enumerable collection of WeekViewModel objects</returns>
        public async Task<IEnumerable<WeekViewModel>> GetWeeksAsync(int seasonID,
            ProFootballEntities dbContextInjected = null)
        {
            var weekViewModels = new List<WeekViewModel>
            {
                new WeekViewModel(null)
            };

            IEnumerable<int> weeks = null;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                try
                {
                    weeks = (await _gameRepository.GetEntitiesAsync(dbContext))
                        .OrderBy(g => g.Week)
                        .Where(g => g.SeasonID == seasonID)
                        .Select(g => g.Week)
                        .Distinct();
                }
                catch (ArgumentNullException ex)
                {
                    Log.Error("ArgumentNullException in GamesService.GetEntitiesAsync: " + ex.Message);
                    return null;
                }
            }

            foreach (var week in weeks)
            {
                weekViewModels.Add(new WeekViewModel(week));
            }

            return weekViewModels;
        }

        ///// <summary>
        ///// Edits each seasonTeam's data (games, points for, points against, adjusted points for, adjusted points against) from all games.
        ///// </summary>
        ///// <param name = "game"></param>
        ///// <param name = "operation"></param>
        ///// <summary>
        ///// Loads _context GamesWindowViewModel object's Games collection.
        ///// </summary>
        ///// <param name = "games"></param>
        //public void LoadEntities(IQueryable<Game> games)
        //{
        //    try
        //    {
        //        _context.Games = new ObservableCollection<Game>();
        //        foreach (var game in games)
        //        {
        //            _context.Games.Add(game);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // _globals.ShowExceptionMessage(ex);
        //    }
        //}

        ///// <summary>
        ///// Populates data entry controls with data from selected game.
        ///// </summary>
        //public void PopulateDataEntryControls()
        //{
        //    try
        //    {
        //        var selectedGame = _context.SelectedGame;
        //        _context.Week = selectedGame.Week;
        //        _context.GuestName = selectedGame.GuestName;
        //        _context.GuestScore = selectedGame.GuestScore;
        //        _context.HostName = selectedGame.HostName;
        //        _context.HostScore = selectedGame.HostScore;
        //        _context.IsPlayoffGame = selectedGame.IsPlayoffGame;
        //        _context.Notes = selectedGame.Notes;

        //        _context.AddEntityControlVisibility = Visibility.Hidden;
        //        _context.EditGameControlVisibility = Visibility.Visible;
        //        _context.RemoveEntityControlVisibility = Visibility.Visible;
        //    }
        //    catch (Exception ex)
        //    {
        //        // _globals.ShowExceptionMessage(ex);
        //    }
        //}

        /// <summary>
        /// Sets the selected season
        /// </summary>
        /// <param name="seasons">An enumerable collection of SeasonViewModel objects</param>
        /// <param name="seasonID">The ID of the selected season</param>
        public void SetSelectedSeason(IEnumerable<SeasonViewModel> seasons, int? seasonID)
        {
            _sharedService.SetSelectedSeason(seasons, seasonID, ref SelectedSeason);
        }

        /// <summary>
        /// Sets the selected week within the selected season
        /// </summary>
        /// <param name="week">The number of the selected week</param>
        public void SetSelectedWeek(int? week)
        {
            SelectedWeek = new WeekViewModel(week);
        }

        ///// <summary>
        ///// Sorts the Games collection by Week ascending, then by HostName ascending.
        ///// </summary>
        //public void SortGamesByDefaultOrder()
        //{
        //    try
        //    {
        //        var games = (from game in _context.DbContextContext.Games
        //                     where game.SeasonID == Globals.SelectedSeason
        //                     orderby game.Week descending
        //                     orderby game.ID descending
        //                     select game).
        //                     AsQueryable();

        //        LoadEntities(games);
        //    }
        //    catch (Exception ex)
        //    {
        //        // _globals.ShowExceptionMessage(ex);
        //    }
        //}

        #region Helpers

        /// <summary>
        /// Adds data of a new game to the database TeamSeasons table
        /// </summary>
        /// <param name="dbContext">The ProFootballEntities object representing the database to be updated</param>
        /// <param name="game">The Game object from which data will be extracted</param>
        private async Task AddEntityToTeams(ProFootballEntities dbContext, Game game)
        {
            //newGame = StoreNewGameValues(newGame);
            await EditTeams(dbContext, game, Direction.Up);
        }

        /// <summary>
        /// Decides this game's winner and loser.
        /// </summary>
        /// <param name="game">The Game object for which a winner and loser will be decided</param>
        private void DecideWinnerAndLoser(Game game)
        {
            // Declare the winner to be the team that scored more points in the game.
            if (game.GuestScore > game.HostScore)
            {
                game.WinnerName = game.GuestName;
                game.WinnerScore = game.GuestScore;
                game.LoserName = game.HostName;
                game.LoserScore = game.HostScore;
            }
            else if (game.HostScore > game.GuestScore)
            {
                game.WinnerName = game.HostName;
                game.WinnerScore = game.HostScore;
                game.LoserName = game.GuestName;
                game.LoserScore = game.GuestScore;
            }
            else
            {
                game.WinnerName = null;
                game.LoserName = null;
            }
        }

        /// <summary>
        /// Removes data of an existing game from the database TeamSeasons table
        /// </summary>
        /// <param name="dbContext">The ProFootballEntities object representing the database to be updated</param>
        /// <param name="oldGame">The Game object from which data will be extracted</param>
        private async Task DeleteGameFromTeams(ProFootballEntities dbContext, Game oldGame)
        {
            await EditTeams(dbContext, oldGame, Direction.Down);
        }

        /// <summary>
        /// Edits data of an existing game in the database TeamSeasons table
        /// </summary>
        /// <param name="dbContext">The ProFootballEntities object representing the database to be updated</param>
        /// <param name="oldGame">The Game object from which data will be extracted for subtraction</param>
        /// <param name="newGame">The Game object from which data will be extracted for addition</param>
        private async Task EditGameInTeams(ProFootballEntities dbContext, Game oldGame, Game newGame)
        {
            await EditTeams(dbContext, oldGame, Direction.Down);
            await EditTeams(dbContext, newGame, Direction.Up);
        }

        /// <summary>
        /// Edits season scoring data for both of a game's contestants
        /// </summary>
        /// <param name="dbContext">The ProFootballEntities object representing the database to be updated</param>
        /// <param name="game">The Game object from which data will be extracted</param>
        /// <param name="operation">The arithmetic operation to be applied to the team's scoring data</param>
        private async Task EditScoringData(ProFootballEntities dbContext, Game game, Operation operation)
        {
            await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore,
                game.HostScore);

            await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore,
                game.GuestScore);
        }

        /// <summary>
        /// Edits all scoring data for the specified Team
        /// </summary>
        /// <param name="dbContext">The ProFootballEntities object representing the database to be updated</param>
        /// <param name="teamName">The name of the selected team</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="operation">The arithmetic operation to be applied to the team's scoring data</param>
        /// <param name="teamScore">The team's game score</param>
        /// <param name="opponentScore">The opponent's game score</param>
        /// <returns></returns>
        private async Task EditScoringDataByTeamSeason(ProFootballEntities dbContext, string teamName, int seasonID,
            Operation operation, double teamScore, double opponentScore)
        {
            var teamSeason = await _teamSeasonRepository.FindEntityAsync(dbContext, teamName, seasonID);

            teamSeason.PointsFor = operation(teamSeason.PointsFor, teamScore);
            teamSeason.PointsAgainst = operation(teamSeason.PointsAgainst, opponentScore);

            var pythPct = _calculator.CalculatePythagoreanWinningPercentage(teamSeason);
            if (pythPct == null)
            {
                teamSeason.PythagoreanWins = 0;
                teamSeason.PythagoreanLosses = 0;
            }
            else
            {
                teamSeason.PythagoreanWins = _calculator.Multiply((double)pythPct, teamSeason.Games);
                teamSeason.PythagoreanLosses = _calculator.Multiply((double)(1 - pythPct), teamSeason.Games);
            }
        }

        /// <summary>
        /// Edits the DataModel's Teams table with data from the specified game.
        /// </summary>
        /// <param name="dbContext">The ProFootballEntities object representing the database to be updated</param>
        /// <param name="game">The Game object from which data will be extracted</param>
        /// <param name="direction">The direction, up or down, by which both teams' data will be adjusted</param>
        private async Task EditTeams(ProFootballEntities dbContext, Game game, Direction direction)
        {
            Operation operation;

            // Decide whether the teams need to be edited up or down.
            // Up for new game, down then up for edited game, down for deleted game.
            switch (direction)
            {
                case Direction.Up:
                    operation = new Operation(_calculator.Add);
                    break;

                case Direction.Down:
                    operation = new Operation(_calculator.Subtract);
                    break;

                default:
                    throw new ArgumentException("direction");
            }

            try
            {
                await ProcessGame(dbContext, game, operation);
            }
            catch (ObjectNotFoundException ex)
            {
                Log.Error("ObjectNotFoundException in GamesService.EditTeams: " + ex.Message);
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Edits each seasonTeam's data (wins, losses, and ties) from all games.
        /// </summary>
        /// <param name="dbContext">The ProFootballEntities object representing the database to be updated</param>
        /// <param name="game">The Game object from which data will be extracted</param>
        /// <param name="operation">The arithmetic operation to be applied to the team's scoring data</param>
        private async Task EditWinLossData(ProFootballEntities dbContext, Game game, Operation operation)
        {
            var seasonID = game.SeasonID;

            var guestSeason = await _teamSeasonRepository.FindEntityAsync(dbContext, game.GuestName, seasonID);
            guestSeason.Games = (int)operation(guestSeason.Games, 1);

            var hostSeason = await _teamSeasonRepository.FindEntityAsync(dbContext, game.HostName, seasonID);
            hostSeason.Games = (int)operation(hostSeason.Games, 1);

            var winnerName = game.WinnerName;
            var loserName = game.LoserName;
            if (String.IsNullOrEmpty(winnerName) || String.IsNullOrEmpty(loserName))
            {
                guestSeason.Ties = (int)operation(guestSeason.Ties, 1);
                hostSeason.Ties = (int)operation(hostSeason.Ties, 1);
            }
            else
            {
                var winnerSeason = await _teamSeasonRepository.FindEntityAsync(dbContext, winnerName, seasonID);
                winnerSeason.Wins = (int)operation(winnerSeason.Wins, 1);

                var loserSeason = await _teamSeasonRepository.FindEntityAsync(dbContext, loserName, seasonID);
                loserSeason.Losses = (int)operation(loserSeason.Losses, 1);
            }

            guestSeason.WinningPercentage = _calculator.CalculateWinningPercentage(guestSeason);
            hostSeason.WinningPercentage = _calculator.CalculateWinningPercentage(hostSeason);
        }

        /// <summary>
        /// Processes a game
        /// </summary>
        /// <param name="dbContext">The ProFootballEntities object representing the database to be updated</param>
        /// <param name="game">The Game object from which data will be extracted</param>
        /// <param name="operation">The arithmetic operation to be applied to the team's scoring data</param>
        private async Task ProcessGame(ProFootballEntities dbContext, Game game, Operation operation)
        {
            await EditWinLossData(dbContext, game, operation);
            await EditScoringData(dbContext, game, operation);
        }

        #endregion Helpers

        #endregion Methods
    }
}
