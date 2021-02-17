using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    /// <summary>
    /// Repository for access to the Team data model
    /// </summary>
    public class TeamRepository : IRepository<Team>
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        /// <summary>
        /// Initializes a new instance of the TeamRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public TeamRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Destroys this instance of the TeamRepository class
        /// </summary>
        ~TeamRepository()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        /// Adds a Team entity to the data store
        /// </summary>
        /// <param name="team">The team entity to add to the data store</param>
        /// <returns>The team entity added to the data store</returns>
        public Team AddEntity(Team team)
        {
            try
            {
                _log.Info("Adding Team entity to data store");

                return _dbContext.Teams.Add(team);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Adds multiple Team entities to the data store
        /// </summary>
        /// <param name="team">The collection of team entities to add to the data store</param>
        /// <returns>The enumerable collection of team entities added to the data store</returns>
        public IEnumerable<Team> AddEntities(IEnumerable<Team> teams)
        {
            try
            {
                _log.Info("Adding Team entities to data store");

                return _dbContext.Teams.AddRange(teams);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Creates a Team entity
        /// </summary>
        /// <returns>The newly created Team entity</returns>
        public Team CreateEntity()
        {
            try
            {
                _log.Info("Creating Team entity");

                return _dbContext.Teams.Create();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Edits a Team entity in the data store
        /// </summary>
        /// <param name="team">The Team entity to be modified</param>
        public void EditEntity(Team team)
        {
            try
            {
                _log.Info("Updating Team entity in data store");

                _dbContext.SetModified(team);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Finds a Team entity in the data store by its name
        /// </summary>
        /// <param name="args">A list containing the name of the Team entity for which to search</param>
        /// <returns></returns>
        public Team FindEntity(params object[] args)
        {
            // Validate args.
            var messagePrefix = $"TeamRepository.FindEntity: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(String))
            {
                throw new ArgumentException(messagePrefix + "Name");
            }

            // Find matching entity.
            Team retVal;
            var name = args[0].ToString();

            try
            {
                retVal = _dbContext.Teams.Find(name);
                if (retVal == null)
                {
                    var errMsg = $"Team entity not found in data store\nName: {name}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"Team entity found in data store\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Team entity not found in data store\nName: {name}";

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
        /// Gets a collection of Team entities from the data store
        /// </summary>
        /// <returns>An enumerable collection of Team entities</returns>
        public IEnumerable<Team> GetEntities()
        {
            try
            {
                _log.Info("Getting Team entities from data store");

                return _dbContext.Teams;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes a Team entity from the data store
        /// </summary>
        /// <param name="team">The deletedTeam entity</param>
        /// <returns></returns>
        public Team RemoveEntity(Team team)
        {
            try
            {
                _log.Info("Removing Team entity from data store");

                return _dbContext.Teams.Remove(team);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes multiple Team entities from the data store
        /// </summary>
        /// <param name="teams">The deleted collection of Team entities</param>
        /// <returns></returns>
        public IEnumerable<Team> RemoveEntities(IEnumerable<Team> teams)
        {
            try
            {
                _log.Info("Removing Team entities from data store");

                return _dbContext.Teams.RemoveRange(teams);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
