using System.Collections.Generic;
using System.Threading.Tasks;
using EldredBrown.ProFootballApplicationWeb.Models;
using FakeItEasy;
using NUnit.Framework;
using ProFootballApplicationWeb.Tests.Repositories;

namespace EldredBrown.ProFootballApplicationWeb.Repositories.Tests
{
    [TestFixture]
    public class GamePredictorRepositoryTest
    {
        [TestCase]
        public async Task GetTeamSeasons()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeTeamSeasonsAsync();

            var seasonID = 2017;

            // Act
            var repository = new GamePredictorRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();

            // Act
            var repository = new GamePredictorRepository();

            // Assert
        }
    }
}
