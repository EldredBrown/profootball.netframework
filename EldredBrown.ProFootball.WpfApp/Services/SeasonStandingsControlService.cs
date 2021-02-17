using System;
using System.Collections.Generic;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Services
{
    public interface ISeasonStandingsControlService
    {
        IEnumerable<League> GetLeaguesBySeason(int seasonID);

        IEnumerable<Conference> GetConferencesByLeagueAndSeason(string leagueName, int seasonID);
        IEnumerable<Division> GetDivisionsByLeagueAndSeason(string leagueName, int seasonID);
        IEnumerable<Division> GetDivisionsByConferenceAndSeason(string conferenceName, int seasonID);

        IEnumerable<GetSeasonStandingsForLeague_Result> GetSeasonStandingsForLeague(int? seasonID,
            string leagueName);

        IEnumerable<GetSeasonStandingsForConference_Result> GetSeasonStandingsForConference(int? seasonID,
            string conferenceName);

        IEnumerable<GetSeasonStandingsForDivision_Result> GetSeasonStandingsForDivision(int? seasonID,
            string divisionName);
    }
    
    /// <summary>
    /// Service class used by the SeasonStandingsControl class
    /// </summary>
    public class SeasonStandingsControlService : ISeasonStandingsControlService
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISharedService _sharedService;
        private readonly IRepository<League> _leagueRepository;
        private readonly IRepository<Conference> _conferenceRepository;
        private readonly IRepository<Division> _divisionRepository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;

        /// <summary>
        /// Initializes a new instance of the SeasonStandingsControlService class
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="leagueRepository"></param>
        /// <param name="conferenceRepository"></param>
        /// <param name="divisionRepository"></param>
        /// <param name="storedProcedureRepository"></param>
        public SeasonStandingsControlService(ISharedService sharedService, IRepository<League> leagueRepository,
            IRepository<Conference> conferenceRepository, IRepository<Division> divisionRepository,
            IStoredProcedureRepository storedProcedureRepository)
        {
            _sharedService = sharedService;
            _leagueRepository = leagueRepository;
            _conferenceRepository = conferenceRepository;
            _divisionRepository = divisionRepository;
            _storedProcedureRepository = storedProcedureRepository;
        }

        /// <summary>
        /// Gets all the leagues for a specified season
        /// </summary>
        /// <param name="seasonID">The ID of the season for which leagues will be fetched</param>
        /// <returns>An enumerable collection of all leagues for the specified season</returns>
        public IEnumerable<League> GetLeaguesBySeason(int seasonID)
        {
            try
            {
                return _leagueRepository.GetEntities()
                    .OrderBy(l => l.Name)
                    .Where(l => l.AssociationExists(seasonID));
            }
            catch (ArgumentNullException ex)
            {
                _log.Error($"ArgumentNullException in SeasonStandingsControlService.GetLeaguesBySeason: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<League>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets all conferences for a specified league and season
        /// </summary>
        /// <param name="leagueName">The name of the league for which conferences will be fetched</param>
        /// <param name="seasonID">The ID of the season for which conferences will be fetched</param>
        /// <returns>An enumerable collection of all the conferences found for the specified league and season</returns>
        public IEnumerable<Conference> GetConferencesByLeagueAndSeason(string leagueName, int seasonID)
        {
            try
            {
                return _conferenceRepository.GetEntities()
                    .OrderBy(c => c.Name)
                    .Where(c => c.AssociationExists(seasonID) && c.LeagueName == leagueName);
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException in SeasonStandingsControlService.GetConferencesByLeagueAndSeason: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<Conference>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets all divisions for a specified league and season
        /// </summary>
        /// <param name="leagueName">The name of the league for which divisions will be fetched</param>
        /// <param name="seasonID">The ID of the season for which divisions will be fetched</param>
        /// <returns>An enumerable collection of all the divisions found for the specified league and season</returns>
        public IEnumerable<Division> GetDivisionsByLeagueAndSeason(string leagueName, int seasonID)
        {
            try
            {
                return _divisionRepository.GetEntities()
                    .OrderBy(d => d.Name)
                    .Where(d => d.AssociationExists(seasonID) && d.LeagueName == leagueName);
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException in SeasonStandingsControlService.GetDivisionsByLeagueAndSeason: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<Division>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets all divisions for a specified league and season
        /// </summary>
        /// <param name="conferenceName">The name of the conference for which divisions will be fetched</param>
        /// <param name="seasonID">The ID of the season for which divisions will be fetched</param>
        /// <returns>An enumerable collection of all the divisions found for the specified conference and season</returns>
        public IEnumerable<Division> GetDivisionsByConferenceAndSeason(string conferenceName, int seasonID)
        {
            try
            {
                return _divisionRepository.GetEntities()
                    .OrderBy(d => d.Name)
                    .Where(d => d.AssociationExists(seasonID) && d.ConferenceName == conferenceName);
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException in SeasonStandingsControlService.GetDivisionsByConferenceAndSeason: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<Division>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the season standings for a specified league
        /// </summary>
        /// <param name="seasonID">The season for which standings will be fetched</param>
        /// <param name="leagueName">The league for which standings will be fetched</param>
        /// <returns>An enumerable collection of the query results</returns>
        public IEnumerable<GetSeasonStandingsForLeague_Result> GetSeasonStandingsForLeague(int? seasonID,
            string leagueName)
        {
            try
            {
                return _storedProcedureRepository.GetSeasonStandingsForLeague(seasonID, leagueName).ToList();
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException caught in SeasonStandingsControlService.GetSeasonStandingsForLeague: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<GetSeasonStandingsForLeague_Result>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the season standings for a specified conference
        /// </summary>
        /// <param name="seasonID">The season for which standings will be fetched</param>
        /// <param name="conferenceName">The conference for which standings will be fetched</param>
        /// <returns>An enumerable collection of the query results</returns>
        public IEnumerable<GetSeasonStandingsForConference_Result> GetSeasonStandingsForConference(int? seasonID,
            string conferenceName)
        {
            try
            {
                return _storedProcedureRepository.GetSeasonStandingsForConference(seasonID, conferenceName).ToList();
            }
            catch(ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException caught in SeasonStandingsControlService.GetSeasonStandingsForConference: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<GetSeasonStandingsForConference_Result>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Gets the season standings for a specified division
        /// </summary>
        /// <param name="seasonID">The season for which standings will be fetched</param>
        /// <param name="divisionName">The conference for which standings will be fetched</param>
        /// <returns>An enumerable collection of the query results</returns>
        public IEnumerable<GetSeasonStandingsForDivision_Result> GetSeasonStandingsForDivision(int? seasonID,
            string divisionName)
        {
            try
            {
                return _storedProcedureRepository.GetSeasonStandingsForDivision(seasonID, divisionName).ToList();
            }
            catch (ArgumentNullException ex)
            {
                _log.Error(
                    $"ArgumentNullException caught in SeasonStandingsControlService.GetSeasonStandingsForDivision: {ex.Message}");

                _sharedService.ShowExceptionMessage(ex);

                return new List<GetSeasonStandingsForDivision_Result>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }
    }
}
