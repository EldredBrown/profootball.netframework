using System;
using System.Collections.Generic;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Services
{
    public interface ITeamSeasonsControlService
    {
        IEnumerable<TeamSeason> GetEntities(int seasonID);
        IEnumerable<GetTeamSeasonScheduleProfile_Result> GetTeamSeasonScheduleProfile(string teamName, int? seasonID);
        IEnumerable<GetTeamSeasonScheduleTotals_Result> GetTeamSeasonScheduleTotals(string teamName, int? seasonID);
        IEnumerable<GetTeamSeasonScheduleAverages_Result> GetTeamSeasonScheduleAverages(string teamName, int? seasonID);
    }

    /// <summary>
    /// Service class used by the TeamSeasonsControl class
    /// </summary>
    public class TeamSeasonsControlService : ITeamSeasonsControlService
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ISharedService _sharedService;
        private IRepository<TeamSeason> _teamSeasonRepository;
        private IStoredProcedureRepository _storedProcedureRepository;

        /// <summary>
        /// Initializes a new instance of the TeamSeasonsControlService class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="teamSeasonRepository"></param>
        /// <param name="storedProcedureRepository"></param>
        public TeamSeasonsControlService(ISharedService sharedService, IRepository<TeamSeason> teamSeasonRepository,
            IStoredProcedureRepository storedProcedureRepository)
        {
            _sharedService = sharedService;
            _teamSeasonRepository = teamSeasonRepository;
            _storedProcedureRepository = storedProcedureRepository;
        }

        /// <summary>
        /// Gets all the TeamSeason objects for a specified season
        /// </summary>
        /// <param name="seasonID">The ID of the season for which teams will be fetched</param>
        /// <returns>An enumerable collection of the fetched TeamSeason objects</returns>
        public IEnumerable<TeamSeason> GetEntities(int seasonID)
        {
            try
            {
                return _teamSeasonRepository.GetEntities().Where(ts => ts.SeasonID == seasonID);
            }
            catch (ArgumentNullException ex)
            {
                _log.Error($"ArgumentNullException caught in TeamSeasonsControlService.GetEntities: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<TeamSeason>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the schedule profile for the selected team and season
        /// </summary>
        /// <param name="teamName">The name of the team for which a schedule profile will be fetched</param>
        /// <param name="seasonID">The ID of the season for which a schedule profile will be fetched</param>
        /// <returns>An enumerable collection of the query results</returns>
        public IEnumerable<GetTeamSeasonScheduleProfile_Result> GetTeamSeasonScheduleProfile(string teamName,
            int? seasonID)
        {
            try
            {
                return _storedProcedureRepository.GetTeamSeasonScheduleProfile(teamName, seasonID);
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException caught in TeamSeasonsControlService.GetTeamSeasonScheduleProfile: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<GetTeamSeasonScheduleProfile_Result>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the schedule totals for the selected team and season
        /// </summary>
        /// <param name="teamName">The name of the team for which a schedule profile will be fetched</param>
        /// <param name="seasonID">The ID of the season for which a schedule profile will be fetched</param>
        /// <returns>An enumerable collection of the query results</returns>
        public IEnumerable<GetTeamSeasonScheduleTotals_Result> GetTeamSeasonScheduleTotals(string teamName,
            int? seasonID)
        {
            try
            {
                return _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamName, seasonID);
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException caught in TeamSeasonsControlService.GetTeamSeasonScheduleTotals: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<GetTeamSeasonScheduleTotals_Result>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the schedule averages for the selected team and season
        /// </summary>
        /// <param name="teamName">The name of the team for which a schedule profile will be fetched</param>
        /// <param name="seasonID">The ID of the season for which a schedule profile will be fetched</param>
        /// <returns>An enumerable collection of the query results</returns>
        public IEnumerable<GetTeamSeasonScheduleAverages_Result> GetTeamSeasonScheduleAverages(string teamName,
            int? seasonID)
        {
            try
            {
                return _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamName, seasonID).ToList();
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException caught in TeamSeasonsControlService.GetTeamSeasonScheduleAverages: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<GetTeamSeasonScheduleAverages_Result>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
