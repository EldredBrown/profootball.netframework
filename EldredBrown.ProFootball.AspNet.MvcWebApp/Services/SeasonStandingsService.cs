using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Services
{
    public interface ISeasonStandingsService
    {
        IEnumerable<SeasonStandingsResultViewModel> GetSeasonStandings(int? selectedSeason, bool? groupByDivision,
            ProFootballEntities dbContextInjected = null);

        void SetSelectedSeason(IEnumerable<SeasonViewModel> seasons, int? seasonID);
    }

    /// <summary>
    /// Service class uesed by the SeasonStandingsController
    /// </summary>
    public class SeasonStandingsService : ISeasonStandingsService
    {
        private readonly ISharedService _sharedService;
        private readonly IDataMapper _dataMapper;
        private readonly IStoredProcedureRepository _storedProcedureRepository;

        /// <summary>
        /// Initializes a new instance of the SeasonStandingsService class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="dataMapper"></param>
        /// <param name="storedProcedureRepository"></param>
        public SeasonStandingsService(ISharedService sharedService, IDataMapper dataMapper,
            IStoredProcedureRepository storedProcedureRepository)
        {
            _sharedService = sharedService;
            _dataMapper = dataMapper;
            _storedProcedureRepository = storedProcedureRepository;
        }

        public static int SelectedSeason;

        /// <summary>
        /// Gets the standings for a selected season
        /// </summary>
        /// <param name="selectedSeason">The selected season</param>
        /// <param name="groupByDivision">A flag indicating whether the standings are to be grouped by division</param>
        /// <param name="dbContextInjected">Injected DbContext object for unit testing</param>
        /// <returns>An enumerable collection of view model objects representing the season standings</returns>
        public IEnumerable<SeasonStandingsResultViewModel> GetSeasonStandings(int? selectedSeason,
            bool? groupByDivision, ProFootballEntities dbContextInjected = null)
        {
            var seasonStandingsViewModels = new List<SeasonStandingsResultViewModel>();

            ObjectResult<GetSeasonStandings_Result> seasonStandingResults;
            using (var dbContext = dbContextInjected ?? new ProFootballEntities())
            {
                seasonStandingResults =
                    _storedProcedureRepository.GetSeasonStandings(dbContext, selectedSeason, groupByDivision);
            }

            foreach (var result in seasonStandingResults)
            {
                var seasonStandingsViewModel = _dataMapper.MapToSeasonStandingsResultViewModel(result);
                seasonStandingsViewModels.Add(seasonStandingsViewModel);
            }

            return seasonStandingsViewModels;
        }

        /// <summary>
        /// Sets the selected season
        /// </summary>
        /// <param name="seasons">An enumerable collection of SeasonViewModel objects</param>
        /// <param name="seasonID">The ID of the selected season</param>
        public void SetSelectedSeason(IEnumerable<SeasonViewModel> seasons, int? seasonID)
        {
            _sharedService.SetSelectedSeason(seasons, seasonID, ref SelectedSeason);
        }
    }
}
