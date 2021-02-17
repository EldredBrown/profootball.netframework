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
    /// Repository for access to the Conference data model
    /// </summary>
    public class ConferenceRepository : IRepository<Conference>
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Adds a Conference entity to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="conference">The Conference entity to add</param>
        /// <returns>The Conference entity added to dbContext</returns>
        public Conference AddEntity(ProFootballEntities dbContext, Conference conference)
        {
            _log.Info("Adding Conference entity to data store");
            return dbContext.Conferences.Add(conference);
        }

        /// <summary>
        /// Adds multiple Conference entities to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="conferences">The collection of Conference entities to add</param>
        /// <returns>The Conference entity collection added to dbContext</returns>
        public IEnumerable<Conference> AddEntities(ProFootballEntities dbContext, IEnumerable<Conference> conferences)
        {
            _log.Info("Adding Conference entities to data store");
            return dbContext.Conferences.AddRange(conferences);
        }

        /// <summary>
        /// Creates a Conference entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>The new Conference entity instance</returns>
        public Conference CreateEntity(ProFootballEntities dbContext)
        {
            _log.Info("Creating Conference entity");
            return dbContext.Conferences.Create();
        }

        /// <summary>
        /// Edits a Conference entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="conference">The Conference entity to modify</param>
        public void EditEntity(ProFootballEntities dbContext, Conference conference)
        {
            _log.Info("Updating Conference entity in data store");
            dbContext.SetModified(conference);
        }

        /// <summary>
        /// Finds a Conference entity by its name; throws an exception if no entity found
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the name of the entity to be found</param>
        /// <returns>The Conference entity with the matching name</returns>
        public Conference FindEntity(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            Conference retVal;
            var name = args[0].ToString();

            try
            {
                retVal = dbContext.Conferences.Find(name);
                if (retVal == null)
                {
                    var errMsg = $"Conference entity not found in ProFootballEntities\nName: {name}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"Conference entity found in ProFootballEntities\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Conference entity not found in ProFootballEntities\nName: {name}\n";

                _log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Finds a Conference entity by its name; throws an exception if no entity found - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the name of the entity to be found</param>
        /// <returns>The Conference entity with the matching name</returns>
        public async Task<Conference> FindEntityAsync(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntityAsync", args);

            Conference retVal;
            var name = args[0].ToString();

            try
            {
                retVal = await dbContext.Conferences.FindAsync(name);
                if (retVal == null)
                {
                    var errMsg = $"Conference entity not found in ProFootballEntities\nName: {name}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"Conference entity found in ProFootballEntities\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Conference entity not found in ProFootballEntities\nName: {name}\n";

                _log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        private void ValidateFindArgs(string methodName, params object[] args)
        {
            var messagePrefix = $"ConferenceRepository.{methodName}: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(String))
            {
                throw new ArgumentException(messagePrefix + "Name");
            }
        }

        /// <summary>
        /// Gets an enumerable collection of Conference entities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Conference objects</returns>
        public IEnumerable<Conference> GetEntities(ProFootballEntities dbContext)
        {
            _log.Info("Getting Conference entities from data store");
            return dbContext.Conferences;
        }

        /// <summary>
        /// Gets an enumerable collection of Conference entities - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Conference objects</returns>
        public async Task<IEnumerable<Conference>> GetEntitiesAsync(ProFootballEntities dbContext)
        {
            _log.Info("Getting Conference entities from data store");
            return await dbContext.Conferences.ToListAsync();
        }

        /// <summary>
        /// Loads a collection of Conference entities into memory
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public void LoadEntities(ProFootballEntities dbContext)
        {
            _log.Info("Loading Conference entities into memory");
            dbContext.Conferences.Load();
        }

        /// <summary>
        /// Loads a collection of Conference entities into memory - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public async Task LoadEntitiesAsync(ProFootballEntities dbContext)
        {
            _log.Info("Loading Conference entities into memory");
            await dbContext.Conferences.LoadAsync();
        }

        /// <summary>
        /// Removes a Conference entity from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="conference">The Conference entity to delete</param>
        /// <returns>The Conference entity deleted from dbContext</returns>
        public Conference RemoveEntity(ProFootballEntities dbContext, Conference conference)
        {
            _log.Info("Removing Conference entity from data store");
            return dbContext.Conferences.Remove(conference);
        }

        /// <summary>
        /// Removes multiple Conference entities from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="conferences">The collection of Conference entities to remove</param>
        /// <returns>The Conference entity collection removed from dbContext</returns>
        public IEnumerable<Conference> RemoveEntities(ProFootballEntities dbContext,
            IEnumerable<Conference> conferences)
        {
            _log.Info("Removing Conference entities from data store");
            return dbContext.Conferences.RemoveRange(conferences);
        }
    }
}
