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
    /// Repository for access to the Division data model
    /// </summary>
    public class DivisionRepository : IRepository<Division>
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Adds a Division entity to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="division">The Division entity to add</param>
        /// <returns>The Division entity added to dbContext</returns>
        public Division AddEntity(ProFootballEntities dbContext, Division division)
        {
            _log.Info("Adding Division entity to data store");
            return dbContext.Divisions.Add(division);
        }

        /// <summary>
        /// Adds multiple Division entities to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="divisions">The collection of Division entities to add</param>
        /// <returns>The Division entity collection added to dbContext</returns>
        public IEnumerable<Division> AddEntities(ProFootballEntities dbContext, IEnumerable<Division> divisions)
        {
            _log.Info("Adding Division entities to data store");
            return dbContext.Divisions.AddRange(divisions);
        }

        /// <summary>
        /// Creates a Division entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>The new Division entity instance</returns>
        public Division CreateEntity(ProFootballEntities dbContext)
        {
            _log.Info("Creating Division entity");
            return dbContext.Divisions.Create();
        }

        /// <summary>
        /// Edits a Division entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="division">The Division entity to modify</param>
        public void EditEntity(ProFootballEntities dbContext, Division division)
        {
            _log.Info("Updating Division entity in data store");
            dbContext.SetModified(division);
        }

        /// <summary>
        /// Finds a Division entity by its name; throws an exception if no entity found
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the name of the entity to be found</param>
        /// <returns>The Division entity with the matching name</returns>
        public Division FindEntity(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            Division retVal;
            var name = args[0].ToString();

            try
            {
                retVal = dbContext.Divisions.Find(name);
                if (retVal == null)
                {
                    var errMsg = $"Division entity not found in ProFootballEntities\nName: {name}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"Division entity found in ProFootballEntities\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Division entity not found in ProFootballEntities\nName: {name}\n";

                _log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Finds a Division entity by its name; throws an exception if no entity found - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the name of the entity to be found</param>
        /// <returns>The Division entity with the matching name</returns>
        public async Task<Division> FindEntityAsync(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntityAsync", args);

            Division retVal;
            var name = args[0].ToString();

            try
            {
                retVal = await dbContext.Divisions.FindAsync(name);
                if (retVal == null)
                {
                    var errMsg = $"Division entity not found in ProFootballEntities\nName: {name}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"Division entity found in ProFootballEntities\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Division entity not found in ProFootballEntities\nName: {name}\n";

                _log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        private void ValidateFindArgs(string methodName, params object[] args)
        {
            var messagePrefix = $"DivisionRepository.{methodName}: ";

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
        /// Gets an enumerable collection of Division entities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Division objects</returns>
        public IEnumerable<Division> GetEntities(ProFootballEntities dbContext)
        {
            _log.Info("Getting Division entities from data store");
            return dbContext.Divisions;
        }

        /// <summary>
        /// Gets an enumerable collection of Division entities - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Division objects</returns>
        public async Task<IEnumerable<Division>> GetEntitiesAsync(ProFootballEntities dbContext)
        {
            _log.Info("Getting Division entities from data store");
            return await dbContext.Divisions.ToListAsync();
        }

        /// <summary>
        /// Loads a collection of Division entities into memory
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public void LoadEntities(ProFootballEntities dbContext)
        {
            _log.Info("Loading Division entities into memory");
            dbContext.Divisions.Load();
        }

        /// <summary>
        /// Loads a collection of Division entities into memory - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public async Task LoadEntitiesAsync(ProFootballEntities dbContext)
        {
            _log.Info("Loading Division entities into memory");
            await dbContext.Divisions.LoadAsync();
        }

        /// <summary>
        /// Removes a Division entity from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="division">The Division entity to delete</param>
        /// <returns>The Division entity deleted from dbContext</returns>
        public Division RemoveEntity(ProFootballEntities dbContext, Division division)
        {
            _log.Info("Removing Division entity from data store");
            return dbContext.Divisions.Remove(division);
        }

        /// <summary>
        /// Removes multiple Division entities from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="divisions">The collection of Division entities to remove</param>
        /// <returns>The Division entity collection removed from dbContext</returns>
        public IEnumerable<Division> RemoveEntities(ProFootballEntities dbContext, IEnumerable<Division> divisions)
        {
            _log.Info("Removing Division entities from data store");
            return dbContext.Divisions.RemoveRange(divisions);
        }
    }
}
