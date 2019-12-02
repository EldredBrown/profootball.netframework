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
    /// Repository for access to the Team data model
    /// </summary>
    public class TeamRepository : IRepository<Team>
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRepository<Team> Implementation

        /// <summary>
        /// Adds a Team entity to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="team">The Team entity to add</param>
        /// <returns>The Team entity added to dbContext</returns>
        public Team AddEntity(ProFootballEntities dbContext, Team team)
        {
            Log.Info("Adding Team entity to data store");
            return dbContext.Teams.Add(team);
        }

        /// <summary>
        /// Adds multiple Team entities to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teams">The collection of Team entities to add</param>
        /// <returns>The Team entity collection added to dbContext</returns>
        public IEnumerable<Team> AddEntities(ProFootballEntities dbContext, IEnumerable<Team> teams)
        {
            Log.Info("Adding Team entities to data store");
            return dbContext.Teams.AddRange(teams);
        }

        /// <summary>
        /// Creates a Team entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>The new Team entity instance</returns>
        public Team CreateEntity(ProFootballEntities dbContext)
        {
            Log.Info("Creating Team entity");
            return dbContext.Teams.Create();
        }

        /// <summary>
        /// Edits a Team entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="team">The Team entity to modify</param>
        public void EditEntity(ProFootballEntities dbContext, Team team)
        {
            Log.Info("Updating Team entity in data store");
            dbContext.SetModified(team);
        }

        /// <summary>
        /// Finds a Team entity by its name; throws an exception if no entity found
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the name of the entity to be found</param>
        /// <returns>The Team entity with the matching name</returns>
        public Team FindEntity(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            Team retVal;
            var name = args[0].ToString();

            try
            {
                retVal = dbContext.Teams.Find(name);
                if (retVal == null)
                {
                    var errMsg = $"Team entity not found in ProFootballEntities\nName: {name}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"Team entity found in ProFootballEntities\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Team entity not found in ProFootballEntities\nName: {name}\n";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Finds a Team entity by its name; throws an exception if no entity found - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the name of the entity to be found</param>
        /// <returns>The Team entity with the matching name</returns>
        public async Task<Team> FindEntityAsync(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntityAsync", args);

            Team retVal;
            var name = args[0].ToString();

            try
            {
                retVal = await dbContext.Teams.FindAsync(name);
                if (retVal == null)
                {
                    var errMsg = $"Team entity not found in ProFootballEntities\nName: {name}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"Team entity found in ProFootballEntities\nName: {name}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Team entity not found in ProFootballEntities\nName: {name}\n";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Gets an enumerable collection of Team entities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Team objects</returns>
        public IEnumerable<Team> GetEntities(ProFootballEntities dbContext)
        {
            Log.Info("Getting Team entities from data store");
            return dbContext.Teams;
        }

        /// <summary>
        /// Gets an enumerable collection of Team entities - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Team objects</returns>
        public async Task<IEnumerable<Team>> GetEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Getting Team entities from data store");
            return await dbContext.Teams.ToListAsync();
        }

        /// <summary>
        /// Loads a collection of Team entities into memory
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public void LoadEntities(ProFootballEntities dbContext)
        {
            Log.Info("Loading Team entities into memory");
            dbContext.Teams.Load();
        }

        /// <summary>
        /// Loads a collection of Team entities into memory - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public async Task LoadEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Loading Team entities into memory");
            await dbContext.Teams.LoadAsync();
        }

        /// <summary>
        /// Removes a Team entity from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="team">The Team entity to delete</param>
        /// <returns>The Team entity deleted from dbContext</returns>
        public Team RemoveEntity(ProFootballEntities dbContext, Team team)
        {
            Log.Info("Removing Team entity from data store");
            return dbContext.Teams.Remove(team);
        }

        /// <summary>
        /// Removes multiple Team entities from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="teams">The collection of Team entities to remove</param>
        /// <returns>The Team entity collection removed from dbContext</returns>
        public IEnumerable<Team> RemoveEntities(ProFootballEntities dbContext, IEnumerable<Team> teams)
        {
            Log.Info("Removing Team entities from data store");
            return dbContext.Teams.RemoveRange(teams);
        }

        #endregion IRepository<Team> Implementation

        #region Helpers

        private void ValidateFindArgs(string methodName, params object[] args)
        {
            var messagePrefix = $"TeamRepository.{methodName}: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(String))
            {
                throw new ArgumentException(messagePrefix + "Name");
            }
        }

        #endregion Helpers
    }
}
