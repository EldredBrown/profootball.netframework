using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Services;
using log4net;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Controllers
{
    /// <summary>
    /// Controller class for the Games page
    /// </summary>
    public class GamesController : Controller
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IGamesService _service;
        private readonly ISharedService _sharedService;

        private static GameViewModel _oldGameViewModel;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the GamesController class
        /// </summary>
        /// <param name="service"></param>
        /// <param name="sharedService"></param>
        public GamesController(IGamesService service, ISharedService sharedService)
        {
            _service = service;
            _sharedService = sharedService;
        }

        #endregion Constructors & Finalizers

        #region Actions

        // GET: Games
        public async Task<ActionResult> Index(int? seasonID, int? week, string guestSearchString,
            string hostSearchString)
        {
            try
            {
                var seasons = await _sharedService.GetSeasonsOrderedAsync();
                _service.SetSelectedSeason(seasons, seasonID);
                ViewBag.SeasonID = new SelectList(seasons, "ID", "ID", GamesService.SelectedSeason);

                var weeks = await _service.GetWeeksAsync(GamesService.SelectedSeason);
                _service.SetSelectedWeek(week);
                ViewBag.Week = new SelectList(weeks, "ID", "ID", GamesService.SelectedWeek);

                var games = await _service.GetGamesAsync(GamesService.SelectedSeason, GamesService.SelectedWeek.ID,
                    guestSearchString, hostSearchString);

                return View(games);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GamesController.Index: " + ex.Message);
                throw;
            }
        }

        // GET: Games/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var game = await _service.FindEntityAsync((int)id);
                if (game == null)
                {
                    return HttpNotFound();
                }

                return View(game);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GamesController.Details: " + ex.Message);
                throw;
            }
        }

        // GET: Games/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                var seasons = await _sharedService.GetSeasonsOrderedAsync();
                ViewBag.SeasonID = new SelectList(seasons, "ID", "ID", GamesService.SelectedSeason);
                ViewBag.Week = GamesService.SelectedWeek.ID;

                var teams = await _service.GetTeamsAsync();
                ViewBag.GuestName = new SelectList(teams, "Name", "Name");
                ViewBag.HostName = new SelectList(teams, "Name", "Name");

                return View();
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GamesController.Create (GET): " + ex.Message);
                throw;
            }
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,SeasonID,Week,GuestName,GuestScore,HostName,HostScore,WinnerName,LoserName,IsPlayoffGame,Notes")] GameViewModel gameViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SetSelectedWeek(gameViewModel.Week);

                    await _service.AddEntity(gameViewModel);

                    return RedirectToAction("Create", new { seasonID = gameViewModel.SeasonID });
                }

                var seasons = await _sharedService.GetSeasonsOrderedAsync();
                ViewBag.SeasonID = new SelectList(seasons, "ID", "ID", gameViewModel.SeasonID);

                ViewBag.Week = GamesService.SelectedWeek.ID;

                var teams = await _service.GetTeamsAsync();
                ViewBag.GuestName = new SelectList(teams, "Name", "Name", gameViewModel.GuestName);
                ViewBag.HostName = new SelectList(teams, "Name", "Name", gameViewModel.HostName);

                return View(gameViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GamesController.Create (POST): " + ex.Message);
                throw;
            }
        }

        // GET: Games/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var game = await _service.FindEntityAsync((int)id);
                if (game == null)
                {
                    return HttpNotFound();
                }

                _oldGameViewModel = game;

                var seasons = await _sharedService.GetSeasonsOrderedAsync();
                ViewBag.SeasonID = new SelectList(seasons, "ID", "ID", game.SeasonID);

                var teams = await _service.GetTeamsAsync();
                ViewBag.GuestName = new SelectList(teams, "Name", "Name", game.GuestName);
                ViewBag.HostName = new SelectList(teams, "Name", "Name", game.HostName);

                return View(game);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GamesController.Edit (GET): " + ex.Message);
                throw;
            }
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,SeasonID,Week,GuestName,GuestScore,HostName,HostScore,WinnerName,LoserName,IsPlayoffGame,Notes")] GameViewModel gameViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.EditGame(_oldGameViewModel, gameViewModel);
                    return RedirectToAction("Index", new { seasonID = gameViewModel.SeasonID });
                }

                var seasons = await _sharedService.GetSeasonsOrderedAsync();
                ViewBag.SeasonID = new SelectList(seasons, "ID", "ID", gameViewModel.SeasonID);

                var teams = await _service.GetTeamsAsync();
                ViewBag.GuestName = new SelectList(teams, "Name", "Name", gameViewModel.GuestName);
                ViewBag.HostName = new SelectList(teams, "Name", "Name", gameViewModel.HostName);

                return View(gameViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GamesController.Edit (POST): " + ex.Message);
                throw;
            }
        }

        // GET: Games/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var gameViewModel = await _service.FindEntityAsync((int)id);
                if (gameViewModel == null)
                {
                    return HttpNotFound();
                }

                return View(gameViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GamesController.Delete (GET): " + ex.Message);
                throw;
            }
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.DeleteGame(id);

                return RedirectToAction("Index", new { seasonID = GamesService.SelectedSeason });
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GamesController.Delete (POST): " + ex.Message);
                throw;
            }
        }

        #endregion Actions
    }
}
