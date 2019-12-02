using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    /// <summary>
    /// Repository for access to the Conference data model
    /// </summary>
    public class ConferenceRepository : IRepository<Conference>
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the ConferenceRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public ConferenceRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Destroys this instance of the ConferenceRepository class
        /// </summary>
        ~ConferenceRepository()
        {
            _dbContext.Dispose();
        }

        #endregion Constructors & Finalizers

        #region IRepository<Conference> Implementation

        /// <summary>
        /// Adds a Conference entity to the data store
        /// </summary>
        /// <param name="conference">The conference entity to add to the data store</param>
        /// <returns>The conference entity added to the data store</returns>
        public Conference AddEntity(Conference conference)
        {
            try
            {
                Log.Info("Adding Conference entity to data store");

                return _dbContext.Conferences.Add(conference);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Adds multiple Conference entities to the data store
        /// </summary>
        /// <param name="conference">The collection of conference entities to add to the data store</param>
        /// <returns>The enumerable collection of conference entities added to the data store</returns>
        public IEnumerable<Conference> AddEntities(IEnumerable<Conference> conferences)
        {
            try
            {
                Log.Info("Adding Conference entities to data store");

                return _dbContext.Conferences.AddRange(conferences);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Creates a Conference entity
        /// </summary>
        /// <returns>The newly created Conference entity</returns>
        public Conference CreateEntity()
        {
            try
            {
                Log.Info("Creating Conference entity");

                return _dbContext.Conferences.Create();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Edits a Conference entity in the data store
        /// </summary>
        /// <param name="conference">The Conference entity to be modified</param>
        public void EditEntity(Conference conference)
        {
            try
            {
                Log.Info("Updating Conference entity in data store");

                _dbContext.SetModified(conference);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Finds a Conference entity in the data store by its name
        /// </summary>
        /// <param name="args">A list containing the name of the Conference entity for which to search</param>
        /// <returns></returns>
        public Conference FindEntity(params object[] args)
        {
            // Validate args.
            var messagePrefix = $"ConferenceRepository.FindEntity: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(String))
            {
                throw new ArgumentException(messagePrefix + "Name");
            }

            // Find matching entity.
            Conference retVal;
            var name = args[0].ToString();

            try
            {
                retVal = _dbContext.Conferences.Find(name);
                if (retVal == null)
                {
                    var errMsg = $"Conference entity not found in data store\nName: {name}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"Conference entity found in data store\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Conference entity not found in data store\nName: {name}";

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
        /// Gets a collection of Conference entities from the data store
        /// </summary>
        /// <returns>An enumerable collection of Conference entities</returns>
        public IEnumerable<Conference> GetEntities()
        {
            try
            {
                Log.Info("Getting Conference entities from data store");

                return _dbContext.Conferences;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes a Conference entity from the data store
        /// </summary>
        /// <param name="conference">The deletedConference entity</param>
        /// <returns></returns>
        public Conference RemoveEntity(Conference conference)
        {
            try
            {
                Log.Info("Removing Conference entity from data store");

                return _dbContext.Conferences.Remove(conference);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes multiple Conference entities from the data store
        /// </summary>
        /// <param name="conferences">The deleted collection of Conference entities</param>
        /// <returns></returns>
        public IEnumerable<Conference> RemoveEntities(IEnumerable<Conference> conferences)
        {
            try
            {
                Log.Info("Removing Conference entities from data store");

                return _dbContext.Conferences.RemoveRange(conferences);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        #endregion IRepository<Conference> Implementation
    }
}
