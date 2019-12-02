using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Services
{
    public interface IGamesWindowService
    {
        void AddGame(Game newGame);
        void DeleteGame(Game oldGame);
        void EditGame(Game oldGame, Game newGame);
        IEnumerable<Game> GetGames(int seasonID, string guestName = null, string hostName = null);
        int GetWeekCount();
    }

    /// <summary>
    /// Service class used by the GamesWindow class
    /// </summary>
    public class GamesWindowService : IGamesWindowService
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISharedService _sharedService;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<TeamSeason> _teamSeasonRepository;
        private readonly IRepository<WeekCount> _weekCountRepository;
        private readonly ProFootballEntities _dbContext;
        private readonly ICalculator _calculator;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the GamesWindowService class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="gameRepository"></param>
        /// <param name="weekCountRepository"></param>
        /// <param name="teamSeasonRepository"></param>
        /// <param name="dbContext"></param>
        /// <param name="calculator"></param>
        public GamesWindowService(ISharedService sharedService, IRepository<Game> gameRepository,
            IRepository<TeamSeason> teamSeasonRepository, IRepository<WeekCount> weekCountRepository,
            ProFootballEntities dbContext, ICalculator calculator)
        {
            // Assign argument values to member fields.
            _sharedService = sharedService;
            _gameRepository = gameRepository;
            _teamSeasonRepository = teamSeasonRepository;
            _weekCountRepository = weekCountRepository;
            _dbContext = dbContext;
            _calculator = calculator;
        }

        #endregion Constructors & Finalizers

        #region Methods

        /// <summary>
        /// Adds a game to the database and updates the Teams table appropriately
        /// </summary>
        /// <param name="newGame">The game to add</param>
        public void AddGame(Game newGame)
        {
            try
            {
                DecideWinnerAndLoser(newGame);

                _gameRepository.AddEntity(newGame);

                AddGameToTeams(newGame);

                _sharedService.SaveChanges(_dbContext);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Edits a game in the database and updates the Teams table appropriately
        /// </summary>
        /// <param name="oldGame">A Game object storing the data to be removed from the database</param>
        /// <param name="newGame">A Game object storing the data to be added to the database</param>
        public void EditGame(Game oldGame, Game newGame)
        {
            try
            {
                var selectedGame = _gameRepository.FindEntity(oldGame.ID);

                // Replace property values of Game instance selected in DataGrid with values from newGame.
                selectedGame.Week = newGame.Week;
                selectedGame.GuestName = newGame.GuestName;
                selectedGame.GuestScore = newGame.GuestScore;
                selectedGame.HostName = newGame.HostName;
                selectedGame.HostScore = newGame.HostScore;
                selectedGame.IsPlayoffGame = newGame.IsPlayoffGame;
                selectedGame.Notes = newGame.Notes;
                DecideWinnerAndLoser(selectedGame);
                _gameRepository.EditEntity(selectedGame);

                DecideWinnerAndLoser(oldGame);
                DecideWinnerAndLoser(newGame);
                EditGameInTeams(oldGame, newGame);

                _sharedService.SaveChanges(_dbContext);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes a game from the database and updates the Teams table appropriately
        /// </summary>
        /// <param name="oldGame"></param>
        public void DeleteGame(Game oldGame)
        {
            try
            {
                oldGame = _gameRepository.FindEntity(oldGame.ID);

                DecideWinnerAndLoser(oldGame);
                DeleteGameFromTeams(oldGame);

                _gameRepository.RemoveEntity(oldGame);
                _sharedService.SaveChanges(_dbContext);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /////// <summary>
        /////// Edits each teamSeason's data (games, points for, points against) from playoff games.
        /////// </summary>
        /////// <param name = "winner"></param>
        /////// <param name = "loser"></param>
        /////// <param name = "operation"></param>
        /////// <param name = "currentGame"></param>
        /////// <param name = "winnerScore"></param>
        /////// <param name = "loserScore"></param>
        ////public static void EditDataFromPlayoffGames(Team winner, Team loser, Operation operation, IGame currentGame)
        ////{
        ////	try
        ////	{
        ////		EditWinLossDataFromPlayoffGames(winner, loser, operation, currentGame);
        ////	}
        ////	catch ( Exception ex )
        ////	{
        ////		_WpfGlobals.ShowExceptionMessage(ex);
        ////	}
        ////}

        /// <summary>
        /// Gets from the data store all games that match the specified filter criteria
        /// </summary>
        /// <param name="seasonID">The ID of the season in which the game was played</param>
        /// <param name="guestName">The guest of the game to fetch</param>
        /// <param name="hostName">Thos host of the game to fetch</param>
        /// <returns></returns>
        public IEnumerable<Game> GetGames(int seasonID, string guestName = null, string hostName = null)
        {
            try
            {
                var games = _gameRepository.GetEntities().Where(g => g.SeasonID == seasonID);
                if (!String.IsNullOrEmpty(guestName))
                {
                    games = games.Where(g => g.GuestName == guestName);
                }
                if (!String.IsNullOrEmpty(hostName))
                {
                    games = games.Where(g => g.HostName == hostName);
                }
                games = games.OrderByDescending(g => g.Week).ThenByDescending(g => g.ID);

                return games;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the week count from the repository
        /// </summary>
        /// <returns></returns>
        public int GetWeekCount()
        {
            try
            {
                return _weekCountRepository.GetEntities().FirstOrDefault().Count;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        #region Helpers

        /// <summary>
        /// Adds data of a new game to the database TeamSeasons table
        /// </summary>
        /// <param name="game">The Game object from which data will be extracted</param>
        private void AddGameToTeams(Game newGame)
        {
            EditTeams(newGame, Direction.Up);
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
        /// <param name="oldGame">The Game object from which data will be extracted</param>
        private void DeleteGameFromTeams(Game oldGame)
        {
            EditTeams(oldGame, Direction.Down);
        }

        /// <summary>
        /// Edits data of an existing game in the database TeamSeasons table
        /// </summary>
        /// <param name="oldGame">The Game object from which data will be extracted for subtraction</param>
        /// <param name="newGame">The Game object from which data will be extracted for addition</param>
        private void EditGameInTeams(Game oldGame, Game newGame)
        {
            EditTeams(oldGame, Direction.Down);
            EditTeams(newGame, Direction.Up);
        }

        /// <summary>
        /// Edits season scoring data for both of a game's contestants
        /// </summary>
        /// <param name="game">The Game object from which data will be extracted</param>
        /// <param name="operation">The arithmetic operation to be applied to the team's scoring data</param>
        private void EditScoringData(Game game, Operation operation)
        {
            EditScoringDataByTeamSeason(game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);
            EditScoringDataByTeamSeason(game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);
        }

        /// <summary>
        /// Edits all scoring data for the specified Team
        /// </summary>
        /// <param name="teamName">The name of the selected team</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="operation">The arithmetic operation to be applied to the team's scoring data</param>
        /// <param name="teamScore">The team's game score</param>
        /// <param name="opponentScore">The opponent's game score</param>
        /// <returns></returns>
        private void EditScoringDataByTeamSeason(string teamName, int seasonID, Operation operation,
            double teamScore, double opponentScore)
        {
            var teamSeason = _teamSeasonRepository.FindEntity(teamName, seasonID);
            if (teamSeason != null)
            {
                teamSeason.PointsFor = (int)operation(teamSeason.PointsFor, teamScore);
                teamSeason.PointsAgainst = (int)operation(teamSeason.PointsAgainst, opponentScore);

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
        }

        /// <summary>
        /// Edits the DataModel's Teams table with data from the specified game.
        /// </summary>
        /// <param name="game">The Game object from which data will be extracted</param>
        /// <param name="direction">The direction, up or down, by which both teams' data will be adjusted</param>
        private void EditTeams(Game game, Direction direction)
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
                ProcessGame(game, operation);
            }
            catch (ObjectNotFoundException ex)
            {
                Log.Error("ObjectNotFoundException in GamesService.EditTeams: " + ex.Message);
                _sharedService.ShowExceptionMessage(ex, "ObjectNotFoundException");
            }
        }

        /// <summary>
        /// Edits each seasonTeam's data (wins, losses, and ties) from all games.
        /// </summary>
        /// <param name="game">The Game object from which data will be extracted</param>
        /// <param name="operation">The arithmetic operation to be applied to the team's scoring data</param>
        private void EditWinLossData(Game game, Operation operation)
        {
            var seasonID = game.SeasonID;

            // Get guest's season stats.
            var guestSeason = _teamSeasonRepository.FindEntity(game.GuestName, seasonID);
            if (guestSeason != null)
            {
                guestSeason.Games = (int)operation(guestSeason.Games, 1);
            }

            // Get host's season stats.
            var hostSeason = _teamSeasonRepository.FindEntity(game.HostName, seasonID);
            if (hostSeason != null)
            {
                hostSeason.Games = (int)operation(hostSeason.Games, 1);
            }

            var winnerName = game.WinnerName;
            var loserName = game.LoserName;
            if (String.IsNullOrEmpty(winnerName) || String.IsNullOrEmpty(loserName))
            {
                // Game is a tie.
                if (guestSeason != null)
                {
                    guestSeason.Ties = (int)operation(guestSeason.Ties, 1);
                }

                if (hostSeason != null)
                {
                    hostSeason.Ties = (int)operation(hostSeason.Ties, 1);
                }
            }
            else
            {
                // Game is not a tie (has a winner and a loser).
                var winnerSeason = _teamSeasonRepository.FindEntity(game.WinnerName, seasonID);
                if (winnerSeason != null)
                {
                    winnerSeason.Wins = (int)operation(winnerSeason.Wins, 1);
                }

                var loserSeason = _teamSeasonRepository.FindEntity(game.LoserName, seasonID);
                if (loserSeason != null)
                {
                    loserSeason.Losses = (int)operation(loserSeason.Losses, 1);
                }
            }

            // Calculate each team's season winning percentage.
            if (guestSeason != null)
            {
                guestSeason.WinningPercentage = _calculator.CalculateWinningPercentage(guestSeason);
            }

            if (hostSeason != null)
            {
                hostSeason.WinningPercentage = _calculator.CalculateWinningPercentage(hostSeason);
            }
        }

        /// <summary>
        /// Processes a game
        /// </summary>
        /// <param name="game">The Game object from which data will be extracted</param>
        /// <param name="operation">The arithmetic operation to be applied to the team's scoring data</param>
        private void ProcessGame(Game game, Operation operation)
        {
            EditWinLossData(game, operation);
            EditScoringData(game, operation);
        }

        #endregion Helpers

        #endregion Methods
    }
}
