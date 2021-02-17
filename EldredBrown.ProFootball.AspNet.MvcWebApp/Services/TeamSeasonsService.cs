using System;
using System.Collections.Generic;
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
    public interface ITeamSeasonsService
    {
        Task<IEnumerable<TeamSeasonViewModel>> GetEntitiesOrderedAsync(int seasonID, string sortOrder,
            ProFootballEntities dbContextInjected = null);

        Task<TeamSeasonDetailsViewModel> GetTeamSeasonDetailsAsync(string teamName, int seasonID,
            ProFootballEntities dbContextInjected = null);

        void SetSelectedSeason(IEnumerable<SeasonViewModel> seasons, int? seasonID, string sortOrder);
        Task UpdateRankings(ProFootballEntities dbContextInjected = null);
    }

    /// <summary>
    /// Service class used by the TeamSeasonsController
    /// </summary>
    public class TeamSeasonsService : ITeamSeasonsService
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISharedService _sharedService;
        private readonly IDataMapper _dataMapper;
        private readonly IRepository<LeagueSeason> _leagueSeasonRepository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly IRepository<TeamSeason> _teamSeasonRepository;
        private readonly ICalculator _calculator;

        /// <summary>
        /// Initializes a new instance of the TeamSeasonsService class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="dataMapper"></param>
        /// <param name="leagueSeasonRepository"></param>
        /// <param name="teamSeasonRepository"></param>
        /// <param name="storedProcedureRepository"></param>
        /// <param name="calculator"></param>
        public TeamSeasonsService(ISharedService sharedService, IDataMapper dataMapper,
            IRepository<LeagueSeason> leagueSeasonRepository, IRepository<TeamSeason> teamSeasonRepository,
            IStoredProcedureRepository storedProcedureRepository, ICalculator calculator)
        {
            _sharedService = sharedService;
            _dataMapper = dataMapper;
            _leagueSeasonRepository = leagueSeasonRepository;
            _teamSeasonRepository = teamSeasonRepository;
            _storedProcedureRepository = storedProcedureRepository;
            _calculator = calculator;
        }

        public static int SelectedSeason;

        /// <summary>
        /// Gets an ordered list of TeamSeasonViewModel objects
        /// </summary>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="sortOrder">The column on which and order by which the list will be sorted</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        /// <returns>The ordered list of TeamSeasonViewModel objects</returns>
        public async Task<IEnumerable<TeamSeasonViewModel>> GetEntitiesOrderedAsync(int seasonID, string sortOrder,
            ProFootballEntities dbContextInjected = null)
        {
            var teamSeasonViewModels = new List<TeamSeasonViewModel>();

            IEnumerable<TeamSeason> teamSeasons;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                teamSeasons = await _teamSeasonRepository.GetEntitiesAsync(dbContext);
            }

            try
            {
                teamSeasons = teamSeasons.Where(ts => ts.SeasonID == seasonID);
                switch (sortOrder)
                {
                    case "team_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.TeamName);
                        break;
                    case "team_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.TeamName);
                        break;
                    case "wins_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.Wins);
                        break;
                    case "wins_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.Wins);
                        break;
                    case "losses_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.Losses);
                        break;
                    case "losses_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.Losses);
                        break;
                    case "ties_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.Ties);
                        break;
                    case "ties_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.Ties);
                        break;
                    case "win_pct_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.WinningPercentage);
                        break;
                    case "win_pct_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.WinningPercentage);
                        break;
                    case "pf_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.PointsFor);
                        break;
                    case "pf_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.PointsFor);
                        break;
                    case "pa_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.PointsAgainst);
                        break;
                    case "pa_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.PointsAgainst);
                        break;
                    case "pyth_wins_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.PythagoreanWins);
                        break;
                    case "pyth_wins_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.PythagoreanWins);
                        break;
                    case "pyth_losses_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.PythagoreanLosses);
                        break;
                    case "pyth_losses_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.PythagoreanLosses);
                        break;
                    case "off_avg_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.OffensiveAverage);
                        break;
                    case "off_avg_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.OffensiveAverage);
                        break;
                    case "off_factor_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.OffensiveFactor);
                        break;
                    case "off_factor_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.OffensiveFactor);
                        break;
                    case "off_index_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.OffensiveIndex);
                        break;
                    case "off_index_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.OffensiveIndex);
                        break;
                    case "def_avg_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.DefensiveAverage);
                        break;
                    case "def_avg_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.DefensiveAverage);
                        break;
                    case "def_factor_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.DefensiveFactor);
                        break;
                    case "def_factor_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.DefensiveFactor);
                        break;
                    case "def_index_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.DefensiveIndex);
                        break;
                    case "def_index_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.DefensiveIndex);
                        break;
                    case "fin_pyth_pct_asc":
                        teamSeasons = teamSeasons.OrderBy(ts => ts.FinalPythagoreanWinningPercentage);
                        break;
                    case "fin_pyth_pct_desc":
                        teamSeasons = teamSeasons.OrderByDescending(ts => ts.FinalPythagoreanWinningPercentage);
                        break;
                    default:
                        teamSeasons = teamSeasons.OrderBy(ts => ts.TeamName);
                        break;
                }
            }
            catch (ArgumentNullException ex)
            {
                _log.Error($"ArgumentNullException in TeamSeasonsService.GetEntitiesOrderedAsync: {ex.Message}");
            }

            foreach (var teamSeason in teamSeasons)
            {
                var teamSeasonViewModel = _dataMapper.MapToTeamSeasonViewModel(teamSeason);
                teamSeasonViewModels.Add(teamSeasonViewModel);
            }

            return teamSeasonViewModels;
        }

        /// <summary>
        /// Gets the details of a team's season asynchronously
        /// </summary>
        /// <param name="teamName">The name of the selected team</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="dbContext">Injected DbContext object for unit testing</param>
        /// <returns>A TeamSeasonDetailsViewModel instance containing the selected team's seasons details</returns>
        public async Task<TeamSeasonDetailsViewModel> GetTeamSeasonDetailsAsync(string teamName, int seasonID,
            ProFootballEntities dbContext = null)
        {
            return new TeamSeasonDetailsViewModel
            {
                TeamSeason = await _sharedService.FindEntityAsync(teamName, seasonID, dbContext),
                TeamSeasonScheduleProfile = GetTeamSeasonScheduleProfile(teamName, seasonID, dbContext),
                TeamSeasonScheduleTotals = GetTeamSeasonScheduleTotals(teamName, seasonID, dbContext),
                TeamSeasonScheduleAverages = GetTeamSeasonScheduleAverages(teamName, seasonID, dbContext)
            };
        }

        private IEnumerable<TeamSeasonScheduleProfileViewModel> GetTeamSeasonScheduleProfile(string teamName,
            int? seasonID, ProFootballEntities dbContextInjected = null)
        {
            var teamSeasonScheduleProfileViewModels = new List<TeamSeasonScheduleProfileViewModel>();

            IEnumerable<GetTeamSeasonScheduleProfile_Result> teamSeasonScheduleProfile;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                teamSeasonScheduleProfile = _storedProcedureRepository
                    .GetTeamSeasonScheduleProfile(dbContext, teamName, seasonID).ToList();
            }
            foreach (var item in teamSeasonScheduleProfile)
            {
                var teamSeasonScheduleProfileViewModel = _dataMapper.MapToTeamSeasonScheduleProfileViewModel(item);
                teamSeasonScheduleProfileViewModels.Add(teamSeasonScheduleProfileViewModel);
            }

            return teamSeasonScheduleProfileViewModels;
        }

        private TeamSeasonScheduleTotalsViewModel GetTeamSeasonScheduleTotals(string teamName, int? seasonID,
            ProFootballEntities dbContextInjected = null)
        {
            GetTeamSeasonScheduleTotals_Result teamSeasonScheduleTotals;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                teamSeasonScheduleTotals = _storedProcedureRepository
                    .GetTeamSeasonScheduleTotals(dbContext, teamName, seasonID).FirstOrDefault();
            }
            return _dataMapper.MapToTeamSeasonScheduleTotalsViewModel(teamSeasonScheduleTotals);
        }

        private TeamSeasonScheduleAveragesViewModel GetTeamSeasonScheduleAverages(string teamName, int? seasonID,
            ProFootballEntities dbContextInjected = null)
        {
            GetTeamSeasonScheduleAverages_Result teamSeasonScheduleAverages;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                teamSeasonScheduleAverages = _storedProcedureRepository
                    .GetTeamSeasonScheduleAverages(dbContext, teamName, seasonID).FirstOrDefault();
            }
            return _dataMapper.MapToTeamSeasonScheduleAveragesViewModel(teamSeasonScheduleAverages);
        }

        /// <summary>
        /// Sets the selected season for this web app
        /// </summary>
        /// <param name="seasons">An enumerable collection of SeasonViewModel objects</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="sortOrder">Order by which the seasons will be sorted</param>
        public void SetSelectedSeason(IEnumerable<SeasonViewModel> seasons, int? seasonID, string sortOrder)
        {
            if (seasonID == null)
            {
                if (string.IsNullOrEmpty(sortOrder))
                {
                    if (WebGlobals.SelectedSeason == null)
                    {
                        SelectedSeason = seasons.First().ID;
                    }
                    else
                    {
                        SelectedSeason = (int)WebGlobals.SelectedSeason;
                    }
                }
            }
            else
            {
                SelectedSeason = (int)seasonID;
            }

            WebGlobals.SelectedSeason = SelectedSeason;
        }

        /// <summary>
        /// Updates all team rankings for the selected season
        /// </summary>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        public async Task UpdateRankings(ProFootballEntities dbContextInjected = null)
        {
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                // Update LeagueSeasons table.
                var leagueSeasonTotals = _storedProcedureRepository
                    .GetLeagueSeasonTotals(dbContext, "NFL", SelectedSeason).FirstOrDefault();

                var leagueSeason =
                    (await _leagueSeasonRepository.GetEntitiesAsync(dbContext)).FirstOrDefault(ls =>
                        ls.SeasonID == SelectedSeason);

                leagueSeason.TotalGames = (double)leagueSeasonTotals.TotalGames;
                leagueSeason.TotalPoints = (double)leagueSeasonTotals.TotalPoints;
                leagueSeason.AveragePoints = (double)leagueSeasonTotals.AveragePoints;

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _log.Error("TeamSeasonsService.UpdateRankings could not save changes to database: " + ex.Message);
                }

                IList<TeamSeason> teamSeasons;
                try
                {
                    // Update TeamSeasons table.
                    teamSeasons = (await _teamSeasonRepository.GetEntitiesAsync(dbContext))
                        .Where(ts => ts.SeasonID == SelectedSeason)
                        .ToList();
                }
                catch (ArgumentNullException ex)
                {
                    _log.Error($"ArgumentNullException in TeamSeasonsService.UpdateRankings: {ex.Message}");

                    MessageBox.Show(
                        "The UpdateRankings function was unable to update TeamSeasons. Please wait a few minutes and try again.",
                        "Exception", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }

                // This looks like the place where I want to make maximum use of parallel threading.
                //Parallel.ForEach(teamSeasons, teamSeason => UpdateRankingsByTeamSeason(teamSeason));

                foreach (var teamSeason in teamSeasons)
                {
                    try
                    {
                        await UpdateRankingsByTeamSeason(dbContext, teamSeason);
                    }
                    catch (ArgumentNullException ex)
                    {
                        _log.Error($"ArgumentNullException in TeamSeasonsService.UpdateRankings: {ex.Message}");
                        return;
                    }
                }

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _log.Error($"TeamSeasonsService.UpdateRankings could not save changes to database: {ex.Message}");
                }
            }
        }

        private async Task UpdateRankingsByTeamSeason(ProFootballEntities dbContext, TeamSeason teamSeason)
        {
            var teamSeasonScheduleTotals = _storedProcedureRepository
                .GetTeamSeasonScheduleTotals(dbContext, teamSeason.TeamName, teamSeason.SeasonID)
                .FirstOrDefault();

            var teamSeasonScheduleAverages = _storedProcedureRepository
                .GetTeamSeasonScheduleAverages(dbContext, teamSeason.TeamName, teamSeason.SeasonID)
                .FirstOrDefault();

            if (!(teamSeasonScheduleTotals == null || teamSeasonScheduleAverages == null ||
                teamSeasonScheduleTotals.ScheduleGames == null))
            {
                teamSeason.OffensiveAverage = _calculator.Divide(teamSeason.PointsFor, teamSeason.Games);
                teamSeason.DefensiveAverage = _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games);

                teamSeason.OffensiveFactor = _calculator.Divide((double)teamSeason.OffensiveAverage,
                    (double)teamSeasonScheduleAverages.PointsAgainst);

                teamSeason.DefensiveFactor = _calculator.Divide((double)teamSeason.DefensiveAverage,
                    (double)teamSeasonScheduleAverages.PointsFor);

                var leagueSeason = (await _leagueSeasonRepository.GetEntitiesAsync(dbContext))
                    .FirstOrDefault(ls =>
                        ls.LeagueName == teamSeason.LeagueName && ls.SeasonID == teamSeason.SeasonID);

                if (leagueSeason != null)
                {
                    teamSeason.OffensiveIndex = (teamSeason.OffensiveAverage +
                                                    teamSeason.OffensiveFactor * leagueSeason.AveragePoints) / 2;
                    teamSeason.DefensiveIndex = (teamSeason.DefensiveAverage +
                                                    teamSeason.DefensiveFactor * leagueSeason.AveragePoints) / 2;
                }

                teamSeason.FinalPythagoreanWinningPercentage =
                    _calculator.CalculatePythagoreanWinningPercentage(teamSeason);
            }
        }
    }
}
