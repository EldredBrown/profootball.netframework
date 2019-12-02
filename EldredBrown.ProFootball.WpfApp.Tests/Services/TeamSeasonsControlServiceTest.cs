using System;
using System.Collections.Generic;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using EldredBrown.ProFootball.WpfApp.Services;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.WpfApp.Tests.Services
{
    [TestFixture]
    public class TeamSeasonsControlServiceTest
    {
        #region Member Fields

        private ISharedService _sharedService;
        private IRepository<TeamSeason> _teamSeasonRepository;
        private IStoredProcedureRepository _storedProcedureRepository;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _teamSeasonRepository = A.Fake<IRepository<TeamSeason>>();
            _storedProcedureRepository = A.Fake<IStoredProcedureRepository>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void GetEntities_HappyPath()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            var teamCount = 3;
            var seasonsPerTeam = 3;
            var teamSeasons = new List<TeamSeason>(teamCount * seasonsPerTeam);
            for (int i = 1; i <= teamCount; i++)
            {
                var lastSeason = 2017;
                for (int j = lastSeason; j > lastSeason - seasonsPerTeam; j--)
                {
                    var teamSeason = new TeamSeason
                    {
                        TeamName = $"Team {i}",
                        SeasonID = j
                    };
                    teamSeasons.Add(teamSeason);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Returns(teamSeasons);

            // Act
            var result = service.GetEntities(seasonID);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<TeamSeason>>(result);

            foreach (var teamSeason in result)
            {
                Assert.AreEqual(seasonID, teamSeason.SeasonID);
            }
        }

        [TestCase]
        public void GetEntities_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Throws(ex);

            // Act
            var result = service.GetEntities(seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<TeamSeason>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetEntities_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Throws<Exception>();

            // Act
            IEnumerable<TeamSeason> result = null;
            Assert.Throws<Exception>(() => result = service.GetEntities(seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleProfile_HappyPath()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var teamName = "Team";
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeTeamSeasonScheduleProfile();

            var teamSeasonScheduleProfile = dbContext.GetTeamSeasonScheduleProfile(teamName, seasonID);
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleProfile(teamName, seasonID))
                .Returns(teamSeasonScheduleProfile);

            // Act
            var result = service.GetTeamSeasonScheduleProfile(teamName, seasonID);

            // Assert
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleProfile(teamName, seasonID))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetTeamSeasonScheduleProfile_Result>>(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleProfile_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var teamName = "Team";
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleProfile(teamName, seasonID)).Throws(ex);

            // Act
            var result = service.GetTeamSeasonScheduleProfile(teamName, seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetTeamSeasonScheduleProfile_Result>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleProfile_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var teamName = "Team";
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleProfile(teamName, seasonID))
                .Throws<Exception>();

            // Act
            IEnumerable<GetTeamSeasonScheduleProfile_Result> result = null;
            Assert.Throws<Exception>(() => result = service.GetTeamSeasonScheduleProfile(teamName, seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleTotals_HappyPath()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var teamName = "Team";
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeTeamSeasonScheduleTotals();

            var teamSeasonScheduleTotals = dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID);
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamName, seasonID))
                .Returns(teamSeasonScheduleTotals);

            // Act
            var result = service.GetTeamSeasonScheduleTotals(teamName, seasonID);

            // Assert
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamName, seasonID))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetTeamSeasonScheduleTotals_Result>>(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleTotals_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var teamName = "Team";
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamName, seasonID)).Throws(ex);

            // Act
            var result = service.GetTeamSeasonScheduleTotals(teamName, seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetTeamSeasonScheduleTotals_Result>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleTotals_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var teamName = "Team";
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamName, seasonID))
                .Throws<Exception>();

            // Act
            IEnumerable<GetTeamSeasonScheduleTotals_Result> result = null;
            Assert.Throws<Exception>(() => result = service.GetTeamSeasonScheduleTotals(teamName, seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleAverages_HappyPath()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var teamName = "Team";
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeTeamSeasonScheduleAverages();

            var teamSeasonScheduleAverages = dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID);
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamName, seasonID))
                .Returns(teamSeasonScheduleAverages);

            // Act
            var result = service.GetTeamSeasonScheduleAverages(teamName, seasonID);

            // Assert
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamName, seasonID))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetTeamSeasonScheduleAverages_Result>>(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleAverages_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var teamName = "Team";
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamName, seasonID)).Throws(ex);

            // Act
            var result = service.GetTeamSeasonScheduleAverages(teamName, seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetTeamSeasonScheduleAverages_Result>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleAverages_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.
            var teamName = "Team";
            var seasonID = 2017;

            // Set up required infrastructure of class under test.
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamName, seasonID))
                .Throws<Exception>();

            // Act
            IEnumerable<GetTeamSeasonScheduleAverages_Result> result = null;
            Assert.Throws<Exception>(() => result = service.GetTeamSeasonScheduleAverages(teamName, seasonID));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new TeamSeasonsControlService(_sharedService, _teamSeasonRepository,
                _storedProcedureRepository);

            // Define argument variables of method under test.

            // Set up required infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }

        #endregion Test Cases
    }
}
