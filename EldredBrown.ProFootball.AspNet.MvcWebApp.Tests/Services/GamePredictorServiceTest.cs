using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Services;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Tests.Services
{
    [TestFixture]
    public class GamePredictorServiceTest
    {
        private IDataMapper _dataMapper;
        private IRepository<Season> _seasonRepository;
        private IRepository<TeamSeason> _teamSeasonRepository;

        [SetUp]
        public void SetUp()
        {
            _dataMapper = A.Fake<IDataMapper>();
            _seasonRepository = A.Fake<IRepository<Season>>();
            _teamSeasonRepository = A.Fake<IRepository<TeamSeason>>();
        }

        [TestCase]
        public void ApplyFilter_GuestAndHostIdsNull_NoFiltersApplied()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            GamePredictorService.GuestSeasonID = null;
            GamePredictorService.HostSeasonID = null;

            int? guestSeasonID = null;
            int? hostSeasonID = null;

            // Act
            service.ApplyFilter(guestSeasonID, hostSeasonID);

            // Assert
            Assert.IsNull(GamePredictorService.GuestSeasonID);
            Assert.IsNull(GamePredictorService.HostSeasonID);
        }

        [TestCase]
        public void ApplyFilter_GuestIdNotNullAndHostIdNull_GuestFilterOnlyApplied()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            GamePredictorService.GuestSeasonID = null;
            GamePredictorService.HostSeasonID = null;

            int? guestSeasonID = 2017;
            int? hostSeasonID = null;

            // Act
            service.ApplyFilter(guestSeasonID, hostSeasonID);

            // Assert
            Assert.AreEqual(guestSeasonID, GamePredictorService.GuestSeasonID);
            Assert.IsNull(GamePredictorService.HostSeasonID);
        }

        [TestCase]
        public void ApplyFilter_GuestAndHostIdsNotNull_GuestAndHostFiltersApplied()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            GamePredictorService.GuestSeasonID = null;
            GamePredictorService.HostSeasonID = null;

            var seasonID = 2017;
            int? guestSeasonID = seasonID;
            int? hostSeasonID = seasonID;

            // Act
            service.ApplyFilter(guestSeasonID, hostSeasonID);

            // Assert
            Assert.AreEqual(seasonID, GamePredictorService.GuestSeasonID);
            Assert.AreEqual(seasonID, GamePredictorService.HostSeasonID);
        }

        [TestCase]
        public async Task GetGuestAndHostSeasonIds_GuestAndHostSeasonIdsNotNull_NoChangesMade()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            var seasonID = 2017;
            GamePredictorService.GuestSeasonID = seasonID;
            GamePredictorService.HostSeasonID = seasonID;

            var dbContext = A.Fake<ProFootballEntities>();

            var seasons = new List<Season>
            {
                new Season { ID = 2017 },
                new Season { ID = 2016 },
                new Season { ID = 2014 }
            };
            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).Returns(seasons);

            // Act
            await service.GetGuestAndHostSeasonIds(dbContext);

            // Assert
            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(seasonID, GamePredictorService.GuestSeasonID);
            Assert.AreEqual(seasonID, GamePredictorService.HostSeasonID);
        }

        [TestCase]
        public async Task GetGuestAndHostSeasonIds_GuestSeasonIdNullAndHostSeasonIdNotNull_GuestSeasonIdChanged()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            var seasonID = 2017;
            GamePredictorService.GuestSeasonID = null;
            GamePredictorService.HostSeasonID = seasonID;

            var dbContext = A.Fake<ProFootballEntities>();

            var seasons = new List<Season>
            {
                new Season { ID = 2017 },
                new Season { ID = 2016 },
                new Season { ID = 2014 }
            };
            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).Returns(seasons);

            // Act
            await service.GetGuestAndHostSeasonIds(dbContext);

            // Assert
            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            // Get guest season.
            Assert.AreEqual(seasonID, GamePredictorService.GuestSeasonID);
            Assert.AreEqual(seasonID, GamePredictorService.HostSeasonID);
        }

        [TestCase]
        public async Task GetGuestAndHostSeasonIds_GuestAndHostSeasonIdsNull_GuestAndHostSeasonIdsChanged()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            GamePredictorService.GuestSeasonID = null;
            GamePredictorService.HostSeasonID = null;

            var dbContext = A.Fake<ProFootballEntities>();

            var seasons = new List<Season>
            {
                new Season { ID = 2017 },
                new Season { ID = 2016 },
                new Season { ID = 2014 }
            };
            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).Returns(seasons);

            // Act
            await service.GetGuestAndHostSeasonIds(dbContext);

            // Assert
            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            // Get guest season.
            Assert.AreEqual(2017, GamePredictorService.GuestSeasonID);
            Assert.AreEqual(2017, GamePredictorService.HostSeasonID);
        }

        [TestCase]
        public async Task GetGuestAndHostSeasonIds_GuestAndHostSeasonIdsNullButArgumentNullExceptionCaught_GuestAndHostSeasonIdsNotChanged()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            GamePredictorService.GuestSeasonID = null;
            GamePredictorService.HostSeasonID = null;

            var dbContext = A.Fake<ProFootballEntities>();

            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).Throws<ArgumentNullException>();

            // Act
            await service.GetGuestAndHostSeasonIds(dbContext);

            // Assert
            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            // Get guest season.
            Assert.IsNull(GamePredictorService.GuestSeasonID);
            Assert.IsNull(GamePredictorService.HostSeasonID);
        }

        [TestCase]
        public async Task GetEntities_HappyPath()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            var seasonID = 2017;
            var dbContext = A.Fake<ProFootballEntities>();

            var count = 3;
            var teamSeasons = new List<TeamSeason>(3);
            for (int i = 1; i <= count; i++)
            {
                var teamSeason = new TeamSeason
                {
                    TeamName = "Team" + i,
                    SeasonID = 2017
                };
                teamSeasons.Add(teamSeason);
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);
            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.Ignored))
                .Returns(new TeamSeasonViewModel());

            // Act
            var result = await service.GetEntities(seasonID, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(count, Times.Exactly);

            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            Assert.AreEqual(count, result.Count());
        }

        [TestCase]
        public async Task GetEntities_ArgumentNullException_AbortsAndReturnsNull()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            var seasonID = 2017;
            var dbContext = A.Fake<ProFootballEntities>();

            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Throws<ArgumentNullException>();
            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.Ignored))
                .Returns(new TeamSeasonViewModel());

            // Act
            var result = await service.GetEntities(seasonID, dbContext);

            // Assert
            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull())).MustNotHaveHappened();
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new GamePredictorService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            // Act

            // Assert
        }
    }
}
