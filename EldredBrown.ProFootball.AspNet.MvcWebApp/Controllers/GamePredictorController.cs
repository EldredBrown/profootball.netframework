using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Services;
using log4net;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Controllers
{
    /// <summary>
    /// Controller class for the GamePredictor page
    /// </summary>
    public class GamePredictorController : Controller
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IGamePredictorService _service;
        private readonly ISharedService _sharedService;

        /// <summary>
        /// Initializes a new instance of the GamePredictorController class
        /// </summary>
        /// <param name="service"></param>
        /// <param name="sharedService"></param>
        public GamePredictorController(IGamePredictorService service, ISharedService sharedService)
        {
            _service = service;
            _sharedService = sharedService;
        }

        // GET: GamePredictor
        public async Task<ActionResult> PredictGame()
        {
            try
            {
                await _service.GetGuestAndHostSeasonIds();

                var seasons = await _sharedService.GetSeasonsOrderedAsync();

                ViewBag.GuestSeasonID = new SelectList(seasons, "ID", "ID", GamePredictorService.GuestSeasonID);

                var guestSeasons = await _service.GetEntities((int)GamePredictorService.GuestSeasonID);
                ViewBag.GuestName = new SelectList(guestSeasons, "TeamName", "TeamName");

                ViewBag.HostSeasonID = new SelectList(seasons, "ID", "ID", GamePredictorService.HostSeasonID);

                var hostSeasons = await _service.GetEntities((int)GamePredictorService.HostSeasonID);
                ViewBag.HostName = new SelectList(hostSeasons, "TeamName", "TeamName");

                return View();
            }
            catch (Exception ex)
            {
                _log.Error("Exception in GamePredictorController.PredictGame (GET): " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Calculates the predicted score of a future or hypothetical game.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PredictGame([Bind(Include = "GuestSeasonID,GuestName,HostSeasonID,HostName")] GamePredictionViewModel matchup)
        {
            try
            {
                var seasons = await _sharedService.GetSeasonsOrderedAsync();

                // Show predicted guest score.
                var guestSeason = await _sharedService.FindEntityAsync(matchup.GuestName, matchup.GuestSeasonID);
                ViewBag.GuestSeasonID = new SelectList(seasons, "ID", "ID", guestSeason.SeasonID);

                var guestSeasons = await _service.GetEntities((int)GamePredictorService.GuestSeasonID);
                ViewBag.GuestName = new SelectList(guestSeasons, "TeamName", "TeamName", guestSeason.TeamName);

                // Show predicted host score.
                var hostSeason = await _sharedService.FindEntityAsync(matchup.HostName, matchup.HostSeasonID);
                ViewBag.HostSeasonID = new SelectList(seasons, "ID", "ID", hostSeason.SeasonID);

                var hostSeasons = await _service.GetEntities((int)GamePredictorService.HostSeasonID);
                ViewBag.HostName = new SelectList(hostSeasons, "TeamName", "TeamName", hostSeason.TeamName);

                return View(matchup);
            }
            catch (Exception ex)
            {
                _log.Error("Exception in GamePredictorController.PredictGame (POST): " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Applies a filter that allows the user to view only those desired teams
        /// </summary>
        /// <param name="guestSeasonID">The ID of the guest's season</param>
        /// <param name="hostSeasonID">The ID of the host's season</param>
        /// <returns></returns>
        public ActionResult ApplyFilter(int? guestSeasonID, int? hostSeasonID)
        {
            try
            {
                _service.ApplyFilter(guestSeasonID, hostSeasonID);

                return RedirectToAction("PredictGame");
            }
            catch (Exception ex)
            {
                _log.Error("Exception in GamePredictorController.ApplyFilter: " + ex.Message);
                throw;
            }
        }
    }
}
