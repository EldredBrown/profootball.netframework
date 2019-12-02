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
    public class DivisionRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddDivision()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var division = new Division
            {
                Name = "Division"
            };
            A.CallTo(() => dbContext.Divisions.Add(A<Division>.Ignored)).Returns(division);

            // Act
            var result = repository.AddEntity(dbContext, division);

            // Assert
            A.CallTo(() => dbContext.Divisions.Add(division)).MustHaveHappenedOnceExactly();
            Assert.AreSame(division, result);
        }

        [TestCase]
        public void AddDivisions()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var divisions = new List<Division>();
            for (int i = 1; i <= 3; i++)
            {
                var division = new Division
                {
                    Name = "Division " + i
                };
                divisions.Add(division);
            }
            A.CallTo(() => dbContext.Divisions.AddRange(A<IEnumerable<Division>>.Ignored)).Returns(divisions);

            // Act
            var result = repository.AddEntities(dbContext, divisions);

            // Assert
            A.CallTo(() => dbContext.Divisions.AddRange(divisions)).MustHaveHappenedOnceExactly();
            Assert.AreSame(divisions, result);
        }

        [TestCase]
        public void CreateDivision()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.Divisions.Create()).Returns(new Division());

            // Act
            var result = repository.CreateEntity(dbContext);

            // Assert
            A.CallTo(() => dbContext.Divisions.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Division>(result);
        }

        [TestCase]
        public void EditDivision()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            A.CallTo(() => dbContext.SetModified(A<Division>.Ignored)).DoesNothing();

            var division = new Division
            {
                Name = "Division"
            };

            // Act
            repository.EditEntity(dbContext, division);

            // Assert
            A.CallTo(() => dbContext.SetModified(division)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void FindEntity_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Division";

            Division division = new Division();
            A.CallTo(() => dbContext.Divisions.Find(A<string>.Ignored)).Returns(division);

            // Act
            var result = repository.FindEntity(dbContext, name);

            // Assert
            A.CallTo(() => dbContext.Divisions.Find(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(division, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Division";

            Division division = null;
            A.CallTo(() => dbContext.Divisions.Find(A<string>.Ignored)).Returns(division);

            // Act
            Division result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, name); });

            // Assert
            A.CallTo(() => dbContext.Divisions.Find(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Division";

            A.CallTo(() => dbContext.Divisions.Find(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Division result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(dbContext, name); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task FindEntityAsync_EntityFoundInDataStore_ReturnsEntity()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Division";

            Division division = new Division();
            A.CallTo(() => dbContext.Divisions.FindAsync(A<string>.Ignored)).Returns(division);

            // Act
            var result = await repository.FindEntityAsync(dbContext, name);

            // Assert
            A.CallTo(() => dbContext.Divisions.FindAsync(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(division, result);
        }

        [TestCase]
        public void FindEntityAsync_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Division";

            Division division = null;
            A.CallTo(() => dbContext.Divisions.FindAsync(A<string>.Ignored)).Returns(division);

            // Act
            Division result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, name);
            });

            // Assert
            A.CallTo(() => dbContext.Divisions.FindAsync(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntityAsync_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var name = "Division";

            A.CallTo(() => dbContext.Divisions.FindAsync(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Division result = null;
            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                result = await repository.FindEntityAsync(dbContext, name);
            });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetDivisions()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();

            // Act
            var result = repository.GetEntities(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Division>>(result);
        }

        [TestCase]
        public async Task GetDivisionsAsync()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            dbContext.SetUpFakeDivisionsAsync();

            // Act
            var result = await repository.GetEntitiesAsync(dbContext);

            // Assert
            Assert.IsInstanceOf<IEnumerable<Division>>(result);
        }

        [TestCase]
        public void RemoveDivision()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var division = new Division
            {
                Name = "Division"
            };
            A.CallTo(() => dbContext.Divisions.Remove(A<Division>.Ignored)).Returns(division);

            // Act
            var result = repository.RemoveEntity(dbContext, division);

            // Assert
            A.CallTo(() => dbContext.Divisions.Remove(division)).MustHaveHappenedOnceExactly();
            Assert.AreSame(division, result);
        }

        [TestCase]
        public void RemoveDivisions()
        {
            // Arrange
            var repository = new DivisionRepository();

            var dbContext = A.Fake<ProFootballEntities>();
            var divisions = new List<Division>();
            for (int i = 1; i <= 3; i++)
            {
                var division = new Division
                {
                    Name = "Division " + i
                };
                divisions.Add(division);
            }
            A.CallTo(() => dbContext.Divisions.RemoveRange(A<IEnumerable<Division>>.Ignored)).Returns(divisions);

            // Act
            var result = repository.RemoveEntities(dbContext, divisions);

            // Assert
            A.CallTo(() => dbContext.Divisions.RemoveRange(divisions)).MustHaveHappenedOnceExactly();
            Assert.AreSame(divisions, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var repository = new DivisionRepository();

            // Act

            // Assert
        }

        #endregion Test Cases
    }
}
