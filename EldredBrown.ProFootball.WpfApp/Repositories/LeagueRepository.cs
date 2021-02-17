using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    /// <summary>
    /// Repository for access to the League data model
    /// </summary>
    public class LeagueRepository : IRepository<League>
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        /// <summary>
        /// Initializes a new instance of the LeagueRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public LeagueRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Destroys this instance of the LeagueRepository class
        /// </summary>
        ~LeagueRepository()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        /// Adds a League entity to the data store
        /// </summary>
        /// <param name="league">The league entity to add to the data store</param>
        /// <returns>The league entity added to the data store</returns>
        public League AddEntity(League league)
        {
            try
            {
                _log.Info("Adding League entity to data store");

                return _dbContext.Leagues.Add(league);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Adds multiple League entities to the data store
        /// </summary>
        /// <param name="league">The collection of league entities to add to the data store</param>
        /// <returns>The enumerable collection of league entities added to the data store</returns>
        public IEnumerable<League> AddEntities(IEnumerable<League> leagues)
        {
            try
            {
                _log.Info("Adding League entities to data store");

                return _dbContext.Leagues.AddRange(leagues);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Creates a League entity
        /// </summary>
        /// <returns>The newly created League entity</returns>
        public League CreateEntity()
        {
            try
            {
                _log.Info("Creating League entity");

                return _dbContext.Leagues.Create();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Edits a League entity in the data store
        /// </summary>
        /// <param name="league">The League entity to be modified</param>
        public void EditEntity(League league)
        {
            try
            {
                _log.Info("Updating League entity in data store");

                _dbContext.SetModified(league);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Finds a League entity in the data store by its name
        /// </summary>
        /// <param name="args">A list containing the name of the League entity for which to search</param>
        /// <returns></returns>
        public League FindEntity(params object[] args)
        {
            // Validate args.
            var messagePrefix = $"LeagueRepository.FindEntity: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(String))
            {
                throw new ArgumentException(messagePrefix + "Name");
            }

            // Find matching entity.
            League retVal;
            var name = args[0].ToString();

            try
            {
                retVal = _dbContext.Leagues.Find(name);
                if (retVal == null)
                {
                    var errMsg = $"League entity not found in data store\nName: {name}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"League entity found in data store\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"League entity not found in data store\nName: {name}";

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
        /// Gets a collection of League entities from the data store
        /// </summary>
        /// <returns>An enumerable collection of League entities</returns>
        public IEnumerable<League> GetEntities()
        {
            try
            {
                _log.Info("Getting League entities from data store");

                return _dbContext.Leagues;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes a League entity from the data store
        /// </summary>
        /// <param name="league">The deletedLeague entity</param>
        /// <returns></returns>
        public League RemoveEntity(League league)
        {
            try
            {
                _log.Info("Removing League entity from data store");

                return _dbContext.Leagues.Remove(league);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes multiple League entities from the data store
        /// </summary>
        /// <param name="leagues">The deleted collection of League entities</param>
        /// <returns></returns>
        public IEnumerable<League> RemoveEntities(IEnumerable<League> leagues)
        {
            try
            {
                _log.Info("Removing League entities from data store");

                return _dbContext.Leagues.RemoveRange(leagues);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
