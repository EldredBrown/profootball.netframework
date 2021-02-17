using System;
using System.Collections.Generic;
using System.Linq;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.WpfApp.Tests.Services
{
    [TestFixture]
    public class GamesWindowServiceTest
    {
        private ISharedService _sharedService;
        private IRepository<Game> _gameRepository;
        private IRepository<WeekCount> _weekCountRepository;
        private IRepository<TeamSeason> _teamSeasonRepository;
        private ProFootballEntities _dbContext;
        private ICalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _gameRepository = A.Fake<IRepository<Game>>();
            _weekCountRepository = A.Fake<IRepository<WeekCount>>();
            _teamSeasonRepository = A.Fake<IRepository<TeamSeason>>();
            _dbContext = A.Fake<ProFootballEntities>();
            _calculator = A.Fake<ICalculator>();
        }

        // TODO - 2020-09-25: Explore how I can have all test cases use the same instance of GamesWindowService to eliminate duplication.

        [TestCase]
        public void AddGame_GuestWinsAndPythPctsAreNotNull_UpdatesWinsAndLossesAndScoringDataAndPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 28;
            var hostName = "Host";
            var hostScore = 14;
            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 14;
            var guestPointsAgainst = 28;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, A<int>.Ignored)).Returns(guestSeason);

            double guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 14;
            var hostPointsAgainst = 28;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            double hostPythPct = 0.55;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.AddGame(newGame);

            // Assert
            #region DecideWinnerAndLoser(newGame);

            Assert.AreEqual(newGame.GuestName, newGame.WinnerName);
            Assert.AreEqual(newGame.GuestScore, newGame.WinnerScore);
            Assert.AreEqual(newGame.HostName, newGame.LoserName);
            Assert.AreEqual(newGame.HostScore, newGame.LoserScore);

            #endregion DecideWinnerAndLoser(newGame);

            A.CallTo(() => _gameRepository.AddEntity(newGame)).MustHaveHappenedOnceExactly();

            #region AddGameToTeams(newGame);

            #region EditTeams(newGame, Direction.Up);

            #region ProcessGame(newGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.HostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #region EditWinLossData(newGame, operation);

            A.CallTo(() => _calculator.Add(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(guestSeasonWins, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostSeasonLosses, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(guestSeasonTies, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(hostSeasonTies, 1)).MustNotHaveHappened();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(newGame, operation);

            #region EditScoringData(newGame, operation);

            #region EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            A.CallTo(() => _calculator.Add(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            #region EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            A.CallTo(() => _calculator.Add(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            #endregion EditScoringData(newGame, operation);

            #endregion ProcessGame(newGame, operation);

            #endregion EditTeams(newGame, Direction.Up);

            #endregion AddGameToTeams(newGame);

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void AddGame_HostWinsAndPythPctsAreNotNull_UpdatesWinsAndLossesAndScoringDataAndPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 14;
            var hostName = "Host";
            var hostScore = 28;
            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 28;
            var guestPointsAgainst = 14;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, A<int>.Ignored)).Returns(guestSeason);

            double guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 28;
            var hostPointsAgainst = 14;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            double hostPythPct = 0.55;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.AddGame(newGame);

            // Assert
            #region DecideWinnerAndLoser(newGame);

            Assert.AreEqual(newGame.HostName, newGame.WinnerName);
            Assert.AreEqual(newGame.HostScore, newGame.WinnerScore);
            Assert.AreEqual(newGame.GuestName, newGame.LoserName);
            Assert.AreEqual(newGame.GuestScore, newGame.LoserScore);

            #endregion DecideWinnerAndLoser(newGame);

            A.CallTo(() => _gameRepository.AddEntity(newGame)).MustHaveHappenedOnceExactly();

            #region AddGameToTeams(newGame);

            #region EditTeams(newGame, Direction.Up);

            #region ProcessGame(newGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.HostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #region EditWinLossData(newGame, operation);

            A.CallTo(() => _calculator.Add(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(hostSeasonWins, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(guestSeasonLosses, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(guestSeasonTies, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(hostSeasonTies, 1)).MustNotHaveHappened();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(newGame, operation);

            #region EditScoringData(newGame, operation);

            #region EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            A.CallTo(() => _calculator.Add(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            #region EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            A.CallTo(() => _calculator.Add(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            #endregion EditScoringData(newGame, operation);

            #endregion ProcessGame(newGame, operation);

            #endregion EditTeams(newGame, Direction.Up);

            #endregion AddGameToTeams(newGame);

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void AddGame_TieAndPythPctsAreNotNull_UpdatesTiesAndScoringDataAndPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 21;
            var hostName = "Host";
            var hostScore = 21;
            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 23;
            var guestPointsAgainst = 22;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, A<int>.Ignored)).Returns(guestSeason);

            double guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 21;
            var hostPointsAgainst = 20;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            double hostPythPct = 0.55;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.AddGame(newGame);

            // Assert
            #region DecideWinnerAndLoser(newGame);

            Assert.IsNull(newGame.WinnerName);
            Assert.IsNull(newGame.LoserName);

            #endregion DecideWinnerAndLoser(newGame);

            A.CallTo(() => _gameRepository.AddEntity(newGame)).MustHaveHappenedOnceExactly();

            #region AddGameToTeams(newGame);

            #region EditTeams(newGame, Direction.Up);

            #region ProcessGame(newGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, seasonID))
                .MustHaveHappenedTwiceExactly();

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.HostName, seasonID))
                .MustHaveHappenedTwiceExactly();

            #region EditWinLossData(newGame, operation);

            A.CallTo(() => _calculator.Add(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(guestSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(guestSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(guestSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(hostSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(hostSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(hostSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(newGame, operation);

            #region EditScoringData(newGame, operation);

            #region EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            A.CallTo(() => _calculator.Add(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            #region EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            A.CallTo(() => _calculator.Add(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            #endregion EditScoringData(newGame, operation);

            #endregion ProcessGame(newGame, operation);

            #endregion EditTeams(newGame, Direction.Up);

            #endregion AddGameToTeams(newGame);

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void AddGame_TieAndGuestPythPctIsNullAndHostPythPctIsNotNull_UpdatesTiesAndScoringDataAndSetsGuestPythWinsAndLossesToZeroAndCalculatesHostPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 21;
            var hostName = "Host";
            var hostScore = 21;
            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 23;
            var guestPointsAgainst = 22;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, A<int>.Ignored)).Returns(guestSeason);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(null);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 21;
            var hostPointsAgainst = 20;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            var hostPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.AddGame(newGame);

            // Assert
            #region DecideWinnerAndLoser(newGame);

            Assert.IsNull(newGame.WinnerName);
            Assert.IsNull(newGame.LoserName);

            #endregion DecideWinnerAndLoser(newGame);

            A.CallTo(() => _gameRepository.AddEntity(newGame)).MustHaveHappenedOnceExactly();

            #region AddGameToTeams(newGame);

            #region EditTeams(newGame, Direction.Up);

            #region ProcessGame(newGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, seasonID))
                .MustHaveHappenedTwiceExactly();

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.HostName, seasonID))
                .MustHaveHappenedTwiceExactly();

            #region EditWinLossData(newGame, operation);

            A.CallTo(() => _calculator.Add(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(guestSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(guestSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(guestSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(hostSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(hostSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(hostSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(newGame, operation);

            #region EditScoringData(newGame, operation);

            #region EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            A.CallTo(() => _calculator.Add(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(0, guestSeason.PythagoreanWins);
            Assert.AreEqual(0, guestSeason.PythagoreanLosses);

            #endregion EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            #region EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            A.CallTo(() => _calculator.Add(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            #endregion EditScoringData(newGame, operation);

            #endregion ProcessGame(newGame, operation);

            #endregion EditTeams(newGame, Direction.Up);

            #endregion AddGameToTeams(newGame);

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void AddGame_TieAndPythPctsAreNull_UpdatesTiesAndScoringDataAndSetsPythWinsAndLossesToZero()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 21;
            var hostName = "Host";
            var hostScore = 21;
            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 23;
            var guestPointsAgainst = 22;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, A<int>.Ignored)).Returns(guestSeason);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(null);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 21;
            var hostPointsAgainst = 20;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            var hostPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.AddGame(newGame);

            // Assert
            #region DecideWinnerAndLoser(newGame);

            Assert.IsNull(newGame.WinnerName);
            Assert.IsNull(newGame.LoserName);

            #endregion DecideWinnerAndLoser(newGame);

            A.CallTo(() => _gameRepository.AddEntity(newGame)).MustHaveHappenedOnceExactly();

            #region AddGameToTeams(newGame);

            #region EditTeams(newGame, Direction.Up);

            #region ProcessGame(newGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, seasonID))
                .MustHaveHappenedTwiceExactly();

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.HostName, seasonID))
                .MustHaveHappenedTwiceExactly();

            #region EditWinLossData(newGame, operation);

            A.CallTo(() => _calculator.Add(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(guestSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(guestSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(guestSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(hostSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(hostSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(hostSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(newGame, operation);

            #region EditScoringData(newGame, operation);

            #region EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            A.CallTo(() => _calculator.Add(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(0, guestSeason.PythagoreanWins);
            Assert.AreEqual(0, guestSeason.PythagoreanLosses);

            #endregion EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            #region EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            A.CallTo(() => _calculator.Add(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(0, hostSeason.PythagoreanWins);
            Assert.AreEqual(0, hostSeason.PythagoreanLosses);

            #endregion EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            #endregion EditScoringData(newGame, operation);

            #endregion ProcessGame(newGame, operation);

            #endregion EditTeams(newGame, Direction.Up);

            #endregion AddGameToTeams(newGame);

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void AddGame_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            A.CallTo(() => _gameRepository.AddEntity(A<Game>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => service.AddGame(new Game()));
        }

        [TestCase]
        public void EditGame_HappyPath()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;

            var oldGuestName = "Old Guest";
            var oldGuestScore = 14;
            var oldHostName = "Old Host";
            var oldHostScore = 28;
            var oldGame = new Game
            {
                SeasonID = seasonID,
                GuestName = oldGuestName,
                GuestScore = oldGuestScore,
                HostName = oldHostName,
                HostScore = oldHostScore,
                WinnerName = oldHostName,
                LoserName = oldGuestName
            };

            var newGuestName = "New Guest";
            var newGuestScore = 28;
            var newHostName = "New Host";
            var newHostScore = 14;
            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = newGuestName,
                GuestScore = newGuestScore,
                HostName = newHostName,
                HostScore = newHostScore,
                WinnerName = newGuestName,
                LoserName = newHostName
            };

            // Set up needed infrastructure of class under test.
            var selectedGame = new Game();
            A.CallTo(() => _gameRepository.FindEntity(A<int>.Ignored)).Returns(selectedGame);

            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 28;
            var guestPointsAgainst = 14;

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 28;
            var hostPointsAgainst = 14;

            var oldGuestSeason = new TeamSeason
            {
                TeamName = oldGuestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, A<int>.Ignored)).Returns(oldGuestSeason);

            double oldGuestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(oldGuestSeason)).Returns(oldGuestPythPct);

            var oldHostSeason = new TeamSeason
            {
                TeamName = oldHostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.HostName, A<int>.Ignored)).Returns(oldHostSeason);

            double oldHostPythPct = 0.55;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(oldHostSeason)).Returns(oldHostPythPct);

            var newGuestSeason = new TeamSeason
            {
                TeamName = newGuestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, A<int>.Ignored)).Returns(newGuestSeason);

            double newGuestPythPct = 0.59;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(newGuestSeason)).Returns(newGuestPythPct);

            var newHostSeason = new TeamSeason
            {
                TeamName = newHostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.HostName, A<int>.Ignored)).Returns(newHostSeason);

            double newHostPythPct = 0.54;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(newHostSeason)).Returns(newHostPythPct);

            // Act
            service.EditGame(oldGame, newGame);

            // Assert
            A.CallTo(() => _gameRepository.FindEntity(oldGame.ID)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(selectedGame);

            Assert.AreEqual(selectedGame.GuestName, selectedGame.WinnerName);
            Assert.AreEqual(selectedGame.GuestScore, selectedGame.WinnerScore);
            Assert.AreEqual(selectedGame.HostName, selectedGame.LoserName);
            Assert.AreEqual(selectedGame.HostScore, selectedGame.LoserScore);

            #endregion DecideWinnerAndLoser(selectedGame);

            A.CallTo(() => _gameRepository.EditEntity(selectedGame)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.AreEqual(oldGame.HostName, oldGame.WinnerName);
            Assert.AreEqual(oldGame.HostScore, oldGame.WinnerScore);
            Assert.AreEqual(oldGame.GuestName, oldGame.LoserName);
            Assert.AreEqual(oldGame.GuestScore, oldGame.LoserScore);

            #endregion DecideWinnerAndLoser(oldGame);

            #region DecideWinnerAndLoser(newGame);

            Assert.AreEqual(newGame.GuestName, newGame.WinnerName);
            Assert.AreEqual(newGame.GuestScore, newGame.WinnerScore);
            Assert.AreEqual(newGame.HostName, newGame.LoserName);
            Assert.AreEqual(newGame.HostScore, newGame.LoserScore);

            #endregion DecideWinnerAndLoser(newGame);

            #region EditGameInTeams(oldGame, newGame);

            #region EditTeams(oldGame, Direction.Down);

            #region ProcessGame(oldGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.HostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #region EditWinLossData(oldGame, operation);

            A.CallTo(() => _calculator.Subtract(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(hostSeasonWins, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(guestSeasonLosses, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(guestSeasonTies, A<double>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(hostSeasonTies, A<double>.Ignored)).MustNotHaveHappened();

            A.CallTo(() => _calculator.CalculateWinningPercentage(oldGuestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(oldHostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(oldGame, operation);

            #region EditScoringData(oldGame, operation);

            #region EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            A.CallTo(() => _calculator.Subtract(guestPointsFor, oldGuestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(guestPointsAgainst, oldHostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(oldGuestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(oldGuestPythPct, oldGuestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - oldGuestPythPct, oldGuestSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            #region EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            A.CallTo(() => _calculator.Subtract(hostPointsFor, oldHostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostPointsAgainst, oldGuestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(oldHostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(oldHostPythPct, oldHostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - oldHostPythPct, oldHostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            #endregion EditScoringData(oldGame, operation);

            #endregion ProcessGame(oldGame, operation);

            #endregion EditTeams(oldGame, Direction.Down);

            #region EditTeams(newGame, Direction.Up);

            #region ProcessGame(newGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.GuestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            A.CallTo(() => _teamSeasonRepository.FindEntity(newGame.HostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #region EditWinLossData(newGame, operation);

            A.CallTo(() => _calculator.Add(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(guestSeasonWins, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostSeasonLosses, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Add(guestSeasonTies, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(hostSeasonTies, 1)).MustNotHaveHappened();

            A.CallTo(() => _calculator.CalculateWinningPercentage(newGuestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(newGuestSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(newGame, operation);

            #region EditScoringData(newGame, operation);

            #region EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            A.CallTo(() => _calculator.Add(guestPointsFor, oldGuestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(guestPointsAgainst, oldHostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(newGuestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(newGuestPythPct, newGuestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - newGuestPythPct, newGuestSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(newGame.GuestName, newGame.SeasonID, operation, newGame.GuestScore, newGame.HostScore);

            #region EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            A.CallTo(() => _calculator.Add(hostPointsFor, oldHostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(hostPointsAgainst, oldGuestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(newHostSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(newHostPythPct, newHostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - newHostPythPct, newHostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(newGame.HostName, newGame.SeasonID, operation, newGame.HostScore, newGame.GuestScore);

            #endregion EditScoringData(newGame, operation);

            #endregion ProcessGame(newGame, operation);

            #endregion EditTeams(newGame, Direction.Up);

            #endregion EditGameInTeams(oldGame, newGame);

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditGame_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            A.CallTo(() => _gameRepository.FindEntity(A<int>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => service.EditGame(new Game(), new Game()));
        }

        [TestCase]
        public void DeleteGame_GuestWinsAndPythPctsAreNotNull_UpdatesWinsAndLossesAndScoringDataAndPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 28;
            var hostName = "Host";
            var hostScore = 14;
            var oldGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _gameRepository.FindEntity(A<int>.Ignored)).Returns(oldGame);

            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 14;
            var guestPointsAgainst = 28;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, A<int>.Ignored)).Returns(guestSeason);

            double guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 14;
            var hostPointsAgainst = 28;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            double hostPythPct = 0.55;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.DeleteGame(oldGame);

            // Assert
            A.CallTo(() => _gameRepository.FindEntity(oldGame.ID)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.AreEqual(oldGame.GuestName, oldGame.WinnerName);
            Assert.AreEqual(oldGame.GuestScore, oldGame.WinnerScore);
            Assert.AreEqual(oldGame.HostName, oldGame.LoserName);
            Assert.AreEqual(oldGame.HostScore, oldGame.LoserScore);

            #endregion DecideWinnerAndLoser(oldGame);

            #region DeleteGameFromTeams(oldGame);

            #region EditTeams(oldGame, Direction.Up);

            #region ProcessGame(oldGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.HostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #region EditWinLossData(oldGame, operation);

            A.CallTo(() => _calculator.Subtract(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(guestSeasonWins, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostSeasonLosses, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(guestSeasonTies, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(hostSeasonTies, 1)).MustNotHaveHappened();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(oldGame, operation);

            #region EditScoringData(oldGame, operation);

            #region EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            A.CallTo(() => _calculator.Subtract(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            #region EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            A.CallTo(() => _calculator.Subtract(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            #endregion EditScoringData(oldGame, operation);

            #endregion ProcessGame(oldGame, operation);

            #endregion EditTeams(oldGame, Direction.Up);

            #endregion DeleteGameFromTeams(oldGame);

            A.CallTo(() => _gameRepository.RemoveEntity(oldGame)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void DeleteGame_HostWinsAndPythPctsAreNotNull_UpdatesWinsAndLossesAndScoringDataAndPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 14;
            var hostName = "Host";
            var hostScore = 28;
            var oldGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _gameRepository.FindEntity(A<int>.Ignored)).Returns(oldGame);

            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 28;
            var guestPointsAgainst = 14;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, A<int>.Ignored)).Returns(guestSeason);

            double guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 28;
            var hostPointsAgainst = 14;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            double hostPythPct = 0.55;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.DeleteGame(oldGame);

            // Assert
            A.CallTo(() => _gameRepository.FindEntity(oldGame.ID)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.AreEqual(oldGame.HostName, oldGame.WinnerName);
            Assert.AreEqual(oldGame.HostScore, oldGame.WinnerScore);
            Assert.AreEqual(oldGame.GuestName, oldGame.LoserName);
            Assert.AreEqual(oldGame.GuestScore, oldGame.LoserScore);

            #endregion DecideWinnerAndLoser(oldGame);

            #region DeleteGameFromTeams(oldGame);

            #region EditTeams(oldGame, Direction.Up);

            #region ProcessGame(oldGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.HostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #region EditWinLossData(oldGame, operation);

            A.CallTo(() => _calculator.Subtract(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(hostSeasonWins, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(guestSeasonLosses, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(guestSeasonTies, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(hostSeasonTies, 1)).MustNotHaveHappened();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(oldGame, operation);

            #region EditScoringData(oldGame, operation);

            #region EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            A.CallTo(() => _calculator.Subtract(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            #region EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            A.CallTo(() => _calculator.Subtract(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            #endregion EditScoringData(oldGame, operation);

            #endregion ProcessGame(oldGame, operation);

            #endregion EditTeams(oldGame, Direction.Up);

            #endregion DeleteGameFromTeams(oldGame);

            A.CallTo(() => _gameRepository.RemoveEntity(oldGame)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void DeleteGame_TieAndPythPctsAreNotNull_UpdatesTiesAndScoringDataAndPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 21;
            var hostName = "Host";
            var hostScore = 21;
            var oldGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _gameRepository.FindEntity(A<int>.Ignored)).Returns(oldGame);

            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 23;
            var guestPointsAgainst = 22;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, A<int>.Ignored)).Returns(guestSeason);

            double guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 21;
            var hostPointsAgainst = 20;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            double hostPythPct = 0.55;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.DeleteGame(oldGame);

            // Assert
            A.CallTo(() => _gameRepository.FindEntity(oldGame.ID)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.IsNull(oldGame.WinnerName);
            Assert.IsNull(oldGame.LoserName);

            #endregion DecideWinnerAndLoser(oldGame);

            #region DeleteGameFromTeams(oldGame);

            #region EditTeams(oldGame, Direction.Up);

            #region ProcessGame(oldGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, seasonID))
                .MustHaveHappenedTwiceExactly();

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.HostName, seasonID))
                .MustHaveHappenedTwiceExactly();

            #region EditWinLossData(oldGame, operation);

            A.CallTo(() => _calculator.Subtract(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(guestSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(guestSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(guestSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(hostSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(hostSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(hostSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(oldGame, operation);

            #region EditScoringData(oldGame, operation);

            #region EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            A.CallTo(() => _calculator.Subtract(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            #region EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            A.CallTo(() => _calculator.Subtract(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            #endregion EditScoringData(oldGame, operation);

            #endregion ProcessGame(oldGame, operation);

            #endregion EditTeams(oldGame, Direction.Up);

            #endregion DeleteGameFromTeams(oldGame);

            A.CallTo(() => _gameRepository.RemoveEntity(oldGame)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void DeleteGame_TieAndGuestPythPctIsNullAndHostPythPctIsNotNull_UpdatesTiesAndScoringDataAndSetsGuestPythWinsAndLossesToZeroAndCalculatesHostPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 21;
            var hostName = "Host";
            var hostScore = 21;
            var oldGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _gameRepository.FindEntity(A<int>.Ignored)).Returns(oldGame);

            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 23;
            var guestPointsAgainst = 22;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName,
                    A<int>.Ignored))
                .Returns(guestSeason);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(null);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 21;
            var hostPointsAgainst = 20;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            var hostPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.DeleteGame(oldGame);

            // Assert
            A.CallTo(() => _gameRepository.FindEntity(oldGame.ID)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.IsNull(oldGame.WinnerName);
            Assert.IsNull(oldGame.LoserName);

            #endregion DecideWinnerAndLoser(oldGame);

            #region DeleteGameFromTeams(oldGame);

            #region EditTeams(oldGame, Direction.Up);

            #region ProcessGame(oldGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, seasonID))
                .MustHaveHappenedTwiceExactly();

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.HostName, seasonID))
                .MustHaveHappenedTwiceExactly();

            #region EditWinLossData(oldGame, operation);

            A.CallTo(() => _calculator.Subtract(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(guestSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(guestSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(guestSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(hostSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(hostSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(hostSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(oldGame, operation);

            #region EditScoringData(oldGame, operation);

            #region EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            A.CallTo(() => _calculator.Subtract(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(0, guestSeason.PythagoreanWins);
            Assert.AreEqual(0, guestSeason.PythagoreanLosses);

            #endregion EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            #region EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            A.CallTo(() => _calculator.Subtract(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply(1 - hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();

            #endregion EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            #endregion EditScoringData(oldGame, operation);

            #endregion ProcessGame(oldGame, operation);

            #endregion EditTeams(oldGame, Direction.Up);

            #endregion DeleteGameFromTeams(oldGame);

            A.CallTo(() => _gameRepository.RemoveEntity(oldGame)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void DeleteGame_TieAndPythPctsAreNull_UpdatesTiesAndScoringDataAndSetsPythWinsAndLossesToZero()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var guestName = "Guest";
            var guestScore = 21;
            var hostName = "Host";
            var hostScore = 21;
            var oldGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _gameRepository.FindEntity(A<int>.Ignored)).Returns(oldGame);

            var guestSeasonGames = 13;
            var guestSeasonWins = 6;
            var guestSeasonLosses = 5;
            var guestSeasonTies = 2;
            var guestPointsFor = 23;
            var guestPointsAgainst = 22;
            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = guestSeasonGames,
                Wins = guestSeasonWins,
                Losses = guestSeasonLosses,
                Ties = guestSeasonTies,
                PointsFor = guestPointsFor,
                PointsAgainst = guestPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, A<int>.Ignored)).Returns(guestSeason);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(null);

            var hostSeasonGames = 8;
            var hostSeasonWins = 4;
            var hostSeasonLosses = 3;
            var hostSeasonTies = 1;
            var hostPointsFor = 21;
            var hostPointsAgainst = 20;
            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = hostSeasonGames,
                Wins = hostSeasonWins,
                Losses = hostSeasonLosses,
                Ties = hostSeasonTies,
                PointsFor = hostPointsFor,
                PointsAgainst = hostPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

            var hostPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            service.DeleteGame(oldGame);

            // Assert
            A.CallTo(() => _gameRepository.FindEntity(oldGame.ID)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.IsNull(oldGame.WinnerName);
            Assert.IsNull(oldGame.LoserName);

            #endregion DecideWinnerAndLoser(oldGame);

            #region DeleteGameFromTeams(oldGame);

            #region EditTeams(oldGame, Direction.Up);

            #region ProcessGame(oldGame, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.GuestName, seasonID))
                .MustHaveHappenedTwiceExactly();

            A.CallTo(() => _teamSeasonRepository.FindEntity(oldGame.HostName, seasonID))
                .MustHaveHappenedTwiceExactly();

            #region EditWinLossData(oldGame, operation);

            A.CallTo(() => _calculator.Subtract(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(guestSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(guestSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(guestSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Subtract(hostSeasonWins, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(hostSeasonLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Subtract(hostSeasonTies, 1)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion EditScoringData(oldGame, operation);

            #region EditScoringData(oldGame, operation);

            #region EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            A.CallTo(() => _calculator.Subtract(guestPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(guestPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(0, guestSeason.PythagoreanWins);
            Assert.AreEqual(0, guestSeason.PythagoreanLosses);

            #endregion EditScoringDataByTeamSeason(oldGame.GuestName, oldGame.SeasonID, operation, oldGame.GuestScore, oldGame.HostScore);

            #region EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            A.CallTo(() => _calculator.Subtract(hostPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(hostPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(0, hostSeason.PythagoreanWins);
            Assert.AreEqual(0, hostSeason.PythagoreanLosses);

            #endregion EditScoringDataByTeamSeason(oldGame.HostName, oldGame.SeasonID, operation, oldGame.HostScore, oldGame.GuestScore);

            #endregion EditScoringData(oldGame, operation);

            #endregion ProcessGame(oldGame, operation);

            #endregion EditTeams(oldGame, Direction.Up);

            #endregion DeleteGameFromTeams(oldGame);

            A.CallTo(() => _gameRepository.RemoveEntity(oldGame)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void DeleteGame_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            A.CallTo(() => _gameRepository.FindEntity(A<int>.Ignored)).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => service.DeleteGame(new Game()));
        }

        //[TestCase]
        //public void EditTeams_DirectionInvalid_ThrowsArgumentException()
        //{
        //    // Arrange
        //    var service = new GamesWindowService(_dbContext, _gameRepository, _teamSeasonRepository,
        //        _weekCountRepository, _sharedService, _calculator);

        //    // Define argument variables of method under test.
        //    var game = new Game();
        //    var direction = "invalid";

        //    // TODO: Set up needed infrastructure of class under test.

        //    // Act
        //    service.EditTeams(game, direction);

        //    // Assert
        //    #region ProcessGame(game, operation);
        //    #endregion ProcessGame(game, operation);
        //}

        //[TestCase]
        //public void EditTeams_ProcessGameThrowsObjectNotFoundException_ShowsExceptionMessage()
        //{
        //    // Arrange
        //    var service = new GamesWindowService(_dbContext, _gameRepository, _teamSeasonRepository,
        //        _weekCountRepository, _sharedService, _calculator);

        //    // Define argument variables of method under test.
        //    var game = new Game();
        //    var direction = Direction.Up;

        //    // TODO: Set up needed infrastructure of class under test.
        //    // ProcessGame cannot be faked, as it is another method of the class under test.

        //    // Act
        //    service.EditTeams(game, direction);

        //    // Assert
        //    #region ProcessGame(game, operation);
        //    #endregion ProcessGame(game, operation);
        //}

        //[TestCase]
        //public void EditWinLossData_WinnerAndLoserAreNull_UpdatesTies()
        //{
        //    // Arrange
        //    var service = new GamesWindowService(_dbContext, _gameRepository, _teamSeasonRepository,
        //        _weekCountRepository, _sharedService, _calculator);

        //    // Define argument variables of method under test.
        //    var seasonID = 2017;
        //    var guestName = "Guest";
        //    var hostName = "Host";
        //    var game = new Game
        //    {
        //        SeasonID = seasonID,
        //        GuestName = guestName,
        //        HostName = hostName
        //    };
        //    var operation = new Operation(_calculator.Add);

        //    // Set up needed infrastructure of class under test.
        //    var guestSeasonGames = 13;
        //    var guestSeasonWins = 6;
        //    var guestSeasonLosses = 5;
        //    var guestSeasonTies = 2;
        //    var guestSeason = new TeamSeason
        //    {
        //        TeamName = guestName,
        //        SeasonID = seasonID,
        //        Games = guestSeasonGames,
        //        Wins = guestSeasonWins,
        //        Losses = guestSeasonLosses,
        //        Ties = guestSeasonTies
        //    };
        //    A.CallTo(() => _teamSeasonRepository.FindEntity(guestName, A<int>.Ignored)).Returns(guestSeason);

        //    var hostSeasonGames = 8;
        //    var hostSeasonWins = 4;
        //    var hostSeasonLosses = 3;
        //    var hostSeasonTies = 1;
        //    var hostSeason = new TeamSeason
        //    {
        //        TeamName = hostName,
        //        SeasonID = seasonID,
        //        Games = hostSeasonGames,
        //        Wins = hostSeasonWins,
        //        Losses = hostSeasonLosses,
        //        Ties = hostSeasonTies
        //    };
        //    A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

        //    // Act
        //    service.EditWinLossData(game, operation);

        //    // Assert
        //    A.CallTo(() => _teamSeasonRepository.FindEntity(game.GuestName, seasonID))
        //        .MustHaveHappenedOnceExactly();
        //    A.CallTo(() => _calculator.Add(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();

        //    A.CallTo(() => _teamSeasonRepository.FindEntity(game.HostName, seasonID))
        //        .MustHaveHappenedOnceExactly();
        //    A.CallTo(() => _calculator.Add(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

        //    A.CallTo(() => _calculator.Add(guestSeasonWins, A<double>.Ignored)).MustNotHaveHappened();
        //    A.CallTo(() => _calculator.Add(guestSeasonLosses, A<double>.Ignored)).MustNotHaveHappened();
        //    A.CallTo(() => _calculator.Add(guestSeasonTies, 1)).MustHaveHappenedOnceExactly();

        //    A.CallTo(() => _calculator.Add(hostSeasonWins, A<double>.Ignored)).MustNotHaveHappened();
        //    A.CallTo(() => _calculator.Add(hostSeasonLosses, A<double>.Ignored)).MustNotHaveHappened();
        //    A.CallTo(() => _calculator.Add(hostSeasonTies, 1)).MustHaveHappenedOnceExactly();

        //    A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
        //    A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
        //}

        //[TestCase]
        //public void EditWinLossData_WinnerAndLoserAreNotNull_UpdatesWinsAndLosses()
        //{
        //    // Arrange
        //    var service = new GamesWindowService(_dbContext, _gameRepository, _teamSeasonRepository,
        //        _weekCountRepository, _sharedService, _calculator);

        //    // Define argument variables of method under test.
        //    var seasonID = 2017;
        //    var guestName = "Guest";
        //    var hostName = "Host";
        //    var game = new Game
        //    {
        //        SeasonID = seasonID,
        //        GuestName = guestName,
        //        HostName = hostName,
        //        WinnerName = hostName,
        //        LoserName = guestName
        //    };
        //    var operation = new Operation(_calculator.Add);

        //    // Set up needed infrastructure of class under test.
        //    var guestSeasonGames = 13;
        //    var guestSeasonWins = 6;
        //    var guestSeasonLosses = 5;
        //    var guestSeasonTies = 2;
        //    var guestSeason = new TeamSeason
        //    {
        //        TeamName = guestName,
        //        SeasonID = seasonID,
        //        Games = guestSeasonGames,
        //        Wins = guestSeasonWins,
        //        Losses = guestSeasonLosses,
        //        Ties = guestSeasonTies
        //    };
        //    A.CallTo(() => _teamSeasonRepository.FindEntity(guestName, A<int>.Ignored)).Returns(guestSeason);

        //    var hostSeasonGames = 8;
        //    var hostSeasonWins = 4;
        //    var hostSeasonLosses = 3;
        //    var hostSeasonTies = 1;
        //    var hostSeason = new TeamSeason
        //    {
        //        TeamName = hostName,
        //        SeasonID = seasonID,
        //        Games = hostSeasonGames,
        //        Wins = hostSeasonWins,
        //        Losses = hostSeasonLosses,
        //        Ties = hostSeasonTies
        //    };
        //    A.CallTo(() => _teamSeasonRepository.FindEntity(hostName, A<int>.Ignored)).Returns(hostSeason);

        //    // Act
        //    service.EditWinLossData(game, operation);

        //    // Assert
        //    A.CallTo(() => _teamSeasonRepository.FindEntity(game.GuestName, seasonID))
        //        .MustHaveHappenedTwiceExactly();
        //    A.CallTo(() => _calculator.Add(guestSeasonGames, 1)).MustHaveHappenedOnceExactly();

        //    A.CallTo(() => _teamSeasonRepository.FindEntity(game.HostName, seasonID))
        //        .MustHaveHappenedTwiceExactly();
        //    A.CallTo(() => _calculator.Add(hostSeasonGames, 1)).MustHaveHappenedOnceExactly();

        //    A.CallTo(() => _calculator.Add(hostSeasonWins, 1)).MustHaveHappenedOnceExactly();
        //    A.CallTo(() => _calculator.Add(guestSeasonLosses, 1)).MustHaveHappenedOnceExactly();

        //    A.CallTo(() => _calculator.Add(guestSeasonTies, A<double>.Ignored)).MustNotHaveHappened();
        //    A.CallTo(() => _calculator.Add(hostSeasonTies, A<double>.Ignored)).MustNotHaveHappened();

        //    A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
        //    A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
        //}

        [TestCase]
        public void GetGames_GuestAndHostBothNull_FiltersGamesOnlyBySeason()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            string guestName = null;
            string hostName = null;

            // Set up needed infrastructure of class under test.
            SetUpGameRepositoryForGetGamesTest();

            // Act
            var result = service.GetGames(seasonID, guestName, hostName);

            // Assert
            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<IEnumerable<Game>>(result);

            var resultCount = result.Count();
            Assert.AreEqual(12, resultCount);
            for (int i = 0; i < resultCount; i++)
            {
                Assert.AreEqual(seasonID, result.ElementAt(i).SeasonID);

                if (i > 0)
                {
                    Assert.LessOrEqual(result.ElementAt(i).Week, result.ElementAt(i - 1).Week);

                    if (result.ElementAt(i).Week == result.ElementAt(i - 1).Week)
                    {
                        Assert.Less(result.ElementAt(i).ID, result.ElementAt(i - 1).ID);
                    }
                }
            }
        }

        [TestCase]
        public void GetGames_GuestEmptyAndHostNull_FiltersGamesOnlyBySeason()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            string guestName = string.Empty;
            string hostName = null;

            // Set up needed infrastructure of class under test.
            SetUpGameRepositoryForGetGamesTest();

            // Act
            var result = service.GetGames(seasonID, guestName, hostName);

            // Assert
            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<IEnumerable<Game>>(result);

            var resultCount = result.Count();
            Assert.AreEqual(12, resultCount);
            for (int i = 0; i < resultCount; i++)
            {
                Assert.AreEqual(seasonID, result.ElementAt(i).SeasonID);

                if (i > 0)
                {
                    Assert.LessOrEqual(result.ElementAt(i).Week, result.ElementAt(i - 1).Week);

                    if (result.ElementAt(i).Week == result.ElementAt(i - 1).Week)
                    {
                        Assert.Less(result.ElementAt(i).ID, result.ElementAt(i - 1).ID);
                    }
                }
            }
        }

        [TestCase]
        public void GetGames_GuestNeitherNullNorEmptyAndHostNull_FiltersGamesBySeasonAndGuestName()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            string guestName = "Guest 2";
            string hostName = null;

            // Set up needed infrastructure of class under test.
            SetUpGameRepositoryForGetGamesTest();

            // Act
            var result = service.GetGames(seasonID, guestName, hostName);

            // Assert
            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<IEnumerable<Game>>(result);

            var resultCount = result.Count();
            Assert.AreEqual(6, resultCount);
            for (int i = 0; i < resultCount; i++)
            {
                Assert.AreEqual(seasonID, result.ElementAt(i).SeasonID);
                Assert.AreEqual(guestName, result.ElementAt(i).GuestName);

                if (i > 0)
                {
                    Assert.LessOrEqual(result.ElementAt(i).Week, result.ElementAt(i - 1).Week);

                    if (result.ElementAt(i).Week == result.ElementAt(i - 1).Week)
                    {
                        Assert.Less(result.ElementAt(i).ID, result.ElementAt(i - 1).ID);
                    }
                }
            }
        }

        [TestCase]
        public void GetGames_GuestNeitherNullNorEmptyAndHostEmpty_FiltersGamesBySeasonAndGuestName()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            string guestName = "Guest 2";
            string hostName = string.Empty;

            // Set up needed infrastructure of class under test.
            SetUpGameRepositoryForGetGamesTest();

            // Act
            var result = service.GetGames(seasonID, guestName, hostName);

            // Assert
            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<IEnumerable<Game>>(result);

            var resultCount = result.Count();
            Assert.AreEqual(6, resultCount);
            for (int i = 0; i < resultCount; i++)
            {
                Assert.AreEqual(seasonID, result.ElementAt(i).SeasonID);
                Assert.AreEqual(guestName, result.ElementAt(i).GuestName);

                if (i > 0)
                {
                    Assert.LessOrEqual(result.ElementAt(i).Week, result.ElementAt(i - 1).Week);

                    if (result.ElementAt(i).Week == result.ElementAt(i - 1).Week)
                    {
                        Assert.Less(result.ElementAt(i).ID, result.ElementAt(i - 1).ID);
                    }
                }
            }
        }

        [TestCase]
        public void GetGames_GuestNeitherNullNorEmptyAndHostNeitherNullNorEmpty_FiltersGamesBySeasonAndGuestNameAndHostName()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            string guestName = "Guest 2";
            string hostName = "Host 2";

            // Set up needed infrastructure of class under test.
            SetUpGameRepositoryForGetGamesTest();

            // Act
            var result = service.GetGames(seasonID, guestName, hostName);

            // Assert
            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<IEnumerable<Game>>(result);

            var resultCount = result.Count();
            Assert.AreEqual(3, resultCount);
            for (int i = 0; i < resultCount; i++)
            {
                Assert.AreEqual(seasonID, result.ElementAt(i).SeasonID);
                Assert.AreEqual(guestName, result.ElementAt(i).GuestName);
                Assert.AreEqual(hostName, result.ElementAt(i).HostName);

                if (i > 0)
                {
                    Assert.LessOrEqual(result.ElementAt(i).Week, result.ElementAt(i - 1).Week);

                    if (result.ElementAt(i).Week == result.ElementAt(i - 1).Week)
                    {
                        Assert.Less(result.ElementAt(i).ID, result.ElementAt(i - 1).ID);
                    }
                }
            }
        }

        [TestCase]
        public void GetGames_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Define argument variables of method under test.
            var seasonID = 2017;
            string guestName = "Guest";
            string hostName = "Host";

            A.CallTo(() => _gameRepository.GetEntities()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => service.GetGames(seasonID, guestName, hostName));
        }

        [TestCase]
        public void GetWeekCount_HappyPath()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // Set up needed infrastructure of class under test.
            var count = 3;
            var startYear = 2016;
            var weekCounts = new List<WeekCount>(count);
            for (int i = startYear; i < startYear + count; i++)
            {
                var weekCount = new WeekCount { SeasonID = i };
                weekCounts.Add(weekCount);
            }
            A.CallTo(() => _weekCountRepository.GetEntities()).Returns(weekCounts);

            // Act
            var result = service.GetWeekCount();

            // Assert
            A.CallTo(() => _weekCountRepository.GetEntities()).MustHaveHappenedOnceExactly();
            Assert.AreEqual(weekCounts.FirstOrDefault().Count, result);
        }

        [TestCase]
        public void GetWeekCount_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            A.CallTo(() => _weekCountRepository.GetEntities()).Throws<Exception>();

            // Act
            int result = 0;
            Assert.Throws<Exception>(() => result = service.GetWeekCount());

            // Assert
            Assert.AreEqual(0, result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new GamesWindowService(_sharedService, _gameRepository, _teamSeasonRepository,
                _weekCountRepository, _dbContext, _calculator);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }

        private void SetUpGameRepositoryForGetGamesTest()
        {
            int id = 1;
            var seasonCount = 3;
            var weekCount = 3;
            var guestCount = 2;
            var hostCount = 2;
            var games = new List<Game>(seasonCount * weekCount * guestCount * hostCount);
            for (int i = 0; i < seasonCount; i++)
            {
                for (int j = weekCount; j > 0; j--)
                {
                    for (int k = 1; k <= guestCount; k++)
                    {
                        for (int l = 1; l <= hostCount; l++)
                        {
                            var game = new Game
                            {
                                ID = id++,
                                SeasonID = 2017 - i,
                                Week = j,
                                GuestName = $"Guest {k}",
                                HostName = $"Host {l}"
                            };
                            games.Add(game);
                        }
                    }
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);
        }
    }
}
