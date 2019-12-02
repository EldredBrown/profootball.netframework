using System;
using System.Collections.Generic;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.WpfApp.Tests.Services
{
    [TestFixture]
    public class GamePredictorWindowServiceTest
    {
        #region Member Fields

        private IRepository<Season> _seasonRepository;
        private IRepository<TeamSeason> _teamSeasonRepository;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _seasonRepository = A.Fake<IRepository<Season>>();
            _teamSeasonRepository = A.Fake<IRepository<TeamSeason>>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void GetSeasonIDs_HappyPath()
        {
            // Arrange
            var service = new GamePredictorWindowService(_seasonRepository, _teamSeasonRepository);

            // Set up needed infrastructure of class under test.
            var lastSeason = 2017;
            var seasonCount = 3;
            var seasons = new List<Season>(seasonCount);
            for (int i = lastSeason; i > lastSeason - seasonCount; i--)
            {
                var season = new Season
                {
                    ID = i
                };
                seasons.Add(season);
            }
            A.CallTo(() => _seasonRepository.GetEntities()).Returns(seasons);

            // Act
            var result = service.GetSeasonIDs();

            // Assert
            A.CallTo(() => _seasonRepository.GetEntities()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<List<int>>(result);

            var resultAsList = result as IList<int>;
            Assert.AreEqual(seasonCount, resultAsList.Count);
            for (int i = 0; i < seasonCount; i++)
            {
                Assert.AreEqual(lastSeason - i, resultAsList[i]);
            }
        }

        [TestCase]
        public void GetSeasonIDs_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new GamePredictorWindowService(_seasonRepository, _teamSeasonRepository);

            A.CallTo(() => _seasonRepository.GetEntities()).Throws<Exception>();

            // Act
            IEnumerable<int> result = null;
            Assert.Throws<Exception>(() => result = service.GetSeasonIDs());

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            // TODO: Instantiate class under test.

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
