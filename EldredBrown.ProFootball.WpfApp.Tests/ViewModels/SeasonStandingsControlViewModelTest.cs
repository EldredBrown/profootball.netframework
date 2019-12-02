using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EldredBrown.ProFootball.WpfApp;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootballWPF.Tests.ViewModels
{
    [TestFixture]
    public class SeasonStandingsControlViewModelTest
    {
        #region Member Fields

        private ISharedService _sharedService;
        private ISeasonStandingsControlService _controlService;
        private IRepository<Conference> _conferenceRepository;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _controlService = A.Fake<ISeasonStandingsControlService>();
            _conferenceRepository = A.Fake<IRepository<Conference>>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void ViewStandingsCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new SeasonStandingsControlViewModel(_sharedService, _controlService, _conferenceRepository);

            // Set up needed infrastructure of class under test.
            var leagues = new List<League>();
            A.CallTo(() => _controlService.GetLeaguesBySeason(A<int>.Ignored)).Returns(leagues);

            // Act
            viewModel.ViewStandingsCommand.Execute(null);

            // Assert
            Assert.IsNull(viewModel.Standings);

            A.CallTo(() => _controlService.GetLeaguesBySeason((int)WpfGlobals.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<ITreeViewItemViewModel>>(viewModel.LeagueNodes);
        }

        [TestCase]
        public void ViewStandingsCommand_ExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new SeasonStandingsControlViewModel(_sharedService, _controlService, _conferenceRepository);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _controlService.GetLeaguesBySeason(A<int>.Ignored)).Throws(ex);

            // Act
            viewModel.ViewStandingsCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new SeasonStandingsControlViewModel(_sharedService, _controlService, _conferenceRepository);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }

        #endregion Test Cases
    }
}
