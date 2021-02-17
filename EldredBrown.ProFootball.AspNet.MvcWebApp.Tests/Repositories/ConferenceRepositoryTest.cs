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
    public class ConferenceRepositoryTest
    {
        [TestCase]
        public void AddConference()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var conference = new Conference
            {
                Name = "Conference"
            };
            A.CallTo(() => dbContext.Conferences.Add(A<Conference>.Ignored)).Returns(conference);

            // Act
            var result = repository.AddEntity(dbContext, conference);

            // Assert
            A.CallTo(() => dbContext.Conferences.Add(conference)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conference, result);
        }

        [TestCase]
        public void AddConferences()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var conferences = new List<Conference>();
            for (int i = 1; i <= 3; i++)
            {
                var conference = new Conference
                {
                    Name = "Conference " + i
                };
                conferences.Add(conference);
            }
            A.CallTo(() => dbContext.Conferences.AddRange(A<IEnumerable<Conference>>.Ignored)).Returns(conferences);

            // Act
            var result = repository.AddEntities(dbContext, conferences);

            // Assert
            A.CallTo(() => dbContext.Conferences.AddRange(conferences)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conferences, result);
        }

        [TestCase]
        public void CreateConference()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.Conferences.Create()).Returns(new Conference());

            // Act
            var result = repository.CreateEntity(dbContext);

            // Assert
            A.CallTo(() => dbContext.Conferences.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Conference>(result);
        }

        [TestCase]
        public void EditConference()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.SetModified(A<Conference>.Ignored)).DoesNothing();

            var conference = new Conference
            {
                Name = "Conference"
            };

            // Act
            repository.EditEntity(dbContext, conference);

            // Assert
            A.CallTo(() => dbContext.SetModified(conference)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void FindEntity_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Conference";

            Conference conference = new Conference();
            A.CallTo(() => dbContext.Conferences.Find(A<string>.Ignored)).Returns(conference);

            // Act
            var result = repository.FindEntity(dbContext, name);

            // Assert
            A.CallTo(() => dbContext.Conferences.Find(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conference, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Conference";

            Conference conference = null;
            A.CallTo(() => dbContext.Conferences.Find(A<string>.Ignored)).Returns(conference);

            // Act
            Conference result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, name); });

            // Assert
            A.CallTo(() => dbContext.Conferences.Find(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Conference";

            A.CallTo(() => dbContext.Conferences.Find(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Conference result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, name); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task FindEntityAsync_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Conference";

            Conference conference = new Conference();
            A.CallTo(() => dbContext.Conferences.FindAsync(A<string>.Ignored)).Returns(conference);

            // Act
            var result = await repository.FindEntityAsync(dbContext, name);

            // Assert
            A.CallTo(() => dbContext.Conferences.FindAsync(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conference, result);
        }

        [TestCase]
        public void FindEntityAsync_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Conference";

            Conference conference = null;
            A.CallTo(() => dbContext.Conferences.FindAsync(A<string>.Ignored)).Returns(conference);

            // Act
            Conference result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, name);
            });

            // Assert
            A.CallTo(() => dbContext.Conferences.FindAsync(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntityAsync_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Conference";

            A.CallTo(() => dbContext.Conferences.FindAsync(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Conference result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, name);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetConferences()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            var result = repository.GetEntities(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Conference>>(result);
        }

        [TestCase]
        public async Task GetConferencesAsync()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeConferencesAsync();

            // Act
            var result = await repository.GetEntitiesAsync(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Conference>>(result);
        }

        [TestCase]
        public void RemoveConference()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var conference = new Conference
            {
                Name = "Conference"
            };
            A.CallTo(() => dbContext.Conferences.Remove(A<Conference>.Ignored)).Returns(conference);

            // Act
            var result = repository.RemoveEntity(dbContext, conference);

            // Assert
            A.CallTo(() => dbContext.Conferences.Remove(conference)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conference, result);
        }

        [TestCase]
        public void RemoveConferences()
        {
            // Arrange
            var repository = new ConferenceRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var conferences = new List<Conference>();
            for (int i = 1; i <= 3; i++)
            {
                var conference = new Conference
                {
                    Name = "Conference " + i
                };
                conferences.Add(conference);
            }
            A.CallTo(() => dbContext.Conferences.RemoveRange(A<IEnumerable<Conference>>.Ignored)).Returns(conferences);

            // Act
            var result = repository.RemoveEntities(dbContext, conferences);

            // Assert
            A.CallTo(() => dbContext.Conferences.RemoveRange(conferences)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conferences, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new ConferenceRepository();

            // Act

            // Assert
        }
    }
}
