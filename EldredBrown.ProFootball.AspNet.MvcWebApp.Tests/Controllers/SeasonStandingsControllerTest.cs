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
    public class SeasonStandingsControllerTest
    {
        private ISeasonStandingsService _service;
        private ISharedService _sharedService;

        [SetUp]
        public void SetUp()
        {
            _service = A.Fake<ISeasonStandingsService>();
            _sharedService = A.Fake<ISharedService>();
        }

        [TestCase]
        public async Task Index_GroupByDivisionNull_GroupByDivisionSetToFalse()
        {
            // Arrange
            var controller = new SeasonStandingsController(_service, _sharedService);

            var seasonID = 2017;
            bool? groupByDivision = null;

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var seasonStandingsResults = new List<SeasonStandingsResultViewModel>();
            A.CallTo(() => _service.GetSeasonStandings(A<int>.Ignored, A<bool>.Ignored, null))
                .Returns(seasonStandingsResults);

            // Act
            var result = await controller.Index(seasonID, groupByDivision);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);
            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(SeasonStandingsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.IsFalse(viewBag.GroupByDivision);

            A.CallTo(() => _service.GetSeasonStandings(SeasonStandingsService.SelectedSeason, false, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(seasonStandingsResults, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_GroupByDivisionNotNull_GroupByDivisionNotReset()
        {
            // Arrange
            var controller = new SeasonStandingsController(_service, _sharedService);

            var seasonID = 2017;
            bool? groupByDivision = true;

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var seasonStandingsResults = new List<SeasonStandingsResultViewModel>();
            A.CallTo(() => _service.GetSeasonStandings(A<int>.Ignored, A<bool>.Ignored, null))
                .Returns(seasonStandingsResults);

            // Act
            var result = await controller.Index(seasonID, groupByDivision);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);
            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(SeasonStandingsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.IsTrue(viewBag.GroupByDivision);

            A.CallTo(() => _service.GetSeasonStandings(SeasonStandingsService.SelectedSeason, true, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(seasonStandingsResults, resultAsViewResult.Model);
        }

        [TestCase]
        public void Index_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new SeasonStandingsController(_service, _sharedService);

            var seasonID = 2017;
            bool? groupByDivision = true;

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.Index(seasonID, groupByDivision));
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var controller = new SeasonStandingsController(_service, _sharedService);

            // Act

            // Assert
        }
    }
}
