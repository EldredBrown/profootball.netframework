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
    public class WeekCountRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddWeekCount()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var weekCount = new WeekCount
            {
                SeasonID = 1
            };
            A.CallTo(() => dbContext.WeekCounts.Add(A<WeekCount>.Ignored)).Returns(weekCount);

            // Act
            var result = repository.AddEntity(dbContext, weekCount);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Add(weekCount)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCount, result);
        }

        [TestCase]
        public void AddWeekCounts()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var weekCounts = new List<WeekCount>();
            for (int i = 1; i <= 3; i++)
            {
                var weekCount = new WeekCount
                {
                    SeasonID = i
                };
                weekCounts.Add(weekCount);
            }
            A.CallTo(() => dbContext.WeekCounts.AddRange(A<IEnumerable<WeekCount>>.Ignored)).Returns(weekCounts);

            // Act
            var result = repository.AddEntities(dbContext, weekCounts);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.AddRange(weekCounts)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCounts, result);
        }

        [TestCase]
        public void CreateWeekCount()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.WeekCounts.Create()).Returns(new WeekCount());

            // Act
            var result = repository.CreateEntity(dbContext);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<WeekCount>(result);
        }

        [TestCase]
        public void EditWeekCount()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.SetModified(A<WeekCount>.Ignored)).DoesNothing();

            var weekCount = new WeekCount
            {
                SeasonID = 2018
            };

            // Act
            repository.EditEntity(dbContext, weekCount);

            // Assert
            A.CallTo(() => dbContext.SetModified(weekCount)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void FindEntity_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var seasonID = 2018;

            WeekCount weekCount = new WeekCount();
            A.CallTo(() => dbContext.WeekCounts.Find(A<int>.Ignored)).Returns(weekCount);

            // Act
            var result = repository.FindEntity(dbContext, seasonID);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Find(seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCount, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var seasonID = 2018;

            WeekCount weekCount = null;
            A.CallTo(() => dbContext.WeekCounts.Find(A<int>.Ignored)).Returns(weekCount);

            // Act
            WeekCount result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, seasonID); });

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Find(seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var id = 1;

            A.CallTo(() => dbContext.WeekCounts.Find(A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            WeekCount result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, id); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task FindEntityAsync_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var seasonID = 2018;

            WeekCount weekCount = new WeekCount();
            A.CallTo(() => dbContext.WeekCounts.FindAsync(A<int>.Ignored)).Returns(weekCount);

            // Act
            var result = await repository.FindEntityAsync(dbContext, seasonID);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.FindAsync(seasonID)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCount, result);
        }

        [TestCase]
        public void FindEntityAsync_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var seasonID = 2018;

            WeekCount weekCount = null;
            A.CallTo(() => dbContext.WeekCounts.FindAsync(A<int>.Ignored)).Returns(weekCount);

            // Act
            WeekCount result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, seasonID);
            });

            // Assert
            A.CallTo(() => dbContext.WeekCounts.FindAsync(seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntityAsync_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var seasonID = 2018;

            A.CallTo(() => dbContext.WeekCounts.FindAsync(A<int>.Ignored)).Throws<InvalidOperationException>();

            // Act
            WeekCount result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, seasonID);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetWeekCounts()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            var result = repository.GetEntities(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<WeekCount>>(result);
        }

        [TestCase]
        public async Task GetWeekCountsAsync()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeWeekCountsAsync();

            // Act
            var result = await repository.GetEntitiesAsync(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<WeekCount>>(result);
        }

        [TestCase]
        public void RemoveWeekCount()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var weekCount = new WeekCount
            {
                SeasonID = 2018
            };
            A.CallTo(() => dbContext.WeekCounts.Remove(A<WeekCount>.Ignored)).Returns(weekCount);

            // Act
            var result = repository.RemoveEntity(dbContext, weekCount);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.Remove(weekCount)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCount, result);
        }

        [TestCase]
        public void RemoveWeekCounts()
        {
            // Arrange
            var repository = new WeekCountRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var weekCounts = new List<WeekCount>();
            for (int i = 2016; i <= 2018; i++)
            {
                var weekCount = new WeekCount
                {
                    SeasonID = i
                };
                weekCounts.Add(weekCount);
            }
            A.CallTo(() => dbContext.WeekCounts.RemoveRange(A<IEnumerable<WeekCount>>.Ignored)).Returns(weekCounts);

            // Act
            var result = repository.RemoveEntities(dbContext, weekCounts);

            // Assert
            A.CallTo(() => dbContext.WeekCounts.RemoveRange(weekCounts)).MustHaveHappenedOnceExactly();
            Assert.AreSame(weekCounts, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new WeekCountRepository();

            // Act

            // Assert
        }

        #endregion Test Cases
    }
}
