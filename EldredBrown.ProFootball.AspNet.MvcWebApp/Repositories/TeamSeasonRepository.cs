using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using log4net;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories
{
    /// <summary>
    /// Repository for access to the TeamSeason data model
    /// </summary>
    public class TeamSeasonRepository : IRepository<TeamSeason>
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRepository<TeamSeason> Implementation

        /// <summary>
        /// Adds a TeamSeason entity to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teamSeason">The TeamSeason entity to add</param>
        /// <returns>The TeamSeason entity added to dbContext</returns>
        public TeamSeason AddEntity(ProFootballEntities dbContext, TeamSeason teamSeason)
        {
            Log.Info("Adding TeamSeason entity to data store");
            return dbContext.TeamSeasons.Add(teamSeason);
        }

        /// <summary>
        /// Adds multiple TeamSeason entities to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teamSeasons">The collection of TeamSeason entities to add</param>
        /// <returns>The TeamSeason entity collection added to dbContext</returns>
        public IEnumerable<TeamSeason> AddEntities(ProFootballEntities dbContext, IEnumerable<TeamSeason> teamSeasons)
        {
            Log.Info("Adding TeamSeason entities to data store");
            return dbContext.TeamSeasons.AddRange(teamSeasons);
        }

        /// <summary>
        /// Creates a TeamSeason entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>The new TeamSeason entity instance</returns>
        public TeamSeason CreateEntity(ProFootballEntities dbContext)
        {
            Log.Info("Creating TeamSeason entity");
            return dbContext.TeamSeasons.Create();
        }

        /// <summary>
        /// Edits a TeamSeason entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teamSeason">The TeamSeason entity to modify</param>
        public void EditEntity(ProFootballEntities dbContext, TeamSeason teamSeason)
        {
            Log.Info("Updating TeamSeason entity in data store");
            dbContext.SetModified(teamSeason);
        }

        /// <summary>
        /// Finds a TeamSeason entity by its TeamName and SeasonID; throws an exception if no entity found
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the TeamName and SeasonID of the entity to be found</param>
        /// <returns>The TeamSeason entity with the matching TeamName and SeasonID</returns>
        public TeamSeason FindEntity(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            TeamSeason retVal;
            var teamName = args[0].ToString();
            int seasonID = Convert.ToInt32(args[1]);

            try
            {
                retVal = dbContext.TeamSeasons.Find(teamName, seasonID);
                if (retVal == null)
                {
                    var errMsg =
                        $"TeamSeason entity not found in ProFootballEntities\n" +
                        $"TeamName: {teamName}\nSeasonID: {seasonID}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info(
                    $"TeamSeason entity found in ProFootballEntities\nTeamName: {teamName}\nSeasonID: {seasonID}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg =
                    $"TeamSeason entity not found in ProFootballEntities\nTeamName: {teamName}\nSeasonID: {seasonID}";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Finds a TeamSeason entity by its TeamName and SeasonID; throws an exception if no entity found -
        /// asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the TeamName and SeasonID of the entity to be found</param>
        /// <returns>The TeamSeason entity with the matching TeamName and SeasonID</returns>
        public async Task<TeamSeason> FindEntityAsync(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            TeamSeason retVal;
            var teamName = args[0].ToString();
            int seasonID = Convert.ToInt32(args[1]);

            try
            {
                retVal = await dbContext.TeamSeasons.FindAsync(teamName, seasonID);
                if (retVal == null)
                {
                    var errMsg =
                        $"TeamSeason entity not found in ProFootballEntities\n" +
                        $"TeamName: {teamName}\nSeasonID: {seasonID}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info(
                    $"TeamSeason entity found in ProFootballEntities\nTeamName: {teamName}\nSeasonID: {seasonID}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg =
                    $"TeamSeason entity not found in ProFootballEntities\nTeamName: {teamName}\nSeasonID: {seasonID}";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Gets an enumerable collection of TeamSeason entities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of TeamSeason objects</returns>
        public IEnumerable<TeamSeason> GetEntities(ProFootballEntities dbContext)
        {
            Log.Info("Getting TeamSeason entities from data store");
            return dbContext.TeamSeasons;
        }

        /// <summary>
        /// Gets an enumerable collection of TeamSeason entities - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of TeamSeason objects</returns>
        public async Task<IEnumerable<TeamSeason>> GetEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Getting TeamSeason entities from data store");
            return await dbContext.TeamSeasons.ToListAsync();
        }

        /// <summary>
        /// Loads a collection of TeamSeason entities into memory
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public void LoadEntities(ProFootballEntities dbContext)
        {
            Log.Info("Loading TeamSeason entities into memory");
            dbContext.TeamSeasons.Load();
        }

        /// <summary>
        /// Loads a collection of TeamSeason entities into memory - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public async Task LoadEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Loading TeamSeason entities into memory");
            await dbContext.TeamSeasons.LoadAsync();
        }

        /// <summary>
        /// Removes a TeamSeason entity from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teamSeason">The TeamSeason entity to delete</param>
        /// <returns>The TeamSeason entity deleted from dbContext</returns>
        public TeamSeason RemoveEntity(ProFootballEntities dbContext, TeamSeason teamSeason)
        {
            Log.Info("Removing TeamSeason entity from data store");
            return dbContext.TeamSeasons.Remove(teamSeason);
        }

        /// <summary>
        /// Removes multiple TeamSeason entities from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teamSeasons">The collection of TeamSeason entities to remove</param>
        /// <returns>The TeamSeason entity collection removed from dbContext</returns>
        public IEnumerable<TeamSeason> RemoveEntities(ProFootballEntities dbContext,
            IEnumerable<TeamSeason> teamSeasons)
        {
            Log.Info("Removing TeamSeason entities from data store");
            return dbContext.TeamSeasons.RemoveRange(teamSeasons);
        }

        #endregion IRepository<TeamSeason> Implementation

        #region Helpers

        private void ValidateFindArgs(string methodName, params object[] args)
        {
            var messagePrefix = $"TeamSeasonRepository.{methodName}: ";

            if (args.Length != 2)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(String))
            {
                throw new ArgumentException(messagePrefix + "TeamName");
            }
            if (args[1].GetType() != typeof(int))
            {
                throw new ArgumentException(messagePrefix + "SeasonID");
            }
        }

        #endregion Helpers
    }
}
