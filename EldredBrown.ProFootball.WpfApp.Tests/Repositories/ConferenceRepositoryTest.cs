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
    public class ConferenceRepositoryTest
    {
        [TestCase]
        public void AddConference_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conference = new Conference();
            A.CallTo(() => dbContext.Conferences.Add(A<Conference>.Ignored)).Returns(conference);

            // Act
            var result = repository.AddEntity(conference);

            // Assert
            A.CallTo(() => dbContext.Conferences.Add(conference)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conference, result);
        }

        [TestCase]
        public void AddConference_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conference = new Conference();
            A.CallTo(() => dbContext.Conferences.Add(A<Conference>.Ignored)).Throws<Exception>();

            // Act
            Conference result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntity(conference));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void AddConferences_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conferences = new List<Conference>();
            A.CallTo(() => dbContext.Conferences.AddRange(A<IEnumerable<Conference>>.Ignored)).Returns(conferences);

            // Act
            var result = repository.AddEntities(conferences);

            // Assert
            A.CallTo(() => dbContext.Conferences.AddRange(conferences)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conferences, result);
        }

        [TestCase]
        public void AddConferences_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conferences = new List<Conference>();
            A.CallTo(() => dbContext.Conferences.AddRange(A<IEnumerable<Conference>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Conference> result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntities(conferences));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void CreateConference_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            A.CallTo(() => dbContext.Conferences.Create()).Returns(new Conference());

            // Act
            var result = repository.CreateEntity();

            // Assert
            A.CallTo(() => dbContext.Conferences.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Conference>(result);
        }

        [TestCase]
        public void CreateConference_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            A.CallTo(() => dbContext.Conferences.Create()).Throws<Exception>();

            // Act
            Conference result = null;
            Assert.Throws<Exception>(() => result = repository.CreateEntity());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void EditConference_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conference = new Conference();

            // Act
            repository.EditEntity(conference);

            // Assert
            A.CallTo(() => dbContext.SetModified(conference)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditConference_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conference = new Conference();

            A.CallTo(() => dbContext.SetModified(A<Conference>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => repository.EditEntity(conference));
        }

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var name = "Conference";

            var conference = new Conference();
            A.CallTo(() => dbContext.Conferences.Find(A<string>.Ignored)).Returns(conference);

            // Act
            var result = repository.FindEntity(name);

            // Assert
            A.CallTo(() => dbContext.Conferences.Find(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conference, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var name = "Conference";

            Conference conference = null;
            A.CallTo(() => dbContext.Conferences.Find(A<string>.Ignored)).Returns(conference);

            // Act
            Conference result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(name); });

            // Assert
            A.CallTo(() => dbContext.Conferences.Find(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var name = "Conference";

            A.CallTo(() => dbContext.Conferences.Find(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Conference result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(name); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var name = "Conference";

            A.CallTo(() => dbContext.Conferences.Find(A<string>.Ignored)).Throws<Exception>();

            // Act
            Conference result = null;
            Assert.Throws<Exception>(() => result = repository.FindEntity(name));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetConferences_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            // Act
            var result = repository.GetEntities();

            // Assert
            Assert.IsInstanceOf<IEnumerable<Conference>>(result);
        }

        [TestCase]
        public void GetConferences_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            A.CallTo(() => dbContext.Conferences).Throws<Exception>();

            // Act
            IEnumerable<Conference> result = null;
            Assert.Throws<Exception>(() => result = repository.GetEntities());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveConference_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conference = new Conference();
            A.CallTo(() => dbContext.Conferences.Remove(A<Conference>.Ignored)).Returns(conference);

            // Act
            var result = repository.RemoveEntity(conference);

            // Assert
            A.CallTo(() => dbContext.Conferences.Remove(conference)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conference, result);
        }

        [TestCase]
        public void RemoveConference_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conference = new Conference();
            A.CallTo(() => dbContext.Conferences.Remove(A<Conference>.Ignored)).Throws<Exception>();

            // Act
            Conference result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntity(conference));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveConferences_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conferences = new List<Conference>();
            A.CallTo(() => dbContext.Conferences.RemoveRange(A<IEnumerable<Conference>>.Ignored)).Returns(conferences);

            // Act
            var result = repository.RemoveEntities(conferences);

            // Assert
            A.CallTo(() => dbContext.Conferences.RemoveRange(conferences)).MustHaveHappenedOnceExactly();
            Assert.AreSame(conferences, result);
        }

        [TestCase]
        public void RemoveConferences_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            var conferences = new List<Conference>();
            A.CallTo(() => dbContext.Conferences.RemoveRange(A<IEnumerable<Conference>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Conference> result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntities(conferences));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new ConferenceRepository(dbContext);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }
    }
}
