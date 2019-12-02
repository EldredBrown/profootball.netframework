using System;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.WpfApp.Tests.Services
{
    [TestFixture]
    public class SharedServiceTest
    {
        #region Member Fields

        private IRepository<TeamSeason> _teamSeasonRepository;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _teamSeasonRepository = A.Fake<IRepository<TeamSeason>>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var service = new SharedService(_teamSeasonRepository);

            var teamName = "Team";
            var seasonID = 2017;

            var teamSeason = new TeamSeason
            {
                TeamName = teamName,
                SeasonID = seasonID
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(teamSeason);

            // Act
            var result = service.FindTeamSeason(teamName, seasonID);

            // Assert
            A.CallTo(() => _teamSeasonRepository.FindEntity(teamName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeason, result);
        }

        [TestCase]
        public void FindEntity_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new SharedService(_teamSeasonRepository);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => _teamSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Throws<Exception>();

            // Act
            TeamSeason result = null;
            Assert.Throws<Exception>(() => result = service.FindTeamSeason(teamName, seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void SaveChanges_HappyPath()
        {
            // Arrange
            var service = new SharedService(_teamSeasonRepository);

            // Define argument variables of method under test.
            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            service.SaveChanges(dbContext);

            // Assert
            A.CallTo(() => dbContext.SaveChanges()).MustHaveHappenedOnceExactly();
        }

        //[TestCase]
        public void SaveChanges_ExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            var service = new SharedService(_teamSeasonRepository);

            // Define argument variables of method under test.
            var dbContext = A.Fake<ProFootballEntities>();

            // Set up needed infrastructure of class under test.
            A.CallTo(() => dbContext.SaveChanges()).Throws<Exception>();

            // Act
            service.SaveChanges(dbContext);

            // Assert
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new SharedService(_teamSeasonRepository);

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
