using System.Data.Entity.Core.Objects;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Tests.Repositories
{
    [TestFixture]
    public class StoredProcedureRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void GetLeagueSeasonTotals()
        {
            // Arrange
            var repository = new StoredProcedureRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueName = "League";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(A.Fake<ObjectResult<GetLeagueSeasonTotals_Result>>());

            // Act
            var result = repository.GetLeagueSeasonTotals(dbContext, leagueName, seasonID);

            // Assert
            A.CallTo(() => dbContext.GetLeagueSeasonTotals(leagueName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ObjectResult<GetLeagueSeasonTotals_Result>>(result);
        }

        [TestCase]
        public void GetSeasonStandings()
        {
            // Arrange
            var repository = new StoredProcedureRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var seasonID = 2017;
            var groupByDivision = false;

            A.CallTo(() => dbContext.GetSeasonStandings(A<int>.Ignored, A<bool>.Ignored))
                .Returns(A.Fake<ObjectResult<GetSeasonStandings_Result>>());

            // Act
            var result = repository.GetSeasonStandings(dbContext, seasonID, groupByDivision);

            // Assert
            A.CallTo(() => dbContext.GetSeasonStandings(seasonID, groupByDivision)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ObjectResult<GetSeasonStandings_Result>>(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleAverages()
        {
            // Arrange
            var repository = new StoredProcedureRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .Returns(A.Fake<ObjectResult<GetTeamSeasonScheduleAverages_Result>>());

            // Act
            var result = repository.GetTeamSeasonScheduleAverages(dbContext, teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ObjectResult<GetTeamSeasonScheduleAverages_Result>>(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleProfile()
        {
            // Arrange
            var repository = new StoredProcedureRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(A<string>.Ignored, A<int>.Ignored))
                .Returns(A.Fake<ObjectResult<GetTeamSeasonScheduleProfile_Result>>());

            // Act
            var result = repository.GetTeamSeasonScheduleProfile(dbContext, teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ObjectResult<GetTeamSeasonScheduleProfile_Result>>(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleTotals()
        {
            // Arrange
            var repository = new StoredProcedureRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(A.Fake<ObjectResult<GetTeamSeasonScheduleTotals_Result>>());

            // Act
            var result = repository.GetTeamSeasonScheduleTotals(dbContext, teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<ObjectResult<GetTeamSeasonScheduleTotals_Result>>(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new StoredProcedureRepository();

            // Act

            // Assert
        }

        #endregion Test Cases
    }
}
