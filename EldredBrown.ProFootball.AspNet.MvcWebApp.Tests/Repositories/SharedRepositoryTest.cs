using System.Collections.Generic;
using System.Threading.Tasks;
using EldredBrown.ProFootballApplicationWeb.Models;
using FakeItEasy;
using NUnit.Framework;
using ProFootballApplicationWeb.Tests.Repositories;

namespace EldredBrown.ProFootballApplicationWeb.Repositories.Tests
{
    [TestFixture]
    public class SharedRepositoryTest
    {
        [TestCase]
        public async Task FindTeamSeasonAsync()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeTeamSeasonsAsync();

            var teamName = "Team";
            var seasonID = 2017;

            // Act
            var repository = new SharedRepository();
            var result = await repository.FindTeamSeasonAsync(dbContext, teamName, seasonID);

            // Assert
            Assert.IsInstanceOf(typeof(TeamSeason), result);
        }

        [TestCase]
        public async Task GetAllSeasons()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeSeasonsAsync();

            // Act
            var repository = new SharedRepository();
            var result = await repository.GetAllSeasons(dbContext);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Season>), result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange

            // Act
            var repository = new SharedRepository();

            // Assert
        }
    }
}
