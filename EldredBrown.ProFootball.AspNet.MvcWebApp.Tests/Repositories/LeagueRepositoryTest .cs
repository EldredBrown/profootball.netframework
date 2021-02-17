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
    public class LeagueRepositoryTest
    {
        [TestCase]
        public void AddLeague()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var league = new League
            {
                Name = "League"
            };
            A.CallTo(() => dbContext.Leagues.Add(A<League>.Ignored)).Returns(league);

            // Act
            var result = repository.AddEntity(dbContext, league);

            // Assert
            A.CallTo(() => dbContext.Leagues.Add(league)).MustHaveHappenedOnceExactly();
            Assert.AreSame(league, result);
        }

        [TestCase]
        public void AddLeagues()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagues = new List<League>();
            for (int i = 1; i <= 3; i++)
            {
                var league = new League
                {
                    Name = "League " + i
                };
                leagues.Add(league);
            }
            A.CallTo(() => dbContext.Leagues.AddRange(A<IEnumerable<League>>.Ignored)).Returns(leagues);

            // Act
            var result = repository.AddEntities(dbContext, leagues);

            // Assert
            A.CallTo(() => dbContext.Leagues.AddRange(leagues)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagues, result);
        }

        [TestCase]
        public void CreateLeague()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.Leagues.Create()).Returns(new League());

            // Act
            var result = repository.CreateEntity(dbContext);

            // Assert
            A.CallTo(() => dbContext.Leagues.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<League>(result);
        }

        [TestCase]
        public void EditLeague()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.SetModified(A<League>.Ignored)).DoesNothing();

            var league = new League
            {
                Name = "League"
            };

            // Act
            repository.EditEntity(dbContext, league);

            // Assert
            A.CallTo(() => dbContext.SetModified(league)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void FindEntity_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "League";

            League league = new League();
            A.CallTo(() => dbContext.Leagues.Find(A<string>.Ignored)).Returns(league);

            // Act
            var result = repository.FindEntity(dbContext, name);

            // Assert
            A.CallTo(() => dbContext.Leagues.Find(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(league, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "League";

            League league = null;
            A.CallTo(() => dbContext.Leagues.Find(A<string>.Ignored)).Returns(league);

            // Act
            League result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, name); });

            // Assert
            A.CallTo(() => dbContext.Leagues.Find(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "League";

            A.CallTo(() => dbContext.Leagues.Find(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            League result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, name); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task FindEntityAsync_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "League";

            League league = new League();
            A.CallTo(() => dbContext.Leagues.FindAsync(A<string>.Ignored)).Returns(league);

            // Act
            var result = await repository.FindEntityAsync(dbContext, name);

            // Assert
            A.CallTo(() => dbContext.Leagues.FindAsync(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(league, result);
        }

        [TestCase]
        public void FindEntityAsync_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "League";

            League league = null;
            A.CallTo(() => dbContext.Leagues.FindAsync(A<string>.Ignored)).Returns(league);

            // Act
            League result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, name);
            });

            // Assert
            A.CallTo(() => dbContext.Leagues.FindAsync(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntityAsync_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "League";

            A.CallTo(() => dbContext.Leagues.FindAsync(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            League result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, name);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetLeagues()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            var result = repository.GetEntities(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<League>>(result);
        }

        [TestCase]
        public async Task GetLeaguesAsync()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeLeaguesAsync();

            // Act
            var result = await repository.GetEntitiesAsync(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<League>>(result);
        }

        [TestCase]
        public void RemoveLeague()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var league = new League
            {
                Name = "League"
            };
            A.CallTo(() => dbContext.Leagues.Remove(A<League>.Ignored)).Returns(league);

            // Act
            var result = repository.RemoveEntity(dbContext, league);

            // Assert
            A.CallTo(() => dbContext.Leagues.Remove(league)).MustHaveHappenedOnceExactly();
            Assert.AreSame(league, result);
        }

        [TestCase]
        public void RemoveLeagues()
        {
            // Arrange
            var repository = new LeagueRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagues = new List<League>();
            for (int i = 1; i <= 3; i++)
            {
                var league = new League
                {
                    Name = "League " + i
                };
                leagues.Add(league);
            }
            A.CallTo(() => dbContext.Leagues.RemoveRange(A<IEnumerable<League>>.Ignored)).Returns(leagues);

            // Act
            var result = repository.RemoveEntities(dbContext, leagues);

            // Assert
            A.CallTo(() => dbContext.Leagues.RemoveRange(leagues)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagues, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new LeagueRepository();

            // Act

            // Assert
        }
    }
}
