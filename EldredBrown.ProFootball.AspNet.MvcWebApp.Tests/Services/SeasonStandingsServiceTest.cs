using System.Collections.Generic;
using System.Linq;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Services;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Tests.Repositories;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Tests.Services
{
    [TestFixture]
    public class SeasonStandingsServiceTest
    {
        private ISharedService _sharedService;
        private IDataMapper _dataMapper;
        private IStoredProcedureRepository _storedProcedureRepository;

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _dataMapper = A.Fake<IDataMapper>();
            _storedProcedureRepository = A.Fake<IStoredProcedureRepository>();
        }

        [TestCase]
        public void GetSeasonStandings()
        {
            // Arrange
            var service = new SeasonStandingsService(_sharedService, _dataMapper, _storedProcedureRepository);

            var selectedSeason = 2017;
            var groupByDivision = false;
            var dbContext = A.Fake<ProFootballEntities>();

            var count = 3;
            var seasonStandings = new List<GetSeasonStandings_Result>(count);
            for (int i = 0; i < count; i++)
            {
                seasonStandings.Add(new GetSeasonStandings_Result());
            }
            dbContext.SetUpFakeSeasonStandings(seasonStandings);

            var seasonStandingResults = dbContext.GetSeasonStandings(selectedSeason, groupByDivision);
            A.CallTo(() => _storedProcedureRepository.GetSeasonStandings(dbContext, A<int>.Ignored, A<bool>.Ignored))
                .Returns(seasonStandingResults);

            A.CallTo(() => _dataMapper.MapToSeasonStandingsResultViewModel(A<GetSeasonStandings_Result>.Ignored))
                .Returns(new SeasonStandingsResultViewModel());

            // Act
            var result = service.GetSeasonStandings(selectedSeason, groupByDivision, dbContext);

            // Assert
            A.CallTo(() => _storedProcedureRepository.GetSeasonStandings(dbContext, selectedSeason, groupByDivision))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() =>
                    _dataMapper.MapToSeasonStandingsResultViewModel(A<GetSeasonStandings_Result>.That.IsNotNull()))
                .MustHaveHappened(count, Times.Exactly);

            Assert.IsInstanceOf<IEnumerable<SeasonStandingsResultViewModel>>(result);
            Assert.AreEqual(count, result.Count());
        }

        [TestCase]
        public void SetSelectedSeason()
        {
            // Arrange
            var service = new SeasonStandingsService(_sharedService, _dataMapper, _storedProcedureRepository);

            var seasons = new List<SeasonViewModel>();
            var seasonID = 2017;

            // Act
            service.SetSelectedSeason(seasons, seasonID);

            // Assert
            A.CallTo(() =>
                    _sharedService.SetSelectedSeason(seasons, seasonID, ref SeasonStandingsService.SelectedSeason))
                .MustHaveHappenedOnceExactly();
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new SeasonStandingsService(_sharedService, _dataMapper, _storedProcedureRepository);

            // Act

            // Assert
        }
    }
}
