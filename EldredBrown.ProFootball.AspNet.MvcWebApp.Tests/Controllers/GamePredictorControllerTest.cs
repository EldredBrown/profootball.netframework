using System;
using System.Collections.Generic;
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
    public class GamePredictorControllerTest
    {
        private IGamePredictorService _service;
        private ISharedService _sharedService;

        [SetUp]
        public void SetUp()
        {
            _service = A.Fake<IGamePredictorService>();
            _sharedService = A.Fake<ISharedService>();
        }

        [TestCase]
        public async Task PredictGameGet_HappyPath()
        {
            // Arrange
            var controller = new GamePredictorController(_service, _sharedService);

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            GamePredictorService.GuestSeasonID = 2017;
            var guestSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntities((int)GamePredictorService.GuestSeasonID, null))
                .Returns(guestSeasons);

            GamePredictorService.HostSeasonID = 2016;
            var hostSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntities((int)GamePredictorService.HostSeasonID, null))
                .Returns(hostSeasons);

            // Act
            var result = await controller.PredictGame();

            // Assert
            A.CallTo(() => _service.GetGuestAndHostSeasonIds(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);

            var viewBag = (result as ViewResult).ViewBag;

            var viewBagGuestSeasonID = viewBag.GuestSeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagGuestSeasonID);
            Assert.AreSame(seasons, viewBagGuestSeasonID.Items);
            Assert.AreEqual("ID", viewBagGuestSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagGuestSeasonID.DataTextField);
            Assert.AreEqual(GamePredictorService.GuestSeasonID, viewBagGuestSeasonID.SelectedValues[0]);

            A.CallTo(() => _service.GetEntities((int)GamePredictorService.GuestSeasonID, null))
                .MustHaveHappenedOnceExactly();

            var viewBagGuestName = viewBag.GuestName;
            Assert.IsInstanceOf<SelectList>(viewBagGuestName);
            Assert.AreSame(guestSeasons, viewBagGuestName.Items);
            Assert.AreEqual("TeamName", viewBagGuestName.DataValueField);
            Assert.AreEqual("TeamName", viewBagGuestName.DataTextField);

            var viewBagHostSeasonID = viewBag.HostSeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagHostSeasonID);
            Assert.AreSame(seasons, viewBagHostSeasonID.Items);
            Assert.AreEqual("ID", viewBagHostSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagHostSeasonID.DataTextField);
            Assert.AreEqual(GamePredictorService.HostSeasonID, viewBagHostSeasonID.SelectedValues[0]);

            A.CallTo(() => _service.GetEntities((int)GamePredictorService.HostSeasonID, null))
                .MustHaveHappenedOnceExactly();

            var viewBagHostName = viewBag.HostName;
            Assert.IsInstanceOf<SelectList>(viewBagHostName);
            Assert.AreSame(hostSeasons, viewBagHostName.Items);
            Assert.AreEqual("TeamName", viewBagHostName.DataValueField);
            Assert.AreEqual("TeamName", viewBagHostName.DataTextField);
        }

        [TestCase]
        public void PredictGameGet_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamePredictorController(_service, _sharedService);

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.PredictGame());
        }

        [TestCase]
        public async Task PredictGamePost_HappyPath()
        {
            // Arrange
            var controller = new GamePredictorController(_service, _sharedService);

            var guestName = "Guest";
            var guestSeasonID = 2017;
            var hostName = "Host";
            var hostSeasonID = 2016;

            GamePredictorService.GuestSeasonID = guestSeasonID;
            GamePredictorService.HostSeasonID = hostSeasonID;

            var matchup = new GamePredictionViewModel
            {
                GuestName = guestName,
                GuestSeasonID = guestSeasonID,
                HostName = hostName,
                HostSeasonID = hostSeasonID
            };

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var guestSeason = new TeamSeasonViewModel
            {
                TeamName = guestName,
                SeasonID = guestSeasonID
            };
            A.CallTo(() => _sharedService.FindEntityAsync(matchup.GuestName, matchup.GuestSeasonID, null))
                .Returns(guestSeason);

            var guestSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntities((int)GamePredictorService.GuestSeasonID, null))
                .Returns(guestSeasons);

            var hostSeason = new TeamSeasonViewModel
            {
                TeamName = hostName,
                SeasonID = hostSeasonID
            };
            A.CallTo(() => _sharedService.FindEntityAsync(matchup.HostName, matchup.HostSeasonID, null))
                .Returns(hostSeason);

            var hostSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntities((int)GamePredictorService.HostSeasonID, null))
                .Returns(hostSeasons);

            // Act
            var result = await controller.PredictGame(matchup);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;

            A.CallTo(() => _sharedService.FindEntityAsync(matchup.GuestName, matchup.GuestSeasonID, null))
                .MustHaveHappenedOnceExactly();

            var viewBagGuestSeasonID = viewBag.GuestSeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagGuestSeasonID);
            Assert.AreSame(seasons, viewBagGuestSeasonID.Items);
            Assert.AreEqual("ID", viewBagGuestSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagGuestSeasonID.DataTextField);
            Assert.AreEqual(guestSeason.SeasonID, viewBagGuestSeasonID.SelectedValues[0]);

            A.CallTo(() => _service.GetEntities((int)GamePredictorService.GuestSeasonID, null))
                .MustHaveHappenedOnceExactly();

            var viewBagGuestName = viewBag.GuestName;
            Assert.IsInstanceOf<SelectList>(viewBagGuestName);
            Assert.AreSame(guestSeasons, viewBagGuestName.Items);
            Assert.AreEqual("TeamName", viewBagGuestName.DataValueField);
            Assert.AreEqual("TeamName", viewBagGuestName.DataTextField);
            Assert.AreEqual(guestSeason.TeamName, viewBagGuestName.SelectedValues[0]);

            A.CallTo(() => _sharedService.FindEntityAsync(matchup.HostName, matchup.HostSeasonID, null))
                .MustHaveHappenedOnceExactly();

            var viewBagHostSeasonID = viewBag.HostSeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagHostSeasonID);
            Assert.AreSame(seasons, viewBagHostSeasonID.Items);
            Assert.AreEqual("ID", viewBagHostSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagHostSeasonID.DataTextField);
            Assert.AreEqual(hostSeason.SeasonID, viewBagHostSeasonID.SelectedValues[0]);

            A.CallTo(() => _service.GetEntities((int)GamePredictorService.HostSeasonID, null))
                .MustHaveHappenedOnceExactly();

            var viewBagHostName = viewBag.HostName;
            Assert.IsInstanceOf<SelectList>(viewBagHostName);
            Assert.AreSame(hostSeasons, viewBagHostName.Items);
            Assert.AreEqual("TeamName", viewBagHostName.DataValueField);
            Assert.AreEqual("TeamName", viewBagHostName.DataTextField);
            Assert.AreEqual(hostSeason.TeamName, viewBagHostName.SelectedValues[0]);

            Assert.AreSame(matchup, resultAsViewResult.Model);
        }

        [TestCase]
        public void PredictGamePost_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamePredictorController(_service, _sharedService);

            var guestName = "Guest";
            var guestSeasonID = 2017;
            var hostName = "Host";
            var hostSeasonID = 2016;

            var matchup = new GamePredictionViewModel
            {
                GuestName = guestName,
                GuestSeasonID = guestSeasonID,
                HostName = hostName,
                HostSeasonID = hostSeasonID
            };

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.PredictGame(matchup));
        }

        [TestCase]
        public void ApplyFilter_HappyPath()
        {
            // Arrange
            var controller = new GamePredictorController(_service, _sharedService);

            var guestSeasonID = 2017;
            var hostSeasonID = 2016;

            // Act
            var result = controller.ApplyFilter(guestSeasonID, hostSeasonID);

            // Assert
            A.CallTo(() => _service.ApplyFilter(guestSeasonID, hostSeasonID)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [TestCase]
        public void ApplyFilter_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new GamePredictorController(_service, _sharedService);

            var guestSeasonID = 2017;
            var hostSeasonID = 2016;

            A.CallTo(() => _service.ApplyFilter(A<int>.Ignored, A<int>.Ignored)).Throws<Exception>();

            // Act
            // Assert
            Assert.Throws<Exception>(() => controller.ApplyFilter(guestSeasonID, hostSeasonID));
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var controller = new GamePredictorController(_service, _sharedService);

            // Act

            // Assert
        }
    }
}
