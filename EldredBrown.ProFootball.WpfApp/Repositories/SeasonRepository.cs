using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    /// <summary>
    /// Repository for access to the Season data model
    /// </summary>
    public class SeasonRepository : IRepository<Season>
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        /// <summary>
        /// Initializes a new instance of the SeasonRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public SeasonRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Destroys this instance of the SeasonRepository class
        /// </summary>
        ~SeasonRepository()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        /// Adds a Season entity to the data store
        /// </summary>
        /// <param name="season">The season entity to add to the data store</param>
        /// <returns>The season entity added to the data store</returns>
        public Season AddEntity(Season season)
        {
            try
            {
                _log.Info("Adding Season entity to data store");

                return _dbContext.Seasons.Add(season);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Adds multiple Season entities to the data store
        /// </summary>
        /// <param name="season">The collection of season entities to add to the data store</param>
        /// <returns>The enumerable collection of season entities added to the data store</returns>
        public IEnumerable<Season> AddEntities(IEnumerable<Season> seasons)
        {
            try
            {
                _log.Info("Adding Season entities to data store");

                return _dbContext.Seasons.AddRange(seasons);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Creates a Season entity
        /// </summary>
        /// <returns>The newly created Season entity</returns>
        public Season CreateEntity()
        {
            try
            {
                _log.Info("Creating Season entity");

                return _dbContext.Seasons.Create();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Edits a Season entity in the data store
        /// </summary>
        /// <param name="season">The Season entity to be modified</param>
        public void EditEntity(Season season)
        {
            try
            {
                _log.Info("Updating Season entity in data store");

                _dbContext.SetModified(season);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Finds a Season entity in the data store by its ID
        /// </summary>
        /// <param name="args">A list containing the ID of the Season entity for which to search</param>
        /// <returns></returns>
        public Season FindEntity(params object[] args)
        {
            // Validate args.
            var messagePrefix = $"SeasonRepository.FindEntity: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(int))
            {
                throw new ArgumentException(messagePrefix + "ID");
            }

            // Find matching entity.
            Season retVal;
            var id = Convert.ToInt32(args[0]);

            try
            {
                retVal = _dbContext.Seasons.Find(id);
                if (retVal == null)
                {
                    var errMsg = $"Season entity not found in data store\nID: {id}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"Season entity found in data store\nID: {id}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Season entity not found in data store\nID: {id}";

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
        /// Gets a collection of Season entities from the data store
        /// </summary>
        /// <returns>An enumerable collection of Season entities</returns>
        public IEnumerable<Season> GetEntities()
        {
            try
            {
                _log.Info("Getting Season entities from data store");

                return _dbContext.Seasons;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes a Season entity from the data store
        /// </summary>
        /// <param name="season">The deletedSeason entity</param>
        /// <returns></returns>
        public Season RemoveEntity(Season season)
        {
            try
            {
                _log.Info("Removing Season entity from data store");

                return _dbContext.Seasons.Remove(season);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes multiple Season entities from the data store
        /// </summary>
        /// <param name="seasons">The deleted collection of Season entities</param>
        /// <returns></returns>
        public IEnumerable<Season> RemoveEntities(IEnumerable<Season> seasons)
        {
            try
            {
                _log.Info("Removing Season entities from data store");

                return _dbContext.Seasons.RemoveRange(seasons);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
