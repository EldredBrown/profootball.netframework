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
    /// Repository for access to the League data model
    /// </summary>
    public class LeagueRepository : IRepository<League>
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRepository<League> Implementation

        /// <summary>
        /// Adds a League entity to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="league">The League entity to add</param>
        /// <returns>The League entity added to dbContext</returns>
        public League AddEntity(ProFootballEntities dbContext, League league)
        {
            Log.Info("Adding League entity to data store");
            return dbContext.Leagues.Add(league);
        }

        /// <summary>
        /// Adds multiple League entities to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="leagues">The collection of League entities to add</param>
        /// <returns>The League entity collection added to dbContext</returns>
        public IEnumerable<League> AddEntities(ProFootballEntities dbContext, IEnumerable<League> leagues)
        {
            Log.Info("Adding League entities to data store");
            return dbContext.Leagues.AddRange(leagues);
        }

        /// <summary>
        /// Creates a League entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>The new League entity instance</returns>
        public League CreateEntity(ProFootballEntities dbContext)
        {
            Log.Info("Creating League entity");
            return dbContext.Leagues.Create();
        }

        /// <summary>
        /// Edits a League entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="league">The League entity to modify</param>
        public void EditEntity(ProFootballEntities dbContext, League league)
        {
            Log.Info("Updating League entity in data store");
            dbContext.SetModified(league);
        }

        /// <summary>
        /// Finds a League entity by its name; throws an exception if no entity found
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the name of the entity to be found</param>
        /// <returns>The League entity with the matching name</returns>
        public League FindEntity(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            League retVal;
            var name = args[0].ToString();

            try
            {
                retVal = dbContext.Leagues.Find(name);
                if (retVal == null)
                {
                    var errMsg = $"League entity not found in ProFootballEntities\nName: {name}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"League entity found in ProFootballEntities\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"League entity not found in ProFootballEntities\nName: {name}\n";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Finds a League entity by its name; throws an exception if no entity found - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the name of the entity to be found</param>
        /// <returns>The League entity with the matching name</returns>
        public async Task<League> FindEntityAsync(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntityAsync", args);

            League retVal;
            var name = args[0].ToString();

            try
            {
                retVal = await dbContext.Leagues.FindAsync(name);
                if (retVal == null)
                {
                    var errMsg = $"League entity not found in ProFootballEntities\nName: {name}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"League entity found in ProFootballEntities\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"League entity not found in ProFootballEntities\nName: {name}\n";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Gets an enumerable collection of League entities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of League objects</returns>
        public IEnumerable<League> GetEntities(ProFootballEntities dbContext)
        {
            Log.Info("Getting League entities from data store");
            return dbContext.Leagues;
        }

        /// <summary>
        /// Gets an enumerable collection of League entities - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of League objects</returns>
        public async Task<IEnumerable<League>> GetEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Getting League entities from data store");
            return await dbContext.Leagues.ToListAsync();
        }

        /// <summary>
        /// Loads a collection of League entities into memory
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public void LoadEntities(ProFootballEntities dbContext)
        {
            Log.Info("Loading League entities into memory");
            dbContext.Leagues.Load();
        }

        /// <summary>
        /// Loads a collection of League entities into memory - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public async Task LoadEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Loading League entities into memory");
            await dbContext.Leagues.LoadAsync();
        }

        /// <summary>
        /// Removes a League entity from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="league">The League entity to delete</param>
        /// <returns>The League entity deleted from dbContext</returns>
        public League RemoveEntity(ProFootballEntities dbContext, League league)
        {
            Log.Info("Removing League entity from data store");
            return dbContext.Leagues.Remove(league);
        }

        /// <summary>
        /// Removes multiple League entities from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="leagues">The collection of League entities to remove</param>
        /// <returns>The League entity collection removed from dbContext</returns>
        public IEnumerable<League> RemoveEntities(ProFootballEntities dbContext, IEnumerable<League> leagues)
        {
            Log.Info("Removing League entities from data store");
            return dbContext.Leagues.RemoveRange(leagues);
        }

        #endregion IRepository<League> Implementation

        #region Helpers

        private void ValidateFindArgs(string methodName, params object[] args)
        {
            var messagePrefix = $"LeagueRepository.{methodName}: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(String))
            {
                throw new ArgumentException(messagePrefix + "Name");
            }
        }

        #endregion Helpers
    }
}
