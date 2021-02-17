using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Services;
using log4net;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Controllers
{
    /// <summary>
    /// Controller class for the TeamSeasons page
    /// </summary>
    public class TeamSeasonsController : Controller
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITeamSeasonsService _service;
        private readonly ISharedService _sharedService;

        /// <summary>
        /// Initializes a new instance of the TeamSeasonsController class
        /// </summary>
        /// <param name="service"></param>
        /// <param name="sharedService"></param>
        public TeamSeasonsController(ITeamSeasonsService service, ISharedService sharedService)
        {
            _service = service;
            _sharedService = sharedService;
        }

        // GET: TeamSeasons
        public async Task<ActionResult> Index(int? seasonID, string sortOrder)
        {
            try
            {
                var seasons = await _sharedService.GetSeasonsOrderedAsync();
                _service.SetSelectedSeason(seasons, seasonID, sortOrder);
                ViewBag.SeasonID = new SelectList(seasons, "ID", "ID", TeamSeasonsService.SelectedSeason);

                ViewBag.TeamSortParm =
                    sortOrder == "team_desc" ? "team_desc" : "team_asc";
                ViewBag.WinsSortParm =
                    sortOrder == "wins_desc" ? "wins_desc" : "wins_asc";
                ViewBag.LossesSortParm =
                    sortOrder == "losses_desc" ? "losses_desc" : "losses_asc";
                ViewBag.TiesSortParm =
                    sortOrder == "ties_desc" ? "ties_desc" : "ties_asc";
                ViewBag.WinningPercentageSortParm =
                    sortOrder == "win_pct_desc" ? "win_pct_desc" : "win_pct_asc";
                ViewBag.PointsForSortParm =
                    sortOrder == "pf_desc" ? "pf_desc" : "pf_asc";
                ViewBag.PointsAgainstSortParm =
                    sortOrder == "pa_desc" ? "pa_desc" : "pa_asc";
                ViewBag.PythagoreanWinsSortParm =
                    sortOrder == "pyth_wins_desc" ? "pyth_wins_desc" : "pyth_wins_asc";
                ViewBag.PythagoreanLossesSortParm =
                    sortOrder == "pyth_losses_desc" ? "pyth_losses_desc" : "pyth_losses_asc";
                ViewBag.OffensiveAverageSortParm =
                    sortOrder == "off_avg_desc" ? "off_avg_desc" : "off_avg_asc";
                ViewBag.OffensiveFactorSortParm =
                    sortOrder == "off_factor_desc" ? "off_factor_desc" : "off_factor_asc";
                ViewBag.OffensiveIndexSortParm =
                    sortOrder == "off_index_desc" ? "off_index_desc" : "off_index_asc";
                ViewBag.DefensiveAverageSortParm =
                    sortOrder == "def_avg_desc" ? "def_avg_desc" : "def_avg_asc";
                ViewBag.DefensiveFactorSortParm =
                    sortOrder == "def_factor_desc" ? "def_factor_desc" : "def_factor_asc";
                ViewBag.DefensiveIndexSortParm =
                    sortOrder == "def_index_desc" ? "def_index_desc" : "def_index_asc";
                ViewBag.FinalPythagoreanWinningPercentageSortParm =
                    sortOrder == "fin_pyth_pct_desc" ? "fin_pyth_pct_desc" : "fin_pyth_pct_asc";

                var teamSeasons =
                    await _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder);

                return View(teamSeasons);
            }
            catch (Exception ex)
            {
                _log.Error("Exception in TeamSeasonsController.Index: " + ex.Message);

                throw;
            }
        }

        // GET: TeamSeasons/Details/5
        public async Task<ActionResult> Details(string teamName, int? seasonID)
        {
            try
            {
                if (string.IsNullOrEmpty(teamName) || seasonID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var teamSeasonDetails = await _service.GetTeamSeasonDetailsAsync(teamName, (int)seasonID);

                if (teamSeasonDetails.TeamSeason == null ||
                    teamSeasonDetails.TeamSeasonScheduleProfile == null ||
                    teamSeasonDetails.TeamSeasonScheduleTotals == null ||
                    teamSeasonDetails.TeamSeasonScheduleAverages == null)
                {
                    return HttpNotFound();
                }

                return View(teamSeasonDetails);
            }
            catch (Exception ex)
            {
                _log.Error("Exception in TeamSeasonsController.Details: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the rankings for the selected season
        /// </summary>
        public async Task UpdateRankings()
        {
            try
            {
                await _service.UpdateRankings();

                if (Url != null)
                {
                    Response.Redirect(Url.Action("Index", "TeamSeasons"));
                }
            }
            catch (Exception ex)
            {
                _log.Error("Exception in TeamSeasonsController.UpdateRankings: " + ex.Message);
                throw;
            }
        }
    }
}
