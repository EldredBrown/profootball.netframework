using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Services;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Tests.Services
{
    [TestFixture]
    public class GamesServiceTest
    {
        private ISharedService _sharedService;
        private IDataMapper _dataMapper;
        private IRepository<Game> _gameRepository;
        private IRepository<Team> _teamRepository;
        private IRepository<TeamSeason> _teamSeasonRepository;
        private ICalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _dataMapper = A.Fake<IDataMapper>();
            _gameRepository = A.Fake<IRepository<Game>>();
            _teamRepository = A.Fake<IRepository<Team>>();
            _teamSeasonRepository = A.Fake<IRepository<TeamSeason>>();
            _calculator = A.Fake<ICalculator>();
        }

        [TestCase]
        public async Task AddEntity_TieAndPythPctNull_AddsTiesToTeamSeasonRecordsAndSetsPythWinsAndLossesToZero()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var newGameViewModel = new GameViewModel();
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonID = 2017;
            var guestName = "Guest";
            var hostName = "Host";
            var teamScore = 21;

            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = teamScore,
                HostName = hostName,
                HostScore = teamScore
            };
            A.CallTo(() => _dataMapper.MapToGame(A<GameViewModel>.Ignored)).Returns(newGame);

            var games = 3;
            var winsAndLosses = 2;
            var ties = 1;
            var teamSeasonPointsFor = 28;
            var teamSeasonPointsAgainst = 14;

            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID)).Returns(guestSeason);

            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID)).Returns(hostSeason);

            double? pythPct = null;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            await service.AddEntity(newGameViewModel, dbContext);

            // Assert
            A.CallTo(() => _dataMapper.MapToGame(newGameViewModel)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(newGame);

            Assert.IsNull(newGame.WinnerName);
            Assert.IsNull(newGame.LoserName);

            #endregion DecideWinnerAndLoser(newGame);

            A.CallTo(() => _gameRepository.AddEntity(dbContext, newGame)).MustHaveHappenedOnceExactly();

            #region await AddEntityToTeams(dbContext, newGame);

            #region await EditTeams(dbContext, game, Direction.Up);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Add(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(winsAndLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(ties, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await EditScoringData(dbContext, game, operation);

            #region await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            #region await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            #endregion await EditScoringData(dbContext, game, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(teamSeasonPointsFor, teamScore)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(teamSeasonPointsAgainst, teamScore)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustNotHaveHappened();

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, game, Direction.Up);

            #endregion await AddEntityToTeams(dbContext, newGame);

            A.CallTo(() => (dbContext as ProFootballEntities).SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task AddEntity_TieAndPythPctNotNull_AddsTiesToTeamSeasonRecordsAndComputesPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var newGameViewModel = new GameViewModel();
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonID = 2017;
            var guestName = "Guest";
            var hostName = "Host";
            var teamScore = 21;

            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = teamScore,
                HostName = hostName,
                HostScore = teamScore
            };
            A.CallTo(() => _dataMapper.MapToGame(A<GameViewModel>.Ignored)).Returns(newGame);

            var games = 3;
            var winsAndLosses = 2;
            var ties = 1;
            var teamSeasonPointsFor = 28;
            var teamSeasonPointsAgainst = 14;

            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID)).Returns(guestSeason);

            var guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID)).Returns(hostSeason);

            var hostPythPct = 0.7;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            await service.AddEntity(newGameViewModel, dbContext);

            // Assert
            A.CallTo(() => _dataMapper.MapToGame(newGameViewModel)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(newGame);

            Assert.IsNull(newGame.WinnerName);
            Assert.IsNull(newGame.LoserName);

            #endregion DecideWinnerAndLoser(newGame);

            A.CallTo(() => _gameRepository.AddEntity(dbContext, newGame));

            #region await AddEntityToTeams(dbContext, newGame);

            #region await EditTeams(dbContext, game, Direction.Up);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Add(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(winsAndLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Add(ties, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await EditScoringData(dbContext, game, operation);

            #region await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double) (1 - guestPythPct), guestSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            #region await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - hostPythPct), hostSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            #endregion await EditScoringData(dbContext, game, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(teamSeasonPointsFor, teamScore)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(teamSeasonPointsAgainst, teamScore)).MustHaveHappenedTwiceExactly();

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, game, Direction.Up);
            
            #endregion await AddEntityToTeams(dbContext, newGame);

            A.CallTo(() => (dbContext as ProFootballEntities).SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task AddEntity_GuestWinsAndPythPctNotNull_AddsWinsAndLossesToTeamSeasonRecordsAndComputesPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var newGameViewModel = new GameViewModel();
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonID = 2017;
            var guestName = "Guest";
            var hostName = "Host";
            var guestScore = 28;
            var hostScore = 14;

            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore,
                WinnerName = guestName,
                WinnerScore = guestScore,
                LoserName = hostName,
                LoserScore = hostScore
            };
            A.CallTo(() => _dataMapper.MapToGame(A<GameViewModel>.Ignored)).Returns(newGame);

            var games = 3;
            var winsAndLosses = 2;
            var ties = 1;
            var teamSeasonPointsFor = 28;
            var teamSeasonPointsAgainst = 14;

            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID)).Returns(guestSeason);

            var guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID)).Returns(hostSeason);

            var hostPythPct = 0.7;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            await service.AddEntity(newGameViewModel, dbContext);

            // Assert
            A.CallTo(() => _dataMapper.MapToGame(newGameViewModel)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(newGame);

            Assert.AreEqual(newGame.GuestName, newGame.WinnerName);
            Assert.AreEqual(newGame.GuestScore, newGame.WinnerScore);
            Assert.AreEqual(newGame.HostName, newGame.LoserName);
            Assert.AreEqual(newGame.HostScore, newGame.LoserScore);

            #endregion DecideWinnerAndLoser(newGame);

            A.CallTo(() => _gameRepository.AddEntity(dbContext, newGame));

            #region await AddEntityToTeams(dbContext, newGame);

            #region await EditTeams(dbContext, game, Direction.Up);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Add(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(winsAndLosses, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(ties, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await EditScoringData(dbContext, game, operation);

            #region await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            A.CallTo(() => _calculator.Add(teamSeasonPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(teamSeasonPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - guestPythPct), guestSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            #region await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => _calculator.Add(teamSeasonPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(teamSeasonPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - hostPythPct), hostSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            #endregion await EditScoringData(dbContext, game, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, game, Direction.Up);

            #endregion await AddEntityToTeams(dbContext, newGame);

            A.CallTo(() => (dbContext as ProFootballEntities).SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task AddEntity_HostWinsAndPythPctNotNull_AddsWinsAndLossesToTeamSeasonRecordsAndComputesPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var newGameViewModel = new GameViewModel();
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonID = 2017;
            var guestName = "Guest";
            var hostName = "Host";
            var guestScore = 14;
            var hostScore = 28;

            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore,
                WinnerName = guestName,
                WinnerScore = guestScore,
                LoserName = hostName,
                LoserScore = hostScore
            };
            A.CallTo(() => _dataMapper.MapToGame(A<GameViewModel>.Ignored)).Returns(newGame);

            var games = 3;
            var winsAndLosses = 2;
            var ties = 1;
            var teamSeasonPointsFor = 28;
            var teamSeasonPointsAgainst = 14;

            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID)).Returns(guestSeason);

            var guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID)).Returns(hostSeason);

            var hostPythPct = 0.7;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            await service.AddEntity(newGameViewModel, dbContext);

            // Assert
            A.CallTo(() => _dataMapper.MapToGame(newGameViewModel)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(newGame);

            Assert.AreEqual(newGame.HostName, newGame.WinnerName);
            Assert.AreEqual(newGame.HostScore, newGame.WinnerScore);
            Assert.AreEqual(newGame.GuestName, newGame.LoserName);
            Assert.AreEqual(newGame.GuestScore, newGame.LoserScore);

            #endregion DecideWinnerAndLoser(newGame);

            A.CallTo(() => _gameRepository.AddEntity(dbContext, newGame));

            #region await AddEntityToTeams(dbContext, newGame);

            #region await EditTeams(dbContext, game, Direction.Up);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Add(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(winsAndLosses, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(ties, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await EditScoringData(dbContext, game, operation);

            #region await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            A.CallTo(() => _calculator.Add(teamSeasonPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(teamSeasonPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - guestPythPct), guestSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            #region await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => _calculator.Add(teamSeasonPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Add(teamSeasonPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - hostPythPct), hostSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            #endregion await EditScoringData(dbContext, game, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, game, Direction.Up);

            #endregion await AddEntityToTeams(dbContext, newGame);

            A.CallTo(() => (dbContext as ProFootballEntities).SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task DeleteGame_TieAndPythPctNull_SubtractsTiesFromTeamSeasonRecordsAndSetsPythWinsAndLossesToZero()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var id = 1;
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonID = 2017;
            var guestName = "Guest";
            var hostName = "Host";
            var teamScore = 21;

            var oldGame = new Game
            {
                ID = id,
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = teamScore,
                HostName = hostName,
                HostScore = teamScore
            };
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, A<int>.Ignored)).Returns(oldGame);

            var games = 3;
            var winsAndLosses = 2;
            var ties = 1;
            var teamSeasonPointsFor = 28;
            var teamSeasonPointsAgainst = 14;

            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID)).Returns(guestSeason);

            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID)).Returns(hostSeason);

            double? pythPct = null;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(A<TeamSeason>.Ignored)).Returns(pythPct);

            // Act
            await service.DeleteGame(id, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, id)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.IsNull(oldGame.WinnerName);
            Assert.IsNull(oldGame.LoserName);

            #endregion DecideWinnerAndLoser(oldGame);

            #region await DeleteGameFromTeams(dbContext, oldGame);

            #region await EditTeams(dbContext, game, Direction.Up);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Subtract(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(ties, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(winsAndLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await EditScoringData(dbContext, game, operation);

            #region await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            #region await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            #endregion await EditScoringData(dbContext, game, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(teamSeasonPointsFor, teamScore)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(teamSeasonPointsAgainst, teamScore)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Multiply(A<double>.Ignored, A<double>.Ignored)).MustNotHaveHappened();

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, game, Direction.Up);

            #endregion await DeleteGameFromTeams(dbContext, oldGame);

            A.CallTo(() => _gameRepository.RemoveEntity(dbContext, oldGame)).MustHaveHappenedOnceExactly();
            A.CallTo(() => (dbContext as ProFootballEntities).SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task DeleteGame_TieAndPythPctNotNull_SubtractsTiesFromTeamSeasonRecordsAndComputesPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var id = 1;
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonID = 2017;
            var guestName = "Guest";
            var hostName = "Host";
            var teamScore = 21;

            var oldGame = new Game
            {
                ID = id,
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = teamScore,
                HostName = hostName,
                HostScore = teamScore
            };
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, A<int>.Ignored)).Returns(oldGame);

            var games = 3;
            var winsAndLosses = 2;
            var ties = 1;
            var teamSeasonPointsFor = 28;
            var teamSeasonPointsAgainst = 14;

            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID)).Returns(guestSeason);

            var guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID)).Returns(hostSeason);

            var hostPythPct = 0.7;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            await service.DeleteGame(id, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, id)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.IsNull(oldGame.WinnerName);
            Assert.IsNull(oldGame.LoserName);

            #endregion DecideWinnerAndLoser(oldGame);

            #region await DeleteGameFromTeams(dbContext, oldGame);

            #region await EditTeams(dbContext, game, Direction.Up);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Subtract(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(ties, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(winsAndLosses, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await EditScoringData(dbContext, game, operation);

            #region await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - guestPythPct), guestSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            #region await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - hostPythPct), hostSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            #endregion await EditScoringData(dbContext, game, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(teamSeasonPointsFor, teamScore)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(teamSeasonPointsAgainst, teamScore)).MustHaveHappenedTwiceExactly();

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, game, Direction.Up);

            #endregion await DeleteGameFromTeams(dbContext, oldGame);

            A.CallTo(() => _gameRepository.RemoveEntity(dbContext, oldGame)).MustHaveHappenedOnceExactly();
            A.CallTo(() => (dbContext as ProFootballEntities).SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task DeleteGame_GuestWinsAndPythPctNotNull_SubtractsWinsAndLossesFromTeamSeasonRecordsAndComputesPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var id = 1;
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonID = 2017;
            var guestName = "Guest";
            var hostName = "Host";
            var guestScore = 28;
            var hostScore = 14;

            var oldGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore,
                WinnerName = guestName,
                WinnerScore = guestScore,
                LoserName = hostName,
                LoserScore = hostScore
            };
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, A<int>.Ignored)).Returns(oldGame);

            var games = 3;
            var winsAndLosses = 2;
            var ties = 1;
            var teamSeasonPointsFor = 28;
            var teamSeasonPointsAgainst = 14;

            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID)).Returns(guestSeason);

            var guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID)).Returns(hostSeason);

            var hostPythPct = 0.7;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            await service.DeleteGame(id, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, id)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.AreEqual(oldGame.GuestName, oldGame.WinnerName);
            Assert.AreEqual(oldGame.GuestScore, oldGame.WinnerScore);
            Assert.AreEqual(oldGame.Host, oldGame.Loser);
            Assert.AreEqual(oldGame.HostScore, oldGame.LoserScore);

            #endregion DecideWinnerAndLoser(oldGame);

            #region await DeleteGameFromTeams(dbContext, oldGame);

            #region await EditTeams(dbContext, game, Direction.Up);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Subtract(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(winsAndLosses, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(ties, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await EditScoringData(dbContext, game, operation);

            #region await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            A.CallTo(() => _calculator.Subtract(teamSeasonPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(teamSeasonPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - guestPythPct), guestSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            #region await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => _calculator.Subtract(teamSeasonPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(teamSeasonPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - hostPythPct), hostSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            #endregion await EditScoringData(dbContext, game, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, game, Direction.Up);

            #endregion await DeleteGameFromTeams(dbContext, oldGame);

            A.CallTo(() => _gameRepository.RemoveEntity(dbContext, oldGame)).MustHaveHappenedOnceExactly();
            A.CallTo(() => (dbContext as ProFootballEntities).SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task DeleteGame_HostWinsAndPythPctNotNull_SubtractsWinsAndLossesFromTeamSeasonRecordsAndComputesPythWinsAndLosses()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var id = 1;
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonID = 2017;
            var guestName = "Guest";
            var hostName = "Host";
            var guestScore = 14;
            var hostScore = 28;

            var oldGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = guestScore,
                HostName = hostName,
                HostScore = hostScore,
                WinnerName = hostName,
                WinnerScore = hostScore,
                LoserName = guestName,
                LoserScore = guestScore
            };
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, A<int>.Ignored)).Returns(oldGame);

            var games = 3;
            var winsAndLosses = 2;
            var ties = 1;
            var teamSeasonPointsFor = 28;
            var teamSeasonPointsAgainst = 14;

            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID)).Returns(guestSeason);

            var guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID)).Returns(hostSeason);

            var hostPythPct = 0.7;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            // Act
            await service.DeleteGame(id, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, id)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.AreEqual(oldGame.HostName, oldGame.WinnerName);
            Assert.AreEqual(oldGame.HostScore, oldGame.WinnerScore);
            Assert.AreEqual(oldGame.GuestName, oldGame.LoserName);
            Assert.AreEqual(oldGame.GuestScore, oldGame.LoserScore);

            #endregion DecideWinnerAndLoser(oldGame);

            #region await DeleteGameFromTeams(dbContext, oldGame);

            #region await EditTeams(dbContext, game, Direction.Up);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Subtract(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(winsAndLosses, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(ties, 1)).MustNotHaveHappened();
            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();

            #endregion await EditWinLossData(dbContext, game, operation);

            #region await EditScoringData(dbContext, game, operation);

            #region await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            A.CallTo(() => _calculator.Subtract(teamSeasonPointsFor, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(teamSeasonPointsAgainst, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)guestPythPct, guestSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - guestPythPct), guestSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.GuestName, game.SeasonID, operation, game.GuestScore, game.HostScore);

            #region await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            A.CallTo(() => _calculator.Subtract(teamSeasonPointsFor, hostScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Subtract(teamSeasonPointsAgainst, guestScore)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)hostPythPct, hostSeason.Games)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - hostPythPct), hostSeason.Games))
                .MustHaveHappenedOnceExactly();

            #endregion await EditScoringDataByTeamSeason(dbContext, game.HostName, game.SeasonID, operation, game.HostScore, game.GuestScore);

            #endregion await EditScoringData(dbContext, game, operation);

            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID))
                .MustHaveHappened(3, Times.Exactly);
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID))
                .MustHaveHappened(3, Times.Exactly);

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, game, Direction.Up);

            #endregion await DeleteGameFromTeams(dbContext, oldGame);

            A.CallTo(() => _gameRepository.RemoveEntity(dbContext, oldGame)).MustHaveHappenedOnceExactly();
            A.CallTo(() => (dbContext as ProFootballEntities).SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task EditEntity()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var oldGameViewModel = new GameViewModel();
            var newGameViewModel = new GameViewModel
            {
                Week = 1,
                GuestName = "Guest",
                GuestScore = 14,
                HostName = "Host",
                HostScore = 28,
                IsPlayoffGame = false,
                Notes = "Notes"
            };
            var dbContext = A.Fake<ProFootballEntities>();

            var selectedGame = new Game();
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, A<int>.Ignored)).Returns(selectedGame);

            var seasonID = 2017;
            var guestName = "Guest";
            var hostName = "Host";
            var oldGuestScore = 28;
            var oldHostScore = 14;

            var oldGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = oldGuestScore,
                HostName = hostName,
                HostScore = oldHostScore,
                WinnerName = guestName,
                WinnerScore = oldGuestScore,
                LoserName = hostName,
                LoserScore = oldHostScore
            };
            A.CallTo(() => _dataMapper.MapToGame(oldGameViewModel)).Returns(oldGame);

            var newGuestScore = 14;
            var newHostScore = 28;

            var newGame = new Game
            {
                SeasonID = seasonID,
                GuestName = guestName,
                GuestScore = newGuestScore,
                HostName = hostName,
                HostScore = newHostScore,
                WinnerName = guestName,
                WinnerScore = newGuestScore,
                LoserName = hostName,
                LoserScore = newHostScore
            };
            A.CallTo(() => _dataMapper.MapToGame(newGameViewModel)).Returns(newGame);

            var games = 3;
            var winsAndLosses = 2;
            var ties = 1;
            var teamSeasonPointsFor = 28;
            var teamSeasonPointsAgainst = 14;

            var guestSeason = new TeamSeason
            {
                TeamName = guestName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID)).Returns(guestSeason);

            var guestPythPct = 0.6;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason)).Returns(guestPythPct);

            var hostSeason = new TeamSeason
            {
                TeamName = hostName,
                SeasonID = seasonID,
                Games = games,
                Wins = winsAndLosses,
                Losses = winsAndLosses,
                Ties = ties,
                PointsFor = teamSeasonPointsFor,
                PointsAgainst = teamSeasonPointsAgainst
            };
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID)).Returns(hostSeason);

            var hostPythPct = 0.7;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason)).Returns(hostPythPct);

            A.CallTo(() => _calculator.Subtract(games, 1)).Returns(games);
            A.CallTo(() => _calculator.Subtract(winsAndLosses, 1)).Returns(winsAndLosses);
            A.CallTo(() => _calculator.Subtract(ties, 1)).Returns(ties);

            // Act
            await service.EditGame(oldGameViewModel, newGameViewModel, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, oldGameViewModel.ID))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual(newGameViewModel.Week, selectedGame.Week);
            Assert.AreEqual(newGameViewModel.GuestName, selectedGame.GuestName);
            Assert.AreEqual(newGameViewModel.GuestScore, selectedGame.GuestScore);
            Assert.AreEqual(newGameViewModel.HostName, selectedGame.HostName);
            Assert.AreEqual(newGameViewModel.HostScore, selectedGame.HostScore);
            Assert.AreEqual(newGameViewModel.IsPlayoffGame, selectedGame.IsPlayoffGame);
            Assert.AreEqual(newGameViewModel.Notes, selectedGame.Notes);

            #region DecideWinnerAndLoser(selectedGame);

            Assert.IsNull(selectedGame.Winner);
            Assert.IsNull(selectedGame.Loser);

            #endregion DecideWinnerAndLoser(selectedGame);

            A.CallTo(() => _gameRepository.EditEntity(dbContext, selectedGame)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToGame(oldGameViewModel)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(oldGame);

            Assert.AreEqual(oldGame.GuestName, oldGame.WinnerName);
            Assert.AreEqual(oldGame.GuestScore, oldGame.WinnerScore);
            Assert.AreEqual(oldGame.HostName, oldGame.LoserName);
            Assert.AreEqual(oldGame.HostScore, oldGame.LoserScore);

            #endregion DecideWinnerAndLoser(oldGame);

            A.CallTo(() => _dataMapper.MapToGame(newGameViewModel)).MustHaveHappenedOnceExactly();

            #region DecideWinnerAndLoser(newGame);

            Assert.AreEqual(newGame.HostName, newGame.WinnerName);
            Assert.AreEqual(newGame.HostScore, newGame.WinnerScore);
            Assert.AreEqual(newGame.GuestName, newGame.LoserName);
            Assert.AreEqual(newGame.GuestScore, newGame.LoserScore);

            #endregion DecideWinnerAndLoser(newGame);

            #region await EditGameInTeams(dbContext, oldGame, newGame);

            #region await EditTeams(dbContext, oldGame, Direction.Down);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Subtract(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(winsAndLosses, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Subtract(ties, 1)).MustNotHaveHappened();

            #endregion await EditWinLossData(dbContext, game, operation);

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, oldGame, Direction.Down);

            #region await EditTeams(dbContext, newGame, Direction.Up);

            #region await ProcessGame(dbContext, game, operation);

            #region await EditWinLossData(dbContext, game, operation);

            A.CallTo(() => _calculator.Add(games, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(winsAndLosses, 1)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Add(ties, 1)).MustNotHaveHappened();

            #endregion await EditWinLossData(dbContext, game, operation);

            #endregion await ProcessGame(dbContext, game, operation);

            #endregion await EditTeams(dbContext, newGame, Direction.Up);

            A.CallTo(() => _calculator.CalculateWinningPercentage(guestSeason)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.CalculateWinningPercentage(hostSeason)).MustHaveHappenedTwiceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(guestSeason))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Multiply((double)guestPythPct, A<double>.Ignored))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - guestPythPct), A<double>.Ignored))
                .MustHaveHappenedTwiceExactly();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(hostSeason))
                .MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Multiply((double)hostPythPct, A<double>.Ignored)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => _calculator.Multiply((double)(1 - hostPythPct), A<double>.Ignored))
                .MustHaveHappenedTwiceExactly();

            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, guestName, seasonID))
                .MustHaveHappened(6, Times.Exactly);
            A.CallTo(() => _teamSeasonRepository.FindEntityAsync(dbContext, hostName, seasonID))
                .MustHaveHappened(6, Times.Exactly);
            
            #endregion  await EditGameInTeams(dbContext, oldGame, newGame);

            A.CallTo(() => (dbContext as ProFootballEntities).SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public async Task FindEntityAsync()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            int id = 1;
            var dbContext = A.Fake<ProFootballEntities>();

            var game = new Game();
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, A<int>.Ignored)).Returns(game);

            var gameViewModel = new GameViewModel();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.Ignored)).Returns(gameViewModel);

            // Act
            var result = await service.FindEntityAsync(id, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.FindEntityAsync(dbContext, id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(game)).MustHaveHappenedOnceExactly();
            Assert.AreSame(gameViewModel, result);
        }

        [TestCase]
        public async Task GetGamesAsync_SelectedWeekAndGuestSearchStringAndHostSearchStringAllNull_GetsAllGamesForSeason()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = null;
            string guestSearchString = null;
            string hostSearchString = null;
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result = 
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustHaveHappened(9, Times.Exactly);
            Assert.IsInstanceOf<IEnumerable<GameViewModel>>(result);
            Assert.AreEqual(9, result.Count());
        }

        [TestCase]
        public async Task GetGamesAsync_SelectedWeekEmptyAndGuestSearchStringAndHostSearchStringNull_GetsAllGamesForSeason()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = string.Empty;
            string guestSearchString = null;
            string hostSearchString = null;
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result = 
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustHaveHappened(9, Times.Exactly);
            Assert.IsInstanceOf<IEnumerable<GameViewModel>>(result);
            Assert.AreEqual(9, result.Count());
        }

        [TestCase]
        public async Task GetGamesAsync_SelectedWeekNeitherNullNorEmptyAndGuestSearchStringAndHostSearchStringNull_GetsAllGamesForSeasonAndSelectedWeek()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = "1";
            string guestSearchString = null;
            string hostSearchString = null;
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result = 
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustHaveHappened(3, Times.Exactly);
            Assert.IsInstanceOf<IEnumerable<GameViewModel>>(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestCase]
        public async Task GetGamesAsync_SelectedWeekNullAndGuestSearchStringEmptyAndHostSearchStringNull_GetsAllGamesForSeason()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = null;
            string guestSearchString = string.Empty;
            string hostSearchString = null;
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result = 
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustHaveHappened(9, Times.Exactly);
            Assert.IsInstanceOf<IEnumerable<GameViewModel>>(result);
            Assert.AreEqual(9, result.Count());
        }

        [TestCase]
        public async Task GetGamesAsync_SelectedWeekNullAndGuestSearchStringNeitherNullNorEmptyAndHostSearchStringNull_GetsAllGamesForSeasonMatchingGuestSearchString()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = null;
            string guestSearchString = "Guest";
            string hostSearchString = null;
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result = 
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustHaveHappened(3, Times.Exactly);
            Assert.IsInstanceOf<IEnumerable<GameViewModel>>(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestCase]
        public async Task GetGamesAsync_SelectedWeekNullAndGuestSearchStringNeitherNullNorEmptyButNoMatchAndHostSearchStringNull_GetsNoGames()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = null;
            string guestSearchString = "None";
            string hostSearchString = null;
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result = 
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustNotHaveHappened();
            Assert.IsInstanceOf<IEnumerable<GameViewModel>>(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestCase]
        public async Task GetGamesAsync_SelectedWeekAndGuestSearchStringNullAndHostSearchStringEmpty_GetsAllGamesForSeason()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = null;
            string guestSearchString = null;
            string hostSearchString = string.Empty;
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result =
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustHaveHappened(9, Times.Exactly);
            Assert.IsInstanceOf<IEnumerable<GameViewModel>>(result);
            Assert.AreEqual(9, result.Count());
        }

        [TestCase]
        public async Task GetGamesAsync_SelectedWeekAndGuestSearchStringNullAndHostSearchStringNeitherNullNorEmpty_GetsAllGamesForSeasonMatchingHostSearchString()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = null;
            string guestSearchString = null;
            string hostSearchString = "Host";
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result =
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustHaveHappened(3, Times.Exactly);
            Assert.IsInstanceOf<IEnumerable<GameViewModel>>(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestCase]
        public async Task GetGamesAsync_SelectedWeekAndGuestSearchStringNullAndHostSearchStringNeitherNullNorEmptyButNoMatch_GetsNoGames()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = null;
            string guestSearchString = null;
            string hostSearchString = "None";
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result =
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustNotHaveHappened();
            Assert.IsInstanceOf<IEnumerable<GameViewModel>>(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestCase]
        public async Task GetGamesAsync_ArgumentNullException_AbortsAndReturnsNull()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            string selectedWeek = null;
            string guestSearchString = null;
            string hostSearchString = null;
            var dbContext = A.Fake<ProFootballEntities>();

            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Throws<ArgumentNullException>();

            // Act
            var result =
                await service.GetGamesAsync(seasonID, selectedWeek, guestSearchString, hostSearchString, dbContext);

            // Assert
            A.CallTo(() => _dataMapper.MapToGameViewModel(A<Game>.That.IsNotNull())).MustNotHaveHappened();
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task GetTeamsAsync()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var dbContext = A.Fake<ProFootballEntities>();

            var count = 3;
            var teams = new List<Team>(count);
            for (int i = 1; i <= count; i++)
            {
                var team = new Team { Name = "Team" + i };
                teams.Add(team);
            }
            A.CallTo(() => _teamRepository.GetEntitiesAsync(dbContext)).Returns(teams);

            // Act
            var result = await service.GetTeamsAsync(dbContext);

            // Assert
            A.CallTo(() => _teamRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dataMapper.MapToTeamViewModel(A<Team>.That.IsNotNull()))
                .MustHaveHappened(count, Times.Exactly);
            Assert.IsInstanceOf<IEnumerable<TeamViewModel>>(result);
            Assert.AreEqual(count, result.Count());
        }

        [TestCase]
        public async Task GetWeeksAsync_HappyPath()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            var dbContext = A.Fake<ProFootballEntities>();

            var games = SetUpFakeGames();
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Returns(games);

            // Act
            var result = await service.GetWeeksAsync(seasonID, dbContext);

            // Assert
            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
            Assert.IsInstanceOf<IEnumerable<WeekViewModel>>(result);

            // Verify correct order (weekID ascending) and that all elements are distinct.
            var resultToList = result.ToList();
            for (int i = 0; i < resultToList.Count; i++)
            {
                if (i == 0)
                {
                    Assert.AreEqual(string.Empty, resultToList.ElementAt(i).ID);
                }
                else if (i < resultToList.Count - 1)
                {
                    Assert.Greater(Convert.ToInt32(resultToList.ElementAt(i + 1).ID),
                        Convert.ToInt32(resultToList.ElementAt(i).ID));
                }
            }
        }

        [TestCase]
        public async Task GetWeeksAsync_ArgumentNullException_AbortsAndReturnsNull()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasonID = 2017;
            var dbContext = A.Fake<ProFootballEntities>();

            A.CallTo(() => _gameRepository.GetEntitiesAsync(dbContext)).Throws<ArgumentNullException>();

            // Act
            var result = await service.GetWeeksAsync(seasonID, dbContext);

            // Assert
            Assert.IsNull(result);
        }

        private IEnumerable<Game> SetUpFakeGames()
        {
            var games = new List<Game>
            {
                new Game
                {
                    SeasonID = 2017,
                    Week = 1,
                    GuestName = "Team",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2017,
                    Week = 1,
                    GuestName = "Guest",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2017,
                    Week = 1,
                    GuestName = "Team",
                    HostName = "Host"
                },
                new Game
                {
                    SeasonID = 2017,
                    Week = 2,
                    GuestName = "Team",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2017,
                    Week = 2,
                    GuestName = "Guest",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2017,
                    Week = 2,
                    GuestName = "Team",
                    HostName = "Host"
                },
                new Game
                {
                    SeasonID = 2017,
                    Week = 3,
                    GuestName = "Team",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2017,
                    Week = 3,
                    GuestName = "Guest",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2017,
                    Week = 3,
                    GuestName = "Team",
                    HostName = "Host"
                },
                new Game
                {
                    SeasonID = 2016,
                    Week = 1,
                    GuestName = "Team",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2016,
                    Week = 1,
                    GuestName = "Guest",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2016,
                    Week = 1,
                    GuestName = "Team",
                    HostName = "Host"
                },
                new Game
                {
                    SeasonID = 2016,
                    Week = 2,
                    GuestName = "Team",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2016,
                    Week = 2,
                    GuestName = "Guest",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2016,
                    Week = 2,
                    GuestName = "Team",
                    HostName = "Host"
                },
                new Game
                {
                    SeasonID = 2016,
                    Week = 3,
                    GuestName = "Team",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2016,
                    Week = 3,
                    GuestName = "Guest",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2016,
                    Week = 3,
                    GuestName = "Team",
                    HostName = "Host"
                },
                new Game
                {
                    SeasonID = 2014,
                    Week = 1,
                    GuestName = "Team",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2014,
                    Week = 1,
                    GuestName = "Guest",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2014,
                    Week = 1,
                    GuestName = "Team",
                    HostName = "Host"
                },
                new Game
                {
                    SeasonID = 2014,
                    Week = 2,
                    GuestName = "Team",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2014,
                    Week = 2,
                    GuestName = "Guest",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2014,
                    Week = 2,
                    GuestName = "Team",
                    HostName = "Host"
                },
                new Game
                {
                    SeasonID = 2014,
                    Week = 3,
                    GuestName = "Team",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2014,
                    Week = 3,
                    GuestName = "Guest",
                    HostName = "Team"
                },
                new Game
                {
                    SeasonID = 2014,
                    Week = 3,
                    GuestName = "Team",
                    HostName = "Host"
                }
            };

            return games;
        }

        [TestCase]
        public void SetSelectedSeason()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var seasons = new List<SeasonViewModel>();
            var seasonID = 2017;

            // Act
            service.SetSelectedSeason(seasons, seasonID);

            // Assert
            A.CallTo(() => _sharedService.SetSelectedSeason(seasons, seasonID, ref GamesService.SelectedSeason))
                .MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void SetSelectedWeek()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            var week = 1;

            // Act
            service.SetSelectedWeek(week);

            // Assert
            Assert.IsInstanceOf<WeekViewModel>(GamesService.SelectedWeek);
            Assert.AreEqual(week.ToString(), GamesService.SelectedWeek.ID);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new GamesService(_sharedService, _dataMapper, _gameRepository, _teamRepository,
                _teamSeasonRepository, _calculator);

            // Act

            // Assert
        }
    }
}
