using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    /// <summary>
    /// Repository for access to the LeagueSeason data model
    /// </summary>
    public class LeagueSeasonRepository : IRepository<LeagueSeason>
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the LeagueSeasonRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public LeagueSeasonRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Destroys this instance of the LeagueSeasonRepository class
        /// </summary>
        ~LeagueSeasonRepository()
        {
            _dbContext.Dispose();
        }

        #endregion Constructors & Finalizers

        #region IRepository<LeagueSeason> Implementation

        /// <summary>
        /// Adds a LeagueSeason entity to the data store
        /// </summary>
        /// <param name="leagueSeason">The leagueSeason entity to add to the data store</param>
        /// <returns>The leagueSeason entity added to the data store</returns>
        public LeagueSeason AddEntity(LeagueSeason leagueSeason)
        {
            try
            {
                Log.Info("Adding LeagueSeason entity to data store");

                return _dbContext.LeagueSeasons.Add(leagueSeason);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Adds multiple LeagueSeason entities to the data store
        /// </summary>
        /// <param name="leagueSeason">The collection of leagueSeason entities to add to the data store</param>
        /// <returns>The enumerable collection of leagueSeason entities added to the data store</returns>
        public IEnumerable<LeagueSeason> AddEntities(IEnumerable<LeagueSeason> leagueSeasons)
        {
            try
            {
                Log.Info("Adding LeagueSeason entities to data store");

                return _dbContext.LeagueSeasons.AddRange(leagueSeasons);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Creates a LeagueSeason entity
        /// </summary>
        /// <returns>The newly created LeagueSeason entity</returns>
        public LeagueSeason CreateEntity()
        {
            try
            {
                Log.Info("Creating LeagueSeason entity");

                return _dbContext.LeagueSeasons.Create();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Edits a LeagueSeason entity in the data store
        /// </summary>
        /// <param name="leagueSeason">The LeagueSeason entity to be modified</param>
        public void EditEntity(LeagueSeason leagueSeason)
        {
            try
            {
                Log.Info("Updating LeagueSeason entity in data store");

                _dbContext.SetModified(leagueSeason);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Finds a LeagueSeason entity in the data store by its LeagueName and SeasonID
        /// </summary>
        /// <param name="args">A list containing the LeagueName and SeasonID of the LeagueSeason entity for which to search</param>
        /// <returns></returns>
        public LeagueSeason FindEntity(params object[] args)
        {
            // Validate args.
            var messagePrefix = $"LeagueSeasonRepository.FindEntity: ";

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

            // Find matching entity.
            LeagueSeason retVal;
            var leagueName = args[0].ToString();
            var seasonID = Convert.ToInt32(args[1]);

            try
            {
                retVal = _dbContext.LeagueSeasons.Find(leagueName, seasonID);
                if (retVal == null)
                {
                    var errMsg = $"LeagueSeason entity not found in data store\n" +
                                 $"LeagueName: {leagueName}\nID: {seasonID}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"LeagueSeason entity found in data store\n" +
                         $"LeagueName: {leagueName}\nID: {seasonID}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"LeagueSeason entity not found in data store\n" +
                             $"LeagueName: {leagueName}\nID: {seasonID}";

                Log.Error($"{errMsg}\n{ex.Message}");

                throw new ObjectNotFoundException(errMsg);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }

            return retVal;
        }

        /// <summary>
        /// Gets a collection of LeagueSeason entities from the data store
        /// </summary>
        /// <returns>An enumerable collection of LeagueSeason entities</returns>
        public IEnumerable<LeagueSeason> GetEntities()
        {
            try
            {
                Log.Info("Getting LeagueSeason entities from data store");

                return _dbContext.LeagueSeasons;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes a LeagueSeason entity from the data store
        /// </summary>
        /// <param name="leagueSeason">The deletedLeagueSeason entity</param>
        /// <returns></returns>
        public LeagueSeason RemoveEntity(LeagueSeason leagueSeason)
        {
            try
            {
                Log.Info("Removing LeagueSeason entity from data store");

                return _dbContext.LeagueSeasons.Remove(leagueSeason);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes multiple LeagueSeason entities from the data store
        /// </summary>
        /// <param name="leagueSeasons">The deleted collection of LeagueSeason entities</param>
        /// <returns></returns>
        public IEnumerable<LeagueSeason> RemoveEntities(IEnumerable<LeagueSeason> leagueSeasons)
        {
            try
            {
                Log.Info("Removing LeagueSeason entities from data store");

                return _dbContext.LeagueSeasons.RemoveRange(leagueSeasons);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        #endregion IRepository<LeagueSeason> Implementation
    }
}
