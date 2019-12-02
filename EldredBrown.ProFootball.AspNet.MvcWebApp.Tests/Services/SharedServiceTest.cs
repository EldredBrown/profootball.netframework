using System.Collections.Generic;
using System.Data.Entity.Core;
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
    public class SharedServiceTest
    {
        #region Member Fields

        private IDataMapper _dataMapper;
        private IRepository<Season> _seasonRepository;
        private IRepository<TeamSeason> _teamSeasonRepository;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _dataMapper = A.Fake<IDataMapper>();
            _seasonRepository = A.Fake<IRepository<Season>>();
            _teamSeasonRepository = A.Fake<IRepository<TeamSeason>>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public async Task FindEntityAsync_HappyPath()
        {
            // Arrange
            var service = new SharedService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            var teamName = "Team";
            var seasonID = 2017;
            var dbContext = A.Fake<ProFootballEntities>();

            var teamSeason = new TeamSeason();
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, A<string>.Ignored, A<int>.Ignored))
                .Returns(teamSeason);

            var teamSeasonViewModel = new TeamSeasonViewModel();
            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.Ignored)).Returns(teamSeasonViewModel);

            // Act
            var result = await service.FindEntityAsync(teamName, seasonID, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).MustHaveHappenedOnceExactly();
            Assert.AreSame(teamSeasonViewModel, result);
        }

        [TestCase]
        public async Task FindEntityAsync_ObjectNotFoundException_MethodAborts()
        {
            // Arrange
            var service = new SharedService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            var teamName = "Team";
            var seasonID = 2017;
            var dbContext = A.Fake<ProFootballEntities>();

            var teamSeason = new TeamSeason();
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, A<string>.Ignored, A<int>.Ignored))
                .Throws<ObjectNotFoundException>();

            var teamSeasonViewModel = new TeamSeasonViewModel();
            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.Ignored)).Returns(teamSeasonViewModel);

            // Act
            var result = await service.FindEntityAsync(teamName, seasonID, dbContext);

            // Assert
            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).MustNotHaveHappened();
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task GetSeasonsOrderedAsync()
        {
            // Arrange
            var service = new SharedService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            var dbContext = A.Fake<ProFootballEntities>();

            var count = 3;
            var seasons = new List<Season>(count);
            for (int i = 1; i <= count; i++)
            {
                var season = new Season
                {
                    ID = i
                };
                seasons.Add(season);
            }
            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).Returns(seasons);

            // Act
            var result = await service.GetSeasonsOrderedAsync(dbContext);

            // Assert
            A.CallTo(() => _seasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToSeasonViewModel(A<Season>.That.IsNotNull()))
                .MustHaveHappened(count, Times.Exactly);

            Assert.IsInstanceOf<IEnumerable<SeasonViewModel>>(result);
            Assert.AreEqual(count, result.Count());
        }

        [TestCase]
        public void SetSelectedSeason_SeasonIdNullAndGlobalsSelectedSeasonNull_FirstSeasonSelected()
        {
            // Arrange
            var service = new SharedService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            WebGlobals.SelectedSeason = null;

            var seasons = new List<SeasonViewModel>
            {
                new SeasonViewModel { ID = 2017 },
                new SeasonViewModel { ID = 2016 },
                new SeasonViewModel { ID = 2014 }
            };
            int? seasonID = null;
            int selectedSeason = 0;

            // Act
            service.SetSelectedSeason(seasons, seasonID, ref selectedSeason);

            // Assert
            Assert.AreEqual(seasons.First().ID, selectedSeason);
            Assert.AreEqual(selectedSeason, WebGlobals.SelectedSeason);
        }

        [TestCase]
        public void SetSelectedSeason_SeasonIdNullAndGlobalsSelectedSeasonNotNull_GlobalsSelectedSeasonSelected()
        {
            // Arrange
            var service = new SharedService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            WebGlobals.SelectedSeason = 2017;

            var seasons = new List<SeasonViewModel>
            {
                new SeasonViewModel { ID = 2017 },
                new SeasonViewModel { ID = 2016 },
                new SeasonViewModel { ID = 2014 }
            };
            int? seasonID = null;
            int selectedSeason = 0;

            // Act
            service.SetSelectedSeason(seasons, seasonID, ref selectedSeason);

            // Assert
            Assert.AreEqual((int)WebGlobals.SelectedSeason, selectedSeason);
        }

        [TestCase]
        public void SetSelectedSeason_SeasonIdNotNull_GlobalsSelectedSeasonSelected()
        {
            // Arrange
            var service = new SharedService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            WebGlobals.SelectedSeason = 2017;

            var seasons = new List<SeasonViewModel>
            {
                new SeasonViewModel { ID = 2017 },
                new SeasonViewModel { ID = 2016 },
                new SeasonViewModel { ID = 2014 }
            };
            int? seasonID = 2014;
            int selectedSeason = 0;

            // Act
            service.SetSelectedSeason(seasons, seasonID, ref selectedSeason);

            // Assert
            Assert.AreEqual((int)seasonID, selectedSeason);
            Assert.AreEqual(selectedSeason, WebGlobals.SelectedSeason);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new SharedService(_dataMapper, _seasonRepository, _teamSeasonRepository);

            // Act

            // Assert
        }

        #endregion Test Cases
    }
}
