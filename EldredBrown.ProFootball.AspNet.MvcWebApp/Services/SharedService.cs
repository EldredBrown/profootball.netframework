using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;
using log4net;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Services
{
    public interface ISharedService
    {
        Task<TeamSeasonViewModel> FindEntityAsync(string teamName, int seasonID,
            ProFootballEntities dbContextInjected = null);

        Task<IEnumerable<SeasonViewModel>> GetSeasonsOrderedAsync(ProFootballEntities dbContextInjected = null);
        void SetSelectedSeason(IEnumerable<SeasonViewModel> seasons, int? seasonID, ref int selectedSeason);
    }

    public class SharedService : ISharedService
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDataMapper _dataMapper;
        private readonly IRepository<Season> _seasonRepository;
        private readonly IRepository<TeamSeason> _teamSeasonRepository;

        /// <summary>
        /// Initializes a new instance of the SharedService class
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="seasonRepository"></param>
        /// <param name="teamSeasonRepository"></param>
        public SharedService(IDataMapper mapper, IRepository<Season> seasonRepository,
            IRepository<TeamSeason> teamSeasonRepository)
        {
            _dataMapper = mapper;
            _seasonRepository = seasonRepository;
            _teamSeasonRepository = teamSeasonRepository;
        }

        /// <summary>
        /// Finds a selected TeamSeason asynchronously
        /// </summary>
        /// <param name="teamName">The TeamName of the desired TeamSeason</param>
        /// <param name="seasonID">The SeasonID of the desired TeamSeason</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        /// <returns>The TeamSeason object with the matching TeamName and SeasonID, null if no entity found</returns>
        public async Task<TeamSeasonViewModel> FindEntityAsync(string teamName, int seasonID,
            ProFootballEntities dbContextInjected = null)
        {
            TeamSeason teamSeason;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                try
                {
                    teamSeason = await _teamSeasonRepository.FindEntityAsync(dbContext,
                        teamName, seasonID);
                }
                catch (ObjectNotFoundException ex)
                {
                    var errMsg = "ObjectNotFoundException in SharedService.FindEntityAsync: " + ex.Message;

                    _log.Error(errMsg);

                    MessageBox.Show(errMsg, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);

                    return null;
                }
            }

            return _dataMapper.MapToTeamSeasonViewModel(teamSeason);
        }

        /// <summary>
        /// Gets an ordered list of seasons
        /// </summary>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        /// <returns>An enumerable collection of SeasonViewModel objects</returns>
        public async Task<IEnumerable<SeasonViewModel>> GetSeasonsOrderedAsync(
            ProFootballEntities dbContextInjected = null)
        {
            var seasonViewModels = new List<SeasonViewModel>();

            IEnumerable<Season> seasons;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                seasons = (await _seasonRepository.GetEntitiesAsync(dbContext)).OrderByDescending(s => s.ID);
            }
            foreach (var season in seasons)
            {
                var seasonViewModel = _dataMapper.MapToSeasonViewModel(season);
                seasonViewModels.Add(seasonViewModel);
            }

            return seasonViewModels;
        }

        /// <summary>
        /// Sets the selected season for this web app
        /// </summary>
        /// <param name="seasons">An enumerable collection of SeasonViewModel objects</param>
        /// <param name="seasonID">The ID of the selected season</param>
        /// <param name="selectedSeason">A reference allowing for modification of an external selectedSeason int</param>
        public void SetSelectedSeason(IEnumerable<SeasonViewModel> seasons, int? seasonID, ref int selectedSeason)
        {
            if (seasonID == null)
            {
                if (WebGlobals.SelectedSeason == null)
                {
                    selectedSeason = seasons.First().ID;
                }
                else
                {
                    selectedSeason = (int)WebGlobals.SelectedSeason;
                }
            }
            else
            {
                selectedSeason = (int)seasonID;
            }

            WebGlobals.SelectedSeason = selectedSeason;
        }
    }
}
