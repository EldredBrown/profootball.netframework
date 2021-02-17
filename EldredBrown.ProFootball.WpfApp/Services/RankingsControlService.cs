using System;
using System.Collections.Generic;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Services
{
    public interface IRankingsControlService
    {
        IEnumerable<GetRankingsOffensive_Result> GetRankingsOffensiveBySeason(int? seasonID);
        IEnumerable<GetRankingsDefensive_Result> GetRankingsDefensiveBySeason(int? seasonID);
        IEnumerable<GetRankingsTotal_Result> GetRankingsTotalBySeason(int? seasonID);
    }

    /// <summary>
    /// Service class used by the RankingsControl class
    /// </summary>
    public class RankingsControlService : IRankingsControlService
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISharedService _sharedService;
        private readonly IStoredProcedureRepository _storedProcedureRepository;

        /// <summary>
        /// Initializes a new instance of the RankingsControlService class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="storedProcedureRepository"></param>
        public RankingsControlService(ISharedService sharedService,
            IStoredProcedureRepository storedProcedureRepository)
        {
            _sharedService = sharedService;
            _storedProcedureRepository = storedProcedureRepository;
        }

        /// <summary>
        /// Gets all offensive rankings for the specified season
        /// </summary>
        /// <param name="seasonID">The ID of the season for which rankings will be fetched</param>
        /// <returns>An enumerable collection of all the team rankings for the specified season</returns>
        public IEnumerable<GetRankingsOffensive_Result> GetRankingsOffensiveBySeason(int? seasonID)
        {
            try
            {
                return _storedProcedureRepository.GetRankingsOffensive(seasonID);
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException caught in RankingsControlService.GetRankingsOffensiveBySeason: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<GetRankingsOffensive_Result>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets all defensive rankings for the specified season
        /// </summary>
        /// <param name="seasonID">The ID of the season for which rankings will be fetched</param>
        /// <returns>An enumerable collection of all the team rankings for the specified season</returns>
        public IEnumerable<GetRankingsDefensive_Result> GetRankingsDefensiveBySeason(int? seasonID)
        {
            try
            {
                return _storedProcedureRepository.GetRankingsDefensive(seasonID);
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException caught in RankingsControlService.GetRankingsDefensiveBySeason: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<GetRankingsDefensive_Result>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets all total rankings for the specified season
        /// </summary>
        /// <param name="seasonID">The ID of the season for which rankings will be fetched</param>
        /// <returns>An enumerable collection of all the team rankings for the specified season</returns>
        public IEnumerable<GetRankingsTotal_Result> GetRankingsTotalBySeason(int? seasonID)
        {
            try
            {
                return _storedProcedureRepository.GetRankingsTotal(seasonID);
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException caught in RankingsControlService.GetRankingsTotalBySeason: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<GetRankingsTotal_Result>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
