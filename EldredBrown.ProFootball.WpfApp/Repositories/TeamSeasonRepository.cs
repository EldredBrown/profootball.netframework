using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    /// <summary>
    /// Repository for access to the TeamSeason data model
    /// </summary>
    public class TeamSeasonRepository : IRepository<TeamSeason>
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        /// <summary>
        /// Initializes a new instance of the TeamSeasonRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public TeamSeasonRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Destroys this instance of the TeamSeasonRepository class
        /// </summary>
        ~TeamSeasonRepository()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        /// Adds a TeamSeason entity to the data store
        /// </summary>
        /// <param name="teamSeason">The teamSeason entity to add to the data store</param>
        /// <returns>The teamSeason entity added to the data store</returns>
        public TeamSeason AddEntity(TeamSeason teamSeason)
        {
            try
            {
                _log.Info("Adding TeamSeason entity to data store");

                return _dbContext.TeamSeasons.Add(teamSeason);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Adds multiple TeamSeason entities to the data store
        /// </summary>
        /// <param name="teamSeason">The collection of teamSeason entities to add to the data store</param>
        /// <returns>The enumerable collection of teamSeason entities added to the data store</returns>
        public IEnumerable<TeamSeason> AddEntities(IEnumerable<TeamSeason> teamSeasons)
        {
            try
            {
                _log.Info("Adding TeamSeason entities to data store");

                return _dbContext.TeamSeasons.AddRange(teamSeasons);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Creates a TeamSeason entity
        /// </summary>
        /// <returns>The newly created TeamSeason entity</returns>
        public TeamSeason CreateEntity()
        {
            try
            {
                _log.Info("Creating TeamSeason entity");

                return _dbContext.TeamSeasons.Create();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Edits a TeamSeason entity in the data store
        /// </summary>
        /// <param name="teamSeason">The TeamSeason entity to be modified</param>
        public void EditEntity(TeamSeason teamSeason)
        {
            try
            {
                _log.Info("Updating TeamSeason entity in data store");

                _dbContext.SetModified(teamSeason);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Finds a TeamSeason entity in the data store by its TeamName and SeasonID
        /// </summary>
        /// <param name="args">A list containing the TeamName and SeasonID of the TeamSeason entity for which to search</param>
        /// <returns></returns>
        public TeamSeason FindEntity(params object[] args)
        {
            // Validate args.
            var messagePrefix = $"TeamSeasonRepository.FindEntity: ";

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

            // Find matching entity.
            TeamSeason retVal;
            var teamName = args[0].ToString();
            var seasonID = Convert.ToInt32(args[1]);

            try
            {
                retVal = _dbContext.TeamSeasons.Find(teamName, seasonID);
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"TeamSeason entity not found in data store\n" +
                             $"TeamName: {teamName}\nID: {seasonID}";

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
        /// Gets a collection of TeamSeason entities from the data store
        /// </summary>
        /// <returns>An enumerable collection of TeamSeason entities</returns>
        public IEnumerable<TeamSeason> GetEntities()
        {
            try
            {
                _log.Info("Getting TeamSeason entities from data store");

                return _dbContext.TeamSeasons;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes a TeamSeason entity from the data store
        /// </summary>
        /// <param name="teamSeason">The deletedTeamSeason entity</param>
        /// <returns></returns>
        public TeamSeason RemoveEntity(TeamSeason teamSeason)
        {
            try
            {
                _log.Info("Removing TeamSeason entity from data store");

                return _dbContext.TeamSeasons.Remove(teamSeason);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes multiple TeamSeason entities from the data store
        /// </summary>
        /// <param name="teamSeasons">The deleted collection of TeamSeason entities</param>
        /// <returns></returns>
        public IEnumerable<TeamSeason> RemoveEntities(IEnumerable<TeamSeason> teamSeasons)
        {
            try
            {
                _log.Info("Removing TeamSeason entities from data store");

                return _dbContext.TeamSeasons.RemoveRange(teamSeasons);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
