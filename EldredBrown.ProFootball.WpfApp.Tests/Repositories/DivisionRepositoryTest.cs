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
    public class DivisionRepositoryTest
    {
        #region Test Cases

        [TestCase]
        public void AddDivision_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var division = new Division();
            A.CallTo(() => dbContext.Divisions.Add(A<Division>.Ignored)).Returns(division);

            // Act
            var result = repository.AddEntity(division);

            // Assert
            A.CallTo(() => dbContext.Divisions.Add(division)).MustHaveHappenedOnceExactly();
            Assert.AreSame(division, result);
        }

        [TestCase]
        public void AddDivision_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var division = new Division();
            A.CallTo(() => dbContext.Divisions.Add(A<Division>.Ignored)).Throws<Exception>();

            // Act
            Division result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntity(division));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void AddDivisions_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var divisions = new List<Division>();
            A.CallTo(() => dbContext.Divisions.AddRange(A<IEnumerable<Division>>.Ignored)).Returns(divisions);

            // Act
            var result = repository.AddEntities(divisions);

            // Assert
            A.CallTo(() => dbContext.Divisions.AddRange(divisions)).MustHaveHappenedOnceExactly();
            Assert.AreSame(divisions, result);
        }

        [TestCase]
        public void AddDivisions_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var divisions = new List<Division>();
            A.CallTo(() => dbContext.Divisions.AddRange(A<IEnumerable<Division>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Division> result = null;
            Assert.Throws<Exception>(() => result = repository.AddEntities(divisions));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void CreateDivision_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            A.CallTo(() => dbContext.Divisions.Create()).Returns(new Division());

            // Act
            var result = repository.CreateEntity();

            // Assert
            A.CallTo(() => dbContext.Divisions.Create()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<Division>(result);
        }

        [TestCase]
        public void CreateDivision_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            A.CallTo(() => dbContext.Divisions.Create()).Throws<Exception>();

            // Act
            Division result = null;
            Assert.Throws<Exception>(() => result = repository.CreateEntity());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void EditDivision_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var division = new Division();

            // Act
            repository.EditEntity(division);

            // Assert
            A.CallTo(() => dbContext.SetModified(division)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditDivision_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var division = new Division();

            A.CallTo(() => dbContext.SetModified(A<Division>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => repository.EditEntity(division));
        }

        [TestCase]
        public void FindEntity_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var name = "Division";

            var division = new Division();
            A.CallTo(() => dbContext.Divisions.Find(A<string>.Ignored)).Returns(division);

            // Act
            var result = repository.FindEntity(name);

            // Assert
            A.CallTo(() => dbContext.Divisions.Find(name)).MustHaveHappenedOnceExactly();
            Assert.AreSame(division, result);
        }

        [TestCase]
        public void FindEntity_EntityNotFoundInDataStore_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var name = "Division";

            Division division = null;
            A.CallTo(() => dbContext.Divisions.Find(A<string>.Ignored)).Returns(division);

            // Act
            Division result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(name); });

            // Assert
            A.CallTo(() => dbContext.Divisions.Find(name)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_InvalidOperationExceptionCaught_ThrowsObjectNotFoundException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var name = "Division";

            A.CallTo(() => dbContext.Divisions.Find(A<string>.Ignored)).Throws<InvalidOperationException>();

            // Act
            Division result = null;
            Assert.Throws<ObjectNotFoundException>(() => { result = repository.FindEntity(name); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void FindEntity_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var name = "Division";

            A.CallTo(() => dbContext.Divisions.Find(A<string>.Ignored)).Throws<Exception>();

            // Act
            Division result = null;
            Assert.Throws<Exception>(() => result = repository.FindEntity(name));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetDivisions_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            // Act
            var result = repository.GetEntities();

            // Assert
            Assert.IsInstanceOf<IEnumerable<Division>>(result);
        }

        [TestCase]
        public void GetDivisions_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            A.CallTo(() => dbContext.Divisions).Throws<Exception>();

            // Act
            IEnumerable<Division> result = null;
            Assert.Throws<Exception>(() => result = repository.GetEntities());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveDivision_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var division = new Division();
            A.CallTo(() => dbContext.Divisions.Remove(A<Division>.Ignored)).Returns(division);

            // Act
            var result = repository.RemoveEntity(division);

            // Assert
            A.CallTo(() => dbContext.Divisions.Remove(division)).MustHaveHappenedOnceExactly();
            Assert.AreSame(division, result);
        }

        [TestCase]
        public void RemoveDivision_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var division = new Division();
            A.CallTo(() => dbContext.Divisions.Remove(A<Division>.Ignored)).Throws<Exception>();

            // Act
            Division result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntity(division));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void RemoveDivisions_HappyPath()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var divisions = new List<Division>();
            A.CallTo(() => dbContext.Divisions.RemoveRange(A<IEnumerable<Division>>.Ignored)).Returns(divisions);

            // Act
            var result = repository.RemoveEntities(divisions);

            // Assert
            A.CallTo(() => dbContext.Divisions.RemoveRange(divisions)).MustHaveHappenedOnceExactly();
            Assert.AreSame(divisions, result);
        }

        [TestCase]
        public void RemoveDivisions_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

            var divisions = new List<Division>();
            A.CallTo(() => dbContext.Divisions.RemoveRange(A<IEnumerable<Division>>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<Division> result = null;
            Assert.Throws<Exception>(() => result = repository.RemoveEntities(divisions));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballEntities>();
            var repository = new DivisionRepository(dbContext);

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
