using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;
using log4net;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Services
{
    public interface IGamePredictorService
    {
        void ApplyFilter(int? guestSeasonID, int? hostSeasonID);
        Task GetGuestAndHostSeasonIds(ProFootballEntities dbContextInjected = null);
        Task<IEnumerable<TeamSeasonViewModel>> GetEntities(int seasonID, ProFootballEntities dbContextInjected = null);
    }

    /// <summary>
    /// Service class used by the GamePredictorController
    /// </summary>
    public class GamePredictorService : IGamePredictorService
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDataMapper _dataMapper;
        private readonly IRepository<Season> _seasonRepository;
        private readonly IRepository<TeamSeason> _teamSeasonRepository;

        /// <summary>
        /// Initializes a new instance of the GamePredictorService class
        /// </summary>
        /// <param name="dataMapper"></param>
        /// <param name="seasonRepository"></param>
        /// <param name="teamSeasonRepository"></param>
        public GamePredictorService(IDataMapper dataMapper, IRepository<Season> seasonRepository,
            IRepository<TeamSeason> teamSeasonRepository)
        {
            _dataMapper = dataMapper;
            _seasonRepository = seasonRepository;
            _teamSeasonRepository = teamSeasonRepository;
        }

        public static int? GuestSeasonID;
        public static int? HostSeasonID;

        /// <summary>
        /// Applies a filter that allows the user to view only those desired teams
        /// </summary>
        /// <param name="guestSeasonID">The ID of the guest's season</param>
        /// <param name="hostSeasonID">The ID of the host's season</param>
        public void ApplyFilter(int? guestSeasonID, int? hostSeasonID)
        {
            if (guestSeasonID != null)
            {
                GuestSeasonID = guestSeasonID;
            }

            if (hostSeasonID != null)
            {
                HostSeasonID = hostSeasonID;
            }
        }

        /// <summary>
        /// Gets the IDs for the guest's and host's seasons
        /// </summary>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        public async Task GetGuestAndHostSeasonIds(ProFootballEntities dbContextInjected = null)
        {
            int? firstSeasonID = null;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                try
                {
                    firstSeasonID = (await _seasonRepository.GetEntitiesAsync(dbContext))
                        .OrderByDescending(s => s.ID)
                        .First()
                        .ID;
                }
                catch (ArgumentNullException ex)
                {
                    Log.Error("ArgumentNullExcetion in GamePredictorService.GetGuestAndHostSeasonIds: " + ex.Message);
                    return;
                }
            }

            // Get guest season.
            if (GuestSeasonID == null)
            {
                GuestSeasonID = firstSeasonID;
            }

            // Get host season.
            if (HostSeasonID == null)
            {
                HostSeasonID = firstSeasonID;
            }
        }

        /// <summary>
        /// Gets all TeamSeason objects for the specified season
        /// </summary>
        /// <param name="seasonID">The ID of the specified season</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        /// <returns>An enumerable collection of TeamSeasonViewModel objects representing the fetched TeamSeason objects</returns>
        public async Task<IEnumerable<TeamSeasonViewModel>> GetEntities(int seasonID,
            ProFootballEntities dbContextInjected = null)
        {
            var teamSeasonViewModels = new List<TeamSeasonViewModel>();

            IEnumerable<TeamSeason> teamSeasons = null;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                try
                {
                    teamSeasons = (await _teamSeasonRepository.GetEntitiesAsync(dbContext))
                        .Where(ts => ts.SeasonID == seasonID);
                }
                catch (ArgumentNullException ex)
                {
                    Log.Error("ArgumentNullExcetion in GamePredictorService.GetGuestAndHostSeasonIds: " + ex.Message);
                    return null;
                }
            }

            foreach (var teamSeason in teamSeasons)
            {
                var teamSeasonViewModel = _dataMapper.MapToTeamSeasonViewModel(teamSeason);
                teamSeasonViewModels.Add(teamSeasonViewModel);
            }

            return teamSeasonViewModels;
        }
    }
}
