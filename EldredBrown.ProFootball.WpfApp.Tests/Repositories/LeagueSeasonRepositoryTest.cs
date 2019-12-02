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
    public class LeagueSeasonRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddLeagueSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeason = new LeagueSeason();
            A.CallTo(() => dbContext.LeagueSeasons.Add(A<LeagueSeason>.Ignored)).Returns(leagueSeason);

            // Act
            var result = repository.AddEntity(leagueSeason);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.Add(leagueSeason)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeason, result);
        }

        [TestCase]
        public void AddLeagueSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeason = new LeagueSeason();
            A.CallTo(() => dbContext.LeagueSeasons.Add(A<LeagueSeason>.Ignored)).Throws<Exception>();

            // Act
            LeagueSeason result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntity(leagueSeason));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void AddLeagueSeasons_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeasons = new List<LeagueSeason>();
            A.CallTo(() => dbContext.LeagueSeasons.AddRange(A<IEnumerable<LeagueSeason>>.Ignored))
                .Returns(leagueSeasons);

            // Act
            var result = repository.AddEntities(leagueSeasons);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.AddRange(leagueSeasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeasons, result);
        }

        [TestCase]
        public void AddLeagueSeasons_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeasons = new List<LeagueSeason>();
            A.CallTo(() => dbContext.LeagueSeasons.AddRange(A<IEnumerable<LeagueSeason>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<LeagueSeason> result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntities(leagueSeasons));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void CreateLeagueSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            A.CallTo(() => dbContext.LeagueSeasons.Create()).Returns(new LeagueSeason());

            // Act
            var result = repository.CreateEntity();

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<LeagueSeason>(result);
        }

        [TestCase]
        public void CreateLeagueSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            A.CallTo(() => dbContext.LeagueSeasons.Create()).Throws<Exception>();

            // Act
            LeagueSeason result = null;
            Assert.Throws<Exception>(() => result = repository.CreateEntity());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void EditLeagueSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeason = new LeagueSeason();

            // Act
            repository.EditEntity(leagueSeason);

            // Assert
            A.CallTo(() => dbContext.SetModified(leagueSeason)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditLeagueSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeason = new LeagueSeason();

            A.CallTo(() => dbContext.SetModified(A<LeagueSeason>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => repository.EditEntity(leagueSeason));
        }

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueName = "League";
            var seasonID = 2017;

            var leagueSeason = new LeagueSeason();
            A.CallTo(() => dbContext.LeagueSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            // Act
            var result = repository.FindEntity(leagueName, seasonID);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.Find(leagueName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeason, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueName = "League";
            var seasonID = 2017;

            LeagueSeason leagueSeason = null;
            A.CallTo(() => dbContext.LeagueSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            // Act
            LeagueSeason result = null;
            Assert.Throws<ObjectNotFoundException>(
                () => { result = repository.FindEntity(leagueName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueName = "League";
            var seasonID = 2017;

            A.CallTo(() => dbContext.LeagueSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            LeagueSeason result = null;
            Assert.Throws<ObjectNotFoundException>(
                () => { result = repository.FindEntity(leagueName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueName = "League";
            var seasonID = 2017;

            A.CallTo(() => dbContext.LeagueSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Throws<Exception>();

            // Act
            LeagueSeason result = null;
            Assert.Throws<Exception>(() => result = repository.FindEntity(leagueName, seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetLeagueSeasons_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            // Act
            var result = repository.GetEntities();

            // Assert
            Assert.IsInstanceOf<IEnumerable<LeagueSeason>>(result);
        }

        [TestCase]
        public void GetLeagueSeasons_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            A.CallTo(() => dbContext.LeagueSeasons).Throws<Exception>();

            // Act
            IEnumerable<LeagueSeason> result = null;
            Assert.Throws<Exception>(() => result = repository.GetEntities());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveLeagueSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeason = new LeagueSeason();
            A.CallTo(() => dbContext.LeagueSeasons.Remove(A<LeagueSeason>.Ignored)).Returns(leagueSeason);

            // Act
            var result = repository.RemoveEntity(leagueSeason);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.Remove(leagueSeason)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeason, result);
        }

        [TestCase]
        public void RemoveLeagueSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeason = new LeagueSeason();
            A.CallTo(() => dbContext.LeagueSeasons.Remove(A<LeagueSeason>.Ignored)).Throws<Exception>();

            // Act
            LeagueSeason result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntity(leagueSeason));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveLeagueSeasons_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeasons = new List<LeagueSeason>();
            A.CallTo(() => dbContext.LeagueSeasons.RemoveRange(A<IEnumerable<LeagueSeason>>.Ignored))
                .Returns(leagueSeasons);

            // Act
            var result = repository.RemoveEntities(leagueSeasons);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.RemoveRange(leagueSeasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeasons, result);
        }

        [TestCase]
        public void RemoveLeagueSeasons_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

            var leagueSeasons = new List<LeagueSeason>();
            A.CallTo(() => dbContext.LeagueSeasons.RemoveRange(A<IEnumerable<LeagueSeason>>.Ignored))
                .Throws<Exception>();

            // Act
            IEnumerable<LeagueSeason> result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntities(leagueSeasons));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueSeasonRepository(dbContext);

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
