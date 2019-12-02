using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.WpfApp.Tests.Repositories
{
    [TestFixture]
    public class TeamSeasonRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddTeamSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeason = new TeamSeason();
            A.CallTo(() => dbContext.TeamSeasons.Add(A<TeamSeason>.Ignored)).Returns(teamSeason);

            // Act
            var result = repository.AddEntity(teamSeason);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.Add(teamSeason)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeason, result);
        }

        [TestCase]
        public void AddTeamSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeason = new TeamSeason();
            A.CallTo(() => dbContext.TeamSeasons.Add(A<TeamSeason>.Ignored)).Throws<Exception>();

            // Act
            TeamSeason result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntity(teamSeason));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void AddTeamSeasons_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeasons = new List<TeamSeason>();
            A.CallTo(() => dbContext.TeamSeasons.AddRange(A<IEnumerable<TeamSeason>>.Ignored)).Returns(teamSeasons);

            // Act
            var result = repository.AddEntities(teamSeasons);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.AddRange(teamSeasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeasons, result);
        }

        [TestCase]
        public void AddTeamSeasons_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeasons = new List<TeamSeason>();
            A.CallTo(() => dbContext.TeamSeasons.AddRange(A<IEnumerable<TeamSeason>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<TeamSeason> result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntities(teamSeasons));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void CreateTeamSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            A.CallTo(() => dbContext.TeamSeasons.Create()).Returns(new TeamSeason());

            // Act
            var result = repository.CreateEntity();

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<TeamSeason>(result);
        }

        [TestCase]
        public void CreateTeamSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            A.CallTo(() => dbContext.TeamSeasons.Create()).Throws<Exception>();

            // Act
            TeamSeason result = null;
            Assert.Throws<Exception>(() => result = repository.CreateEntity());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void EditTeamSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeason = new TeamSeason();

            // Act
            repository.EditEntity(teamSeason);

            // Assert
            A.CallTo(() => dbContext.SetModified(teamSeason)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditTeamSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeason = new TeamSeason();

            A.CallTo(() => dbContext.SetModified(A<TeamSeason>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => repository.EditEntity(teamSeason));
        }

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            var teamSeason = new TeamSeason();
            A.CallTo(() => dbContext.TeamSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Returns(teamSeason);

            // Act
            var result = repository.FindEntity(teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.Find(teamName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeason, result);
        }

        //[TestCase]
        //public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        //{
        //    // Arrange
        //    var dbContext = A.Fake<ProFootballEntities>();
        //    var repository = new TeamSeasonRepository(dbContext);

        //    var teamName = "Team";
        //    var seasonID = 2017;

        //    TeamSeason teamSeason = null;
        //    A.CallTo(() => dbContext.TeamSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Returns(teamSeason);

        //    // Act
        //    TeamSeason result = null;
        //    Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(teamName, seasonID); });

        //    // Assert
        //    Assert.IsNull(result);
        //}

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.TeamSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            TeamSeason result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(teamName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.TeamSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Throws<Exception>();

            // Act
            TeamSeason result = null;
            Assert.Throws<Exception>(() => result = repository.FindEntity(teamName, seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetEntities_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            // Act
            var result = repository.GetEntities();

            // Assert
            Assert.IsInstanceOf<IEnumerable<TeamSeason>>(result);
        }

        [TestCase]
        public void GetEntities_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            A.CallTo(() => dbContext.TeamSeasons).Throws<Exception>();

            // Act
            IEnumerable<TeamSeason> result = null;
            Assert.Throws<Exception>(() => result = repository.GetEntities());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveTeamSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeason = new TeamSeason();
            A.CallTo(() => dbContext.TeamSeasons.Remove(A<TeamSeason>.Ignored)).Returns(teamSeason);

            // Act
            var result = repository.RemoveEntity(teamSeason);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.Remove(teamSeason)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeason, result);
        }

        [TestCase]
        public void RemoveTeamSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeason = new TeamSeason();
            A.CallTo(() => dbContext.TeamSeasons.Remove(A<TeamSeason>.Ignored)).Throws<Exception>();

            // Act
            TeamSeason result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntity(teamSeason));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveTeamSeasons_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeasons = new List<TeamSeason>();
            A.CallTo(() => dbContext.TeamSeasons.RemoveRange(A<IEnumerable<TeamSeason>>.Ignored)).Returns(teamSeasons);

            // Act
            var result = repository.RemoveEntities(teamSeasons);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.RemoveRange(teamSeasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeasons, result);
        }

        [TestCase]
        public void RemoveTeamSeasons_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

            var teamSeasons = new List<TeamSeason>();
            A.CallTo(() => dbContext.TeamSeasons.RemoveRange(A<IEnumerable<TeamSeason>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<TeamSeason> result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntities(teamSeasons));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamSeasonRepository(dbContext);

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
