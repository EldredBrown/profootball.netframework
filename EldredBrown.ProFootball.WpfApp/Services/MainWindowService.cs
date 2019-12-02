using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using EldredBrown.ProFootball.WpfApp.Windows;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Services
{
    public interface IMainWindowService
    {
        IEnumerable<int> GetAllSeasonIds();
        int GetGameCount(int? seasonID);
        void PredictGameScore();
        void RunWeeklyUpdate();
        void ShowGames(IGamesWindow gamesWindowInjected = null);
    }

    /// <summary>
    /// Service class used by the MainWindow class
    /// </summary>
    public class MainWindowService : IMainWindowService
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISharedService _sharedService;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Season> _seasonRepository;
        private readonly IRepository<LeagueSeason> _leagueSeasonRepository;
        private readonly IRepository<TeamSeason> _teamSeasonRepository;
        private readonly IRepository<WeekCount> _weekCountRepository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ProFootballEntities _dbContext;
        private readonly ICalculator _calculator;
        private readonly IGamePredictorWindow _gamePredictorWindow;

        private object _dbLock = new object();

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the MainWindowService class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="gameRepository"></param>
        /// <param name="seasonRepository"></param>
        /// <param name="weekCountRepository"></param>
        /// <param name="leagueSeasonRepository"></param>
        /// <param name="teamSeasonRepository"></param>
        /// <param name="storedProcedureRepository"></param>
        /// <param name="dbContext"></param>
        /// <param name="calculator"></param>
        public MainWindowService(ISharedService sharedService, IRepository<Game> gameRepository,
            IRepository<Season> seasonRepository, IRepository<LeagueSeason> leagueSeasonRepository,
            IRepository<TeamSeason> teamSeasonRepository, IRepository<WeekCount> weekCountRepository,
            IStoredProcedureRepository storedProcedureRepository, ProFootballEntities dbContext,
            ICalculator calculator, IGamePredictorWindow gamePredictorWindow)
        {
            _sharedService = sharedService;
            _gameRepository = gameRepository;
            _seasonRepository = seasonRepository;
            _leagueSeasonRepository = leagueSeasonRepository;
            _teamSeasonRepository = teamSeasonRepository;
            _weekCountRepository = weekCountRepository;
            _storedProcedureRepository = storedProcedureRepository;
            _dbContext = dbContext;
            _calculator = calculator;
            _gamePredictorWindow = gamePredictorWindow;
        }

        #endregion Constructors & Finalizers

        #region Methods

        /// <summary>
        /// Gets all the season IDs
        /// </summary>
        /// <returns>An enumerable collection of all season IDs</returns>
        public IEnumerable<int> GetAllSeasonIds()
        {
            IEnumerable<Season> seasons;
            try
            {
                seasons = _seasonRepository.GetEntities().OrderByDescending(s => s.ID);
            }
            catch (ArgumentNullException ex)
            {
                Log.Error($"ArgumentNullException caught in MainWindowService.GetAllSeasonIds: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                seasons = new List<Season>();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }

            foreach (var season in seasons)
            {
                yield return season.ID;
            }
        }

        /// <summary>
        /// Gets the game count for a specified season
        /// </summary>
        /// <param name="seasonID">The season for which a game count will be fetched</param>
        /// <returns>The count of games played in the specified season</returns>
        public int GetGameCount(int? seasonID)
        {
            int gameCount = 0;

            try
            {
                gameCount = _gameRepository.GetEntities().Count(g => g.SeasonID == seasonID);
            }
            catch (ArgumentNullException ex)
            {
                Log.Error($"ArgumentNullException caught in MainWindowService.GetGameCount: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);
            }
            catch (OverflowException ex)
            {
                Log.Error($"OverflowException caught in MainWindowService.GetGameCount: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex, "OverflowException");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }

            return gameCount;
        }

        /// <summary>
        /// Opens the Game Predictor Window
        /// </summary>
        /// <param name="gamePredictorWindowInjected">GamePredictorWindow dependency injection used only for unit testing</param>
        public void PredictGameScore()
        {
            try
            {
                _gamePredictorWindow.Show();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Runs a weekly update
        /// </summary>
        public void RunWeeklyUpdate()
        {
            // I experimented with farming this long running operation out to a separate thread to improve UI
            // responsiveness, but I eventually concluded that I actually DON'T want the UI to be responsive while
            // this update operation is running. An attempt to view data that's in the process of changing may
            // cause the application to crash, and the data won't mean anything, anyway.
            var dlgResult = _sharedService.ShowMessageBox(
                "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (dlgResult == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                // This hard-coded value is a bit of a hack at this time, but I intend to make this selectable by the
                // user in the future.
                var leagueName = "APFA";

                var seasonID = (int)WpfGlobals.SelectedSeason;
                UpdateLeagueSeason(leagueName, seasonID);
                var srcWeekCount = UpdateWeekCount(seasonID);

                _sharedService.SaveChanges(_dbContext);

                if (srcWeekCount >= 3)
                {
                    UpdateRankings();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }

            _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName, MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Shows the Games window
        /// </summary>
        /// <param name="gamesWindowInjected">IGamesWindow dependency injection used only for unit testing</param>
        public void ShowGames(IGamesWindow gamesWindowInjected = null)
        {
            try
            {
                // Show the Games window.
                var gamesWindow = gamesWindowInjected ?? new GamesWindow();
                gamesWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        #region Helpers

        /// <summary>
        /// Updates a specified LeagueSeason
        /// </summary>
        /// <param name="leagueName">The LeagueName of the LeagueSeason to update</param>
        /// <param name="seasonID">The SeasonID of the LeagueSeason to update</param>
        private void UpdateLeagueSeason(string leagueName, int seasonID)
        {
            var leagueSeason = _leagueSeasonRepository.FindEntity(leagueName, seasonID);

            GetLeagueSeasonTotals_Result leagueSeasonTotalsRow;
            try
            {
                leagueSeasonTotalsRow = _storedProcedureRepository.GetLeagueSeasonTotals(leagueName, seasonID)
                    .FirstOrDefault();
            }
            catch (ArgumentNullException ex)
            {
                Log.Error($"ArgumentNullException caught in MainWindowService.UpdateLeagueSeason: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return;
            }

            if (leagueSeasonTotalsRow == null)
            {
                return;
            }

            leagueSeason.TotalGames = (double)leagueSeasonTotalsRow.TotalGames;
            leagueSeason.TotalPoints = (double)leagueSeasonTotalsRow.TotalPoints;

            leagueSeason.AveragePoints = _calculator.Divide((double)leagueSeasonTotalsRow.TotalPoints,
                (double)leagueSeasonTotalsRow.TotalGames);
        }

        /// <summary>
        /// Updates the rankings of all teams
        /// </summary>
        private void UpdateRankings()
        {
            // Get the list of TeamSeason objects for the selected season.
            IEnumerable<TeamSeason> teamSeasons;
            try
            {
                teamSeasons = _teamSeasonRepository.GetEntities()
                    .Where(ts => ts.SeasonID == WpfGlobals.SelectedSeason);
            }
            catch (ArgumentNullException ex)
            {
                Log.Error($"ArgumentNullException in MainWindowService.UpdateRankings: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                teamSeasons = new List<TeamSeason>();
            }

            // This looks like the place where I want to make maximum use of parallel threading.
            //Parallel.ForEach(teamSeasons, seasonTeam => UpdateRankingsByTeamSeason(seasonTeam));

            // Iterate through the list of TeamSeason objects.
            foreach (var teamSeason in teamSeasons)
            {
                UpdateRankingsByTeamSeason(teamSeason);
            }

            // Save changes.
            _sharedService.SaveChanges(_dbContext);
        }

        /// <summary>
        /// Updates the rankings for a specified TeamSeason
        /// </summary>
        /// <param name="teamSeason">The TeamSeason object for which rankings will be updated</param>
        private void UpdateRankingsByTeamSeason(TeamSeason teamSeason)
        {
            try
            {
                lock (_dbLock)
                {
                    // Get needed stored procedure results.
                    var teamSeasonScheduleTotals = _storedProcedureRepository.GetTeamSeasonScheduleTotals(
                        teamSeason.TeamName, teamSeason.SeasonID).FirstOrDefault();

                    var teamSeasonScheduleAverages = _storedProcedureRepository.GetTeamSeasonScheduleAverages(
                        teamSeason.TeamName, teamSeason.SeasonID).FirstOrDefault();

                    // Calculate new rankings.
                    if (teamSeasonScheduleTotals != null && teamSeasonScheduleAverages != null &&
                        teamSeasonScheduleTotals.ScheduleGames != null)
                    {
                        teamSeason.OffensiveAverage = _calculator.Divide(teamSeason.PointsFor, teamSeason.Games);
                        teamSeason.DefensiveAverage = _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games);

                        teamSeason.OffensiveFactor =
                            _calculator.Divide((double)teamSeason.OffensiveAverage,
                            (double)teamSeasonScheduleAverages.PointsAgainst);

                        teamSeason.DefensiveFactor =
                            _calculator.Divide((double)teamSeason.DefensiveAverage,
                            (double)teamSeasonScheduleAverages.PointsFor);

                        var leagueSeason = _leagueSeasonRepository.FindEntity(teamSeason.LeagueName,
                            teamSeason.SeasonID);

                        teamSeason.OffensiveIndex = (teamSeason.OffensiveAverage + teamSeason.OffensiveFactor *
                            leagueSeason.AveragePoints) / 2;

                        teamSeason.DefensiveIndex = (teamSeason.DefensiveAverage + teamSeason.DefensiveFactor *
                            leagueSeason.AveragePoints) / 2;

                        teamSeason.FinalPythagoreanWinningPercentage =
                            _calculator.CalculatePythagoreanWinningPercentage(teamSeason);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Nullable object must have a value.")
                {
                    // Ignore exception.
                }
                else
                {
                    _sharedService.ShowExceptionMessage(ex, $"InvalidOperationException: {teamSeason.TeamName}");
                }
            }
            catch (Exception ex)
            {
                _sharedService.ShowExceptionMessage(ex.InnerException, $"Exception: {teamSeason.TeamName}");
            }
        }

        /// <summary>
        /// Updates the season week count
        /// </summary>
        /// <param name="seasonID">The ID of the season for which the week count will be updated</param>
        /// <returns></returns>
        private int UpdateWeekCount(int seasonID)
        {
            int srcWeekCount = 0;
            try
            {
                srcWeekCount = _gameRepository.GetEntities()
                    .Where(g => g.SeasonID == seasonID)
                    .Select(g => g.Week)
                    .Max();

                var destWeekCount = _weekCountRepository.FindEntity(seasonID);

                destWeekCount.Count = srcWeekCount;
            }
            catch (ArgumentNullException ex)
            {
                Log.Error($"ArgumentNullException caught in MainWindowService.UpdateWeekCount: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);
            }

            return srcWeekCount;
        }

        #endregion Helpers

        #endregion Methods
    }
}
