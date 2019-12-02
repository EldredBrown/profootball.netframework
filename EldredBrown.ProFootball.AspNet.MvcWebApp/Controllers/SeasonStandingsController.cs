using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Services;
using log4net;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Controllers
{
    /// <summary>
    /// Controller class for the SeasonStandings page
    /// </summary>
    public class SeasonStandingsController : Controller
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISeasonStandingsService _service;
        private readonly ISharedService _sharedService;

        /// <summary>
        /// Initializes a new instance of the SeasonStandingsController class
        /// </summary>
        /// <param name="service"></param>
        /// <param name="sharedService"></param>
        public SeasonStandingsController(ISeasonStandingsService service, ISharedService sharedService)
        {
            _service = service;
            _sharedService = sharedService;
        }

        // GET: SeasonStandings
        public async Task<ActionResult> Index(int? seasonID, bool? groupByDivision)
        {
            try
            {
                var seasons = await _sharedService.GetSeasonsOrderedAsync();
                _service.SetSelectedSeason(seasons, seasonID);
                ViewBag.SeasonID = new SelectList(seasons, "ID", "ID", SeasonStandingsService.SelectedSeason);

                if (groupByDivision == null)
                {
                    groupByDivision = false;
                }

                ViewBag.GroupByDivision = groupByDivision;

                var seasonStandingsResults =
                    _service.GetSeasonStandings(SeasonStandingsService.SelectedSeason, groupByDivision);

                return View(seasonStandingsResults);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in SeasonStandingsController.Index: " + ex.Message);
                throw;
            }
        }
    }
}
