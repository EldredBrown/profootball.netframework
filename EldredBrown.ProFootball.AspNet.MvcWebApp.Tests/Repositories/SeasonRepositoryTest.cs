using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Tests.Repositories
{
    [TestFixture]
    public class SeasonRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddSeason()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var season = new Season
            {
                ID = 2017
            };
            A.CallTo(() => dbContext.Seasons.Add(A<Season>.Ignored)).Returns(season);

            // Act
            var result = repository.AddEntity(dbContext, season);

            // Assert
            A.CallTo(() => dbContext.Seasons.Add(season)).MustHaveHappenedOnceExactly();
            Assert.AreSame(season, result);
        }

        [TestCase]
        public void AddSeasons()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var seasons = new List<Season>();
            for (int i = 0; i < 3; i++)
            {
                var season = new Season
                {
                    ID = 2017 - i
                };
                seasons.Add(season);
            }
            A.CallTo(() => dbContext.Seasons.AddRange(A<IEnumerable<Season>>.Ignored)).Returns(seasons);

            // Act
            var result = repository.AddEntities(dbContext, seasons);

            // Assert
            A.CallTo(() => dbContext.Seasons.AddRange(seasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(seasons, result);
        }

        [TestCase]
        public void CreateSeason()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.Seasons.Create()).Returns(new Season());

            // Act
            var result = repository.CreateEntity(dbContext);

            // Assert
            A.CallTo(() => dbContext.Seasons.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Season>(result);
        }

        [TestCase]
        public void EditSeason()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.SetModified(A<Season>.Ignored)).DoesNothing();

            var season = new Season
            {
                ID = 2017
            };

            // Act
            repository.EditEntity(dbContext, season);

            // Assert
            A.CallTo(() => dbContext.SetModified(season)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void FindEntity_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 2017;

            Season season = new Season();
            A.CallTo(() => dbContext.Seasons.Find(A<int>.Ignored)).Returns(season);

            // Act
            var result = repository.FindEntity(dbContext, id);

            // Assert
            A.CallTo(() => dbContext.Seasons.Find(id)).MustHaveHappenedOnceExactly();
            Assert.AreSame(season, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 2017;

            Season season = null;
            A.CallTo(() => dbContext.Seasons.Find(A<int>.Ignored)).Returns(season);

            // Act
            Season result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, id); });

            // Assert
            A.CallTo(() => dbContext.Seasons.Find(id)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 2017;

            A.CallTo(() => dbContext.Seasons.Find(A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Season result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, id); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task FindEntityAsync_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 2017;

            Season season = new Season();
            A.CallTo(() => dbContext.Seasons.FindAsync(A<int>.Ignored)).Returns(season);

            // Act
            var result = await repository.FindEntityAsync(dbContext, id);

            // Assert
            A.CallTo(() => dbContext.Seasons.FindAsync(id)).MustHaveHappenedOnceExactly();
            Assert.AreSame(season, result);
        }

        [TestCase]
        public void FindEntityAsync_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 2017;

            Season season = null;
            A.CallTo(() => dbContext.Seasons.FindAsync(A<int>.Ignored)).Returns(season);

            // Act
            Season result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, id);
            });

            // Assert
            A.CallTo(() => dbContext.Seasons.FindAsync(id)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntityAsync_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 2017;

            A.CallTo(() => dbContext.Seasons.FindAsync(A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Season result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, id);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetSeasons()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            var result = repository.GetEntities(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Season>>(result);
        }

        [TestCase]
        public async Task GetSeasonsAsync()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeSeasonsAsync();

            // Act
            var result = await repository.GetEntitiesAsync(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Season>>(result);
        }

        [TestCase]
        public void RemoveSeason()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var season = new Season
            {
                ID = 2017
            };
            A.CallTo(() => dbContext.Seasons.Remove(A<Season>.Ignored)).Returns(season);

            // Act
            var result = repository.RemoveEntity(dbContext, season);

            // Assert
            A.CallTo(() => dbContext.Seasons.Remove(season)).MustHaveHappenedOnceExactly();
            Assert.AreSame(season, result);
        }

        [TestCase]
        public void RemoveSeasons()
        {
            // Arrange
            var repository = new SeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var seasons = new List<Season>();
            for (int i = 0; i < 3; i++)
            {
                var season = new Season
                {
                    ID = 2017 - i
                };
                seasons.Add(season);
            }
            A.CallTo(() => dbContext.Seasons.RemoveRange(A<IEnumerable<Season>>.Ignored)).Returns(seasons);

            // Act
            var result = repository.RemoveEntities(dbContext, seasons);

            // Assert
            A.CallTo(() => dbContext.Seasons.RemoveRange(seasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(seasons, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new SeasonRepository();

            // Act

            // Assert
        }

        #endregion Test Cases
    }
}
