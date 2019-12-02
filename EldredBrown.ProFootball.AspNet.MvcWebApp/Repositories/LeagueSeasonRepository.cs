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
    /// Repository for access to the LeagueSeason data model
    /// </summary>
    public class LeagueSeasonRepository : IRepository<LeagueSeason>
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRepository<LeagueSeason> Implementation

        /// <summary>
        /// Adds a LeagueSeason entity to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="leagueSeason">The LeagueSeason entity to add</param>
        /// <returns>The LeagueSeason entity added to dbContext</returns>
        public LeagueSeason AddEntity(ProFootballEntities dbContext, LeagueSeason leagueSeason)
        {
            Log.Info("Adding LeagueSeason entity to data store");
            return dbContext.LeagueSeasons.Add(leagueSeason);
        }

        /// <summary>
        /// Adds multiple LeagueSeason entities to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="leagueSeasons">The collection of LeagueSeason entities to add</param>
        /// <returns>The LeagueSeason entity collection added to dbContext</returns>
        public IEnumerable<LeagueSeason> AddEntities(ProFootballEntities dbContext,
            IEnumerable<LeagueSeason> leagueSeasons)
        {
            Log.Info("Adding LeagueSeason entities to data store");
            return dbContext.LeagueSeasons.AddRange(leagueSeasons);
        }

        /// <summary>
        /// Creates a LeagueSeason entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>The new LeagueSeason entity instance</returns>
        public LeagueSeason CreateEntity(ProFootballEntities dbContext)
        {
            Log.Info("Creating LeagueSeason entity");
            return dbContext.LeagueSeasons.Create();
        }

        /// <summary>
        /// Edits a LeagueSeason entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="leagueSeason">The LeagueSeason entity to modify</param>
        public void EditEntity(ProFootballEntities dbContext, LeagueSeason leagueSeason)
        {
            Log.Info("Updating LeagueSeason entity in data store");
            dbContext.SetModified(leagueSeason);
        }

        /// <summary>
        /// Finds a LeagueSeason entity by its LeagueName and SeasonID; throws an exception if no entity found
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the LeagueName and SeasonID of the entity to be found</param>
        /// <returns>The LeagueSeason entity with the matching LeagueName and SeasonID</returns>
        public LeagueSeason FindEntity(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            LeagueSeason retVal;
            var leagueName = args[0].ToString();
            int seasonID = Convert.ToInt32(args[1]);

            try
            {
                retVal = dbContext.LeagueSeasons.Find(leagueName, seasonID);
                if (retVal == null)
                {
                    var errMsg =
                        $"LeagueSeason entity not found in ProFootballEntities\n" +
                        $"LeagueName: {leagueName}\nSeasonID: {seasonID}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info(
                    $"LeagueSeason entity found in ProFootballEntities\n" +
                    $"LeagueName: {leagueName}\nSeasonID: {seasonID}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg =
                    $"LeagueSeason entity not found in ProFootballEntities\n" +
                    $"LeagueName: {leagueName}\nSeasonID: {seasonID}";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Finds a LeagueSeason entity by its LeagueName and SeasonID; throws an exception if no entity found -
        /// asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the LeagueName and SeasonID of the entity to be found</param>
        /// <returns>The LeagueSeason entity with the matching LeagueName and SeasonID</returns>
        public async Task<LeagueSeason> FindEntityAsync(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            LeagueSeason retVal;
            var leagueName = args[0].ToString();
            int seasonID = Convert.ToInt32(args[1]);

            try
            {
                retVal = await dbContext.LeagueSeasons.FindAsync(leagueName, seasonID);
                if (retVal == null)
                {
                    var errMsg =
                        $"LeagueSeason entity not found in ProFootballEntities\n" +
                        $"LeagueName: {leagueName}\nSeasonID: {seasonID}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info(
                    $"LeagueSeason entity found in ProFootballEntities\n" +
                    $"LeagueName: {leagueName}\nSeasonID: {seasonID}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg =
                    $"LeagueSeason entity not found in ProFootballEntities\n" +
                    $"LeagueName: {leagueName}\nSeasonID: {seasonID}";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Gets an enumerable collection of LeagueSeason entities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of LeagueSeason objects</returns>
        public IEnumerable<LeagueSeason> GetEntities(ProFootballEntities dbContext)
        {
            Log.Info("Getting LeagueSeason entities from data store");
            return dbContext.LeagueSeasons;
        }

        /// <summary>
        /// Gets an enumerable collection of LeagueSeason entities - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of LeagueSeason objects</returns>
        public async Task<IEnumerable<LeagueSeason>> GetEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Getting LeagueSeason entities from data store");
            return await dbContext.LeagueSeasons.ToListAsync();
        }

        public void LoadEntities(ProFootballEntities dbContext)
        {
            Log.Info("Loading LeagueSeason entities into memory");
            dbContext.LeagueSeasons.Load();
        }

        public async Task LoadEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Loading LeagueSeason entities into memory");
            await dbContext.LeagueSeasons.LoadAsync();
        }

        /// <summary>
        /// Removes a LeagueSeason entity from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="leagueSeason">The LeagueSeason entity to delete</param>
        /// <returns>The LeagueSeason entity deleted from dbContext</returns>
        public LeagueSeason RemoveEntity(ProFootballEntities dbContext, LeagueSeason leagueSeason)
        {
            Log.Info("Removing LeagueSeason entity from data store");
            return dbContext.LeagueSeasons.Remove(leagueSeason);
        }

        /// <summary>
        /// Removes multiple LeagueSeason entities from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="leagueSeasons">The collection of LeagueSeason entities to remove</param>
        /// <returns>The LeagueSeason entity collection removed from dbContext</returns>
        public IEnumerable<LeagueSeason> RemoveEntities(ProFootballEntities dbContext,
            IEnumerable<LeagueSeason> leagueSeasons)
        {
            Log.Info("Removing LeagueSeason entities from data store");
            return dbContext.LeagueSeasons.RemoveRange(leagueSeasons);
        }

        #endregion IRepository<LeagueSeason> Implementation

        #region Helpers

        private void ValidateFindArgs(string methodName, params object[] args)
        {
            var messagePrefix = $"LeagueSeasonRepository.{methodName}: ";

            if (args.Length != 2)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(String))
            {
                throw new ArgumentException(messagePrefix + "LeagueName");
            }
            if (args[1].GetType() != typeof(int))
            {
                throw new ArgumentException(messagePrefix + "SeasonID");
            }
        }

        #endregion Helpers
    }
}
