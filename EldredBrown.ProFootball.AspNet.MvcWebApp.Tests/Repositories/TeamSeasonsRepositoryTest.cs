using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using EldredBrown.ProFootballApplicationShared;
using EldredBrown.ProFootballApplicationWeb.Models;
using FakeItEasy;
using NUnit.Framework;
using ProFootballApplicationWeb.Tests.Repositories;

namespace EldredBrown.ProFootballApplicationWeb.Repositories.Tests
{
    [TestFixture]
    public class TeamSeasonsRepositoryTest
    {
        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsTeamAsc_SortOnTeamNameAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team3", SeasonID = 2017 },
                new TeamSeason { TeamName = "Team2", SeasonID = 2017 },
                new TeamSeason { TeamName = "Team1", SeasonID = 2017 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "team_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.TeamName.CompareTo(prevItem.TeamName) == 1);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsTeamDesc_SortOnTeamNameDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team1", SeasonID = 2017 },
                new TeamSeason { TeamName = "Team2", SeasonID = 2017 },
                new TeamSeason { TeamName = "Team3", SeasonID = 2017 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "team_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.TeamName.CompareTo(prevItem.TeamName) == -1);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsWinsAsc_SortOnWinsAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Wins = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Wins = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Wins = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "wins_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.Wins > prevItem.Wins);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsWinsDesc_SortOnWinsDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Wins = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Wins = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Wins = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "wins_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.Wins < prevItem.Wins);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsLossesAsc_SortOnLossesAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Losses = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Losses = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Losses = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "losses_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.Losses > prevItem.Losses);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsLossesDesc_SortOnLossesDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Losses = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Losses = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Losses = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "losses_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.Losses < prevItem.Losses);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsTiesAsc_SortOnTiesAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Ties = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Ties = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Ties = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "ties_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.Ties > prevItem.Ties);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsTiesDesc_SortOnTiesDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Ties = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Ties = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, Ties = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "ties_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.Ties < prevItem.Ties);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsWinPctAsc_SortOnWinningPercentageAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, WinningPercentage = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, WinningPercentage = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, WinningPercentage = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "win_pct_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.WinningPercentage > prevItem.WinningPercentage);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsWinPctDesc_SortOnWinningPercentageDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, WinningPercentage = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, WinningPercentage = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, WinningPercentage = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "win_pct_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.WinningPercentage < prevItem.WinningPercentage);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsPFAsc_SortOnPointsForAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsFor = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsFor = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsFor = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "pf_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.PointsFor > prevItem.PointsFor);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsPFDesc_SortOnPointsForDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsFor = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsFor = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsFor = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "pf_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.PointsFor < prevItem.PointsFor);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsPAAsc_SortOnPointsAgainstAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsAgainst = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsAgainst = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsAgainst = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "pa_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.PointsAgainst > prevItem.PointsAgainst);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsPADesc_SortOnPointsAgainstDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsAgainst = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsAgainst = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PointsAgainst = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "pa_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.PointsAgainst < prevItem.PointsAgainst);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsPythWinsAsc_SortOnPythagoreanWinsAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanWins = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanWins = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanWins = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "pyth_wins_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.PythagoreanWins > prevItem.PythagoreanWins);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsPythWinsDesc_SortOnPythagoreanWinsDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanWins = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanWins = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanWins = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "pyth_wins_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.PythagoreanWins < prevItem.PythagoreanWins);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsPythLossesAsc_SortOnPythagoreanLossesAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanLosses = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanLosses = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanLosses = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "pyth_losses_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.PythagoreanLosses > prevItem.PythagoreanLosses);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsPythLossesDesc_SortOnPythagoreanLossesDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanLosses = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanLosses = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, PythagoreanLosses = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "pyth_losses_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.PythagoreanLosses < prevItem.PythagoreanLosses);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsOffAvgAsc_SortOnOffensiveAverageAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveAverage = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveAverage = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveAverage = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "off_avg_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.OffensiveAverage > prevItem.OffensiveAverage);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsOffAvgDesc_SortOnOffensiveAverageDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveAverage = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveAverage = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveAverage = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "off_avg_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.OffensiveAverage < prevItem.OffensiveAverage);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsOffFactorAsc_SortOnOffensiveFactorAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveFactor = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveFactor = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveFactor = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "off_factor_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.OffensiveFactor > prevItem.OffensiveFactor);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsOffFactorDesc_SortOnOffensiveFactorDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveFactor = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveFactor = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveFactor = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "off_factor_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.OffensiveFactor < prevItem.OffensiveFactor);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsOffIndexAsc_SortOnOffensiveIndexAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveIndex = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveIndex = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveIndex = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "off_index_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.OffensiveIndex > prevItem.OffensiveIndex);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsOffIndexDesc_SortOnOffensiveIndexDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveIndex = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveIndex = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, OffensiveIndex = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "off_index_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.OffensiveIndex < prevItem.OffensiveIndex);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsDefAvgAsc_SortOnDefensiveAverageAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveAverage = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveAverage = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveAverage = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "def_avg_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.DefensiveAverage > prevItem.DefensiveAverage);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsDefAvgDesc_SortOnDefensiveAverageDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveAverage = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveAverage = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveAverage = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "def_avg_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.DefensiveAverage < prevItem.DefensiveAverage);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsDefFactorAsc_SortOnDefensiveFactorAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveFactor = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveFactor = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveFactor = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "def_factor_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.DefensiveFactor > prevItem.DefensiveFactor);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsDefFactorDesc_SortOnDefensiveFactorDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveFactor = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveFactor = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveFactor = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "def_factor_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.DefensiveFactor < prevItem.DefensiveFactor);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsDefIndexAsc_SortOnDefensiveIndexAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveIndex = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveIndex = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveIndex = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "def_index_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.DefensiveIndex > prevItem.DefensiveIndex);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsDefIndexDesc_SortOnDefensiveIndexDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveIndex = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveIndex = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, DefensiveIndex = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "def_index_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.DefensiveIndex < prevItem.DefensiveIndex);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsFinPythPctAsc_SortOnFinalPythagoreanWinningPercentageAscending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, FinalPythagoreanWinningPercentage = 3 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, FinalPythagoreanWinningPercentage = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, FinalPythagoreanWinningPercentage = 1 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "fin_pyth_pct_asc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.FinalPythagoreanWinningPercentage > prevItem.FinalPythagoreanWinningPercentage);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsFinPythPctDesc_SortOnFinalPythagoreanWinningPercentageDescending()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017, FinalPythagoreanWinningPercentage = 1 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, FinalPythagoreanWinningPercentage = 2 },
                new TeamSeason { TeamName = "Team", SeasonID = 2017, FinalPythagoreanWinningPercentage = 3 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = "fin_pyth_pct_desc";

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.FinalPythagoreanWinningPercentage < prevItem.FinalPythagoreanWinningPercentage);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public async Task GetTeamSeasons_SortOrderIsNotSelected_SortOnTeamNameAscendingByDefault()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team3", SeasonID = 2017 },
                new TeamSeason { TeamName = "Team2", SeasonID = 2017 },
                new TeamSeason { TeamName = "Team1", SeasonID = 2017 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var seasonID = 2017;
            var sortOrder = string.Empty;

            // Act
            var repository = new TeamSeasonsRepository();
            var result = await repository.GetTeamSeasons(dbContext, seasonID, sortOrder);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<TeamSeason>), result);

            var corrOrder = true;
            for (int i = 1; i < result.Count(); i++)
            {
                var currItem = result.ElementAt(i);
                var prevItem = result.ElementAt(i - 1);
                corrOrder &= (currItem.TeamName.CompareTo(prevItem.TeamName) == 1);
            }
            Assert.IsTrue(corrOrder);
        }

        [TestCase]
        public void GetTeamSeasonScheduleAverages()
        {
            // Arrange
            var teamName = "Team";
            var seasonID = 2017;

            var dbContext = A.Fake<ProFootballDbEntities>();
            var fakeObjectResult = A.Fake<ObjectResult<usp_GetTeamSeasonScheduleAverages_Result>>();
            A.CallTo(() => dbContext.usp_GetTeamSeasonScheduleAverages(teamName, seasonID)).Returns(fakeObjectResult);

            // Act
            var repository = new TeamSeasonsRepository();
            var result = repository.GetTeamSeasonScheduleAverages(dbContext, teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.usp_GetTeamSeasonScheduleAverages(teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf(typeof(ObjectResult<usp_GetTeamSeasonScheduleAverages_Result>), result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleProfile()
        {
            // Arrange
            var teamName = "Team";
            var seasonID = 2017;

            var dbContext = A.Fake<ProFootballDbEntities>();
            var fakeObjectResult = A.Fake<ObjectResult<usp_GetTeamSeasonScheduleProfile_Result>>();
            A.CallTo(() => dbContext.usp_GetTeamSeasonScheduleProfile(teamName, seasonID)).Returns(fakeObjectResult);

            // Act
            var repository = new TeamSeasonsRepository();
            var result = repository.GetTeamSeasonScheduleProfile(dbContext, teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.usp_GetTeamSeasonScheduleProfile(teamName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf(typeof(ObjectResult<usp_GetTeamSeasonScheduleProfile_Result>), result);
        }

        [TestCase]
        public void GetTeamSeasonScheduleTotals()
        {
            // Arrange
            var teamName = "Team";
            var seasonID = 2017;

            var dbContext = A.Fake<ProFootballDbEntities>();
            var fakeObjectResult = A.Fake<ObjectResult<usp_GetTeamSeasonScheduleTotals_Result>>();
            A.CallTo(() => dbContext.usp_GetTeamSeasonScheduleTotals(teamName, seasonID)).Returns(fakeObjectResult);

            // Act
            var repository = new TeamSeasonsRepository();
            var result = repository.GetTeamSeasonScheduleTotals(dbContext, teamName, seasonID);

            // Assert
            A.CallTo(() => dbContext.usp_GetTeamSeasonScheduleTotals(teamName, seasonID)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf(typeof(ObjectResult<usp_GetTeamSeasonScheduleTotals_Result>), result);
        }

        [TestCase]
        public async Task UpdateRankings()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeLeagueSeasonTotals();
            dbContext.SetUpFakeLeagueSeasonsAsync();
            dbContext.SetUpFakeTeamSeasonsAsync();

            var selectedSeason = 2017;

            // Act
            var repository = new TeamSeasonsRepository();
            await repository.UpdateRankings(dbContext, selectedSeason);

            // Assert
            A.CallTo(() => dbContext.usp_GetLeagueSeasonTotals(selectedSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedTwiceExactly();
        }

        [TestCase]
        public void UpdateRankingsByTeamSeason_TeamSeasonScheduleTotalsIsNull_MethodAborts()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeLeagueSeasons();
            var selectedSeason = 2017;
            var teamSeason = new TeamSeason
            {
                TeamName = "Team",
                SeasonID = selectedSeason
            };
            ICalculator calculator = null;

            // Act
            var repository = new TeamSeasonsRepository();
            repository.UpdateRankingsByTeamSeason(dbContext, selectedSeason, teamSeason, calculator);

            // Assert
            Assert.IsNull(calculator);
        }

        [TestCase]
        public void UpdateRankingsByTeamSeason_TeamSeasonScheduleAveragesIsNull_MethodAborts()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeLeagueSeasons();

            var teamSeasonScheduleTotals = new List<usp_GetTeamSeasonScheduleTotals_Result>
            {
                new usp_GetTeamSeasonScheduleTotals_Result()
            };
            dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotals);

            var selectedSeason = 2017;
            var teamSeason = new TeamSeason
            {
                TeamName = "Team",
                SeasonID = selectedSeason
            };
            ICalculator calculator = null;

            // Act
            var repository = new TeamSeasonsRepository();
            repository.UpdateRankingsByTeamSeason(dbContext, selectedSeason, teamSeason, calculator);

            // Assert
            Assert.IsNull(calculator);
        }

        [TestCase]
        public void UpdateRankingsByTeamSeason_TeamSeasonScheduleScheduleGamesIsNull_MethodAborts()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeLeagueSeasons();

            var teamSeasonScheduleTotals = new List<usp_GetTeamSeasonScheduleTotals_Result>
            {
                new usp_GetTeamSeasonScheduleTotals_Result()
            };
            dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotals);

            var teamSeasonScheduleAverages = new List<usp_GetTeamSeasonScheduleAverages_Result>
            {
                new usp_GetTeamSeasonScheduleAverages_Result()
            };
            dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAverages);

            var selectedSeason = 2017;
            var teamSeason = new TeamSeason
            {
                TeamName = "Team",
                SeasonID = selectedSeason
            };
            ICalculator calculator = null;

            // Act
            var repository = new TeamSeasonsRepository();
            repository.UpdateRankingsByTeamSeason(dbContext, selectedSeason, teamSeason, calculator);

            // Assert
            Assert.IsNull(calculator);
        }

        [TestCase]
        public void UpdateRankingsByTeamSeason_NoStoredProceduresReturnNull_MethodCompletes()
        {
            // Arrange
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeLeagueSeasons();

            var teamSeasonScheduleTotals = new List<usp_GetTeamSeasonScheduleTotals_Result>
            {
                new usp_GetTeamSeasonScheduleTotals_Result
                {
                    ScheduleGames = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotals);

            var teamSeasonScheduleAverages = new List<usp_GetTeamSeasonScheduleAverages_Result>
            {
                new usp_GetTeamSeasonScheduleAverages_Result
                {
                    PointsFor = 1,
                    PointsAgainst = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAverages);

            var selectedSeason = 2017;
            var teamSeason = new TeamSeason
            {
                TeamName = "Team",
                SeasonID = selectedSeason,
                LeagueName = "NFL",
                OffensiveAverage = 1,
                OffensiveFactor = 1,
                DefensiveAverage = 1,
                DefensiveFactor = 1
            };
            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(1d);

            // Act
            var repository = new TeamSeasonsRepository();
            repository.UpdateRankingsByTeamSeason(dbContext, selectedSeason, teamSeason, calculator);

            // Assert
            Assert.IsInstanceOf(typeof(ICalculator), calculator);
            A.CallTo(() => calculator.Divide(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange

            // Act
            var repository = new TeamSeasonsRepository();

            // Assert
        }
    }
}
