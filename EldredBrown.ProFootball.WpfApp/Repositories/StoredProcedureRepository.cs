using System;
using System.Data.Entity.Core.Objects;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    public interface IStoredProcedureRepository
    {
        ObjectResult<GetLeagueSeasonTotals_Result> GetLeagueSeasonTotals(string leagueName, int? seasonID);
        ObjectResult<GetRankingsOffensive_Result> GetRankingsOffensive(int? seasonID);
        ObjectResult<GetRankingsDefensive_Result> GetRankingsDefensive(int? seasonID);
        ObjectResult<GetRankingsTotal_Result> GetRankingsTotal(int? seasonID);

        ObjectResult<GetSeasonStandingsForLeague_Result> GetSeasonStandingsForLeague(int? seasonID,
            string leagueName);

        ObjectResult<GetSeasonStandingsForConference_Result> GetSeasonStandingsForConference(int? seasonID,
            string conferenceName);

        ObjectResult<GetSeasonStandingsForDivision_Result> GetSeasonStandingsForDivision(int? seasonID,
            string divisionName);

        ObjectResult<GetTeamSeasonScheduleProfile_Result> GetTeamSeasonScheduleProfile(string teamName, int? seasonID);
        ObjectResult<GetTeamSeasonScheduleTotals_Result> GetTeamSeasonScheduleTotals(string teamName, int? seasonID);
        ObjectResult<GetTeamSeasonScheduleAverages_Result> GetTeamSeasonScheduleAverages(string teamName, int? seasonID);
    }

    /// <summary>
    /// Repository for access to this app's stored procedures (programmed at the database)
    /// </summary>
    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the StoredProcedureRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public StoredProcedureRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Constructors & Finalizers

        #region Methods

        /// <summary>
        /// Gets totals for a league's season
        /// </summary>
        /// <param name="leagueName">The name of the selected league</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetLeagueSeasonTotals_Result> GetLeagueSeasonTotals(string leagueName, int? seasonID)
        {
            try
            {
                return _dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            }
            catch (ArgumentException ex)
            {
                Log.Error($"Argument exception in StoredProcedureRepository.GetLeagueSeasonTotals: {ex.Message}");

                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error($"Invalid operation in StoredProcedureRepository.GetLeagueSeasonTotals: {ex.Message}");

                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets all offensive rankings for the selected season
        /// </summary>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetRankingsOffensive_Result> GetRankingsOffensive(int? seasonID)
        {
            try
            {
                return _dbContext.GetRankingsOffensive(seasonID);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets all defensive rankings for the selected season
        /// </summary>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetRankingsDefensive_Result> GetRankingsDefensive(int? seasonID)
        {
            try
            {
                return _dbContext.GetRankingsDefensive(seasonID);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets all total rankings for the selected season
        /// </summary>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetRankingsTotal_Result> GetRankingsTotal(int? seasonID)
        {
            try
            {
                return _dbContext.GetRankingsTotal(seasonID);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the standings for a specified season and league
        /// </summary>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="leagueName">The name of the selected league</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetSeasonStandingsForLeague_Result> GetSeasonStandingsForLeague(int? seasonID,
            string leagueName)
        {
            try
            {
                return _dbContext.GetSeasonStandingsForLeague(seasonID, leagueName);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the standings for a specified season and conference
        /// </summary>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="conferenceName">The name of the selected conference</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetSeasonStandingsForConference_Result> GetSeasonStandingsForConference(int? seasonID,
            string conferenceName)
        {
            try
            {
                return _dbContext.GetSeasonStandingsForConference(seasonID, conferenceName);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the standings for a specified season and conference
        /// </summary>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="divisionName">The name of the selected division</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetSeasonStandingsForDivision_Result> GetSeasonStandingsForDivision(int? seasonID,
            string divisionName)
        {
            try
            {
                return _dbContext.GetSeasonStandingsForDivision(seasonID, divisionName);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the profile of a team's season schedule
        /// </summary>
        /// <param name="teamName">The name of the selected team</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetTeamSeasonScheduleProfile_Result> GetTeamSeasonScheduleProfile(string teamName,
            int? seasonID)
        {
            try
            {
                return _dbContext.GetTeamSeasonScheduleProfile(teamName, seasonID);
            }
            catch (ArgumentException ex)
            {
                Log.Error(
                    $"Argument exception in StoredProcedureRepository.GetTeamSeasonScheduleProfile: {ex.Message}");

                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error($"Invalid operation in StoredProcedureRepository.GetTeamSeasonScheduleProfile: {ex.Message}");

                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the totals of a team's season schedule
        /// </summary>
        /// <param name="teamName">The name of the selected team</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetTeamSeasonScheduleTotals_Result> GetTeamSeasonScheduleTotals(string teamName,
            int? seasonID)
        {
            try
            {
                return _dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID);
            }
            catch (ArgumentException ex)
            {
                Log.Error($"Argument exception in StoredProcedureRepository.GetTeamSeasonScheduleTotals: {ex.Message}");

                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error($"Invalid operation in StoredProcedureRepository.GetTeamSeasonScheduleTotals: {ex.Message}");

                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the averages of a team's season schedule
        /// </summary>
        /// <param name="teamName">The name of the selected team</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetTeamSeasonScheduleAverages_Result> GetTeamSeasonScheduleAverages(string teamName,
            int? seasonID)
        {
            try
            {
                return _dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID);
            }
            catch (ArgumentException ex)
            {
                Log.Error(
                    $"Argument exception in StoredProcedureRepository.GetTeamSeasonScheduleAverages: {ex.Message}");

                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(
                    $"Invalid operation in StoredProcedureRepository.GetTeamSeasonScheduleAverages: {ex.Message}");

                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        #endregion Methods
    }
}
