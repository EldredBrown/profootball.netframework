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
    public class TeamRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddTeam()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var team = new Team
            {
                Name = "Team"
            };
            A.CallTo(() => dbContext.Teams.Add(A<Team>.Ignored)).Returns(team);

            // Act
            var result = repository.AddEntity(dbContext, team);

            // Assert
            A.CallTo(() => dbContext.Teams.Add(team)).MustHaveHappenedOnceExactly();
            Assert.AreSame(team, result);
        }

        [TestCase]
        public void AddTeams()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teams = new List<Team>();
            for (int i = 1; i <= 3; i++)
            {
                var team = new Team
                {
                    Name = "Team " + i
                };
                teams.Add(team);
            }
            A.CallTo(() => dbContext.Teams.AddRange(A<IEnumerable<Team>>.Ignored)).Returns(teams);

            // Act
            var result = repository.AddEntities(dbContext, teams);

            // Assert
            A.CallTo(() => dbContext.Teams.AddRange(teams)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teams, result);
        }

        [TestCase]
        public void CreateTeam()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.Teams.Create()).Returns(new Team());

            // Act
            var result = repository.CreateEntity(dbContext);

            // Assert
            A.CallTo(() => dbContext.Teams.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Team>(result);
        }

        [TestCase]
        public void EditTeam()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.SetModified(A<Team>.Ignored)).DoesNothing();

            var team = new Team
            {
                Name = "Team"
            };

            // Act
            repository.EditEntity(dbContext, team);

            // Assert
            A.CallTo(() => dbContext.SetModified(team)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void FindEntity_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Team";

            Team team = new Team();
            A.CallTo(() => dbContext.Teams.Find(A<string>.Ignored)).Returns(team);

            // Act
            var result = repository.FindEntity(dbContext, name);

            // Assert
            A.CallTo(() => dbContext.Teams.Find(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(team, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Team";

            Team team = null;
            A.CallTo(() => dbContext.Teams.Find(A<string>.Ignored)).Returns(team);

            // Act
            Team result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, name); });

            // Assert
            A.CallTo(() => dbContext.Teams.Find(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Team";

            A.CallTo(() => dbContext.Teams.Find(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Team result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, name); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task FindEntityAsync_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Team";

            Team team = new Team();
            A.CallTo(() => dbContext.Teams.FindAsync(A<string>.Ignored)).Returns(team);

            // Act
            var result = await repository.FindEntityAsync(dbContext, name);

            // Assert
            A.CallTo(() => dbContext.Teams.FindAsync(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(team, result);
        }

        [TestCase]
        public void FindEntityAsync_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Team";

            Team team = null;
            A.CallTo(() => dbContext.Teams.FindAsync(A<string>.Ignored)).Returns(team);

            // Act
            Team result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, name);
            });

            // Assert
            A.CallTo(() => dbContext.Teams.FindAsync(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntityAsync_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Team";

            A.CallTo(() => dbContext.Teams.FindAsync(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Team result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, name);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeams()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            var result = repository.GetEntities(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Team>>(result);
        }

        [TestCase]
        public async Task GetTeamsAsync()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeTeamsAsync();

            // Act
            var result = await repository.GetEntitiesAsync(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Team>>(result);
        }

        [TestCase]
        public void RemoveTeam()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var team = new Team
            {
                Name = "Team"
            };
            A.CallTo(() => dbContext.Teams.Remove(A<Team>.Ignored)).Returns(team);

            // Act
            var result = repository.RemoveEntity(dbContext, team);

            // Assert
            A.CallTo(() => dbContext.Teams.Remove(team)).MustHaveHappenedOnceExactly();
            Assert.AreSame(team, result);
        }

        [TestCase]
        public void RemoveTeams()
        {
            // Arrange
            var repository = new TeamRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var teams = new List<Team>();
            for (int i = 1; i <= 3; i++)
            {
                var team = new Team
                {
                    Name = "Team " + i
                };
                teams.Add(team);
            }
            A.CallTo(() => dbContext.Teams.RemoveRange(A<IEnumerable<Team>>.Ignored)).Returns(teams);

            // Act
            var result = repository.RemoveEntities(dbContext, teams);

            // Assert
            A.CallTo(() => dbContext.Teams.RemoveRange(teams)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teams, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new TeamRepository();

            // Act

            // Assert
        }

        #endregion Test Cases
    }
}
