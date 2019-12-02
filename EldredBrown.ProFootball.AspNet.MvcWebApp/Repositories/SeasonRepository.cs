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
    /// Repository for access to the Season data model
    /// </summary>
    public class SeasonRepository : IRepository<Season>
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRepository<Season> Implementation

        /// <summary>
        /// Adds a Season entity to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="season">The Season entity to add</param>
        /// <returns>The Season entity added to dbContext</returns>
        public Season AddEntity(ProFootballEntities dbContext, Season season)
        {
            Log.Info("Adding Season entity to data store");
            return dbContext.Seasons.Add(season);
        }

        /// <summary>
        /// Adds multiple Season entities to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="seasons">The collection of Season entities to add</param>
        /// <returns>The Season entity collection added to dbContext</returns>
        public IEnumerable<Season> AddEntities(ProFootballEntities dbContext, IEnumerable<Season> seasons)
        {
            Log.Info("Adding Season entities to data store");
            return dbContext.Seasons.AddRange(seasons);
        }

        /// <summary>
        /// Creates a Season entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>The new Season entity instance</returns>
        public Season CreateEntity(ProFootballEntities dbContext)
        {
            Log.Info("Creating Season entity");
            return dbContext.Seasons.Create();
        }

        /// <summary>
        /// Edits a Season entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="season">The Season entity to modify</param>
        public void EditEntity(ProFootballEntities dbContext, Season season)
        {
            Log.Info("Updating Season entity in data store");
            dbContext.SetModified(season);
        }

        /// <summary>
        /// Finds a Season entity by its ID; throws an exception if no entity found
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the ID of the entity to be found</param>
        /// <returns>The Season entity with the matching ID</returns>
        public Season FindEntity(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            Season retVal;
            var id = Convert.ToInt32(args[0]);

            try
            {
                retVal = dbContext.Seasons.Find(id);
                if (retVal == null)
                {
                    var errMsg = $"Season entity not found in ProFootballEntities\nID: {id}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"Season entity found in ProFootballEntities\nID: {id}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Season entity not found in ProFootballEntities\nID: {id}\n";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Finds a Season entity by its ID; throws an exception if no entity found - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the ID of the entity to be found</param>
        /// <returns>The Season entity with the matching ID</returns>
        public async Task<Season> FindEntityAsync(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntityAsync", args);

            Season retVal;
            var id = Convert.ToInt32(args[0]);

            try
            {
                retVal = await dbContext.Seasons.FindAsync(id);
                if (retVal == null)
                {
                    var errMsg = $"Season entity not found in ProFootballEntities\nID: {id}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"Season entity found in ProFootballEntities\nID: {id}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Season entity not found in ProFootballEntities\nID: {id}\n";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Gets an enumerable collection of Season entities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Season objects</returns>
        public IEnumerable<Season> GetEntities(ProFootballEntities dbContext)
        {
            Log.Info("Getting Season entities from data store");
            return dbContext.Seasons;
        }

        /// <summary>
        /// Gets an enumerable collection of Season entities - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Season objects</returns>
        public async Task<IEnumerable<Season>> GetEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Getting Season entities from data store");
            return await dbContext.Seasons.ToListAsync();
        }

        /// <summary>
        /// Loads a collection of Season entities into memory
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public void LoadEntities(ProFootballEntities dbContext)
        {
            Log.Info("Loading Season entities into memory");
            dbContext.Seasons.Load();
        }

        /// <summary>
        /// Loads a collection of Season entities into memory - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public async Task LoadEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Loading Season entities into memory");
            await dbContext.Seasons.LoadAsync();
        }

        /// <summary>
        /// Removes a Season entity from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="season">The Season entity to delete</param>
        /// <returns>The Season entity deleted from dbContext</returns>
        public Season RemoveEntity(ProFootballEntities dbContext, Season season)
        {
            Log.Info("Removing Season entity from data store");
            return dbContext.Seasons.Remove(season);
        }

        /// <summary>
        /// Removes multiple Season entities from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="seasons">The collection of Season entities to remove</param>
        /// <returns>The Season entity collection removed from dbContext</returns>
        public IEnumerable<Season> RemoveEntities(ProFootballEntities dbContext, IEnumerable<Season> seasons)
        {
            Log.Info("Removing Season entities from data store");
            return dbContext.Seasons.RemoveRange(seasons);
        }

        #endregion IRepository<Season> Implementation

        #region Helpers

        private void ValidateFindArgs(string methodName, params object[] args)
        {
            var messagePrefix = $"SeasonRepository.{methodName}: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(int))
            {
                throw new ArgumentException(messagePrefix + "ID");
            }
        }

        #endregion Helpers
    }
}
