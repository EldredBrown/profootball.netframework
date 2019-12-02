using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Repositories
{
    /// <summary>
    /// Repository for access to the Game data model
    /// </summary>
    public class GameRepository : IRepository<Game>
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProFootballEntities _dbContext;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the GameRepository class
        /// </summary>
        /// <param name="dbContext"></param>
        public GameRepository(ProFootballEntities dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Destroys this instance of the GameRepository class
        /// </summary>
        ~GameRepository()
        {
            _dbContext.Dispose();
        }

        #endregion Constructors & Finalizers

        #region IRepository<Game> Implementation

        /// <summary>
        /// Adds a Game entity to the data store
        /// </summary>
        /// <param name="game">The game entity to add to the data store</param>
        /// <returns>The game entity added to the data store</returns>
        public Game AddEntity(Game game)
        {
            try
            {
                Log.Info("Adding Game entity to data store");

                return _dbContext.Games.Add(game);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Adds multiple Game entities to the data store
        /// </summary>
        /// <param name="game">The collection of game entities to add to the data store</param>
        /// <returns>The enumerable collection of game entities added to the data store</returns>
        public IEnumerable<Game> AddEntities(IEnumerable<Game> games)
        {
            try
            {
                Log.Info("Adding Game entities to data store");

                return _dbContext.Games.AddRange(games);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Creates a Game entity
        /// </summary>
        /// <returns>The newly created Game entity</returns>
        public Game CreateEntity()
        {
            try
            {
                Log.Info("Creating Game entity");

                return _dbContext.Games.Create();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Edits a Game entity in the data store
        /// </summary>
        /// <param name="game">The Game entity to be modified</param>
        public void EditEntity(Game game)
        {
            try
            {
                Log.Info("Updating Game entity in data store");

                _dbContext.SetModified(game);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Finds a Game entity in the data store by its ID
        /// </summary>
        /// <param name="args">A list containing the ID of the Game entity for which to search</param>
        /// <returns></returns>
        public Game FindEntity(params object[] args)
        {
            // Validate args.
            var messagePrefix = $"GameRepository.FindEntity: ";

            if (args.Length != 1)
            {
                throw new ArgumentException(messagePrefix + "Invalid number of arguments received");
            }
            if (args[0].GetType() != typeof(int))
            {
                throw new ArgumentException(messagePrefix + "ID");
            }

            // Find matching entity.
            Game retVal;
            var id = Convert.ToInt32(args[0]);

            try
            {
                retVal = _dbContext.Games.Find(id);
                if (retVal == null)
                {
                    var errMsg = $"Game entity not found in data store\nID: {id}";

                    Log.Error(errMsg);

                    throw new ObjectNotFoundException(errMsg);
                }

                Log.Info($"Game entity found in data store\nID: {id}");
            }
            catch (InvalidOperationException ex)
            {
                var errMsg = $"Game entity not found in data store\nID: {id}";

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
        /// Gets a collection of Game entities from the data store
        /// </summary>
        /// <returns>An enumerable collection of Game entities</returns>
        public IEnumerable<Game> GetEntities()
        {
            try
            {
                Log.Info("Getting Game entities from data store");

                return _dbContext.Games;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes a Game entity from the data store
        /// </summary>
        /// <param name="game">The deletedGame entity</param>
        /// <returns></returns>
        public Game RemoveEntity(Game game)
        {
            try
            {
                Log.Info("Removing Game entity from data store");

                return _dbContext.Games.Remove(game);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Deletes multiple Game entities from the data store
        /// </summary>
        /// <param name="games">The deleted collection of Game entities</param>
        /// <returns></returns>
        public IEnumerable<Game> RemoveEntities(IEnumerable<Game> games)
        {
            try
            {
                Log.Info("Removing Game entities from data store");

                return _dbContext.Games.RemoveRange(games);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        #endregion IRepository<Game> Implementation
    }
}
