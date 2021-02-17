using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    /// <summary>
    /// Repository for access to the Division data model
    /// </summary>
    public class DivisionRepository : IRepository<Division>
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        /// <summary>
        /// Initializes a new instance of the DivisionRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public DivisionRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Destroys this instance of the DivisionRepository class
        /// </summary>
        ~DivisionRepository()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        /// Adds a Division entity to the data store
        /// </summary>
        /// <param name="division">The division entity to add to the data store</param>
        /// <returns>The division entity added to the data store</returns>
        public Division AddEntity(Division division)
        {
            try
            {
                _log.Info("Adding Division entity to data store");

                return _dbContext.Divisions.Add(division);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Adds multiple Division entities to the data store
        /// </summary>
        /// <param name="division">The collection of division entities to add to the data store</param>
        /// <returns>The enumerable collection of division entities added to the data store</returns>
        public IEnumerable<Division> AddEntities(IEnumerable<Division> divisions)
        {
            try
            {
                _log.Info("Adding Division entities to data store");

                return _dbContext.Divisions.AddRange(divisions);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Creates a Division entity
        /// </summary>
        /// <returns>The newly created Division entity</returns>
        public Division CreateEntity()
        {
            try
            {
                _log.Info("Creating Division entity");

                return _dbContext.Divisions.Create();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Edits a Division entity in the data store
        /// </summary>
        /// <param name="division">The Division entity to be modified</param>
        public void EditEntity(Division division)
        {
            try
            {
                _log.Info("Updating Division entity in data store");

                _dbContext.SetModified(division);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Finds a Division entity in the data store by its name
        /// </summary>
        /// <param name="args">A list containing the name of the Division entity for which to search</param>
        /// <returns></returns>
        public Division FindEntity(params object[] args)
        {
            // Validate args.
            var messagePrefix = $"DivisionRepository.FindEntity: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(String))
            {
                throw new ArgumentException(messagePrefix + "Name");
            }

            // Find matching entity.
            Division retVal;
            var name = args[0].ToString();

            try
            {
                retVal = _dbContext.Divisions.Find(name);
                if (retVal == null)
                {
                    var errMsg = $"Division entity not found in data store\nName: {name}";

                    _log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                _log.Info($"Division entity found in data store\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Division entity not found in data store\nName: {name}";

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
        /// Gets a collection of Division entities from the data store
        /// </summary>
        /// <returns>An enumerable collection of Division entities</returns>
        public IEnumerable<Division> GetEntities()
        {
            try
            {
                _log.Info("Getting Division entities from data store");

                return _dbContext.Divisions;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes a Division entity from the data store
        /// </summary>
        /// <param name="division">The deletedDivision entity</param>
        /// <returns></returns>
        public Division RemoveEntity(Division division)
        {
            try
            {
                _log.Info("Removing Division entity from data store");

                return _dbContext.Divisions.Remove(division);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes multiple Division entities from the data store
        /// </summary>
        /// <param name="divisions">The deleted collection of Division entities</param>
        /// <returns></returns>
        public IEnumerable<Division> RemoveEntities(IEnumerable<Division> divisions)
        {
            try
            {
                _log.Info("Removing Division entities from data store");

                return _dbContext.Divisions.RemoveRange(divisions);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
