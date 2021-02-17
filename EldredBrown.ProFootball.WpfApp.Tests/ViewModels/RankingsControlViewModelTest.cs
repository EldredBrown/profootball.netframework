using System;
using System.Collections.ObjectModel;
using EldredBrown.ProFootball.WpfApp;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootballWPF.Tests.ViewModels
{
    [TestFixture]
    public class RankingsControlViewModelTest
    {
        private ISharedService _sharedService;
        private IRankingsControlService _controlService;

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _controlService = A.Fake<IRankingsControlService>();
        }

        [TestCase]
        public void ViewRankingsCommand_HappyPath()
        {
            // Arrange
            var viewModel = new RankingsControlViewModel(_sharedService, _controlService);

            // Set up needed infrastructure of class under test.
            var seasonID = (int)WpfGlobals.SelectedSeason;

            // Act
            viewModel.ViewRankingsCommand.Execute(null);

            // Assert
            A.CallTo(() => _controlService.GetRankingsTotalBySeason(seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ReadOnlyCollection<GetRankingsTotal_Result>>(viewModel.TotalRankings);

            A.CallTo(() => _controlService.GetRankingsOffensiveBySeason(seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ReadOnlyCollection<GetRankingsOffensive_Result>>(viewModel.OffensiveRankings);

            A.CallTo(() => _controlService.GetRankingsDefensiveBySeason(seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ReadOnlyCollection<GetRankingsDefensive_Result>>(viewModel.DefensiveRankings);
        }

        [TestCase]
        public void ViewRankingsCommand_ExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            var viewModel = new RankingsControlViewModel(_sharedService, _controlService);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _controlService.GetRankingsTotalBySeason(A<int>.Ignored)).Throws(ex);

            // Act
            viewModel.ViewRankingsCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var viewModel = new RankingsControlViewModel(_sharedService, _controlService);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }
    }
}
