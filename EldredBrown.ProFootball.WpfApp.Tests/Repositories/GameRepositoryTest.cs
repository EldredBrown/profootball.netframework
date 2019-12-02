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
    public class GameRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var game = new Game();
            A.CallTo(() => dbContext.Games.Add(A<Game>.Ignored)).Returns(game);

            // Act
            var result = repository.AddEntity(game);

            // Assert
            A.CallTo(() => dbContext.Games.Add(game)).MustHaveHappenedOnceExactly();
            Assert.AreSame(game, result);
        }

        [TestCase]
        public void AddEntity_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var game = new Game();
            A.CallTo(() => dbContext.Games.Add(A<Game>.Ignored)).Throws<Exception>();

            // Act
            Game result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntity(game));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void AddEntitys_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var games = new List<Game>();
            A.CallTo(() => dbContext.Games.AddRange(A<IEnumerable<Game>>.Ignored)).Returns(games);

            // Act
            var result = repository.AddEntities(games);

            // Assert
            A.CallTo(() => dbContext.Games.AddRange(games)).MustHaveHappenedOnceExactly();
            Assert.AreSame(games, result);
        }

        [TestCase]
        public void AddEntitys_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var games = new List<Game>();
            A.CallTo(() => dbContext.Games.AddRange(A<IEnumerable<Game>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Game> result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntities(games));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void CreateGame_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            A.CallTo(() => dbContext.Games.Create()).Returns(new Game());

            // Act
            var result = repository.CreateEntity();

            // Assert
            A.CallTo(() => dbContext.Games.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Game>(result);
        }

        [TestCase]
        public void CreateGame_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            A.CallTo(() => dbContext.Games.Create()).Throws<Exception>();

            // Act
            Game result = null;
            Assert.Throws<Exception>(() => result = repository.CreateEntity());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void EditGame_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var game = new Game();

            // Act
            repository.EditEntity(game);

            // Assert
            A.CallTo(() => dbContext.SetModified(game)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditGame_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var game = new Game();

            A.CallTo(() => dbContext.SetModified(A<Game>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => repository.EditEntity(game));
        }

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var id = 1;

            var game = new Game();
            A.CallTo(() => dbContext.Games.Find(A<int>.Ignored)).Returns(game);

            // Act
            var result = repository.FindEntity(id);

            // Assert
            A.CallTo(() => dbContext.Games.Find(id)).MustHaveHappenedOnceExactly();
            Assert.AreSame(game, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var id = 1;

            Game game = null;
            A.CallTo(() => dbContext.Games.Find(A<int>.Ignored)).Returns(game);

            // Act
            Game result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(id); });

            // Assert
            A.CallTo(() => dbContext.Games.Find(id)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var id = 1;

            A.CallTo(() => dbContext.Games.Find(A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Game result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(id); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var id = 1;

            A.CallTo(() => dbContext.Games.Find(A<int>.Ignored)).Throws<Exception>();

            // Act
            Game result = null;
            Assert.Throws<Exception>(() => result = repository.FindEntity(id));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetGames_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            // Act
            var result = repository.GetEntities();

            // Assert
            Assert.IsInstanceOf<IEnumerable<Game>>(result);
        }

        [TestCase]
        public void GetGames_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            A.CallTo(() => dbContext.Games).Throws<Exception>();

            // Act
            IEnumerable<Game> result = null;
            Assert.Throws<Exception>(() => result = repository.GetEntities());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var game = new Game();
            A.CallTo(() => dbContext.Games.Remove(A<Game>.Ignored)).Returns(game);

            // Act
            var result = repository.RemoveEntity(game);

            // Assert
            A.CallTo(() => dbContext.Games.Remove(game)).MustHaveHappenedOnceExactly();
            Assert.AreSame(game, result);
        }

        [TestCase]
        public void RemoveEntity_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var game = new Game();
            A.CallTo(() => dbContext.Games.Remove(A<Game>.Ignored)).Throws<Exception>();

            // Act
            Game result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntity(game));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveEntitys_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var games = new List<Game>();
            A.CallTo(() => dbContext.Games.RemoveRange(A<IEnumerable<Game>>.Ignored)).Returns(games);

            // Act
            var result = repository.RemoveEntities(games);

            // Assert
            A.CallTo(() => dbContext.Games.RemoveRange(games)).MustHaveHappenedOnceExactly();
            Assert.AreSame(games, result);
        }

        [TestCase]
        public void RemoveEntitys_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

            var games = new List<Game>();
            A.CallTo(() => dbContext.Games.RemoveRange(A<IEnumerable<Game>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Game> result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntities(games));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new GameRepository(dbContext);

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
