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
    public class LeagueRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddLeague_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var league = new League();
            A.CallTo(() => dbContext.Leagues.Add(A<League>.Ignored)).Returns(league);

            // Act
            var result = repository.AddEntity(league);

            // Assert
            A.CallTo(() => dbContext.Leagues.Add(league)).MustHaveHappenedOnceExactly();
            Assert.AreSame(league, result);
        }

        [TestCase]
        public void AddLeague_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var league = new League();
            A.CallTo(() => dbContext.Leagues.Add(A<League>.Ignored)).Throws<Exception>();

            // Act
            League result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntity(league));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void AddLeagues_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var leagues = new List<League>();
            A.CallTo(() => dbContext.Leagues.AddRange(A<IEnumerable<League>>.Ignored)).Returns(leagues);

            // Act
            var result = repository.AddEntities(leagues);

            // Assert
            A.CallTo(() => dbContext.Leagues.AddRange(leagues)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagues, result);
        }

        [TestCase]
        public void AddLeagues_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var leagues = new List<League>();
            A.CallTo(() => dbContext.Leagues.AddRange(A<IEnumerable<League>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<League> result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntities(leagues));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void CreateLeague_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            A.CallTo(() => dbContext.Leagues.Create()).Returns(new League());

            // Act
            var result = repository.CreateEntity();

            // Assert
            A.CallTo(() => dbContext.Leagues.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<League>(result);
        }

        [TestCase]
        public void CreateLeague_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            A.CallTo(() => dbContext.Leagues.Create()).Throws<Exception>();

            // Act
            League result = null;
            Assert.Throws<Exception>(() => result = repository.CreateEntity());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void EditLeague_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var league = new League();

            // Act
            repository.EditEntity(league);

            // Assert
            A.CallTo(() => dbContext.SetModified(league)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditLeague_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var league = new League();

            A.CallTo(() => dbContext.SetModified(A<League>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => repository.EditEntity(league));
        }

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var name = "League";

            var league = new League();
            A.CallTo(() => dbContext.Leagues.Find(A<string>.Ignored)).Returns(league);

            // Act
            var result = repository.FindEntity(name);

            // Assert
            A.CallTo(() => dbContext.Leagues.Find(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(league, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var name = "League";

            League league = null;
            A.CallTo(() => dbContext.Leagues.Find(A<string>.Ignored)).Returns(league);

            // Act
            League result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(name); });

            // Assert
            A.CallTo(() => dbContext.Leagues.Find(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var name = "League";

            A.CallTo(() => dbContext.Leagues.Find(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            League result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(name); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var name = "League";

            A.CallTo(() => dbContext.Leagues.Find(A<string>.Ignored)).Throws<Exception>();

            // Act
            League result = null;
            Assert.Throws<Exception>(() => result = repository.FindEntity(name));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetLeagues_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            // Act
            var result = repository.GetEntities();

            // Assert
            Assert.IsInstanceOf<IEnumerable<League>>(result);
        }

        [TestCase]
        public void GetLeagues_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            A.CallTo(() => dbContext.Leagues).Throws<Exception>();

            // Act
            IEnumerable<League> result = null;
            Assert.Throws<Exception>(() => result = repository.GetEntities());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveLeague_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var league = new League();
            A.CallTo(() => dbContext.Leagues.Remove(A<League>.Ignored)).Returns(league);

            // Act
            var result = repository.RemoveEntity(league);

            // Assert
            A.CallTo(() => dbContext.Leagues.Remove(league)).MustHaveHappenedOnceExactly();
            Assert.AreSame(league, result);
        }

        [TestCase]
        public void RemoveLeague_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var league = new League();
            A.CallTo(() => dbContext.Leagues.Remove(A<League>.Ignored)).Throws<Exception>();

            // Act
            League result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntity(league));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveLeagues_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var leagues = new List<League>();
            A.CallTo(() => dbContext.Leagues.RemoveRange(A<IEnumerable<League>>.Ignored)).Returns(leagues);

            // Act
            var result = repository.RemoveEntities(leagues);

            // Assert
            A.CallTo(() => dbContext.Leagues.RemoveRange(leagues)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagues, result);
        }

        [TestCase]
        public void RemoveLeagues_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

            var leagues = new List<League>();
            A.CallTo(() => dbContext.Leagues.RemoveRange(A<IEnumerable<League>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<League> result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntities(leagues));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new LeagueRepository(dbContext);

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
