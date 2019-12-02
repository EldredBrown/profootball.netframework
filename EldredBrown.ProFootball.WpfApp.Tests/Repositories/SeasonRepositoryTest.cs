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
    public class SeasonRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var season = new Season();
            A.CallTo(() => dbContext.Seasons.Add(A<Season>.Ignored)).Returns(season);

            // Act
            var result = repository.AddEntity(season);

            // Assert
            A.CallTo(() => dbContext.Seasons.Add(season)).MustHaveHappenedOnceExactly();
            Assert.AreSame(season, result);
        }

        [TestCase]
        public void AddSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var season = new Season();
            A.CallTo(() => dbContext.Seasons.Add(A<Season>.Ignored)).Throws<Exception>();

            // Act
            Season result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntity(season));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void AddSeasons_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var seasons = new List<Season>();
            A.CallTo(() => dbContext.Seasons.AddRange(A<IEnumerable<Season>>.Ignored)).Returns(seasons);

            // Act
            var result = repository.AddEntities(seasons);

            // Assert
            A.CallTo(() => dbContext.Seasons.AddRange(seasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(seasons, result);
        }

        [TestCase]
        public void AddSeasons_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var seasons = new List<Season>();
            A.CallTo(() => dbContext.Seasons.AddRange(A<IEnumerable<Season>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Season> result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntities(seasons));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void CreateSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            A.CallTo(() => dbContext.Seasons.Create()).Returns(new Season());

            // Act
            var result = repository.CreateEntity();

            // Assert
            A.CallTo(() => dbContext.Seasons.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Season>(result);
        }

        [TestCase]
        public void CreateSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            A.CallTo(() => dbContext.Seasons.Create()).Throws<Exception>();

            // Act
            Season result = null;
            Assert.Throws<Exception>(() => result = repository.CreateEntity());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void EditSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var season = new Season();

            // Act
            repository.EditEntity(season);

            // Assert
            A.CallTo(() => dbContext.SetModified(season)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var season = new Season();

            A.CallTo(() => dbContext.SetModified(A<Season>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => repository.EditEntity(season));
        }

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var id = 2017;

            var season = new Season();
            A.CallTo(() => dbContext.Seasons.Find(A<int>.Ignored)).Returns(season);

            // Act
            var result = repository.FindEntity(id);

            // Assert
            A.CallTo(() => dbContext.Seasons.Find(id)).MustHaveHappenedOnceExactly();
            Assert.AreSame(season, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var id = 2017;

            Season season = null;
            A.CallTo(() => dbContext.Seasons.Find(A<int>.Ignored)).Returns(season);

            // Act
            Season result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(id); });

            // Assert
            A.CallTo(() => dbContext.Seasons.Find(id)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var id = 2017;

            A.CallTo(() => dbContext.Seasons.Find(A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Season result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(id); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var id = 2017;

            A.CallTo(() => dbContext.Seasons.Find(A<int>.Ignored)).Throws<Exception>();

            // Act
            Season result = null;
            Assert.Throws<Exception>(() => result = repository.FindEntity(id));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetSeasons_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            // Act
            var result = repository.GetEntities();

            // Assert
            Assert.IsInstanceOf<IEnumerable<Season>>(result);
        }

        [TestCase]
        public void GetSeasons_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            A.CallTo(() => dbContext.Seasons).Throws<Exception>();

            // Act
            IEnumerable<Season> result = null;
            Assert.Throws<Exception>(() => result = repository.GetEntities());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveSeason_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var season = new Season();
            A.CallTo(() => dbContext.Seasons.Remove(A<Season>.Ignored)).Returns(season);

            // Act
            var result = repository.RemoveEntity(season);

            // Assert
            A.CallTo(() => dbContext.Seasons.Remove(season)).MustHaveHappenedOnceExactly();
            Assert.AreSame(season, result);
        }

        [TestCase]
        public void RemoveSeason_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var season = new Season();
            A.CallTo(() => dbContext.Seasons.Remove(A<Season>.Ignored)).Throws<Exception>();

            // Act
            Season result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntity(season));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveSeasons_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var seasons = new List<Season>();
            A.CallTo(() => dbContext.Seasons.RemoveRange(A<IEnumerable<Season>>.Ignored)).Returns(seasons);

            // Act
            var result = repository.RemoveEntities(seasons);

            // Assert
            A.CallTo(() => dbContext.Seasons.RemoveRange(seasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(seasons, result);
        }

        [TestCase]
        public void RemoveSeasons_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

            var seasons = new List<Season>();
            A.CallTo(() => dbContext.Seasons.RemoveRange(A<IEnumerable<Season>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Season> result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntities(seasons));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new SeasonRepository(dbContext);

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
