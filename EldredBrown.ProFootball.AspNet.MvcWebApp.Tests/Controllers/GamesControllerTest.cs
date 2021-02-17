using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Controllers;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Services;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Tests.Controllers
{
    [TestFixture]
    public class GamesControllerTest
    {
        private IGamesService _service;
        private ISharedService _sharedService;

        [SetUp]
        public void SetUp()
        {
            _service = A.Fake<IGamesService>();
            _sharedService = A.Fake<ISharedService>();
        }

        [TestCase]
        public async Task Index_HappyPath()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            var seasonID = 2017;
            var week = 1;
            var guestSearchString = "Guest";
            var hostSearchString = "Host";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);
            GamesService.SelectedSeason = seasonID;

            var weeks = new List<WeekViewModel>();
            A.CallTo(() => _service.GetWeeksAsync(A<int>.Ignored, null)).Returns(weeks);
            GamesService.SelectedWeek = new WeekViewModel(week);

            var games = new List<GameViewModel>();
            A.CallTo(() => _service.GetGamesAsync(A<int>.Ignored, A<string>.Ignored, A<string>.Ignored,
                A<string>.Ignored, null)).Returns(games);

            // Act
            var result = await controller.Index(seasonID, week, guestSearchString, hostSearchString);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, GamesService.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);
            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(GamesService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            A.CallTo(() => _service.GetWeeksAsync(seasonID, null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedWeek(week)).MustHaveHappenedOnceExactly();

            var viewBagWeek = viewBag.Week;
            Assert.IsInstanceOf<SelectList>(viewBagWeek);
            Assert.AreSame(weeks, viewBagWeek.Items);
            Assert.AreEqual("ID", viewBagWeek.DataValueField);
            Assert.AreEqual("ID", viewBagWeek.DataTextField);
            Assert.AreEqual(GamesService.SelectedWeek, viewBagWeek.SelectedValues[0]);

            A.CallTo(() => _service.GetGamesAsync(seasonID, GamesService.SelectedWeek.ID,
                guestSearchString, hostSearchString, null)).MustHaveHappenedOnceExactly();

            Assert.AreSame(games, resultAsViewResult.Model);
        }

        [TestCase]
        public void Index_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            var seasonID = 2017;
            var week = 1;
            var guestSearchString = "Guest";
            var hostSearchString = "Host";

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () =>
                await controller.Index(seasonID, week, guestSearchString, hostSearchString));
        }

        [TestCase]
        public async Task Details_IdNull_ReturnsBadHttpRequest()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = null;

            // Act
            var result = await controller.Details(id);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (result as HttpStatusCodeResult).StatusCode);
        }

        [TestCase]
        public async Task Details_IdNotNullAndGameViewModelNull_ReturnsHttpNotFound()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = 1;

            GameViewModel game = null;
            A.CallTo(() => _service.FindEntityAsync(A<int>.Ignored, null)).Returns(game);

            // Act
            var result = await controller.Details(id);

            // Assert
            A.CallTo(() => _service.FindEntityAsync((int)id, null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [TestCase]
        public async Task Details_IdNotNullAndGameViewModelNotNull_ReturnsViewResult()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = 1;

            var game = new GameViewModel();
            A.CallTo(() => _service.FindEntityAsync(A<int>.Ignored, null)).Returns(game);

            // Act
            var result = await controller.Details(id);

            // Assert
            A.CallTo(() => _service.FindEntityAsync((int)id, null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(game, (result as ViewResult).Model);
        }

        [TestCase]
        public void Details_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = 1;

            A.CallTo(() => _service.FindEntityAsync(A<int>.Ignored, null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.Details(id));
        }

        [TestCase]
        public async Task CreateGet_HappyPath()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            GamesService.SelectedSeason = 2017;
            GamesService.SelectedWeek = new WeekViewModel(1);

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teams = new List<TeamViewModel>();
            A.CallTo(() => _service.GetTeamsAsync(null)).Returns(teams);

            // Act
            var result = await controller.Create();

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);

            var viewBag = (result as ViewResult).ViewBag;

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);
            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(GamesService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual(GamesService.SelectedWeek.ID, viewBag.Week);

            A.CallTo(() => _service.GetTeamsAsync(null)).MustHaveHappenedOnceExactly();

            var viewBagGuestName = viewBag.GuestName;
            Assert.IsInstanceOf<SelectList>(viewBagGuestName);
            Assert.AreSame(teams, viewBagGuestName.Items);
            Assert.AreEqual("Name", viewBagGuestName.DataValueField);
            Assert.AreEqual("Name", viewBagGuestName.DataTextField);

            var viewBagHostName = viewBag.HostName;
            Assert.IsInstanceOf<SelectList>(viewBagHostName);
            Assert.AreSame(teams, viewBagHostName.Items);
            Assert.AreEqual("Name", viewBagHostName.DataValueField);
            Assert.AreEqual("Name", viewBagHostName.DataTextField);
        }

        [TestCase]
        public void CreateGet_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.Create());
        }

        [TestCase]
        public async Task CreatePost_ModelStateValid()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            var gameViewModel = new GameViewModel
            {
                SeasonID = 2017
            };

            // Act
            var result = await controller.Create(gameViewModel);

            // Assert
            A.CallTo(() => _service.SetSelectedWeek(gameViewModel.Week)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.AddEntity(gameViewModel, null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var resultAsRedirectToRouteResult = result as RedirectToRouteResult;

            Assert.AreEqual("Create", resultAsRedirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual(gameViewModel.SeasonID, resultAsRedirectToRouteResult.RouteValues["seasonID"]);
        }

        [TestCase]
        public async Task CreatePost_ModelStateNotValid()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);
            controller.ModelState.AddModelError("GuestScore", "Guest score is required.");

            var gameViewModel = new GameViewModel
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host"
            };

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            GamesService.SelectedWeek = new WeekViewModel(1);

            var teams = new List<TeamViewModel>();
            A.CallTo(() => _service.GetTeamsAsync(null)).Returns(teams);

            // Act
            var result = await controller.Create(gameViewModel);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);

            var resultAsViewResult = result as ViewResult;
            var viewBag = resultAsViewResult.ViewBag;

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);
            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(gameViewModel.SeasonID, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual(GamesService.SelectedWeek.ID, viewBag.Week);

            var viewBagGuestName = viewBag.GuestName;
            Assert.IsInstanceOf<SelectList>(viewBagGuestName);
            Assert.AreSame(teams, viewBagGuestName.Items);
            Assert.AreEqual("Name", viewBagGuestName.DataValueField);
            Assert.AreEqual("Name", viewBagGuestName.DataTextField);
            Assert.AreEqual(gameViewModel.GuestName, viewBagGuestName.SelectedValues[0]);

            var viewBagHostName = viewBag.HostName;
            Assert.IsInstanceOf<SelectList>(viewBagHostName);
            Assert.AreSame(teams, viewBagHostName.Items);
            Assert.AreEqual("Name", viewBagHostName.DataValueField);
            Assert.AreEqual("Name", viewBagHostName.DataTextField);
            Assert.AreEqual(gameViewModel.HostName, viewBagHostName.SelectedValues[0]);

            Assert.AreSame(gameViewModel, resultAsViewResult.Model);
        }

        [TestCase]
        public void CreatePost_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);
            controller.ModelState.AddModelError("GuestScore", "Guest score is required.");

            var gameViewModel = new GameViewModel
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host"
            };

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.Create(gameViewModel));
        }

        [TestCase]
        public async Task EditGet_IdNull_ReturnsBadHttpRequest()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = null;

            // Act
            var result = await controller.Edit(id);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (result as HttpStatusCodeResult).StatusCode);
        }

        [TestCase]
        public async Task EditGet_IdNotNullAndGameViewModelNull_ReturnsHttpNotFound()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = 1;

            GameViewModel game = null;
            A.CallTo(() => _service.FindEntityAsync(A<int>.Ignored, null)).Returns(game);

            // Act
            var result = await controller.Edit(id);

            // Assert
            A.CallTo(() => _service.FindEntityAsync((int)id, null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [TestCase]
        public async Task EditGet_IdNotNullAndGameViewModelNotNull_ReturnsViewResult()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = 1;

            var game = new GameViewModel
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host"
            };
            A.CallTo(() => _service.FindEntityAsync(A<int>.Ignored, null)).Returns(game);

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teams = new List<TeamViewModel>();
            A.CallTo(() => _service.GetTeamsAsync(null)).Returns(teams);

            // Act
            var result = await controller.Edit(id);

            // Assert
            A.CallTo(() => _service.FindEntityAsync((int)id, null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);

            var viewBag = (result as ViewResult).ViewBag;

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);
            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(game.SeasonID, viewBagSeasonID.SelectedValues[0]);

            A.CallTo(() => _service.GetTeamsAsync(null)).MustHaveHappenedOnceExactly();

            var viewBagGuestName = viewBag.GuestName;
            Assert.IsInstanceOf<SelectList>(viewBagGuestName);
            Assert.AreSame(teams, viewBagGuestName.Items);
            Assert.AreEqual("Name", viewBagGuestName.DataValueField);
            Assert.AreEqual("Name", viewBagGuestName.DataTextField);
            Assert.AreEqual(game.GuestName, viewBagGuestName.SelectedValues[0]);

            var viewBagHostName = viewBag.HostName;
            Assert.IsInstanceOf<SelectList>(viewBagHostName);
            Assert.AreSame(teams, viewBagHostName.Items);
            Assert.AreEqual("Name", viewBagHostName.DataValueField);
            Assert.AreEqual("Name", viewBagHostName.DataTextField);
            Assert.AreEqual(game.HostName, viewBagHostName.SelectedValues[0]);
        }

        [TestCase]
        public void EditGet_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = 1;

            A.CallTo(() => _service.FindEntityAsync(A<int>.Ignored, null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.Edit(id));
        }

        [TestCase]
        public async Task EditPost_ModelStateValid()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            var gameViewModel = new GameViewModel
            {
                SeasonID = 2017
            };

            // Act
            var result = await controller.Edit(gameViewModel);

            // Assert
            A.CallTo(() => _service.EditGame(A<GameViewModel>.Ignored, gameViewModel, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var resultAsRedirectToRouteResult = result as RedirectToRouteResult;

            Assert.AreEqual("Index", resultAsRedirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual(gameViewModel.SeasonID, resultAsRedirectToRouteResult.RouteValues["seasonID"]);
        }

        [TestCase]
        public async Task EditPost_ModelStateNotValid()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);
            controller.ModelState.AddModelError("GuestScore", "Guest score is required.");

            var gameViewModel = new GameViewModel
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host"
            };

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            GamesService.SelectedWeek = new WeekViewModel(1);

            var teams = new List<TeamViewModel>();
            A.CallTo(() => _service.GetTeamsAsync(null)).Returns(teams);

            // Act
            var result = await controller.Edit(gameViewModel);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);
            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(gameViewModel.SeasonID, viewBagSeasonID.SelectedValues[0]);

            A.CallTo(() => _service.GetTeamsAsync(null)).MustHaveHappenedOnceExactly();

            var viewBagGuestName = viewBag.GuestName;
            Assert.IsInstanceOf<SelectList>(viewBagGuestName);
            Assert.AreSame(teams, viewBagGuestName.Items);
            Assert.AreEqual("Name", viewBagGuestName.DataValueField);
            Assert.AreEqual("Name", viewBagGuestName.DataTextField);
            Assert.AreEqual(gameViewModel.GuestName, viewBagGuestName.SelectedValues[0]);

            var viewBagHostName = viewBag.HostName;
            Assert.IsInstanceOf<SelectList>(viewBagHostName);
            Assert.AreSame(teams, viewBagHostName.Items);
            Assert.AreEqual("Name", viewBagHostName.DataValueField);
            Assert.AreEqual("Name", viewBagHostName.DataTextField);
            Assert.AreEqual(gameViewModel.HostName, viewBagHostName.SelectedValues[0]);

            Assert.AreSame(gameViewModel, resultAsViewResult.Model);
        }

        [TestCase]
        public void EditPost_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);
            controller.ModelState.AddModelError("GuestScore", "Guest score is required.");

            var gameViewModel = new GameViewModel
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host"
            };

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.Edit(gameViewModel));
        }

        [TestCase]
        public async Task DeleteGet_IdNull_ReturnsBadHttpRequest()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = null;

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (result as HttpStatusCodeResult).StatusCode);
        }

        [TestCase]
        public async Task DeleteGet_IdNotNullAndGameViewModelNull_ReturnsHttpNotFound()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = 1;

            GameViewModel gameViewModel = null;
            A.CallTo(() => _service.FindEntityAsync(A<int>.Ignored, null)).Returns(gameViewModel);

            // Act
            var result = await controller.Delete(id);

            // Assert
            A.CallTo(() => _service.FindEntityAsync((int)id, null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [TestCase]
        public async Task DeleteGet_IdNotNullAndGameViewModelNotNull_ReturnsViewResult()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = 1;

            GameViewModel gameViewModel = new GameViewModel();
            A.CallTo(() => _service.FindEntityAsync(A<int>.Ignored, null)).Returns(gameViewModel);

            // Act
            var result = await controller.Delete(id);

            // Assert
            A.CallTo(() => _service.FindEntityAsync((int)id, null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(gameViewModel, (result as ViewResult).Model);
        }

        [TestCase]
        public void DeleteGet_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int? id = 1;

            A.CallTo(() => _service.FindEntityAsync(A<int>.Ignored, null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.Delete(id));
        }

        [TestCase]
        public async Task DeleteConfirmed_HappyPath()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int id = 1;

            // Act
            var result = await controller.DeleteConfirmed(id);

            // Assert
            A.CallTo(() => _service.DeleteGame(id, null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var resultAsRedirectToRouteResult = result as RedirectToRouteResult;

            Assert.AreEqual("Index", resultAsRedirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual(GamesService.SelectedSeason, resultAsRedirectToRouteResult.RouteValues["seasonID"]);
        }

        [TestCase]
        public void DeleteConfirmed_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            int id = 1;

            A.CallTo(() => _service.DeleteGame(A<int>.Ignored, null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.DeleteConfirmed(id));
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var controller = new GamesController(_service, _sharedService);

            // Act

            // Assert
        }
    }
}
