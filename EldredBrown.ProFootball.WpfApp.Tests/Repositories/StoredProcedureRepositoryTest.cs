using System;
using System.Data.Entity.Core.Objects;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.WpfApp.Tests.Repositories
{
    [TestFixture]
    public class StoredProcedureRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void GetLeagueSeasonTotals_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var leagueName = "League";
            var seasonID = 2017;

            var leagueSeasonTotals = A.Fake<ObjectResult<GetLeagueSeasonTotals_Result>>();
            A.CallTo(() => dbContext.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            // Act
            var result = repository.GetLeagueSeasonTotals(leagueName, seasonID);

            // Assert
            A.CallTo(() => dbContext.GetLeagueSeasonTotals(leagueName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeasonTotals, result);
        }

        [TestCase]
        public void GetLeagueSeasonTotals_ArgumentExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var leagueName = "League";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Throws<ArgumentException>();

            // Act
            ObjectResult<GetLeagueSeasonTotals_Result> result = null;
            Assert.Throws<ArgumentException>(
                () => { result = repository.GetLeagueSeasonTotals(leagueName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetLeagueSeasonTotals_InvalidOperationExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var leagueName = "League";
            var seasonID = 2017;

            var leagueSeasonTotals = A.Fake<ObjectResult<GetLeagueSeasonTotals_Result>>();
            A.CallTo(() => dbContext.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Throws<InvalidOperationException>();

            // Act
            ObjectResult<GetLeagueSeasonTotals_Result> result = null;
            Assert.Throws<InvalidOperationException>(
                () => { result = repository.GetLeagueSeasonTotals(leagueName, seasonID); });

            // Assert
            A.CallTo(() => dbContext.GetLeagueSeasonTotals(leagueName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetLeagueSeasonTotals_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var leagueName = "League";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Throws<Exception>();

            // Act
            ObjectResult<GetLeagueSeasonTotals_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = repository.GetLeagueSeasonTotals(leagueName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetRankingsOffensive_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;

            var offensiveRankings = A.Fake<ObjectResult<GetRankingsOffensive_Result>>();
            A.CallTo(() => dbContext.GetRankingsOffensive(A<int>.Ignored)).Returns(offensiveRankings);

            // Act
            var result = repository.GetRankingsOffensive(seasonID);

            // Assert
            A.CallTo(() => dbContext.GetRankingsOffensive(seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(offensiveRankings, result);
        }

        [TestCase]
        public void GetRankingsOffensive_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;

            A.CallTo(() => dbContext.GetRankingsOffensive(A<int>.Ignored)).Throws<Exception>();

            // Act
            ObjectResult<GetRankingsOffensive_Result> result = null;
            Assert.Throws<Exception>(() => result = result = repository.GetRankingsOffensive(seasonID));
            
            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetRankingsDefensive_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;

            var defensiveRankings = A.Fake<ObjectResult<GetRankingsDefensive_Result>>();
            A.CallTo(() => dbContext.GetRankingsDefensive(A<int>.Ignored)).Returns(defensiveRankings);

            // Act
            var result = repository.GetRankingsDefensive(seasonID);

            // Assert
            A.CallTo(() => dbContext.GetRankingsDefensive(seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(defensiveRankings, result);
        }

        [TestCase]
        public void GetRankingsDefensive_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;

            A.CallTo(() => dbContext.GetRankingsDefensive(A<int>.Ignored)).Throws<Exception>();

            // Act
            ObjectResult<GetRankingsDefensive_Result> result = null;
            Assert.Throws<Exception>(() => result = result = repository.GetRankingsDefensive(seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetRankingsTotal_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;

            var totalRankings = A.Fake<ObjectResult<GetRankingsTotal_Result>>();
            A.CallTo(() => dbContext.GetRankingsTotal(A<int>.Ignored)).Returns(totalRankings);

            // Act
            var result = repository.GetRankingsTotal(seasonID);

            // Assert
            A.CallTo(() => dbContext.GetRankingsTotal(seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(totalRankings, result);
        }

        [TestCase]
        public void GetRankingsTotal_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;

            A.CallTo(() => dbContext.GetRankingsTotal(A<int>.Ignored)).Throws<Exception>();

            // Act
            ObjectResult<GetRankingsTotal_Result> result = null;
            Assert.Throws<Exception>(() => result = result = repository.GetRankingsTotal(seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetSeasonStandingsForLeague_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;
            var leagueName = "League";

            var standings = A.Fake<ObjectResult<GetSeasonStandingsForLeague_Result>>();
            A.CallTo(() => dbContext.GetSeasonStandingsForLeague(A<int>.Ignored, A<string>.Ignored))
                .Returns(standings);

            // Act
            var result = repository.GetSeasonStandingsForLeague(seasonID, leagueName);

            // Assert
            A.CallTo(() => dbContext.GetSeasonStandingsForLeague(seasonID, leagueName))
                .MustHaveHappenedOnceExactly();
            Assert.AreSame(standings, result);
        }

        [TestCase]
        public void GetSeasonStandingsForLeague_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;
            var leagueName = "League";

            A.CallTo(() => dbContext.GetSeasonStandingsForLeague(A<int>.Ignored, A<string>.Ignored))
                .Throws<Exception>();

            // Act
            ObjectResult<GetSeasonStandingsForLeague_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = repository.GetSeasonStandingsForLeague(seasonID, leagueName); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetSeasonStandingsForConference_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;
            var conferenceName = "Conference";

            var standings = A.Fake<ObjectResult<GetSeasonStandingsForConference_Result>>();
            A.CallTo(() => dbContext.GetSeasonStandingsForConference(A<int>.Ignored, A<string>.Ignored))
                .Returns(standings);

            // Act
            var result = repository.GetSeasonStandingsForConference(seasonID, conferenceName);

            // Assert
            A.CallTo(() => dbContext.GetSeasonStandingsForConference(seasonID, conferenceName))
                .MustHaveHappenedOnceExactly();
            Assert.AreSame(standings, result);
        }

        [TestCase]
        public void GetSeasonStandingsForConference_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;
            var conferenceName = "Conference";

            A.CallTo(() => dbContext.GetSeasonStandingsForConference(A<int>.Ignored, A<string>.Ignored))
                .Throws<Exception>();

            // Act
            ObjectResult<GetSeasonStandingsForConference_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = repository.GetSeasonStandingsForConference(seasonID, conferenceName); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetSeasonStandingsForDivision_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;
            var divisionName = "Division";

            var standings = A.Fake<ObjectResult<GetSeasonStandingsForDivision_Result>>();
            A.CallTo(() => dbContext.GetSeasonStandingsForDivision(A<int>.Ignored, A<string>.Ignored))
                .Returns(standings);

            // Act
            var result = repository.GetSeasonStandingsForDivision(seasonID, divisionName);

            // Assert
            A.CallTo(() => dbContext.GetSeasonStandingsForDivision(seasonID, divisionName)).
                MustHaveHappenedOnceExactly();
            Assert.AreSame(standings, result);
        }

        [TestCase]
        public void GetSeasonStandingsForDivision_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var seasonID = 2017;
            var divisionName = "Division";

            A.CallTo(() => dbContext.GetSeasonStandingsForDivision(A<int>.Ignored, A<string>.Ignored))
                .Throws<Exception>();

            // Act
            ObjectResult<GetSeasonStandingsForDivision_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = repository.GetSeasonStandingsForDivision(seasonID, divisionName); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleProfile_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            var teamSeasonScheduleProfile = A.Fake<ObjectResult<GetTeamSeasonScheduleProfile_Result>>();
            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(A<string>.Ignored, A<int>.Ignored))
                .Returns(teamSeasonScheduleProfile);

            // Act
            var result = repository.GetTeamSeasonScheduleProfile(teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeasonScheduleProfile, result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleProfile_ArgumentExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(A<string>.Ignored, A<int>.Ignored))
                .Throws<ArgumentException>();

            // Act
            ObjectResult<GetTeamSeasonScheduleProfile_Result> result = null;
            Assert.Throws<ArgumentException>(
                () => { result = repository.GetTeamSeasonScheduleProfile(teamName, seasonID); });

            // Assert
            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleProfile_InvalidOperationExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(A<string>.Ignored, A<int>.Ignored))
                .Throws<InvalidOperationException>();

            // Act
            ObjectResult<GetTeamSeasonScheduleProfile_Result> result = null;
            Assert.Throws<InvalidOperationException>(
                () => { result = repository.GetTeamSeasonScheduleProfile(teamName, seasonID); });

            // Assert
            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleProfile_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(A<string>.Ignored, A<int>.Ignored))
                .Throws<Exception>();

            // Act
            ObjectResult<GetTeamSeasonScheduleProfile_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = repository.GetTeamSeasonScheduleProfile(teamName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleTotals_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            var teamSeasonScheduleTotals = A.Fake<ObjectResult<GetTeamSeasonScheduleTotals_Result>>();
            A.CallTo(() => dbContext.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(teamSeasonScheduleTotals);

            // Act
            var result = repository.GetTeamSeasonScheduleTotals(teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeasonScheduleTotals, result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleTotals_ArgumentExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .Throws<ArgumentException>();

            // Act
            ObjectResult<GetTeamSeasonScheduleTotals_Result> result = null;
            Assert.Throws<ArgumentException>(
                () => { result = repository.GetTeamSeasonScheduleTotals(teamName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleTotals_InvalidOperationExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .Throws<InvalidOperationException>();

            // Act
            ObjectResult<GetTeamSeasonScheduleTotals_Result> result = null;
            Assert.Throws<InvalidOperationException>(
                () => { result = repository.GetTeamSeasonScheduleTotals(teamName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleTotals_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .Throws<Exception>();

            // Act
            ObjectResult<GetTeamSeasonScheduleTotals_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = repository.GetTeamSeasonScheduleTotals(teamName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleAverages_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            var teamSeasonScheduleAverages = A.Fake<ObjectResult<GetTeamSeasonScheduleAverages_Result>>();
            A.CallTo(() => dbContext.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .Returns(teamSeasonScheduleAverages);

            // Act
            var result = repository.GetTeamSeasonScheduleAverages(teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeasonScheduleAverages, result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleAverages_ArgumentExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .Throws<ArgumentException>();

            // Act
            ObjectResult<GetTeamSeasonScheduleAverages_Result> result = null;
            Assert.Throws<ArgumentException>(
                () => { result = repository.GetTeamSeasonScheduleAverages(teamName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleAverages_InvalidOperationExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .Throws<InvalidOperationException>();

            // Act
            ObjectResult<GetTeamSeasonScheduleAverages_Result> result = null;
            Assert.Throws<InvalidOperationException>(
                () => { result = repository.GetTeamSeasonScheduleAverages(teamName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleAverages_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .Throws<Exception>();

            // Act
            ObjectResult<GetTeamSeasonScheduleAverages_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = repository.GetTeamSeasonScheduleAverages(teamName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new StoredProcedureRepository(dbContext);

            // Act

            // Assert
        }

        #endregion Test Cases
    }
}
