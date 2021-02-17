using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    /// <summary>
    /// Repository for access to the WeekCount data model
    /// </summary>
    public class WeekCountRepository : IRepository<WeekCount>
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        /// <summary>
        /// Initializes a new instance of the WeekCountRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public WeekCountRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Destroys this instance of the WeekCountRepository class
        /// </summary>
        ~WeekCountRepository()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        /// Adds a WeekCount entity to the data store
        /// </summary>
        /// <param name="weekCount">The weekCount entity to add to the data store</param>
        /// <returns>The weekCount entity added to the data store</returns>
        public WeekCount AddEntity(WeekCount weekCount)
        {
            try
            {
                _log.Info("Adding WeekCount entity to data store");

                return _dbContext.WeekCounts.Add(weekCount);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Adds multiple WeekCount entities to the data store
        /// </summary>
        /// <param name="weekCount">The collection of weekCount entities to add to the data store</param>
        /// <returns>The enumerable collection of weekCount entities added to the data store</returns>
        public IEnumerable<WeekCount> AddEntities(IEnumerable<WeekCount> weekCounts)
        {
            try
            {
                _log.Info("Adding WeekCount entities to data store");

                return _dbContext.WeekCounts.AddRange(weekCounts);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Creates a WeekCount entity
        /// </summary>
        /// <returns>The newly created WeekCount entity</returns>
        public WeekCount CreateEntity()
        {
            try
            {
                _log.Info("Creating WeekCount entity");

                return _dbContext.WeekCounts.Create();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Edits a WeekCount entity in the data store
        /// </summary>
        /// <param name="weekCount">The WeekCount entity to be modified</param>
        public void EditEntity(WeekCount weekCount)
        {
            try
            {
                _log.Info("Updating WeekCount entity in data store");

                _dbContext.SetModified(weekCount);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Finds a WeekCount entity in the data store by its ID
        /// </summary>
        /// <param name="args">A list containing the ID of the WeekCount entity for which to search</param>
        /// <returns></returns>
        public WeekCount FindEntity(params object[] args)
        {
            // Validate args.
            var messagePrefix = $"WeekCountRepository.FindEntity: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(int))
            {
                throw new ArgumentException(messagePrefix + "ID");
            }

            // Find matching entity.
            WeekCount retVal;
            var seasonID = Convert.ToInt32(args[0]);

            try
            {
                retVal = _dbContext.WeekCounts.Find(seasonID);
                if (retVal == null)
                {
                    var errMsg = $"WeekCount entity not found in data store\nSeasonID: {seasonID}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"WeekCount entity found in data store\nSeasonID: {seasonID}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"WeekCount entity not found in data store\nSeasonID: {seasonID}";

                _log.Error($"{errMsg}\n{ex.Message}");

                throw new ObjectNotFoundException(errMsg);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }

            return retVal;
        }

        /// <summary>
        /// Gets a collection of WeekCount entities from the data store
        /// </summary>
        /// <returns>An enumerable collection of WeekCount entities</returns>
        public IEnumerable<WeekCount> GetEntities()
        {
            try
            {
                _log.Info("Getting WeekCount entities from data store");

                return _dbContext.WeekCounts;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes a WeekCount entity from the data store
        /// </summary>
        /// <param name="weekCount">The deletedWeekCount entity</param>
        /// <returns></returns>
        public WeekCount RemoveEntity(WeekCount weekCount)
        {
            try
            {
                _log.Info("Removing WeekCount entity from data store");

                return _dbContext.WeekCounts.Remove(weekCount);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes multiple WeekCount entities from the data store
        /// </summary>
        /// <param name="weekCounts">The deleted collection of WeekCount entities</param>
        /// <returns></returns>
        public IEnumerable<WeekCount> RemoveEntities(IEnumerable<WeekCount> weekCounts)
        {
            try
            {
                _log.Info("Removing WeekCount entities from data store");

                return _dbContext.WeekCounts.RemoveRange(weekCounts);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
