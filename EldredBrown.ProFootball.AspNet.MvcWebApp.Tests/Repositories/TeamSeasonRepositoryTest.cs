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
    public class TeamSeasonRepositoryTest
    {
        [TestCase]
        public void AddTeamSeason()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamSeason = new TeamSeason
            {
                TeamName = "Team",
                SeasonID = 2017
            };
            A.CallTo(() => dbContext.TeamSeasons.Add(A<TeamSeason>.Ignored)).Returns(teamSeason);

            // Act
            var result = repository.AddEntity(dbContext, teamSeason);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.Add(teamSeason)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeason, result);
        }

        [TestCase]
        public void AddTeamSeasons()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamSeasons = new List<TeamSeason>();
            for (int i = 1; i <= 3; i++)
            {
                var teamSeason = new TeamSeason
                {
                    TeamName = "Team " + i,
                    SeasonID = 2017
                };
                teamSeasons.Add(teamSeason);
            }
            A.CallTo(() => dbContext.TeamSeasons.AddRange(A<IEnumerable<TeamSeason>>.Ignored)).Returns(teamSeasons);

            // Act
            var result = repository.AddEntities(dbContext, teamSeasons);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.AddRange(teamSeasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeasons, result);
        }

        [TestCase]
        public void CreateTeamSeason()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.TeamSeasons.Create()).Returns(new TeamSeason());

            // Act
            var result = repository.CreateEntity(dbContext);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<TeamSeason>(result);
        }

        [TestCase]
        public void EditTeamSeason()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.SetModified(A<TeamSeason>.Ignored)).DoesNothing();

            var teamSeason = new TeamSeason
            {
                TeamName = "Team",
                SeasonID = 2017
            };

            // Act
            repository.EditEntity(dbContext, teamSeason);

            // Assert
            A.CallTo(() => dbContext.SetModified(teamSeason)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void FindEntity_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamName = "Team";
            var seasonID = 2017;

            TeamSeason teamSeason = new TeamSeason();
            A.CallTo(() => dbContext.TeamSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Returns(teamSeason);

            // Act
            var result = repository.FindEntity(dbContext, teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.Find(teamName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeason, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamName = "Team";
            var seasonID = 2017;

            TeamSeason teamSeason = null;
            A.CallTo(() => dbContext.TeamSeasons.Find(A<string>.Ignored, A<int>.Ignored)).Returns(teamSeason);

            // Act
            TeamSeason result = null;
            Assert.Throws<ObjectNotFoundException>(() =>
            {
                result = repository.FindEntity(dbContext, teamName, seasonID);
            });

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.Find(teamName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.TeamSeasons.Find(A<string>.Ignored, A<int>.Ignored))
                .Throws<InvalidOperationException>();

            // Act
            TeamSeason result = null;
            Assert.Throws<ObjectNotFoundException>(() =>
            {
                result = repository.FindEntity(dbContext, teamName, seasonID);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task FindEntityAsync_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamName = "Team";
            var seasonID = 2017;

            TeamSeason teamSeason = new TeamSeason();
            A.CallTo(() => dbContext.TeamSeasons.FindAsync(A<string>.Ignored, A<int>.Ignored)).Returns(teamSeason);

            // Act
            var result = await repository.FindEntityAsync(dbContext, teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.FindAsync(teamName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeason, result);
        }

        [TestCase]
        public void FindEntityAsync_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamName = "Team";
            var seasonID = 2017;

            TeamSeason teamSeason = null;
            A.CallTo(() => dbContext.TeamSeasons.FindAsync(A<string>.Ignored, A<int>.Ignored)).Returns(teamSeason);

            // Act
            TeamSeason result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, teamName, seasonID);
            });

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.FindAsync(teamName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntityAsync_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamName = "Team";
            var seasonID = 2017;

            A.CallTo(() => dbContext.TeamSeasons.FindAsync(A<string>.Ignored, A<int>.Ignored))
                .Throws<InvalidOperationException>();

            // Act
            TeamSeason result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, teamName, seasonID);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetEntities()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            var result = repository.GetEntities(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<TeamSeason>>(result);
        }

        [TestCase]
        public async Task GetEntitiesAsync()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeTeamSeasonsAsync();

            // Act
            var result = await repository.GetEntitiesAsync(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<TeamSeason>>(result);
        }

        [TestCase]
        public void RemoveTeamSeason()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamSeason = new TeamSeason
            {
                TeamName = "Team",
                SeasonID = 2017
            };
            A.CallTo(() => dbContext.TeamSeasons.Remove(A<TeamSeason>.Ignored)).Returns(teamSeason);

            // Act
            var result = repository.RemoveEntity(dbContext, teamSeason);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.Remove(teamSeason)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeason, result);
        }

        [TestCase]
        public void RemoveTeamSeasons()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teamSeasons = new List<TeamSeason>();
            for (int i = 1; i <= 3; i++)
            {
                var teamSeason = new TeamSeason
                {
                    TeamName = "Team " + i,
                    SeasonID = 2017
                };
                teamSeasons.Add(teamSeason);
            }
            A.CallTo(() => dbContext.TeamSeasons.RemoveRange(A<IEnumerable<TeamSeason>>.Ignored)).Returns(teamSeasons);

            // Act
            var result = repository.RemoveEntities(dbContext, teamSeasons);

            // Assert
            A.CallTo(() => dbContext.TeamSeasons.RemoveRange(teamSeasons)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeasons, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new TeamSeasonRepository();

            // Act

            // Assert
        }
    }
}
