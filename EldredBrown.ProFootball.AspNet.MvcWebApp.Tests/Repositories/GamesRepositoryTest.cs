using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class GamesRepositoryTest
    {
        [TestCase]
        public async Task AddGame()
        {
            // Arrange
            const double Sum = 2d;
            const double Product = 2;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);
            dbContext.SetUpFakeGamesAsync();

            var newGame = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.AddGame(dbContext, newGame);

            // Assert
            A.CallTo(() => dbContext.Games.Add(A<Game>.Ignored)).MustHaveHappenedOnceExactly();

            #region  await repository.EditTeams(newGame, Direction.Up);
            #region await repository.ProcessGame(game, operation);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == newGame.GuestName && ts.SeasonID == newGame.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == newGame.HostName && ts.SeasonID == newGame.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();
            var winnerSeason = await (from ts in dbContext.TeamSeasons
                                      where ts.TeamName == newGame.WinnerName && ts.SeasonID == newGame.SeasonID
                                      select ts)
                                      .FirstOrDefaultAsync();
            var loserSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == newGame.LoserName && ts.SeasonID == newGame.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();

            #region await EditWinLossData(dbContext, game, operation);

            Assert.AreEqual(Sum, guestSeason.Games);
            Assert.AreEqual(Sum, hostSeason.Games);

            Assert.AreEqual(Sum, winnerSeason.Wins);
            Assert.AreEqual(Sum, loserSeason.Losses);

            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            Assert.AreEqual(Sum, guestSeason.PointsFor);
            Assert.AreEqual(Sum, guestSeason.PointsAgainst);
            Assert.AreEqual(Product, guestSeason.PythagoreanWins);
            Assert.AreEqual(Product, guestSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            Assert.AreEqual(Sum, hostSeason.PointsFor);
            Assert.AreEqual(Sum, hostSeason.PointsAgainst);
            Assert.AreEqual(Product, hostSeason.PythagoreanWins);
            Assert.AreEqual(Product, hostSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);

            #endregion await repository.ProcessGame(dbContext, game, operation);
            #endregion  await repository.EditTeams(dbContext, newGame, Direction.Up);

            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task AddGameToTeams()
        {
            // Arrange
            const double Sum = 2d;
            const double Product = 2;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var newGame = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.AddGameToTeams(dbContext, newGame);

            // Assert
            #region  await repository.EditTeams(dbContext, newGame, Direction.Up);
            #region await repository.ProcessGame(dbContext, game, operation);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == newGame.GuestName && ts.SeasonID == newGame.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == newGame.HostName && ts.SeasonID == newGame.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();
            var winnerSeason = await (from ts in dbContext.TeamSeasons
                                      where ts.TeamName == newGame.WinnerName && ts.SeasonID == newGame.SeasonID
                                      select ts)
                                      .FirstOrDefaultAsync();
            var loserSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == newGame.LoserName && ts.SeasonID == newGame.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();

            #region await EditWinLossData(dbContext, game, operation);

            Assert.AreEqual(Sum, guestSeason.Games);
            Assert.AreEqual(Sum, hostSeason.Games);

            Assert.AreEqual(Sum, winnerSeason.Wins);
            Assert.AreEqual(Sum, loserSeason.Losses);

            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            Assert.AreEqual(Sum, guestSeason.PointsFor);
            Assert.AreEqual(Sum, guestSeason.PointsAgainst);
            Assert.AreEqual(Product, guestSeason.PythagoreanWins);
            Assert.AreEqual(Product, guestSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            Assert.AreEqual(Sum, hostSeason.PointsFor);
            Assert.AreEqual(Sum, hostSeason.PointsAgainst);
            Assert.AreEqual(Product, hostSeason.PythagoreanWins);
            Assert.AreEqual(Product, hostSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);

            #endregion await repository.ProcessGame(dbContext, game, operation);
            #endregion  await repository.EditTeams(dbContext, newGame, Direction.Up);
        }

        [TestCase]
        public async Task DeleteGame()
        {
            // Arrange
            const double Difference = 0;
            const double Product = 2;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).Returns(Difference);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);
            dbContext.SetUpFakeGamesAsync();

            var id = 1;

            var oldGame = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };

            A.CallTo(() => dbContext.Games.FindAsync(id)).Returns(oldGame);

            var copyGame = new Game(oldGame);

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.DeleteGame(dbContext, id);

            // Assert
            A.CallTo(() => dbContext.Games.FindAsync(id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => dbContext.Games.Remove(oldGame)).MustHaveHappenedOnceExactly();

            #region await repository.EditTeams(dbContext, oldGame, Direction.Down);
            #region await repository.ProcessGame(dbContext, game, operation);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == oldGame.GuestName && ts.SeasonID == oldGame.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == oldGame.HostName && ts.SeasonID == oldGame.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();
            var winnerSeason = await (from ts in dbContext.TeamSeasons
                                      where ts.TeamName == oldGame.WinnerName && ts.SeasonID == oldGame.SeasonID
                                      select ts)
                                      .FirstOrDefaultAsync();
            var loserSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == oldGame.LoserName && ts.SeasonID == oldGame.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();

            #region await EditWinLossData(dbContext, game, operation);

            Assert.AreEqual(Difference, guestSeason.Games);
            Assert.AreEqual(Difference, hostSeason.Games);

            Assert.AreEqual(Difference, winnerSeason.Wins);
            Assert.AreEqual(Difference, loserSeason.Losses);

            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            Assert.AreEqual(Difference, guestSeason.PointsFor);
            Assert.AreEqual(Difference, guestSeason.PointsAgainst);
            Assert.AreEqual(Product, guestSeason.PythagoreanWins);
            Assert.AreEqual(Product, guestSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            Assert.AreEqual(Difference, hostSeason.PointsFor);
            Assert.AreEqual(Difference, hostSeason.PointsAgainst);
            Assert.AreEqual(Product, hostSeason.PythagoreanWins);
            Assert.AreEqual(Product, hostSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);

            #endregion await repository.ProcessGame(dbContext, game, operation);
            #endregion await repository.EditTeams(dbContext, oldGame, Direction.Down);

            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task DeleteGameFromTeams()
        {
            // Arrange
            const double Difference = 0;
            const double Product = 2;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).Returns(Difference);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var oldGame = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.DeleteGameFromTeams(dbContext, oldGame);

            // Assert
            #region await repository.EditTeams(dbContext, oldGame, Direction.Down);
            #region await repository.ProcessGame(dbContext, game, operation);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == oldGame.GuestName && ts.SeasonID == oldGame.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == oldGame.HostName && ts.SeasonID == oldGame.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();
            var winnerSeason = await (from ts in dbContext.TeamSeasons
                                      where ts.TeamName == oldGame.WinnerName && ts.SeasonID == oldGame.SeasonID
                                      select ts)
                                      .FirstOrDefaultAsync();
            var loserSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == oldGame.LoserName && ts.SeasonID == oldGame.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();

            #region await EditWinLossData(dbContext, game, operation);

            Assert.AreEqual(Difference, guestSeason.Games);
            Assert.AreEqual(Difference, hostSeason.Games);

            Assert.AreEqual(Difference, winnerSeason.Wins);
            Assert.AreEqual(Difference, loserSeason.Losses);

            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            Assert.AreEqual(Difference, guestSeason.PointsFor);
            Assert.AreEqual(Difference, guestSeason.PointsAgainst);
            Assert.AreEqual(Product, guestSeason.PythagoreanWins);
            Assert.AreEqual(Product, guestSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            Assert.AreEqual(Difference, hostSeason.PointsFor);
            Assert.AreEqual(Difference, hostSeason.PointsAgainst);
            Assert.AreEqual(Product, hostSeason.PythagoreanWins);
            Assert.AreEqual(Product, hostSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);

            #endregion await repository.ProcessGame(dbContext, game, operation);
            #endregion await repository.EditTeams(dbContext, oldGame, Direction.Down);
        }

        [TestCase]
        public async Task EditGame()
        {
            // Arrange
            const double Sum = 2;
            const double Difference = 0;
            const double Product = 2;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).Returns(Difference);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);
            dbContext.SetUpFakeGamesAsync();

            var oldGame = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };

            var newGame = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Guest",
                LoserName = "Host"
            };

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditGame(dbContext, oldGame, newGame);

            // Assert
            Assert.AreEqual(EntityState.Modified, dbContext.Entry(newGame).State);

            #region await repository.EditGameInTeams(dbContext, oldGame, newGame);

            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(4, Times.Exactly);

            #endregion await repository.EditGameInTeams(dbContext, oldGame, newGame);

            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task EditGameInTeams()
        {
            // Arrange
            const double Sum = 2;
            const double Difference = 0;
            const double Product = 2;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).Returns(Difference);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var oldGame = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };

            var newGame = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Guest",
                LoserName = "Host"
            };

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditGameInTeams(dbContext, oldGame, newGame);

            // Assert
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(4, Times.Exactly);
        }

        [TestCase]
        public async Task EditScoringDataByTeam_PythPctIsNull_PythWinsAndLossesSetToZero()
        {
            // Arrange
            const double Sum = 2;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Team",
                    SeasonID = 2017,
                    PointsFor = 1,
                    PointsAgainst = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game { SeasonID = 2017 };
            var teamName = "Team";
            var operation = new Operation(calculator.Add);
            var teamScore = 1;
            var opponentScore = 1;

            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(2);

            double? pythPct = null;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditScoringDataByTeam(dbContext, game, teamName, operation, teamScore, opponentScore);

            // Assert
            var teamSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == teamName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();

            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(2, Times.Exactly);
            Assert.AreEqual(Sum, teamSeason.PointsFor);
            Assert.AreEqual(Sum, teamSeason.PointsAgainst);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustNotHaveHappened();
            Assert.AreEqual(0, teamSeason.PythagoreanWins);
            Assert.AreEqual(0, teamSeason.PythagoreanLosses);
        }

        [TestCase]
        public async Task EditScoringDataByTeam_PythPctIsNotNull_PythWinsAndLossesCalculated()
        {
            // Arrange
            const double Sum = 2;
            const double Product = 2;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Team",
                    SeasonID = 2017,
                    PointsFor = 1,
                    PointsAgainst = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game { SeasonID = 2017 };
            var teamName = "Team";
            var operation = new Operation(calculator.Add);
            var teamScore = 1;
            var opponentScore = 1;

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditScoringDataByTeam(dbContext, game, teamName, operation, teamScore, opponentScore);

            // Assert
            var teamSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == teamName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();

            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(2, Times.Exactly);
            Assert.AreEqual(Sum, teamSeason.PointsFor);
            Assert.AreEqual(Sum, teamSeason.PointsAgainst);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(2, Times.Exactly);
            Assert.AreEqual(Product, teamSeason.PythagoreanWins);
            Assert.AreEqual(Product, teamSeason.PythagoreanLosses);
        }

        [TestCase]
        public async Task EditTeams_DirectionUp_CaculatorAdd()
        {
            // Arrange
            const double Sum = 2d;
            const double Product = 2;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };

            var direction = Direction.Up;

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditTeams(dbContext, game, direction);

            // Assert
            #region await repository.ProcessGame(dbContext, game, operation);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.GuestName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == game.HostName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();
            var winnerSeason = await (from ts in dbContext.TeamSeasons
                                      where ts.TeamName == game.WinnerName && ts.SeasonID == game.SeasonID
                                      select ts)
                                      .FirstOrDefaultAsync();
            var loserSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.LoserName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();

            #region await EditWinLossData(dbContext, game, operation);

            Assert.AreEqual(Sum, guestSeason.Games);
            Assert.AreEqual(Sum, hostSeason.Games);

            Assert.AreEqual(Sum, winnerSeason.Wins);
            Assert.AreEqual(Sum, loserSeason.Losses);

            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            Assert.AreEqual(Sum, guestSeason.PointsFor);
            Assert.AreEqual(Sum, guestSeason.PointsAgainst);
            Assert.AreEqual(Product, guestSeason.PythagoreanWins);
            Assert.AreEqual(Product, guestSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            Assert.AreEqual(Sum, hostSeason.PointsFor);
            Assert.AreEqual(Sum, hostSeason.PointsAgainst);
            Assert.AreEqual(Product, hostSeason.PythagoreanWins);
            Assert.AreEqual(Product, hostSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);

            #endregion await repository.ProcessGame(dbContext, game, operation);
        }

        [TestCase]
        public async Task EditTeams_DirectionDown_CalculatorSubtract()
        {
            // Arrange
            const double Difference = 0;
            const double Product = 2;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).Returns(Difference);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };

            var direction = Direction.Down;

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditTeams(dbContext, game, direction);

            // Assert
            #region await repository.ProcessGame(dbContext, game, operation);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.GuestName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == game.HostName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();
            var winnerSeason = await (from ts in dbContext.TeamSeasons
                                      where ts.TeamName == game.WinnerName && ts.SeasonID == game.SeasonID
                                      select ts)
                                      .FirstOrDefaultAsync();
            var loserSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.LoserName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();

            #region await EditWinLossData(dbContext, game, operation);

            Assert.AreEqual(Difference, guestSeason.Games);
            Assert.AreEqual(Difference, hostSeason.Games);

            Assert.AreEqual(Difference, winnerSeason.Wins);
            Assert.AreEqual(Difference, loserSeason.Losses);

            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            Assert.AreEqual(Difference, guestSeason.PointsFor);
            Assert.AreEqual(Difference, guestSeason.PointsAgainst);
            Assert.AreEqual(Product, guestSeason.PythagoreanWins);
            Assert.AreEqual(Product, guestSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            Assert.AreEqual(Difference, hostSeason.PointsFor);
            Assert.AreEqual(Difference, hostSeason.PointsAgainst);
            Assert.AreEqual(Product, hostSeason.PythagoreanWins);
            Assert.AreEqual(Product, hostSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => calculator.Subtract(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);

            #endregion await repository.ProcessGame(dbContext, game, operation);
        }

        //[TestCase]
        public async Task EditTeams_DirectionNotSelected_ExceptionThrown()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();
            var dbContext = A.Fake<ProFootballDbEntities>();
            var game = new Game();
            Direction? direction = null;

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditTeams(dbContext, game, (Direction)direction);

            // Assert
        }

        [TestCase]
        public async Task EditWinLossData_WinnerNameNull_TiesCalculated()
        {
            // Arrange
            const double Sum = 2d;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = null,
                LoserName = "Loser"
            };
            var operation = new Operation(calculator.Add);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditWinLossData(dbContext, game, operation);

            // Assert
            A.CallTo(() => calculator.Add(A<double>.Ignored, 1)).MustHaveHappened(4, Times.Exactly);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.GuestName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == game.HostName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();

            Assert.AreEqual(Sum, guestSeason.Games);
            Assert.AreEqual(Sum, hostSeason.Games);

            Assert.AreEqual(Sum, guestSeason.Ties);
            Assert.AreEqual(Sum, hostSeason.Ties);

            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);
        }

        [TestCase]
        public async Task EditWinLossData_WinnerNameEmpty_TiesCalculated()
        {
            // Arrange
            const double Sum = 2d;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = String.Empty,
                LoserName = "Loser"
            };
            var operation = new Operation(calculator.Add);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditWinLossData(dbContext, game, operation);

            // Assert
            A.CallTo(() => calculator.Add(A<double>.Ignored, 1)).MustHaveHappened(4, Times.Exactly);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.GuestName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == game.HostName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();

            Assert.AreEqual(Sum, guestSeason.Games);
            Assert.AreEqual(Sum, hostSeason.Games);

            Assert.AreEqual(Sum, guestSeason.Ties);
            Assert.AreEqual(Sum, hostSeason.Ties);

            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);
        }

        [TestCase]
        public async Task EditWinLossData_LoserNameNull_TiesCalculated()
        {
            // Arrange
            const double Sum = 2d;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Winner",
                LoserName = null
            };
            var operation = new Operation(calculator.Add);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditWinLossData(dbContext, game, operation);

            // Assert
            A.CallTo(() => calculator.Add(A<double>.Ignored, 1)).MustHaveHappened(4, Times.Exactly);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.GuestName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == game.HostName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();

            Assert.AreEqual(Sum, guestSeason.Games);
            Assert.AreEqual(Sum, hostSeason.Games);

            Assert.AreEqual(Sum, guestSeason.Ties);
            Assert.AreEqual(Sum, hostSeason.Ties);

            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);
        }

        [TestCase]
        public async Task EditWinLossData_LoserNameEmpty_TiesCalculated()
        {
            // Arrange
            const double Sum = 2d;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Winner",
                LoserName = String.Empty
            };
            var operation = new Operation(calculator.Add);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditWinLossData(dbContext, game, operation);

            // Assert
            A.CallTo(() => calculator.Add(A<double>.Ignored, 1)).MustHaveHappened(4, Times.Exactly);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.GuestName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == game.HostName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();

            Assert.AreEqual(Sum, guestSeason.Games);
            Assert.AreEqual(Sum, hostSeason.Games);

            Assert.AreEqual(Sum, guestSeason.Ties);
            Assert.AreEqual(Sum, hostSeason.Ties);

            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);
        }

        [TestCase]
        public async Task EditWinLossData_NeitherWinnerNameNorLoserNameNullOrEmpty_WinsAndLossesCalculated()
        {
            // Arrange
            const double Sum = 2d;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };
            var operation = new Operation(calculator.Add);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.EditWinLossData(dbContext, game, operation);

            // Assert
            A.CallTo(() => calculator.Add(A<double>.Ignored, 1)).MustHaveHappened(4, Times.Exactly);

            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.GuestName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == game.HostName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();

            Assert.AreEqual(Sum, guestSeason.Games);
            Assert.AreEqual(Sum, hostSeason.Games);

            var winnerSeason = await (from ts in dbContext.TeamSeasons
                                      where ts.TeamName == game.WinnerName && ts.SeasonID == game.SeasonID
                                      select ts)
                                      .FirstOrDefaultAsync();
            var loserSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.LoserName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();

            Assert.AreEqual(Sum, winnerSeason.Wins);
            Assert.AreEqual(Sum, loserSeason.Losses);

            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);
        }

        [TestCase]
        public async Task FindGameAsync()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeGamesAsync();

            var id = 1;

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.FindGameAsync(dbContext, id);

            // Assert
            A.CallTo(() => dbContext.Games.FindAsync(id)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf(typeof(Game), result);
        }

        [TestCase]
        public async Task GetAllTeams()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();
            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeTeamsAsync();

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetAllTeams(dbContext);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Team>), result);
        }

        [TestCase]
        public async Task GetGames_SelectedWeekNull_ReturnsList()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            var games = new List<Game>
            {
                new Game{ SeasonID = 2017 },
                new Game{ SeasonID = 2016 },
                new Game{ SeasonID = 2015 }
            };
            dbContext.SetUpFakeGamesAsync(games);

            var selectedSeason = 2017;
            String selectedWeek = null;
            String guestSearchString = null;
            String hostSearchString = null;

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetGames(dbContext, selectedSeason, selectedWeek, guestSearchString, hostSearchString);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Game>), result);
        }

        [TestCase]
        public async Task GetGames_SelectedWeekEmpty_ReturnsList()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            var games = new List<Game>
            {
                new Game{ SeasonID = 2017 },
                new Game{ SeasonID = 2016 },
                new Game{ SeasonID = 2015 }
            };
            dbContext.SetUpFakeGamesAsync(games);

            var selectedSeason = 2017;
            var selectedWeek = String.Empty;
            String guestSearchString = null;
            String hostSearchString = null;

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetGames(dbContext, selectedSeason, selectedWeek, guestSearchString, hostSearchString);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Game>), result);
        }

        [TestCase]
        public async Task GetGames_SelectedWeekNeitherNullNorEmptyAndGuestSearchStringNull_ReturnsList()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            var games = new List<Game>
            {
                new Game{ SeasonID = 2017, Week = 1 },
                new Game{ SeasonID = 2017, Week = 2 },
                new Game{ SeasonID = 2017, Week = 3 },
                new Game{ SeasonID = 2016, Week = 1 },
                new Game{ SeasonID = 2016, Week = 2 },
                new Game{ SeasonID = 2016, Week = 3 },
                new Game{ SeasonID = 2015, Week = 1 },
                new Game{ SeasonID = 2015, Week = 2 },
                new Game{ SeasonID = 2015, Week = 3 }
            };
            dbContext.SetUpFakeGamesAsync(games);

            var selectedSeason = 2017;
            var selectedWeek = "1";
            String guestSearchString = null;
            String hostSearchString = null;

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetGames(dbContext, selectedSeason, selectedWeek, guestSearchString, hostSearchString);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Game>), result);
        }

        [TestCase]
        public async Task GetGames_SelectedWeekNeitherNullNorEmptyAndGuestSearchStringEmpty_ReturnsList()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            var games = new List<Game>
            {
                new Game{ SeasonID = 2017, Week = 1 },
                new Game{ SeasonID = 2017, Week = 2 },
                new Game{ SeasonID = 2017, Week = 3 },
                new Game{ SeasonID = 2016, Week = 1 },
                new Game{ SeasonID = 2016, Week = 2 },
                new Game{ SeasonID = 2016, Week = 3 },
                new Game{ SeasonID = 2015, Week = 1 },
                new Game{ SeasonID = 2015, Week = 2 },
                new Game{ SeasonID = 2015, Week = 3 }
            };
            dbContext.SetUpFakeGamesAsync();

            var selectedSeason = 2017;
            var selectedWeek = "1";
            String guestSearchString = String.Empty;
            String hostSearchString = null;

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetGames(dbContext, selectedSeason, selectedWeek, guestSearchString, hostSearchString);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Game>), result);
        }

        [TestCase]
        public async Task GetGames_SelectedWeekNeitherNullNorEmptyAndGuestSearchStringNeitherNullNorEmptyAndHostSearchStringNull_ReturnsList()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            var games = new List<Game>
            {
                new Game{ SeasonID = 2017, Week = 1, GuestName = "Guest" },
                new Game{ SeasonID = 2017, Week = 1, GuestName = "Team" },
                new Game{ SeasonID = 2017, Week = 2, GuestName = "Guest" },
                new Game{ SeasonID = 2017, Week = 2, GuestName = "Team" },
                new Game{ SeasonID = 2017, Week = 3, GuestName = "Guest" },
                new Game{ SeasonID = 2017, Week = 3, GuestName = "Team" },
                new Game{ SeasonID = 2016, Week = 1, GuestName = "Guest" },
                new Game{ SeasonID = 2016, Week = 1, GuestName = "Team" },
                new Game{ SeasonID = 2016, Week = 2, GuestName = "Guest" },
                new Game{ SeasonID = 2016, Week = 2, GuestName = "Team" },
                new Game{ SeasonID = 2016, Week = 3, GuestName = "Guest" },
                new Game{ SeasonID = 2016, Week = 3, GuestName = "Team" },
                new Game{ SeasonID = 2015, Week = 1, GuestName = "Guest" },
                new Game{ SeasonID = 2015, Week = 1, GuestName = "Team" },
                new Game{ SeasonID = 2015, Week = 2, GuestName = "Guest" },
                new Game{ SeasonID = 2015, Week = 2, GuestName = "Team" },
                new Game{ SeasonID = 2015, Week = 3, GuestName = "Guest" },
                new Game{ SeasonID = 2015, Week = 3, GuestName = "Team" }
            };
            dbContext.SetUpFakeGamesAsync();

            var selectedSeason = 2017;
            var selectedWeek = "1";
            var guestSearchString = "guest";
            String hostSearchString = null;

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetGames(dbContext, selectedSeason, selectedWeek, guestSearchString, hostSearchString);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Game>), result);
        }

        [TestCase]
        public async Task GetGames_SelectedWeekNeitherNullNorEmptyAndGuestSearchStringNeitherNullNorEmptyAndHostSearchStringEmpty_ReturnsList()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            var games = new List<Game>
            {
                new Game{ SeasonID = 2017, Week = 1, GuestName = "Guest" },
                new Game{ SeasonID = 2017, Week = 1, GuestName = "Team" },
                new Game{ SeasonID = 2017, Week = 2, GuestName = "Guest" },
                new Game{ SeasonID = 2017, Week = 2, GuestName = "Team" },
                new Game{ SeasonID = 2017, Week = 3, GuestName = "Guest" },
                new Game{ SeasonID = 2017, Week = 3, GuestName = "Team" },
                new Game{ SeasonID = 2016, Week = 1, GuestName = "Guest" },
                new Game{ SeasonID = 2016, Week = 1, GuestName = "Team" },
                new Game{ SeasonID = 2016, Week = 2, GuestName = "Guest" },
                new Game{ SeasonID = 2016, Week = 2, GuestName = "Team" },
                new Game{ SeasonID = 2016, Week = 3, GuestName = "Guest" },
                new Game{ SeasonID = 2016, Week = 3, GuestName = "Team" },
                new Game{ SeasonID = 2015, Week = 1, GuestName = "Guest" },
                new Game{ SeasonID = 2015, Week = 1, GuestName = "Team" },
                new Game{ SeasonID = 2015, Week = 2, GuestName = "Guest" },
                new Game{ SeasonID = 2015, Week = 2, GuestName = "Team" },
                new Game{ SeasonID = 2015, Week = 3, GuestName = "Guest" },
                new Game{ SeasonID = 2015, Week = 3, GuestName = "Team" }
            };
            dbContext.SetUpFakeGamesAsync();

            var selectedSeason = 2017;
            var selectedWeek = "1";
            var guestSearchString = "guest";
            String hostSearchString = String.Empty;

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetGames(dbContext, selectedSeason, selectedWeek, guestSearchString, hostSearchString);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Game>), result);
        }

        [TestCase]
        public async Task GetGames_SelectedWeekNeitherNullNorEmptyAndGuestSearchStringNeitherNullNorEmptyAndHostSearchStringNeitherNullNorEmpty_ReturnsList()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            var games = new List<Game>
            {
                new Game{ SeasonID = 2017, Week = 1, GuestName = "Guest", HostName = "Team" },
                new Game{ SeasonID = 2017, Week = 1, GuestName = "Guest", HostName = "Host" },
                new Game{ SeasonID = 2017, Week = 1, GuestName = "Team", HostName = "Team" },
                new Game{ SeasonID = 2017, Week = 1, GuestName = "Team", HostName = "Host" },
                new Game{ SeasonID = 2017, Week = 2, GuestName = "Guest", HostName = "Team" },
                new Game{ SeasonID = 2017, Week = 2, GuestName = "Guest", HostName = "Host" },
                new Game{ SeasonID = 2017, Week = 2, GuestName = "Team", HostName = "Team" },
                new Game{ SeasonID = 2017, Week = 2, GuestName = "Team", HostName = "Host" },
                new Game{ SeasonID = 2017, Week = 3, GuestName = "Guest", HostName = "Team" },
                new Game{ SeasonID = 2017, Week = 3, GuestName = "Guest", HostName = "Host" },
                new Game{ SeasonID = 2017, Week = 3, GuestName = "Team", HostName = "Team" },
                new Game{ SeasonID = 2017, Week = 3, GuestName = "Team", HostName = "Host" },
                new Game{ SeasonID = 2016, Week = 1, GuestName = "Guest", HostName = "Team" },
                new Game{ SeasonID = 2016, Week = 1, GuestName = "Guest", HostName = "Host" },
                new Game{ SeasonID = 2016, Week = 1, GuestName = "Team", HostName = "Team" },
                new Game{ SeasonID = 2016, Week = 1, GuestName = "Team", HostName = "Host" },
                new Game{ SeasonID = 2016, Week = 2, GuestName = "Guest", HostName = "Team" },
                new Game{ SeasonID = 2016, Week = 2, GuestName = "Guest", HostName = "Host" },
                new Game{ SeasonID = 2016, Week = 2, GuestName = "Team", HostName = "Team" },
                new Game{ SeasonID = 2016, Week = 2, GuestName = "Team", HostName = "Host" },
                new Game{ SeasonID = 2016, Week = 3, GuestName = "Guest", HostName = "Team" },
                new Game{ SeasonID = 2016, Week = 3, GuestName = "Guest", HostName = "Host" },
                new Game{ SeasonID = 2016, Week = 3, GuestName = "Team", HostName = "Team" },
                new Game{ SeasonID = 2016, Week = 3, GuestName = "Team", HostName = "Host" },
                new Game{ SeasonID = 2015, Week = 1, GuestName = "Guest", HostName = "Team" },
                new Game{ SeasonID = 2015, Week = 1, GuestName = "Guest", HostName = "Host" },
                new Game{ SeasonID = 2015, Week = 1, GuestName = "Team", HostName = "Team" },
                new Game{ SeasonID = 2015, Week = 1, GuestName = "Team", HostName = "Host" },
                new Game{ SeasonID = 2015, Week = 2, GuestName = "Guest", HostName = "Team" },
                new Game{ SeasonID = 2015, Week = 2, GuestName = "Guest", HostName = "Host" },
                new Game{ SeasonID = 2015, Week = 2, GuestName = "Team", HostName = "Team" },
                new Game{ SeasonID = 2015, Week = 2, GuestName = "Team", HostName = "Host" },
                new Game{ SeasonID = 2015, Week = 3, GuestName = "Guest", HostName = "Team" },
                new Game{ SeasonID = 2015, Week = 3, GuestName = "Guest", HostName = "Host" },
                new Game{ SeasonID = 2015, Week = 3, GuestName = "Team", HostName = "Team" },
                new Game{ SeasonID = 2015, Week = 3, GuestName = "Team", HostName = "Host" },
            };
            dbContext.SetUpFakeGamesAsync();

            var selectedSeason = 2017;
            var selectedWeek = "1";
            var guestSearchString = "guest";
            String hostSearchString = "host";

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetGames(dbContext, selectedSeason, selectedWeek, guestSearchString, hostSearchString);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Game>), result);
        }

        [TestCase]
        public async Task GetGameTeamSeason()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason { TeamName = "Team", SeasonID = 2017 },
                new TeamSeason { TeamName = "Team", SeasonID = 2016 },
                new TeamSeason { TeamName = "Team", SeasonID = 2015 }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game { SeasonID = 2017 };
            var teamName = "Team";

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetGameTeamSeason(dbContext, game, teamName);

            // Assert
            Assert.IsInstanceOf(typeof(TeamSeason), result);
        }

        [TestCase]
        public async Task GetWeeks()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();

            var dbContext = A.Fake<ProFootballDbEntities>();
            dbContext.SetUpFakeGamesAsync();

            var selectedSeason = 2017;

            // Act
            var repository = new GamesRepository(calculator);
            var result = await repository.GetWeeks(dbContext, selectedSeason);

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<int>), result);
        }

        [TestCase]
        public async Task ProcessGame()
        {
            // Arrange
            const double Sum = 2d;
            const double Product = 2;
            const double wPct = 0.5;

            var calculator = A.Fake<ICalculator>();
            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).Returns(Sum);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).Returns(Product);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).Returns(wPct);

            var dbContext = A.Fake<ProFootballDbEntities>();
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = "Guest",
                    SeasonID = 2017,
                    Games = 1
                },
                new TeamSeason
                {
                    TeamName = "Host",
                    SeasonID = 2017,
                    Games = 1
                }
            };
            dbContext.SetUpFakeTeamSeasonsAsync(teamSeasons);

            var game = new Game
            {
                SeasonID = 2017,
                GuestName = "Guest",
                HostName = "Host",
                WinnerName = "Host",
                LoserName = "Guest"
            };

            var operation = new Operation(calculator.Add);

            double? pythPct = 0.5;
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            var repository = new GamesRepository(calculator);
            await repository.ProcessGame(dbContext, game, operation);

            // Assert
            var guestSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.GuestName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();
            var hostSeason = await (from ts in dbContext.TeamSeasons
                                    where ts.TeamName == game.HostName && ts.SeasonID == game.SeasonID
                                    select ts)
                                    .FirstOrDefaultAsync();
            var winnerSeason = await (from ts in dbContext.TeamSeasons
                                      where ts.TeamName == game.WinnerName && ts.SeasonID == game.SeasonID
                                      select ts)
                                      .FirstOrDefaultAsync();
            var loserSeason = await (from ts in dbContext.TeamSeasons
                                     where ts.TeamName == game.LoserName && ts.SeasonID == game.SeasonID
                                     select ts)
                                     .FirstOrDefaultAsync();

            #region await EditWinLossData(dbContext, game, operation);

            Assert.AreEqual(Sum, guestSeason.Games);
            Assert.AreEqual(Sum, hostSeason.Games);

            Assert.AreEqual(Sum, winnerSeason.Wins);
            Assert.AreEqual(Sum, loserSeason.Losses);

            Assert.AreEqual(wPct, guestSeason.WinningPercentage);
            Assert.AreEqual(wPct, hostSeason.WinningPercentage);

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            Assert.AreEqual(Sum, guestSeason.PointsFor);
            Assert.AreEqual(Sum, guestSeason.PointsAgainst);
            Assert.AreEqual(Product, guestSeason.PythagoreanWins);
            Assert.AreEqual(Product, guestSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.GuestName, operation, game.GuestScore, game.HostScore);

            #region await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            Assert.AreEqual(Sum, hostSeason.PointsFor);
            Assert.AreEqual(Sum, hostSeason.PointsAgainst);
            Assert.AreEqual(Product, hostSeason.PythagoreanWins);
            Assert.AreEqual(Product, hostSeason.PythagoreanLosses);

            #endregion await repository.EditScoringDataByTeam(dbContext, game, game.HostName, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => calculator.Add(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(8, Times.Exactly);
            A.CallTo(() => calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => calculator.CalculateWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
            A.CallTo(() => calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).MustHaveHappened(2, Times.Exactly);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var calculator = A.Fake<ICalculator>();
            var dbContext = A.Fake<ProFootballDbEntities>();

            // Act
            var repository = new GamesRepository(calculator);

            // Assert
        }
    }
}
