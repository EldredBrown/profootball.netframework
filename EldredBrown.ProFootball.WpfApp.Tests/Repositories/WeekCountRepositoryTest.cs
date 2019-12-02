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
    public class WeekCountRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddWeekCount_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCount = new WeekCount();
            A.CallTo(() => dbContext.WeekCounts.Add(A<WeekCount>.Ignored)).Returns(weekCount);

            // Act
            var result = repository.AddEntity(weekCount);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Add(weekCount)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCount, result);
        }

        [TestCase]
        public void AddWeekCount_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCount = new WeekCount();
            A.CallTo(() => dbContext.WeekCounts.Add(A<WeekCount>.Ignored)).Throws<Exception>();

            // Act
            WeekCount result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntity(weekCount));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void AddWeekCounts_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCounts = new List<WeekCount>();
            A.CallTo(() => dbContext.WeekCounts.AddRange(A<IEnumerable<WeekCount>>.Ignored)).Returns(weekCounts);

            // Act
            var result = repository.AddEntities(weekCounts);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.AddRange(weekCounts)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCounts, result);
        }

        [TestCase]
        public void AddWeekCounts_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCounts = new List<WeekCount>();
            A.CallTo(() => dbContext.WeekCounts.AddRange(A<IEnumerable<WeekCount>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<WeekCount> result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntities(weekCounts));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void CreateWeekCount_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            A.CallTo(() => dbContext.WeekCounts.Create()).Returns(new WeekCount());

            // Act
            var result = repository.CreateEntity();

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<WeekCount>(result);
        }

        [TestCase]
        public void CreateWeekCount_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            A.CallTo(() => dbContext.WeekCounts.Create()).Throws<Exception>();

            // Act
            WeekCount result = null;
            Assert.Throws<Exception>(() => result = repository.CreateEntity());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void EditWeekCount_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCount = new WeekCount();

            // Act
            repository.EditEntity(weekCount);

            // Assert
            A.CallTo(() => dbContext.SetModified(weekCount)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditWeekCount_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCount = new WeekCount();

            A.CallTo(() => dbContext.SetModified(A<WeekCount>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => repository.EditEntity(weekCount));
        }

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var seasonID = 2018;

            var weekCount = new WeekCount();
            A.CallTo(() => dbContext.WeekCounts.Find(A<int>.Ignored)).Returns(weekCount);

            // Act
            var result = repository.FindEntity(seasonID);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Find(seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCount, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var seasonID = 2018;

            WeekCount weekCount = null;
            A.CallTo(() => dbContext.WeekCounts.Find(A<int>.Ignored)).Returns(weekCount);

            // Act
            WeekCount result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(seasonID); });

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Find(seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var seasonID = 2018;

            A.CallTo(() => dbContext.WeekCounts.Find(A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            WeekCount result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var seasonID = 2018;

            A.CallTo(() => dbContext.WeekCounts.Find(A<int>.Ignored)).Throws<Exception>();

            // Act
            WeekCount result = null;
            Assert.Throws<Exception>(() => result = repository.FindEntity(seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetWeekCounts_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            // Act
            var result = repository.GetEntities();

            // Assert
            Assert.IsInstanceOf<IEnumerable<WeekCount>>(result);
        }

        [TestCase]
        public void GetWeekCounts_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            A.CallTo(() => dbContext.WeekCounts).Throws<Exception>();

            // Act
            IEnumerable<WeekCount> result = null;
            Assert.Throws<Exception>(() => result = repository.GetEntities());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveWeekCount_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCount = new WeekCount();
            A.CallTo(() => dbContext.WeekCounts.Remove(A<WeekCount>.Ignored)).Returns(weekCount);

            // Act
            var result = repository.RemoveEntity(weekCount);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Remove(weekCount)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCount, result);
        }

        [TestCase]
        public void RemoveWeekCount_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCount = new WeekCount();
            A.CallTo(() => dbContext.WeekCounts.Remove(A<WeekCount>.Ignored)).Throws<Exception>();

            // Act
            WeekCount result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntity(weekCount));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveWeekCounts_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCounts = new List<WeekCount>();
            A.CallTo(() => dbContext.WeekCounts.RemoveRange(A<IEnumerable<WeekCount>>.Ignored)).Returns(weekCounts);

            // Act
            var result = repository.RemoveEntities(weekCounts);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.RemoveRange(weekCounts)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCounts, result);
        }

        [TestCase]
        public void RemoveWeekCounts_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

            var weekCounts = new List<WeekCount>();
            A.CallTo(() => dbContext.WeekCounts.RemoveRange(A<IEnumerable<WeekCount>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<WeekCount> result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntities(weekCounts));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new WeekCountRepository(dbContext);

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
