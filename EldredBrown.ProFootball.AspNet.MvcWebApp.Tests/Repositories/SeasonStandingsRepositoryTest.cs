using System.Data.Entity.Core.Objects;
using EldredBrown.ProFootballApplicationWeb.Models;
using FakeItEasy;
using NUnit.Framework;
using ProFootballApplicationWeb.Tests.Repositories;

namespace EldredBrown.ProFootballApplicationWeb.Repositories.Tests
{
    [TestFixture]
    public class SeasonStandingsRepositoryTest
    {
        [TestCase]
        public void GetSeasonStandings()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeSeasonStandings();

            var selectedSeason = 2017;
            var groupByDivision = false;

            // Act
            var repository = new SeasonStandingsRepository();
            var result = repository.GetSeasonStandings(dbContext, selectedSeason, groupByDivision);

            // Assert
            Assert.IsInstanceOf(typeof(ObjectResult<usp_GetSeasonStandings_Result>), result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();

            // Act
            var repository = new SeasonStandingsRepository();

            // Assert
        }
    }
}
