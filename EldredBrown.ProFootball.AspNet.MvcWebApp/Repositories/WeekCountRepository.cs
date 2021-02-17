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
    /// Repository for access to the WeekCount data model
    /// </summary>
    public class WeekCountRepository : IRepository<WeekCount>
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Adds a WeekCount entity to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="weekCount">The WeekCount entity to add</param>
        /// <returns>The WeekCount entity added to dbContext</returns>
        public WeekCount AddEntity(ProFootballEntities dbContext, WeekCount weekCount)
        {
            _log.Info("Adding WeekCount entity to data store");
            return dbContext.WeekCounts.Add(weekCount);
        }

        /// <summary>
        /// Adds multiple WeekCount entities to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="weekCounts">The collection of WeekCount entities to add</param>
        /// <returns>The WeekCount entity collection added to dbContext</returns>
        public IEnumerable<WeekCount> AddEntities(ProFootballEntities dbContext, IEnumerable<WeekCount> weekCounts)
        {
            _log.Info("Adding WeekCount entities to data store");
            return dbContext.WeekCounts.AddRange(weekCounts);
        }

        /// <summary>
        /// Creates a WeekCount entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>The new WeekCount entity instance</returns>
        public WeekCount CreateEntity(ProFootballEntities dbContext)
        {
            _log.Info("Creating WeekCount entity");
            return dbContext.WeekCounts.Create();
        }

        /// <summary>
        /// Edits a WeekCount entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="weekCount">The WeekCount entity to modify</param>
        public void EditEntity(ProFootballEntities dbContext, WeekCount weekCount)
        {
            _log.Info("Updating WeekCount entity in data store");
            dbContext.SetModified(weekCount);
        }

        /// <summary>
        /// Finds a WeekCount entity by its SeasonID; throws an exception if no entity found
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the SeasonID of the WeekCount entity to find</param>
        /// <returns>The WeekCount entity with the matching SeasonID</returns>
        public WeekCount FindEntity(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            WeekCount retVal;
            var seasonID = Convert.ToInt32(args[0]);

            try
            {
                retVal = dbContext.WeekCounts.Find(seasonID);
                if (retVal == null)
                {
                    var errMsg = $"WeekCount entity not found in ProFootballEntities\nSeasonID: {seasonID}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"WeekCount entity found in ProFootballEntities\nSeasonID: {seasonID}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"WeekCount entity not found in ProFootballEntities\nSeasonID: {seasonID}";

                _log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Finds a WeekCount entity by its SeasonID; throws an exception if no entity found - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the SeasonID of the WeekCount entity to find</param>
        /// <returns>The WeekCount entity with the matching SeasonID</returns>
        public async Task<WeekCount> FindEntityAsync(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntityAsync", args);

            WeekCount retVal;
            var seasonID = Convert.ToInt32(args[0]);

            try
            {
                retVal = await dbContext.WeekCounts.FindAsync(seasonID);
                if (retVal == null)
                {
                    var errMsg = $"WeekCount entity not found in ProFootballEntities\nSeasonID: {seasonID}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"WeekCount entity found in ProFootballEntities\nSeasonID: {seasonID}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"WeekCount entity not found in ProFootballEntities\nSeasonID: {seasonID}";

                _log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        private void ValidateFindArgs(string methodName, params object[] args)
        {
            var messagePrefix = $"WeekCountRepository.{methodName}: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(int))
            {
                throw new ArgumentException(messagePrefix + "ID");
            }
        }

        /// <summary>
        /// Gets an enumerable collection of WeekCount entities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of WeekCount objects</returns>
        public IEnumerable<WeekCount> GetEntities(ProFootballEntities dbContext)
        {
            _log.Info("Getting WeekCount entities from data store");
            return dbContext.WeekCounts;
        }

        /// <summary>
        /// Gets an enumerable collection of WeekCount entities - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of WeekCount objects</returns>
        public async Task<IEnumerable<WeekCount>> GetEntitiesAsync(ProFootballEntities dbContext)
        {
            _log.Info("Getting WeekCount entities from data store");
            return await dbContext.WeekCounts.ToListAsync();
        }

        public void LoadEntities(ProFootballEntities dbContext)
        {
            throw new NotImplementedException();
        }

        public Task LoadEntitiesAsync(ProFootballEntities dbContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a WeekCount entity from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="weekCount">The WeekCount entity to delete</param>
        /// <returns>The WeekCount entity deleted from dbContext</returns>
        public WeekCount RemoveEntity(ProFootballEntities dbContext, WeekCount weekCount)
        {
            _log.Info("Removing WeekCount entity from data store");
            return dbContext.WeekCounts.Remove(weekCount);
        }

        /// <summary>
        /// Removes multiple WeekCount entities from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="weekCounts">The collection of WeekCount entities to remove</param>
        /// <returns>The WeekCount entity collection removed from dbContext</returns>
        public IEnumerable<WeekCount> RemoveEntities(ProFootballEntities dbContext, IEnumerable<WeekCount> weekCounts)
        {
            _log.Info("Removing WeekCount entities from data store");
            return dbContext.WeekCounts.RemoveRange(weekCounts);
        }
    }
}
