using System;
using System.Collections.Generic;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Services
{
    public interface IGamePredictorWindowService
    {
        IEnumerable<int> GetSeasonIDs();
    }

    /// <summary>
    /// Service class used by the GamePredictorWindow
    /// </summary>
    public class GamePredictorWindowService : IGamePredictorWindowService
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IRepository<Season> _seasonRepository;
        private readonly IRepository<TeamSeason> _teamSeasonRepository;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the GamePredictorWindowService class
        /// </summary>
        /// <param name="seasonRepository"></param>
        /// <param name="teamSeasonRepository"></param>
        public GamePredictorWindowService(IRepository<Season> seasonRepository,
            IRepository<TeamSeason> teamSeasonRepository)
        {
            _seasonRepository = seasonRepository;
            _teamSeasonRepository = teamSeasonRepository;
        }

        #endregion Constructors & Finalizers

        #region Methods

        /// <summary>
        /// Gets a list of all season IDs sorted in descending order
        /// </summary>
        /// <returns>An enumerable collection of integers representing seasons</returns>
        public IEnumerable<int> GetSeasonIDs()
        {
            try
            {
                var seasonIDs = new List<int>();

                var seasons = _seasonRepository.GetEntities().OrderByDescending(s => s.ID);
                foreach (var season in seasons)
                {
                    seasonIDs.Add(season.ID);
                }

                return seasonIDs;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        #endregion Methods
    }
}
