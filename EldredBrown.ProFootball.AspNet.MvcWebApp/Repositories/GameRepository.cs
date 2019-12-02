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
    /// Repository for access to the Game data model
    /// </summary>
    public class GameRepository : IRepository<Game>
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRepository<Game> Implementation

        /// <summary>
        /// Adds a Game entity to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="game">The Game entity to add</param>
        /// <returns>The Game entity added to dbContext</returns>
        public Game AddEntity(ProFootballEntities dbContext, Game game)
        {
            Log.Info("Adding Game entity to data store");
            return dbContext.Games.Add(game);
        }

        /// <summary>
        /// Adds multiple Game entities to ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="games">The collection of Game entities to add</param>
        /// <returns>The Game entity collection added to dbContext</returns>
        public IEnumerable<Game> AddEntities(ProFootballEntities dbContext, IEnumerable<Game> games)
        {
            Log.Info("Adding Game entities to data store");
            return dbContext.Games.AddRange(games);
        }

        /// <summary>
        /// Creates a Game entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>The new Game entity instance</returns>
        public Game CreateEntity(ProFootballEntities dbContext)
        {
            Log.Info("Creating Game entity");
            return dbContext.Games.Create();
        }

        /// <summary>
        /// Edits a Game entity
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="game">The Game entity to modify</param>
        public void EditEntity(ProFootballEntities dbContext, Game game)
        {
            Log.Info("Updating Game entity in data store");
            dbContext.SetModified(game);
        }

        /// <summary>
        /// Finds a Game entity by its ID; throws an exception if no entity found
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the ID of the entity to be found</param>
        /// <returns>The Game entity with the matching ID</returns>
        public Game FindEntity(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntity", args);

            Game retVal;
            var id = Convert.ToInt32(args[0]);

            try
            {
                retVal = dbContext.Games.Find(id);
                if (retVal == null)
                {
                    var errMsg = $"Game entity not found in ProFootballEntities\nID: {id}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"Game entity found in ProFootballEntities\nID: {id}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Game entity not found in ProFootballEntities\nID: {id}\n";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Finds a Game entity by its ID; throws an exception if no entity found - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="args">List containing the ID of the entity to be found</param>
        /// <returns>The Game entity with the matching ID</returns>
        public async Task<Game> FindEntityAsync(ProFootballEntities dbContext, params object[] args)
        {
            ValidateFindArgs("FindEntityAsync", args);

            Game retVal;
            var id = Convert.ToInt32(args[0]);

            try
            {
                retVal = await dbContext.Games.FindAsync(id);
                if (retVal == null)
                {
                    var errMsg = $"Game entity not found in ProFootballEntities\nID: {id}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"Game entity found in ProFootballEntities\nID: {id}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Game entity not found in ProFootballEntities\nID: {id}\n";

                Log.Error(errMsg + "\n" + ex.Message);

                throw new ObjectNotFoundException(errMsg);
            }

            return retVal;
        }

        /// <summary>
        /// Gets an enumerable collection of Game entities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Game objects</returns>
        public IEnumerable<Game> GetEntities(ProFootballEntities dbContext)
        {
            Log.Info("Getting Game entities from data store");
            return dbContext.Games;
        }

        /// <summary>
        /// Gets an enumerable collection of Game entities - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <returns>An enumerable collection of Game objects</returns>
        public async Task<IEnumerable<Game>> GetEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Getting Game entities from data store");
            return await dbContext.Games.ToListAsync();
        }

        /// <summary>
        /// Loads a collection of Game entities into memory
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public void LoadEntities(ProFootballEntities dbContext)
        {
            Log.Info("Loading Game entities into memory");
            dbContext.Games.Load();
        }

        /// <summary>
        /// Loads a collection of Game entities into memory - asynchronous
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        public async Task LoadEntitiesAsync(ProFootballEntities dbContext)
        {
            Log.Info("Loading Game entities into memory");
            await dbContext.Games.LoadAsync();
        }

        /// <summary>
        /// Removes a Game entity from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="game">The Game entity to delete</param>
        /// <returns>The Game entity deleted from dbContext</returns>
        public Game RemoveEntity(ProFootballEntities dbContext, Game game)
        {
            Log.Info("Removing Game entity from data store");
            return dbContext.Games.Remove(game);
        }

        /// <summary>
        /// Removes multiple Game entities from ProFootballEntities
        /// </summary>
        /// <param name="dbContext">An instance of the ProFootballEntities class</param>
        /// <param name="games">The collection of Game entities to remove</param>
        /// <returns>The Game entity collection removed from dbContext</returns>
        public IEnumerable<Game> RemoveEntities(ProFootballEntities dbContext, IEnumerable<Game> games)
        {
            Log.Info("Removing Game entities from data store");
            return dbContext.Games.RemoveRange(games);
        }

        #endregion IRepository<Game> Implementation

        #region Helpers

        private void ValidateFindArgs(string methodName, params object[] args)
        {
            var messagePrefix = $"GameRepository.{methodName}: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(int))
            {
                throw new ArgumentException(messagePrefix + "ID");
            }
        }

        #endregion Helpers
    }
}
