using System;
using System.Data.Entity.Core.Objects;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using log4net;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories
{
    public interface IStoredProcedureRepository
    {
        ObjectResult<GetLeagueSeasonTotals_Result> GetLeagueSeasonTotals(ProFootballEntities dbContext,
            string leagueName, int? seasonID);

        ObjectResult<GetSeasonStandings_Result> GetSeasonStandings(ProFootballEntities dbContext, int? seasonID,
            bool? groupByDivision);

        ObjectResult<GetTeamSeasonScheduleAverages_Result> GetTeamSeasonScheduleAverages(ProFootballEntities dbContext,
            string teamName, int? seasonID);

        ObjectResult<GetTeamSeasonScheduleProfile_Result> GetTeamSeasonScheduleProfile(ProFootballEntities dbContext,
            string teamName, int? seasonID);

        ObjectResult<GetTeamSeasonScheduleTotals_Result> GetTeamSeasonScheduleTotals(ProFootballEntities dbContext,
            string teamName, int? seasonID);
    }

    /// <summary>
    /// Repository for access to this app's stored procedures (programmed at the database)
    /// </summary>
    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Methods

        /// <summary>
        /// Gets totals for a league's season
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="leagueName">The name of the selected league</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetLeagueSeasonTotals_Result> GetLeagueSeasonTotals(ProFootballEntities dbContext,
            string leagueName, int? seasonID)
        {
            ObjectResult<GetLeagueSeasonTotals_Result> retVal = null;
            try
            {
                retVal = dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            }
            catch (ArgumentException ex)
            {
                Log.Error("Argument exception in StoredProcedureRepository.GetLeagueSeasonTotals: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Invalid operation in StoredProcedureRepository.GetLeagueSeasonTotals: " + ex.Message);
            }

            return retVal;
        }

        /// <summary>
        /// Gets the standings for a specified season
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="groupByDivision">A flag to tell the application whether to group the standings by division</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetSeasonStandings_Result> GetSeasonStandings(ProFootballEntities dbContext,
            int? seasonID, bool? groupByDivision)
        {
            ObjectResult<GetSeasonStandings_Result> retVal = null;
            try
            {
                retVal = dbContext.GetSeasonStandings(seasonID, groupByDivision);
            }
            catch (ArgumentException ex)
            {
                Log.Error("Argument exception in StoredProcedureRepository.GetSeasonStandings: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Invalid operation in StoredProcedureRepository.GetSeasonStandings: " + ex.Message);
            }

            return retVal;
        }

        /// <summary>
        /// Gets the averages of a team's season schedule
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teamName">The name of the selected team</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetTeamSeasonScheduleAverages_Result> GetTeamSeasonScheduleAverages(
            ProFootballEntities dbContext, string teamName, int? seasonID)
        {
            ObjectResult<GetTeamSeasonScheduleAverages_Result> retVal = null;
            try
            {
                retVal = dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID);
            }
            catch (ArgumentException ex)
            {
                Log.Error(
                    "Argument exception in StoredProcedureRepository.GetTeamSeasonScheduleAverages: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Invalid operation in StoredProcedureRepository.GetTeamSeasonScheduleAverages: " +
                          ex.Message);
            }

            return retVal;
        }

        /// <summary>
        /// Gets the profile of a team's season schedule
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teamName">The name of the selected team</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetTeamSeasonScheduleProfile_Result> GetTeamSeasonScheduleProfile(
            ProFootballEntities dbContext, string teamName, int? seasonID)
        {
            ObjectResult<GetTeamSeasonScheduleProfile_Result> retVal = null;
            try
            {
                retVal = dbContext.GetTeamSeasonScheduleProfile(teamName, seasonID);
            }
            catch (ArgumentException ex)
            {
                Log.Error(
                    "Argument exception in StoredProcedureRepository.GetTeamSeasonScheduleProfile: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Invalid operation in StoredProcedureRepository.GetTeamSeasonScheduleProfile: " + ex.Message);
            }

            return retVal;
        }

        /// <summary>
        /// Gets the totals of a team's season schedule
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teamName">The name of the selected team</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <returns>The result of the stored procedure execution</returns>
        public ObjectResult<GetTeamSeasonScheduleTotals_Result> GetTeamSeasonScheduleTotals(
            ProFootballEntities dbContext, string teamName, int? seasonID)
        {
            ObjectResult<GetTeamSeasonScheduleTotals_Result> retVal = null;
            try
            {
                retVal = dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID);
            }
            catch (ArgumentException ex)
            {
                Log.Error("Argument exception in StoredProcedureRepository.GetTeamSeasonScheduleTotals: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Invalid operation in StoredProcedureRepository.GetTeamSeasonScheduleTotals: " + ex.Message);
            }

            return retVal;
        }

        #endregion Methods
    }
}
