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
    public class LeagueSeasonRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddLeagueSeason()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueSeason = new LeagueSeason
            {
                LeagueName = "League",
                SeasonID = 2017
            };
            A.CallTo(() => dbContext.LeagueSeasons.Add(A<LeagueSeason>.Ignored)).Returns(leagueSeason);

            // Act
            var result = repository.AddEntity(dbContext, leagueSeason);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.Add(leagueSeason)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeason, result);
        }

        [TestCase]
        public void AddLeagueSeasons()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueSeasons = new List<LeagueSeason>();
            for (int i = 1; i <= 3; i++)
            {
                var leagueSeason = new LeagueSeason
                {
                    LeagueName = "League " + i,
                    SeasonID = 2017
                };
                leagueSeasons.Add(leagueSeason);
            }
            A.CallTo(() => dbContext.LeagueSeasons.AddRange(A<IEnumerable<LeagueSeason>>.Ignored)).Returns(leagueSeasons);

            // Act
            var result = repository.AddEntities(dbContext, leagueSeasons);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.AddRange(leagueSeasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeasons, result);
        }

        [TestCase]
        public void CreateLeagueSeason()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.LeagueSeasons.Create()).Returns(new LeagueSeason());

            // Act
            var result = repository.CreateEntity(dbContext);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<LeagueSeason>(result);
        }

        [TestCase]
        public void EditLeagueSeason()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.SetModified(A<LeagueSeason>.Ignored)).DoesNothing();

            var leagueSeason = new LeagueSeason
            {
                LeagueName = "League",
                SeasonID = 2017
            };

            // Act
            repository.EditEntity(dbContext, leagueSeason);

            // Assert
            A.CallTo(() => dbContext.SetModified(leagueSeason)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void FindEntity_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueName = "League";
            var seasonID = 2017;

            LeagueSeason leagueSeason = new LeagueSeason();
            A.CallTo(() => dbContext.LeagueSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            // Act
            var result = repository.FindEntity(dbContext, leagueName, seasonID);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.Find(leagueName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeason, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueName = "League";
            var seasonID = 2017;

            LeagueSeason leagueSeason = null;
            A.CallTo(() => dbContext.LeagueSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            // Act
            LeagueSeason result = null;
            Assert.Throws<ObjectNotFoundException>(() =>
            {
                result = repository.FindEntity(dbContext, leagueName, seasonID);
            });

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.Find(leagueName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueName = "League";
            var seasonID = 2017;

            A.CallTo(() => dbContext.LeagueSeasons.Find(A<string>.Ignored, A<int>.Ignored))
                .Throws<InvalidOperationException>();

            // Act
            LeagueSeason result = null;
            Assert.Throws<ObjectNotFoundException>(() =>
            {
                result = repository.FindEntity(dbContext, leagueName, seasonID);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task FindEntityAsync_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueName = "League";
            var seasonID = 2017;

            LeagueSeason leagueSeason = new LeagueSeason();
            A.CallTo(() => dbContext.LeagueSeasons.FindAsync(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            // Act
            var result = await repository.FindEntityAsync(dbContext, leagueName, seasonID);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.FindAsync(leagueName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeason, result);
        }

        [TestCase]
        public void FindEntityAsync_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueName = "League";
            var seasonID = 2017;

            LeagueSeason leagueSeason = null;
            A.CallTo(() => dbContext.LeagueSeasons.FindAsync(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            // Act
            LeagueSeason result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, leagueName, seasonID);
            });

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.FindAsync(leagueName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntityAsync_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueName = "League";
            var seasonID = 2017;

            A.CallTo(() => dbContext.LeagueSeasons.FindAsync(A<string>.Ignored, A<int>.Ignored))
                .Throws<InvalidOperationException>();

            // Act
            LeagueSeason result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, leagueName, seasonID);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetLeagueSeasons()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            var result = repository.GetEntities(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<LeagueSeason>>(result);
        }

        [TestCase]
        public async Task GetLeagueSeasonsAsync()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeLeagueSeasonsAsync();

            // Act
            var result = await repository.GetEntitiesAsync(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<LeagueSeason>>(result);
        }

        [TestCase]
        public void RemoveLeagueSeason()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueSeason = new LeagueSeason
            {
                LeagueName = "League",
                SeasonID = 2017
            };
            A.CallTo(() => dbContext.LeagueSeasons.Remove(A<LeagueSeason>.Ignored)).Returns(leagueSeason);

            // Act
            var result = repository.RemoveEntity(dbContext, leagueSeason);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.Remove(leagueSeason)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeason, result);
        }

        [TestCase]
        public void RemoveLeagueSeasons()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var leagueSeasons = new List<LeagueSeason>();
            for (int i = 1; i <= 3; i++)
            {
                var leagueSeason = new LeagueSeason
                {
                    LeagueName = "League " + i,
                    SeasonID = 2017
                };
                leagueSeasons.Add(leagueSeason);
            }
            A.CallTo(() => dbContext.LeagueSeasons.RemoveRange(A<IEnumerable<LeagueSeason>>.Ignored)).Returns(leagueSeasons);

            // Act
            var result = repository.RemoveEntities(dbContext, leagueSeasons);

            // Assert
            A.CallTo(() => dbContext.LeagueSeasons.RemoveRange(leagueSeasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(leagueSeasons, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new LeagueSeasonRepository();

            // Act

            // Assert
        }

        #endregion Test Cases
    }
}
