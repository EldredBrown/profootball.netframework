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
    public class TeamSeasonsControllerTest
    {
        #region Member Fields

        private ITeamSeasonsService _service;
        private ISharedService _sharedService;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _service = A.Fake<ITeamSeasonsService>();
            _sharedService = A.Fake<ISharedService>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public async Task Index_SortOrderIsTeamAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "team_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsTeamDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "team_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_desc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsWinsAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "wins_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsWinsDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "wins_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_desc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsLossesAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "losses_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsLossesDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "losses_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_desc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsTiesAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "ties_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsTiesDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "ties_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_desc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsWinPctAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "win_pct_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsWinPctDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "win_pct_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_desc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsPointsForAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "pf_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsPointsForDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "pf_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_desc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsPointsAgainstAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "pa_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsPointsAgainstDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "pa_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_desc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsPythWinsAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "pyth_wins_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsPythWinsDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "pyth_wins_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_desc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsPythLossesAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "pyth_losses_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsPythLossesDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "pyth_losses_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_desc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsOffensiveAverageAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "off_avg_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsOffensiveAverageDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "off_avg_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_desc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsOffensiveFactorAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "off_factor_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsOffensiveFactorDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "off_factor_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_desc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsOffensiveIndexAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "off_index_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsOffensiveIndexDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "off_index_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_desc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsDefensiveAverageAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "def_avg_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsDefensiveAverageDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "def_avg_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_desc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsDefensiveFactorAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "def_factor_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsDefensiveFactorDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "def_factor_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_desc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsDefensiveIndexAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "def_index_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsDefensiveIndexDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "def_index_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_desc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsFinalPythWinPctAscending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "fin_pyth_pct_asc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_asc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public async Task Index_SortOrderIsFinalPythWinPctDescending()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "fin_pyth_pct_desc";

            var seasons = new List<SeasonViewModel>();
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Returns(seasons);

            var teamSeasons = new List<TeamSeasonViewModel>();
            A.CallTo(() => _service.GetEntitiesOrderedAsync(A<int>.Ignored, A<string>.Ignored, null))
                .Returns(teamSeasons);

            // Act
            var result = await controller.Index(seasonID, sortOrder);

            // Assert
            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _service.SetSelectedSeason(seasons, seasonID, sortOrder)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            var resultAsViewResult = result as ViewResult;

            var viewBag = resultAsViewResult.ViewBag;
            Assert.IsNotNull(viewBag);

            var viewBagSeasonID = viewBag.SeasonID;
            Assert.IsInstanceOf<SelectList>(viewBagSeasonID);

            Assert.AreSame(seasons, viewBagSeasonID.Items);
            Assert.AreEqual("ID", viewBagSeasonID.DataValueField);
            Assert.AreEqual("ID", viewBagSeasonID.DataTextField);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, viewBagSeasonID.SelectedValues[0]);

            Assert.AreEqual("team_asc", viewBag.TeamSortParm);
            Assert.AreEqual("wins_asc", viewBag.WinsSortParm);
            Assert.AreEqual("losses_asc", viewBag.LossesSortParm);
            Assert.AreEqual("ties_asc", viewBag.TiesSortParm);
            Assert.AreEqual("win_pct_asc", viewBag.WinningPercentageSortParm);
            Assert.AreEqual("pf_asc", viewBag.PointsForSortParm);
            Assert.AreEqual("pa_asc", viewBag.PointsAgainstSortParm);
            Assert.AreEqual("pyth_wins_asc", viewBag.PythagoreanWinsSortParm);
            Assert.AreEqual("pyth_losses_asc", viewBag.PythagoreanLossesSortParm);
            Assert.AreEqual("off_avg_asc", viewBag.OffensiveAverageSortParm);
            Assert.AreEqual("off_factor_asc", viewBag.OffensiveFactorSortParm);
            Assert.AreEqual("off_index_asc", viewBag.OffensiveIndexSortParm);
            Assert.AreEqual("def_avg_asc", viewBag.DefensiveAverageSortParm);
            Assert.AreEqual("def_factor_asc", viewBag.DefensiveFactorSortParm);
            Assert.AreEqual("def_index_asc", viewBag.DefensiveIndexSortParm);
            Assert.AreEqual("fin_pyth_pct_desc", viewBag.FinalPythagoreanWinningPercentageSortParm);

            A.CallTo(() => _service.GetEntitiesOrderedAsync(TeamSeasonsService.SelectedSeason, sortOrder, null))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeasons, resultAsViewResult.Model);
        }

        [TestCase]
        public void Index_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            var seasonID = 2017;
            var sortOrder = "fin_pyth_pct_desc";

            A.CallTo(() => _sharedService.GetSeasonsOrderedAsync(null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.Index(seasonID, sortOrder));
        }

        [TestCase]
        public async Task Details_TeamNameNull_ReturnsBadHttpRequest()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            string teamName = null;
            int? seasonID = 2017;

            // Act
            var result = await controller.Details(teamName, seasonID);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (result as HttpStatusCodeResult).StatusCode);
        }

        [TestCase]
        public async Task Details_TeamNameEmpty_ReturnsBadHttpRequest()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            string teamName = String.Empty;
            int? seasonID = 2017;

            // Act
            var result = await controller.Details(teamName, seasonID);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (result as HttpStatusCodeResult).StatusCode);
        }

        [TestCase]
        public async Task Details_SeasonIdNull_ReturnsBadHttpRequest()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            string teamName = "Team";
            int? seasonID = null;

            // Act
            var result = await controller.Details(teamName, seasonID);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (result as HttpStatusCodeResult).StatusCode);
        }

        [TestCase]
        public async Task Details_TeamNameNeitherNullNorEmptyAndSeasonIdNotNull_TeamSeasonNull_ReturnsHttpNotFound()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            string teamName = "Team";
            int? seasonID = 2017;

            var teamSeasonDetails = new TeamSeasonDetailsViewModel
            {
                TeamSeason = null
            };
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(A<string>.Ignored, A<int>.Ignored, null))
                .Returns(teamSeasonDetails);

            // Act
            var result = await controller.Details(teamName, seasonID);

            // Assert
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(teamName, (int)seasonID, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [TestCase]
        public async Task Details_TeamNameNeitherNullNorEmptyAndSeasonIdNotNull_TeamSeasonScheduleProfileNull_ReturnsHttpNotFound()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            string teamName = "Team";
            int? seasonID = 2017;

            var teamSeasonDetails = new TeamSeasonDetailsViewModel
            {
                TeamSeason = new TeamSeasonViewModel(),
                TeamSeasonScheduleProfile = null
            };
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(A<string>.Ignored, A<int>.Ignored, null))
                .Returns(teamSeasonDetails);

            // Act
            var result = await controller.Details(teamName, seasonID);

            // Assert
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(teamName, (int)seasonID, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [TestCase]
        public async Task Details_TeamNameNeitherNullNorEmptyAndSeasonIdNotNull_TeamSeasonScheduleTotalsNull_ReturnsHttpNotFound()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            string teamName = "Team";
            int? seasonID = 2017;

            var teamSeasonDetails = new TeamSeasonDetailsViewModel
            {
                TeamSeason = new TeamSeasonViewModel(),
                TeamSeasonScheduleProfile = new List<TeamSeasonScheduleProfileViewModel>(),
                TeamSeasonScheduleTotals = null
            };
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(A<string>.Ignored, A<int>.Ignored, null))
                .Returns(teamSeasonDetails);

            // Act
            var result = await controller.Details(teamName, seasonID);

            // Assert
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(teamName, (int)seasonID, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [TestCase]
        public async Task Details_TeamNameNeitherNullNorEmptyAndSeasonIdNotNull_TeamSeasonScheduleAveragesNull_ReturnsHttpNotFound()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            string teamName = "Team";
            int? seasonID = 2017;

            var teamSeasonDetails = new TeamSeasonDetailsViewModel
            {
                TeamSeason = new TeamSeasonViewModel(),
                TeamSeasonScheduleProfile = new List<TeamSeasonScheduleProfileViewModel>(),
                TeamSeasonScheduleTotals = new TeamSeasonScheduleTotalsViewModel(),
                TeamSeasonScheduleAverages = null
            };
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(A<string>.Ignored, A<int>.Ignored, null))
                .Returns(teamSeasonDetails);

            // Act
            var result = await controller.Details(teamName, seasonID);

            // Assert
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(teamName, (int)seasonID, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [TestCase]
        public async Task Details_TeamNameNeitherNullNorEmptyAndSeasonIdNotNull_NoComponentViewModelsNull_ReturnsView()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            string teamName = "Team";
            int? seasonID = 2017;

            var teamSeasonDetails = new TeamSeasonDetailsViewModel
            {
                TeamSeason = new TeamSeasonViewModel(),
                TeamSeasonScheduleProfile = new List<TeamSeasonScheduleProfileViewModel>(),
                TeamSeasonScheduleTotals = new TeamSeasonScheduleTotalsViewModel(),
                TeamSeasonScheduleAverages = new TeamSeasonScheduleAveragesViewModel()
            };
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(A<string>.Ignored, A<int>.Ignored, null))
                .Returns(teamSeasonDetails);

            // Act
            var result = await controller.Details(teamName, seasonID);

            // Assert
            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(teamName, (int)seasonID, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(teamSeasonDetails, (result as ViewResult).Model);
        }

        [TestCase]
        public void Details_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            string teamName = "Team";
            int? seasonID = 2017;

            A.CallTo(() => _service.GetTeamSeasonDetailsAsync(A<string>.Ignored, A<int>.Ignored, null))
                .Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.Details(teamName, seasonID));
        }

        [TestCase]
        public async Task UpdateRankings_NoExceptionCaught_RankingsUpdated()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            // Act
            await controller.UpdateRankings();

            // Assert
            A.CallTo(() => _service.UpdateRankings(null)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void UpdateRankings_ExceptionCaught_RethrowsException()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            A.CallTo(() => _service.UpdateRankings(null)).Throws<Exception>();

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.UpdateRankings());
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var controller = new TeamSeasonsController(_service, _sharedService);

            // Act

            // Assert
        }

        #endregion Test Cases
    }
}
