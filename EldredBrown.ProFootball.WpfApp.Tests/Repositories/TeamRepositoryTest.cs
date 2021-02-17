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
    public class TeamRepositoryTest
    {
        [TestCase]
        public void AddTeam_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var team = new Team();
            A.CallTo(() => dbContext.Teams.Add(A<Team>.Ignored)).Returns(team);

            // Act
            var result = repository.AddEntity(team);

            // Assert
            A.CallTo(() => dbContext.Teams.Add(team)).MustHaveHappenedOnceExactly();
            Assert.AreSame(team, result);
        }

        [TestCase]
        public void AddTeam_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var team = new Team();
            A.CallTo(() => dbContext.Teams.Add(A<Team>.Ignored)).Throws<Exception>();

            // Act
            Team result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntity(team));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void AddTeams_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var teams = new List<Team>();
            A.CallTo(() => dbContext.Teams.AddRange(A<IEnumerable<Team>>.Ignored)).Returns(teams);

            // Act
            var result = repository.AddEntities(teams);

            // Assert
            A.CallTo(() => dbContext.Teams.AddRange(teams)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teams, result);
        }

        [TestCase]
        public void AddTeams_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var teams = new List<Team>();
            A.CallTo(() => dbContext.Teams.AddRange(A<IEnumerable<Team>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Team> result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntities(teams));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void CreateTeam_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            A.CallTo(() => dbContext.Teams.Create()).Returns(new Team());

            // Act
            var result = repository.CreateEntity();

            // Assert
            A.CallTo(() => dbContext.Teams.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Team>(result);
        }

        [TestCase]
        public void CreateTeam_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            A.CallTo(() => dbContext.Teams.Create()).Throws<Exception>();

            // Act
            Team result = null;
            Assert.Throws<Exception>(() => result = repository.CreateEntity());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void EditTeam_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var team = new Team();

            // Act
            repository.EditEntity(team);

            // Assert
            A.CallTo(() => dbContext.SetModified(team)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditTeam_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var team = new Team();

            A.CallTo(() => dbContext.SetModified(A<Team>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => repository.EditEntity(team));
        }

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var name = "Team";

            var team = new Team();
            A.CallTo(() => dbContext.Teams.Find(A<string>.Ignored)).Returns(team);

            // Act
            var result = repository.FindEntity(name);

            // Assert
            A.CallTo(() => dbContext.Teams.Find(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(team, result);
        }

        //[TestCase]
        //public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        //{
        //    // Arrange
        //    var dbContext = A.Fake<ProFootballEntities>();
        //    var repository = new TeamRepository(dbContext);

        //    var name = "Team";

        //    Team team = null;
        //    A.CallTo(() => dbContext.Teams.Find(A<string>.Ignored)).Returns(team);

        //    // Act
        //    Team result = null;
        //    Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(name); });

        //    // Assert
        //    A.CallTo(() => dbContext.Teams.Find(name)).MustHaveHappenedOnceExactly();
        //    Assert.IsNull(result);
        //}

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var name = "Team";

            A.CallTo(() => dbContext.Teams.Find(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Team result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(name); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var name = "Team";

            A.CallTo(() => dbContext.Teams.Find(A<string>.Ignored)).Throws<Exception>();

            // Act
            Team result = null;
            Assert.Throws<Exception>(() => result = repository.FindEntity(name));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetTeams_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            // Act
            var result = repository.GetEntities();

            // Assert
            Assert.IsInstanceOf<IEnumerable<Team>>(result);
        }

        [TestCase]
        public void GetTeams_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            A.CallTo(() => dbContext.Teams).Throws<Exception>();

            // Act
            IEnumerable<Team> result = null;
            Assert.Throws<Exception>(() => result = repository.GetEntities());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveTeam_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var team = new Team();
            A.CallTo(() => dbContext.Teams.Remove(A<Team>.Ignored)).Returns(team);

            // Act
            var result = repository.RemoveEntity(team);

            // Assert
            A.CallTo(() => dbContext.Teams.Remove(team)).MustHaveHappenedOnceExactly();
            Assert.AreSame(team, result);
        }

        [TestCase]
        public void RemoveTeam_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var team = new Team();
            A.CallTo(() => dbContext.Teams.Remove(A<Team>.Ignored)).Throws<Exception>();

            // Act
            Team result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntity(team));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveTeams_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var teams = new List<Team>();
            A.CallTo(() => dbContext.Teams.RemoveRange(A<IEnumerable<Team>>.Ignored)).Returns(teams);

            // Act
            var result = repository.RemoveEntities(teams);

            // Assert
            A.CallTo(() => dbContext.Teams.RemoveRange(teams)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teams, result);
        }

        [TestCase]
        public void RemoveTeams_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            var teams = new List<Team>();
            A.CallTo(() => dbContext.Teams.RemoveRange(A<IEnumerable<Team>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Team> result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntities(teams));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new TeamRepository(dbContext);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }
    }
}
