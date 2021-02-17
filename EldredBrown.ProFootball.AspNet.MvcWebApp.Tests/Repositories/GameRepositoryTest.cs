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
    public class GameRepositoryTest
    {
        [TestCase]
        public void AddEntity()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var game = new Game
            {
                ID = 1
            };
            A.CallTo(() => dbContext.Games.Add(A<Game>.Ignored)).Returns(game);

            // Act
            var result = repository.AddEntity(dbContext, game);

            // Assert
            A.CallTo(() => dbContext.Games.Add(game)).MustHaveHappenedOnceExactly();
            Assert.AreSame(game, result);
        }

        [TestCase]
        public void AddEntitys()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var games = new List<Game>();
            for (int i = 1; i <= 3; i++)
            {
                var game = new Game
                {
                    ID = i
                };
                games.Add(game);
            }
            A.CallTo(() => dbContext.Games.AddRange(A<IEnumerable<Game>>.Ignored)).Returns(games);

            // Act
            var result = repository.AddEntities(dbContext, games);

            // Assert
            A.CallTo(() => dbContext.Games.AddRange(games)).MustHaveHappenedOnceExactly();
            Assert.AreSame(games, result);
        }

        [TestCase]
        public void CreateGame()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.Games.Create()).Returns(new Game());

            // Act
            var result = repository.CreateEntity(dbContext);

            // Assert
            A.CallTo(() => dbContext.Games.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Game>(result);
        }

        [TestCase]
        public void EditEntity()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.SetModified(A<Game>.Ignored)).DoesNothing();

            var game = new Game
            {
                ID = 1
            };

            // Act
            repository.EditEntity(dbContext, game);

            // Assert
            A.CallTo(() => dbContext.SetModified(game)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void FindEntity_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 1;

            Game game = new Game();
            A.CallTo(() => dbContext.Games.Find(A<int>.Ignored)).Returns(game);

            // Act
            var result = repository.FindEntity(dbContext, id);

            // Assert
            A.CallTo(() => dbContext.Games.Find(id)).MustHaveHappenedOnceExactly();
            Assert.AreSame(game, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 1;

            Game game = null;
            A.CallTo(() => dbContext.Games.Find(A<int>.Ignored)).Returns(game);

            // Act
            Game result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, id); });

            // Assert
            A.CallTo(() => dbContext.Games.Find(id)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 1;

            A.CallTo(() => dbContext.Games.Find(A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Game result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, id); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task FindEntityAsync_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 1;

            Game game = new Game();
            A.CallTo(() => dbContext.Games.FindAsync(A<int>.Ignored)).Returns(game);

            // Act
            var result = await repository.FindEntityAsync(dbContext, id);

            // Assert
            A.CallTo(() => dbContext.Games.FindAsync(id)).MustHaveHappenedOnceExactly();
            Assert.AreSame(game, result);
        }

        [TestCase]
        public void FindEntityAsync_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 1;

            Game game = null;
            A.CallTo(() => dbContext.Games.FindAsync(A<int>.Ignored)).Returns(game);

            // Act
            Game result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, id);
            });

            // Assert
            A.CallTo(() => dbContext.Games.FindAsync(id)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntityAsync_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 1;

            A.CallTo(() => dbContext.Games.FindAsync(A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Game result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, id);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetEntities()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            var result = repository.GetEntities(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Game>>(result);
        }

        [TestCase]
        public async Task GetGamesAsync()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeGamesAsync();

            // Act
            var result = await repository.GetEntitiesAsync(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Game>>(result);
        }

        [TestCase]
        public void RemoveEntity()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var game = new Game
            {
                ID = 1
            };
            A.CallTo(() => dbContext.Games.Remove(A<Game>.Ignored)).Returns(game);

            // Act
            var result = repository.RemoveEntity(dbContext, game);

            // Assert
            A.CallTo(() => dbContext.Games.Remove(game)).MustHaveHappenedOnceExactly();
            Assert.AreSame(game, result);
        }

        [TestCase]
        public void RemoveEntitys()
        {
            // Arrange
            var repository = new GameRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var games = new List<Game>();
            for (int i = 1; i <= 3; i++)
            {
                var game = new Game
                {
                    ID = i
                };
                games.Add(game);
            }
            A.CallTo(() => dbContext.Games.RemoveRange(A<IEnumerable<Game>>.Ignored)).Returns(games);

            // Act
            var result = repository.RemoveEntities(dbContext, games);

            // Assert
            A.CallTo(() => dbContext.Games.RemoveRange(games)).MustHaveHappenedOnceExactly();
            Assert.AreSame(games, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new GameRepository();

            // Act

            // Assert
        }
    }
}
