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
    public class TeamSeasonsControlViewModelTest
    {
        private ISharedService _sharedService;
        private ITeamSeasonsControlService _controlService;

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _controlService = A.Fake<ITeamSeasonsControlService>();
        }

        [TestCase]
        public void ViewTeamScheduleCommand_SelectedTeamIsNull_SetsTeamSeasonScheduleProfileTotalsAndAveragesToNull()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new TeamSeasonsControlViewModel(_sharedService, _controlService)
            {
                // Set up needed infrastructure of class under test.
                SelectedTeam = null
            };

            // Act
            viewModel.ViewTeamScheduleCommand.Execute(null);

            // Assert
            Assert.IsNull(viewModel.TeamSeasonScheduleProfile);
            Assert.IsNull(viewModel.TeamSeasonScheduleTotals);
            Assert.IsNull(viewModel.TeamSeasonScheduleAverages);
        }

        [TestCase]
        public void ViewTeamScheduleCommand_SelectedTeamIsNotNull_SetsTeamSeasonScheduleProfileTotalsAndAveragesToNotNull()
        {
            // Arrange
            // Instantiate class under test.
            var teamName = "Team";
            WpfGlobals.SelectedSeason = 2017;

            var viewModel = new TeamSeasonsControlViewModel(_sharedService, _controlService)
            {
                // Set up needed infrastructure of class under test.
                SelectedTeam = new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = (int)WpfGlobals.SelectedSeason
                }
            };

            // Act
            viewModel.ViewTeamScheduleCommand.Execute(null);

            // Assert
            A.CallTo(() => _controlService.GetTeamSeasonScheduleProfile(teamName, WpfGlobals.SelectedSeason))
                .MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ReadOnlyCollection<GetTeamSeasonScheduleProfile_Result>>(
                viewModel.TeamSeasonScheduleProfile);

            A.CallTo(() => _controlService.GetTeamSeasonScheduleTotals(teamName, WpfGlobals.SelectedSeason))
                .MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ReadOnlyCollection<GetTeamSeasonScheduleTotals_Result>>(
                viewModel.TeamSeasonScheduleTotals);

            A.CallTo(() => _controlService.GetTeamSeasonScheduleAverages(teamName, WpfGlobals.SelectedSeason))
                .MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ReadOnlyCollection<GetTeamSeasonScheduleAverages_Result>>(
                viewModel.TeamSeasonScheduleAverages);
        }

        [TestCase]
        public void ViewTeamScheduleCommand_ExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var teamName = "Team";
            WpfGlobals.SelectedSeason = 2017;

            var viewModel = new TeamSeasonsControlViewModel(_sharedService, _controlService)
            {
                // Set up needed infrastructure of class under test.
                SelectedTeam = new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = (int)WpfGlobals.SelectedSeason
                }
            };

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _controlService.GetTeamSeasonScheduleProfile(A<string>.Ignored, A<int>.Ignored)).Throws(ex);
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).DoesNothing();

            // Act
            viewModel.ViewTeamScheduleCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void ViewTeamsCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new TeamSeasonsControlViewModel(_sharedService, _controlService);

            // Act
            viewModel.ViewTeamsCommand.Execute(null);

            // Assert
            A.CallTo(() => _controlService.GetEntities((int)WpfGlobals.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<TeamSeason>>(viewModel.Teams);
        }

        [TestCase]
        public void ViewTeamsCommand_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new TeamSeasonsControlViewModel(_sharedService, _controlService);

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _controlService.GetEntities(A<int>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => viewModel.ViewTeamsCommand.Execute(null));
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            // TODO: Instantiate class under test.
            var viewModel = new TeamSeasonsControlViewModel(_sharedService, _controlService);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }
    }
}
