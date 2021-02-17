using System;
using System.Collections.Generic;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using EldredBrown.ProFootball.WpfApp.Services;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.WpfApp.Tests.Services
{
    [TestFixture]
    public class RankingsControlServiceTest
    {
        private ISharedService _sharedService;
        private IStoredProcedureRepository _storedProcedureRepository;

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _storedProcedureRepository = A.Fake<IStoredProcedureRepository>();
        }

        [TestCase]
        public void GetRankingsOffensiveBySeason_HappyPath()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var dbContext = A.Fake<ProFootballEntities>();

            var rankingsCount = 3;
            var rankings = new List<GetRankingsOffensive_Result>(rankingsCount);
            for (int i = 1; i <= rankingsCount; i++)
            {
                var ranking = new GetRankingsOffensive_Result
                {
                    Team = $"Team {i}"
                };
                rankings.Add(ranking);
            }
            dbContext.SetUpFakeRankingsOffensive(rankings);

            A.CallTo(() => _storedProcedureRepository.GetRankingsOffensive(A<int>.Ignored))
                .Returns(dbContext.GetRankingsOffensive(seasonID));

            // Act
            var result = service.GetRankingsOffensiveBySeason(seasonID);

            // Assert
            A.CallTo(() => _storedProcedureRepository.GetRankingsOffensive(seasonID)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetRankingsOffensive_Result>>(result);
            Assert.AreEqual(rankingsCount, result.Count());
        }

        [TestCase]
        public void GetRankingsOffensiveBySeason_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _storedProcedureRepository.GetRankingsOffensive(A<int>.Ignored)).Throws(ex);

            // Act
            var result = service.GetRankingsOffensiveBySeason(seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetRankingsOffensive_Result>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetRankingsOffensiveBySeason_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _storedProcedureRepository.GetRankingsOffensive(A<int>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<GetRankingsOffensive_Result> result = null;
            Assert.Throws<Exception>(() => result = service.GetRankingsOffensiveBySeason(seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetRankingsDefensiveBySeason_HappyPath()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var dbContext = A.Fake<ProFootballEntities>();

            var rankingsCount = 3;
            var rankings = new List<GetRankingsDefensive_Result>(rankingsCount);
            for (int i = 1; i <= rankingsCount; i++)
            {
                var ranking = new GetRankingsDefensive_Result
                {
                    Team = $"Team {i}"
                };
                rankings.Add(ranking);
            }
            dbContext.SetUpFakeRankingsDefensive(rankings);

            A.CallTo(() => _storedProcedureRepository.GetRankingsDefensive(A<int>.Ignored))
                .Returns(dbContext.GetRankingsDefensive(seasonID));

            // Act
            var result = service.GetRankingsDefensiveBySeason(seasonID);

            // Assert
            A.CallTo(() => _storedProcedureRepository.GetRankingsDefensive(seasonID)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetRankingsDefensive_Result>>(result);
            Assert.AreEqual(rankingsCount, result.Count());
        }

        [TestCase]
        public void GetRankingsDefensiveBySeason_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _storedProcedureRepository.GetRankingsDefensive(A<int>.Ignored)).Throws(ex);

            // Act
            var result = service.GetRankingsDefensiveBySeason(seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetRankingsDefensive_Result>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetRankingsDefensiveBySeason_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _storedProcedureRepository.GetRankingsDefensive(A<int>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<GetRankingsDefensive_Result> result = null;
            Assert.Throws<Exception>(() => result = service.GetRankingsDefensiveBySeason(seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetRankingsTotalBySeason_HappyPath()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var dbContext = A.Fake<ProFootballEntities>();

            var rankingsCount = 3;
            var rankings = new List<GetRankingsTotal_Result>(rankingsCount);
            for (int i = 1; i <= rankingsCount; i++)
            {
                var ranking = new GetRankingsTotal_Result
                {
                    Team = $"Team {i}"
                };
                rankings.Add(ranking);
            }
            dbContext.SetUpFakeRankingsTotal(rankings);

            A.CallTo(() => _storedProcedureRepository.GetRankingsTotal(A<int>.Ignored))
                .Returns(dbContext.GetRankingsTotal(seasonID));

            // Act
            var result = service.GetRankingsTotalBySeason(seasonID);

            // Assert
            A.CallTo(() => _storedProcedureRepository.GetRankingsTotal(seasonID)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetRankingsTotal_Result>>(result);
            Assert.AreEqual(rankingsCount, result.Count());
        }

        [TestCase]
        public void GetRankingsTotalBySeason_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _storedProcedureRepository.GetRankingsTotal(A<int>.Ignored)).Throws(ex);

            // Act
            var result = service.GetRankingsTotalBySeason(seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetRankingsTotal_Result>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetRankingsTotalBySeason_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _storedProcedureRepository.GetRankingsTotal(A<int>.Ignored)).Throws<Exception>();

            // Act
            IEnumerable<GetRankingsTotal_Result> result = null;
            Assert.Throws<Exception>(() => result = service.GetRankingsTotalBySeason(seasonID));

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new RankingsControlService(_sharedService, _storedProcedureRepository);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }
    }
}
