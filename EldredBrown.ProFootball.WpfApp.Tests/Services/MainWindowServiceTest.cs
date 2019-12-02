using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.Windows;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.WpfApp.Tests.Services
{
    [TestFixture]
    public class MainWindowServiceTest
    {
        #region Member Fields

        private ISharedService _sharedService;
        private IRepository<Game> _gameRepository;
        private IRepository<Season> _seasonRepository;
        private IRepository<LeagueSeason> _leagueSeasonRepository;
        private IRepository<TeamSeason> _teamSeasonRepository;
        private IRepository<WeekCount> _weekCountRepository;
        private IStoredProcedureRepository _storedProcedureRepository;
        private ProFootballEntities _dbContext;
        private ICalculator _calculator;
        private IGamePredictorWindow _gamePredictorWindow;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _gameRepository = A.Fake<IRepository<Game>>();
            _seasonRepository = A.Fake<IRepository<Season>>();
            _leagueSeasonRepository = A.Fake<IRepository<LeagueSeason>>();
            _teamSeasonRepository = A.Fake<IRepository<TeamSeason>>();
            _weekCountRepository = A.Fake<IRepository<WeekCount>>();
            _storedProcedureRepository = A.Fake<IStoredProcedureRepository>();
            _dbContext = A.Fake<ProFootballEntities>();
            _calculator = A.Fake<ICalculator>();
            _gamePredictorWindow = A.Fake<IGamePredictorWindow>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void GetAllSeasonIds_HappyPath()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            var seasonCount = 3;
            var seasons = new List<Season>(seasonCount);
            for (int i = 1; i <= seasonCount; i++)
            {
                var season = new Season {ID = i};
                seasons.Add(season);
            }
            A.CallTo(() => _seasonRepository.GetEntities()).Returns(seasons);

            // Act
            var result = service.GetAllSeasonIds();

            // Assert
            Assert.IsInstanceOf<IEnumerable<int>>(result);

            // Enumerate the enumerable.
            var resultToList = result.ToList();

            A.CallTo(() => _seasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            // Verify correct sort order (descending).
            for (int i = 1; i < resultToList.Count; i++)
            {
                Assert.Less(resultToList[i], resultToList[i - 1]);
            }
        }

        [TestCase]
        public void GetAllSeasonIds_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _seasonRepository.GetEntities()).Throws(ex);

            // Act
            var result = service.GetAllSeasonIds();

            // Assert
            Assert.IsInstanceOf<IEnumerable<int>>(result);

            // Enumerate the enumerable.
            var resultToList = result.ToList();

            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsEmpty(resultToList);
        }

        [TestCase]
        public void GetAllSeasonIds_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _seasonRepository.GetEntities()).Throws<Exception>();

            // Act
            IEnumerable<int> result = null;
            Assert.Throws<Exception>(() => result = service.GetAllSeasonIds().ToList());

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetGameCount_HappyPath()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Define argument variables of method under test.
            var seasonID = 1920;

            // Set up needed infrastructure of class under test.
            var seasonCount = 3;
            var gameCountPerSeason = 3;
            var games = new List<Game>(seasonCount * gameCountPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                for (int j = 0; j < gameCountPerSeason; j++)
                {
                    var game = new Game
                    {
                        ID = j,
                        SeasonID = 1922 - i
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            // Act
            var result = service.GetGameCount(seasonID);

            // Assert
            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            Assert.AreEqual(gameCountPerSeason, result);
        }

        [TestCase]
        public void GetGameCount_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsZero()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Define argument variables of method under test.
            var seasonID = 1920;

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _gameRepository.GetEntities()).Throws(ex);

            // Act
            var result = service.GetGameCount(seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(0, result);
        }

        [TestCase]
        public void GetGameCount_OverflowExceptionCaught_ShowsExceptionMessageAndReturnsZero()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Define argument variables of method under test.
            var seasonID = 1920;

            // Set up needed infrastructure of class under test.
            var ex = new OverflowException();
            A.CallTo(() => _gameRepository.GetEntities()).Throws(ex);

            // Act
            var result = service.GetGameCount(seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "OverflowException")).MustHaveHappenedOnceExactly();

            Assert.AreEqual(0, result);
        }

        [TestCase]
        public void GetGameCount_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Define argument variables of method under test.
            var seasonID = 1920;

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _gameRepository.GetEntities()).Throws<Exception>();

            // Act
            int result = 0;
            Assert.Throws<Exception>(() => result = service.GetGameCount(seasonID));

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestCase]
        public void PredictGameScore_HappyPath()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Define argument variables of method under test.
            // Act
            service.PredictGameScore();

            // Assert
            A.CallTo(() => _gamePredictorWindow.Show()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void PredictGameScore_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Define argument variables of method under test.
            A.CallTo(() => _gamePredictorWindow.Show()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => service.PredictGameScore());
        }

        [TestCase]
        public void RunWeeklyUpdate_UserAnswersNoToFirstQuestion_MethodAborts()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.No);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).MustNotHaveHappened();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).MustNotHaveHappened();

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustNotHaveHappened();
            A.CallTo(() => _weekCountRepository.GetEntities()).MustNotHaveHappened();

            #endregion UpdateWeekCount

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustNotHaveHappened();

            #endregion SaveChanges

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustNotHaveHappened();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustNotHaveHappened();
        }

        [TestCase]
        public void RunWeeklyUpdate_UserAnswersYesToFirstQuestionAndWeekCountLessThan3_MethodCompletesWithoutUpdatingRankings()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 2;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            //var leagueSeasonTotalsRow = leagueSeasonTotals.FirstOrDefault();
            //Assert.AreEqual(leagueSeasonTotalsRow.TotalGames, leagueSeason.TotalGames);
            //Assert.AreEqual(leagueSeasonTotalsRow.TotalPoints, leagueSeason.TotalPoints);

            //A.CallTo(() => _calculator.Divide((double)leagueSeasonTotalsRow.TotalPoints,
            //    (double)leagueSeasonTotalsRow.TotalGames)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();

            #endregion SaveChanges

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustNotHaveHappened();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_UserAnswersYesToFirstQuestionAndWeekCountEquals3AndTeamSeasonScheduleTotalsIsNull_MethodUpdatesRankingsButUpdateRankingsByTeamSeasonAborts()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            var teamCount = 3;
            var teamSeasons = new List<TeamSeason>(teamCount * seasonCount);
            var teamSeasonScheduleTotalsEnumerable =
                new List<GetTeamSeasonScheduleTotals_Result>(teamCount * seasonCount);
            var teamSeasonScheduleAveragesEnumerable =
                new List<GetTeamSeasonScheduleAverages_Result>(teamCount * seasonCount);

            double offensiveAverage = 28;
            double defensiveAverage = 21;

            for (int i = 1; i <= teamCount; i++)
            {
                for (int j = 0; j < seasonCount; j++)
                {
                    var ts = new TeamSeason
                    {
                        TeamName = $"Team {i}",
                        SeasonID = selectedSeason - j,
                        Games = 16,
                        PointsFor = 448,
                        PointsAgainst = 336,
                        OffensiveAverage = 0,
                        DefensiveAverage = 0
                    };
                    teamSeasons.Add(ts);

                    var teamSeasonScheduleAveragesItem = new GetTeamSeasonScheduleAverages_Result
                    {
                        PointsFor = offensiveAverage,
                        PointsAgainst = defensiveAverage
                    };
                    teamSeasonScheduleAveragesEnumerable.Add(teamSeasonScheduleAveragesItem);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Returns(teamSeasons);

            _dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            _dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);

            var teamName = "Team 2";
            var teamSeason = teamSeasons.Find(ts => ts.TeamName == teamName && ts.SeasonID == selectedSeason);

            var teamSeasonScheduleTotals = _dbContext.GetTeamSeasonScheduleTotals(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleTotals);

            var teamSeasonScheduleAverages = _dbContext.GetTeamSeasonScheduleAverages(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleAverages);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games))
                .Returns(offensiveAverage);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games))
                .Returns(defensiveAverage);

            double offensiveFactor = 1.333;
            A.CallTo(() => _calculator.Divide(offensiveAverage, defensiveAverage)).Returns(offensiveFactor);

            double defensiveFactor = 0.75;
            A.CallTo(() => _calculator.Divide(defensiveAverage, offensiveAverage)).Returns(defensiveFactor);

            var teamSeasonFinalPythPct = 1;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason))
                .Returns(teamSeasonFinalPythPct);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games)).MustNotHaveHappened();

            A.CallTo(() => _calculator.Divide((double)teamSeason.OffensiveAverage, defensiveAverage))
                .MustNotHaveHappened();
            A.CallTo(() => _calculator.Divide((double)teamSeason.DefensiveAverage, offensiveAverage))
                .MustNotHaveHappened();

            A.CallTo(() => _leagueSeasonRepository.FindEntity(teamSeason.TeamName, teamSeason.SeasonID))
                .MustNotHaveHappened();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason)).MustNotHaveHappened();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_UserAnswersYesToFirstQuestionAndWeekCountEquals3AndTeamSeasonScheduleAveragesIsNull_MethodUpdatesRankingsButUpdateRankingsByTeamSeasonAborts()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            var teamCount = 3;
            var teamSeasons = new List<TeamSeason>(teamCount * seasonCount);
            var teamSeasonScheduleTotalsEnumerable =
                new List<GetTeamSeasonScheduleTotals_Result>(teamCount * seasonCount);
            var teamSeasonScheduleAveragesEnumerable =
                new List<GetTeamSeasonScheduleAverages_Result>(teamCount * seasonCount);

            double offensiveAverage = 28;
            double defensiveAverage = 21;

            for (int i = 1; i <= teamCount; i++)
            {
                for (int j = 0; j < seasonCount; j++)
                {
                    var ts = new TeamSeason
                    {
                        TeamName = $"Team {i}",
                        SeasonID = selectedSeason - j,
                        Games = 16,
                        PointsFor = 448,
                        PointsAgainst = 336,
                        OffensiveAverage = 0,
                        DefensiveAverage = 0
                    };
                    teamSeasons.Add(ts);

                    var teamSeasonScheduleTotalsItem = new GetTeamSeasonScheduleTotals_Result
                    {
                        ScheduleGames = 0
                    };
                    teamSeasonScheduleTotalsEnumerable.Add(teamSeasonScheduleTotalsItem);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Returns(teamSeasons);

            _dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            _dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);

            var teamName = "Team 2";
            var teamSeason = teamSeasons.Find(ts => ts.TeamName == teamName && ts.SeasonID == selectedSeason);

            var teamSeasonScheduleTotals = _dbContext.GetTeamSeasonScheduleTotals(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleTotals);

            var teamSeasonScheduleAverages = _dbContext.GetTeamSeasonScheduleAverages(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleAverages);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games))
                .Returns(offensiveAverage);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games))
                .Returns(defensiveAverage);

            double offensiveFactor = 1.333;
            A.CallTo(() => _calculator.Divide(offensiveAverage, defensiveAverage)).Returns(offensiveFactor);

            double defensiveFactor = 0.75;
            A.CallTo(() => _calculator.Divide(defensiveAverage, offensiveAverage)).Returns(defensiveFactor);

            var teamSeasonFinalPythPct = 1;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason))
                .Returns(teamSeasonFinalPythPct);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games)).MustNotHaveHappened();

            A.CallTo(() => _calculator.Divide((double)teamSeason.OffensiveAverage, defensiveAverage))
                .MustNotHaveHappened();
            A.CallTo(() => _calculator.Divide((double)teamSeason.DefensiveAverage, offensiveAverage))
                .MustNotHaveHappened();

            A.CallTo(() => _leagueSeasonRepository.FindEntity(teamSeason.TeamName, teamSeason.SeasonID))
                .MustNotHaveHappened();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason)).MustNotHaveHappened();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_UserAnswersYesToFirstQuestionAndWeekCountEquals3AndTeamSeasonScheduleTotalsScheduleGamesIsNull_MethodUpdatesRankingsButUpdateRankingsByTeamSeasonAborts()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            var teamCount = 3;
            var teamSeasons = new List<TeamSeason>(teamCount * seasonCount);
            var teamSeasonScheduleTotalsEnumerable =
                new List<GetTeamSeasonScheduleTotals_Result>(teamCount * seasonCount);
            var teamSeasonScheduleAveragesEnumerable =
                new List<GetTeamSeasonScheduleAverages_Result>(teamCount * seasonCount);

            double offensiveAverage = 28;
            double defensiveAverage = 21;

            for (int i = 1; i <= teamCount; i++)
            {
                for (int j = 0; j < seasonCount; j++)
                {
                    var ts = new TeamSeason
                    {
                        TeamName = $"Team {i}",
                        SeasonID = selectedSeason - j,
                        Games = 16,
                        PointsFor = 448,
                        PointsAgainst = 336,
                        OffensiveAverage = 0,
                        DefensiveAverage = 0
                    };
                    teamSeasons.Add(ts);

                    var teamSeasonScheduleTotalsItem = new GetTeamSeasonScheduleTotals_Result();
                    teamSeasonScheduleTotalsEnumerable.Add(teamSeasonScheduleTotalsItem);

                    var teamSeasonScheduleAveragesItem = new GetTeamSeasonScheduleAverages_Result
                    {
                        PointsFor = offensiveAverage,
                        PointsAgainst = defensiveAverage
                    };
                    teamSeasonScheduleAveragesEnumerable.Add(teamSeasonScheduleAveragesItem);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Returns(teamSeasons);

            _dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            _dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);

            var teamName = "Team 2";
            var teamSeason = teamSeasons.Find(ts => ts.TeamName == teamName && ts.SeasonID == selectedSeason);

            var teamSeasonScheduleTotals = _dbContext.GetTeamSeasonScheduleTotals(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleTotals);

            var teamSeasonScheduleAverages = _dbContext.GetTeamSeasonScheduleAverages(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleAverages);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games))
                .Returns(offensiveAverage);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games))
                .Returns(defensiveAverage);

            double offensiveFactor = 1.333;
            A.CallTo(() => _calculator.Divide(offensiveAverage, defensiveAverage)).Returns(offensiveFactor);

            double defensiveFactor = 0.75;
            A.CallTo(() => _calculator.Divide(defensiveAverage, offensiveAverage)).Returns(defensiveFactor);

            var teamSeasonFinalPythPct = 1;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason))
                .Returns(teamSeasonFinalPythPct);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games)).MustNotHaveHappened();
            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games)).MustNotHaveHappened();

            A.CallTo(() => _calculator.Divide((double)teamSeason.OffensiveAverage, defensiveAverage))
                .MustNotHaveHappened();
            A.CallTo(() => _calculator.Divide((double)teamSeason.DefensiveAverage, offensiveAverage))
                .MustNotHaveHappened();

            A.CallTo(() => _leagueSeasonRepository.FindEntity(teamSeason.TeamName, teamSeason.SeasonID))
                .MustNotHaveHappened();

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason)).MustNotHaveHappened();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_UserAnswersYesToFirstQuestionAndWeekCountEquals3AndHappyPath()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            var teamCount = 3;
            var teamSeasons = new List<TeamSeason>(teamCount * seasonCount);
            var teamSeasonScheduleTotalsEnumerable =
                new List<GetTeamSeasonScheduleTotals_Result>(teamCount * seasonCount);
            var teamSeasonScheduleAveragesEnumerable =
                new List<GetTeamSeasonScheduleAverages_Result>(teamCount * seasonCount);

            double offensiveAverage = 28;
            double defensiveAverage = 21;

            for (int i = 1; i <= teamCount; i++)
            {
                for (int j = 0; j < seasonCount; j++)
                {
                    var ts = new TeamSeason
                    {
                        TeamName = $"Team {i}",
                        SeasonID = selectedSeason - j,
                        Games = 16,
                        PointsFor = 448,
                        PointsAgainst = 336,
                        OffensiveAverage = 0,
                        DefensiveAverage = 0
                    };
                    teamSeasons.Add(ts);

                    var teamSeasonScheduleTotalsItem = new GetTeamSeasonScheduleTotals_Result
                    {
                        ScheduleGames = 0
                    };
                    teamSeasonScheduleTotalsEnumerable.Add(teamSeasonScheduleTotalsItem);

                    var teamSeasonScheduleAveragesItem = new GetTeamSeasonScheduleAverages_Result
                    {
                        PointsFor = offensiveAverage,
                        PointsAgainst = defensiveAverage
                    };
                    teamSeasonScheduleAveragesEnumerable.Add(teamSeasonScheduleAveragesItem);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Returns(teamSeasons);

            _dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            _dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);

            var teamName = "Team 2";
            var teamSeason = teamSeasons.Find(ts => ts.TeamName == teamName && ts.SeasonID == selectedSeason);

            var teamSeasonScheduleTotals = _dbContext.GetTeamSeasonScheduleTotals(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleTotals);

            var teamSeasonScheduleAverages = _dbContext.GetTeamSeasonScheduleAverages(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleAverages);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games))
                .Returns(offensiveAverage);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games))
                .Returns(defensiveAverage);

            double offensiveFactor = 1.333;
            A.CallTo(() => _calculator.Divide(offensiveAverage, defensiveAverage)).Returns(offensiveFactor);

            double defensiveFactor = 0.75;
            A.CallTo(() => _calculator.Divide(defensiveAverage, offensiveAverage)).Returns(defensiveFactor);

            var teamSeasonFinalPythPct = 1;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason))
                .Returns(teamSeasonFinalPythPct);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            var leagueSeasonTotalsRow = leagueSeasonTotalsEnumerable.FirstOrDefault();
            A.CallTo(() => _calculator.Divide((double)leagueSeasonTotalsRow.TotalPoints,
                (double)leagueSeasonTotalsRow.TotalGames)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(offensiveAverage, teamSeason.OffensiveAverage);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(defensiveAverage, teamSeason.DefensiveAverage);

            A.CallTo(() => _calculator.Divide((double)teamSeason.OffensiveAverage, defensiveAverage))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(offensiveFactor, teamSeason.OffensiveFactor);

            A.CallTo(() => _calculator.Divide((double)teamSeason.DefensiveAverage, offensiveAverage))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(defensiveFactor, teamSeason.DefensiveFactor);

            A.CallTo(() => _leagueSeasonRepository.FindEntity(teamSeason.LeagueName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual((teamSeason.OffensiveAverage + teamSeason.OffensiveFactor * leagueSeason.AveragePoints) / 2,
                teamSeason.OffensiveIndex);
            Assert.AreEqual((teamSeason.DefensiveAverage + teamSeason.DefensiveFactor * leagueSeason.AveragePoints) / 2,
                teamSeason.DefensiveIndex);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(teamSeasonFinalPythPct, teamSeason.FinalPythagoreanWinningPercentage);

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_LeagueSeasonTotalsRowIsNull_MethodUpdatesRankings()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(null);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).MustNotHaveHappened();

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            // Verify that UpdateRankings executed.

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_ArgumentNullExceptionCaughtInUpdateLeagueSeason_ShowsExceptionMessageAndDoesNotUpdateLeagueSeason()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Throws<ArgumentNullException>();

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _sharedService.ShowExceptionMessage(A<ArgumentNullException>.That.IsNotNull()))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).MustNotHaveHappened();

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            #region UpdateRankingsByTeamSeason

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_ArgumentNullExceptionCaughtInUpdateWeekCountFromGameRepositoryGetEntities_ShowsExceptioMessageAndDoesNotUpdateLeagueSeason()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            _dbContext.SetUpFakeLeagueSeasonTotals();
            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Throws<ArgumentNullException>();

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustNotHaveHappened();

            A.CallTo(() => _sharedService.ShowExceptionMessage(A<ArgumentNullException>.That.IsNotNull()))
                .MustHaveHappenedOnceExactly();

            #endregion UpdateWeekCount

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedOnceExactly();

            #endregion SaveChanges

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustNotHaveHappened();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_ArgumentNullExceptionCaughtInUpdateWeekCountFromWeekCountRepositoryGetEntities_ShowsExceptioMessageAndDoesNotUpdateLeagueSeason()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);
            A.CallTo(() => _weekCountRepository.FindEntity(A<int>.Ignored)).Throws<ArgumentNullException>();

            var teamCount = 3;
            var teamSeasons = new List<TeamSeason>(teamCount * seasonCount);
            var teamSeasonScheduleTotalsEnumerable =
                new List<GetTeamSeasonScheduleTotals_Result>(teamCount * seasonCount);
            var teamSeasonScheduleAveragesEnumerable =
                new List<GetTeamSeasonScheduleAverages_Result>(teamCount * seasonCount);

            double offensiveAverage = 28;
            double defensiveAverage = 21;

            for (int i = 1; i <= teamCount; i++)
            {
                for (int j = 0; j < seasonCount; j++)
                {
                    var ts = new TeamSeason
                    {
                        TeamName = $"Team {i}",
                        SeasonID = selectedSeason - j,
                        Games = 16,
                        PointsFor = 448,
                        PointsAgainst = 336,
                        OffensiveAverage = 0,
                        DefensiveAverage = 0
                    };
                    teamSeasons.Add(ts);

                    var teamSeasonScheduleTotalsItem = new GetTeamSeasonScheduleTotals_Result
                    {
                        ScheduleGames = 0
                    };
                    teamSeasonScheduleTotalsEnumerable.Add(teamSeasonScheduleTotalsItem);

                    var teamSeasonScheduleAveragesItem = new GetTeamSeasonScheduleAverages_Result
                    {
                        PointsFor = offensiveAverage,
                        PointsAgainst = defensiveAverage
                    };
                    teamSeasonScheduleAveragesEnumerable.Add(teamSeasonScheduleAveragesItem);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Returns(teamSeasons);

            _dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            _dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);

            var teamName = "Team 2";
            var teamSeason = teamSeasons.Find(ts => ts.TeamName == teamName && ts.SeasonID == selectedSeason);

            var teamSeasonScheduleTotals = _dbContext.GetTeamSeasonScheduleTotals(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleTotals);

            var teamSeasonScheduleAverages = _dbContext.GetTeamSeasonScheduleAverages(teamName, selectedSeason);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .Returns(teamSeasonScheduleAverages);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games))
                .Returns(offensiveAverage);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games))
                .Returns(defensiveAverage);

            double offensiveFactor = 1.333;
            A.CallTo(() => _calculator.Divide(offensiveAverage, defensiveAverage)).Returns(offensiveFactor);

            double defensiveFactor = 0.75;
            A.CallTo(() => _calculator.Divide(defensiveAverage, offensiveAverage)).Returns(defensiveFactor);

            var teamSeasonFinalPythPct = 1;
            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason))
                .Returns(teamSeasonFinalPythPct);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            var leagueSeasonTotalsRow = leagueSeasonTotalsEnumerable.FirstOrDefault();
            A.CallTo(() => _calculator.Divide((double)leagueSeasonTotalsRow.TotalPoints,
                (double)leagueSeasonTotalsRow.TotalGames)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _sharedService.ShowExceptionMessage(A<ArgumentNullException>.That.IsNotNull()))
                .MustHaveHappenedOnceExactly();

            #endregion UpdateWeekCount

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(teamSeason.TeamName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(offensiveAverage, teamSeason.OffensiveAverage);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(defensiveAverage, teamSeason.DefensiveAverage);

            A.CallTo(() => _calculator.Divide((double)teamSeason.OffensiveAverage, defensiveAverage))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(offensiveFactor, teamSeason.OffensiveFactor);

            A.CallTo(() => _calculator.Divide((double)teamSeason.DefensiveAverage, offensiveAverage))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(defensiveFactor, teamSeason.DefensiveFactor);

            A.CallTo(() => _leagueSeasonRepository.FindEntity(teamSeason.LeagueName, teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual((teamSeason.OffensiveAverage + teamSeason.OffensiveFactor * leagueSeason.AveragePoints) / 2,
                teamSeason.OffensiveIndex);
            Assert.AreEqual((teamSeason.DefensiveAverage + teamSeason.DefensiveFactor * leagueSeason.AveragePoints) / 2,
                teamSeason.DefensiveIndex);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(teamSeasonFinalPythPct, teamSeason.FinalPythagoreanWinningPercentage);

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_ArgumentNullExceptionCaughtInUpdateRankings_ShowsExceptionMessageAndUpdatesNoTeamSeasons()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            A.CallTo(() => _teamSeasonRepository.GetEntities()).Throws<ArgumentNullException>();

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            var leagueSeasonTotalsRow = leagueSeasonTotalsEnumerable.FirstOrDefault();
            A.CallTo(() => _calculator.Divide((double)leagueSeasonTotalsRow.TotalPoints,
                (double)leagueSeasonTotalsRow.TotalGames)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == WpfGlobals.SelectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == WpfGlobals.SelectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            #region UpdateRankingsByTeamSeason

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_InvalidOperationExceptionCaughtInUpdateRankingsByTeamSeasonAndMessageIsNullableObjectMustHaveAValue_ExceptionIgnored()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            var teamCount = 3;
            var teamSeasons = new List<TeamSeason>(teamCount * seasonCount);
            var teamSeasonScheduleTotalsEnumerable =
                new List<GetTeamSeasonScheduleTotals_Result>(teamCount * seasonCount);
            var teamSeasonScheduleAveragesEnumerable =
                new List<GetTeamSeasonScheduleAverages_Result>(teamCount * seasonCount);

            double offensiveAverage = 28;
            double defensiveAverage = 21;

            for (int i = 1; i <= teamCount; i++)
            {
                for (int j = 0; j < seasonCount; j++)
                {
                    var ts = new TeamSeason
                    {
                        TeamName = $"Team {i}",
                        SeasonID = selectedSeason - j,
                        Games = 16,
                        PointsFor = 448,
                        PointsAgainst = 336,
                        OffensiveAverage = 0,
                        DefensiveAverage = 0
                    };
                    teamSeasons.Add(ts);

                    var teamSeasonScheduleTotalsItem = new GetTeamSeasonScheduleTotals_Result
                    {
                        ScheduleGames = 0
                    };
                    teamSeasonScheduleTotalsEnumerable.Add(teamSeasonScheduleTotalsItem);

                    var teamSeasonScheduleAveragesItem = new GetTeamSeasonScheduleAverages_Result
                    {
                        PointsFor = offensiveAverage,
                        PointsAgainst = defensiveAverage
                    };
                    teamSeasonScheduleAveragesEnumerable.Add(teamSeasonScheduleAveragesItem);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Returns(teamSeasons);

            _dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            _dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);

            var teamName = "Team 2";
            var teamSeason = teamSeasons.Find(ts => ts.TeamName == teamName && ts.SeasonID == selectedSeason);

            var ex = new InvalidOperationException("Nullable object must have a value.");
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .Throws(ex);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            var leagueSeasonTotalsRow = leagueSeasonTotalsEnumerable.FirstOrDefault();
            A.CallTo(() => _calculator.Divide((double)leagueSeasonTotalsRow.TotalPoints,
                (double)leagueSeasonTotalsRow.TotalGames)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() => _sharedService.ShowExceptionMessage(A<Exception>.Ignored, A<string>.Ignored))
                .MustNotHaveHappened();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_InvalidOperationExceptionCaughtInUpdateRankingsByTeamSeasonAndMessageIsNotNullableObjectMustHaveAValue_ShowsExceptionMessage()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            var teamCount = 3;
            var teamSeasons = new List<TeamSeason>(teamCount * seasonCount);
            var teamSeasonScheduleTotalsEnumerable =
                new List<GetTeamSeasonScheduleTotals_Result>(teamCount * seasonCount);
            var teamSeasonScheduleAveragesEnumerable =
                new List<GetTeamSeasonScheduleAverages_Result>(teamCount * seasonCount);

            double offensiveAverage = 28;
            double defensiveAverage = 21;

            for (int i = 1; i <= teamCount; i++)
            {
                for (int j = 0; j < seasonCount; j++)
                {
                    var ts = new TeamSeason
                    {
                        TeamName = $"Team {i}",
                        SeasonID = selectedSeason - j,
                        Games = 16,
                        PointsFor = 448,
                        PointsAgainst = 336,
                        OffensiveAverage = 0,
                        DefensiveAverage = 0
                    };
                    teamSeasons.Add(ts);

                    var teamSeasonScheduleTotalsItem = new GetTeamSeasonScheduleTotals_Result
                    {
                        ScheduleGames = 0
                    };
                    teamSeasonScheduleTotalsEnumerable.Add(teamSeasonScheduleTotalsItem);

                    var teamSeasonScheduleAveragesItem = new GetTeamSeasonScheduleAverages_Result
                    {
                        PointsFor = offensiveAverage,
                        PointsAgainst = defensiveAverage
                    };
                    teamSeasonScheduleAveragesEnumerable.Add(teamSeasonScheduleAveragesItem);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Returns(teamSeasons);

            _dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            _dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);

            var teamName = "Team 2";
            var teamSeason = teamSeasons.Find(ts => ts.TeamName == teamName && ts.SeasonID == selectedSeason);

            var ex = new InvalidOperationException("Message");
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .Throws(ex);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            var leagueSeasonTotalsRow = leagueSeasonTotalsEnumerable.FirstOrDefault();
            A.CallTo(() => _calculator.Divide((double)leagueSeasonTotalsRow.TotalPoints,
                (double)leagueSeasonTotalsRow.TotalGames)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, $"InvalidOperationException: {teamSeason.TeamName}"))
                .MustHaveHappenedOnceExactly();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_OtherExceptionCaughtInUpdateRankingsByTeamSeason_ShowsExceptionMessage()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var selectedSeason = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = selectedSeason,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, selectedSeason);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCounts = new List<WeekCount>(seasonCount);

            var weeksPerSeason = 3;
            var games = new List<Game>(seasonCount * weeksPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                var seasonID = 1922 - i;
                var weekCount = new WeekCount
                {
                    SeasonID = seasonID,
                    Count = 0
                };
                weekCounts.Add(weekCount);
                A.CallTo(() => _weekCountRepository.FindEntity(seasonID)).Returns(weekCount);

                for (int j = 1; j <= weeksPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = seasonID,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            var teamCount = 3;
            var teamSeasons = new List<TeamSeason>(teamCount * seasonCount);
            var teamSeasonScheduleTotalsEnumerable =
                new List<GetTeamSeasonScheduleTotals_Result>(teamCount * seasonCount);
            var teamSeasonScheduleAveragesEnumerable =
                new List<GetTeamSeasonScheduleAverages_Result>(teamCount * seasonCount);

            double offensiveAverage = 28;
            double defensiveAverage = 21;

            for (int i = 1; i <= teamCount; i++)
            {
                for (int j = 0; j < seasonCount; j++)
                {
                    var ts = new TeamSeason
                    {
                        TeamName = $"Team {i}",
                        SeasonID = selectedSeason - j,
                        Games = 16,
                        PointsFor = 448,
                        PointsAgainst = 336,
                        OffensiveAverage = 0,
                        DefensiveAverage = 0
                    };
                    teamSeasons.Add(ts);

                    var teamSeasonScheduleTotalsItem = new GetTeamSeasonScheduleTotals_Result
                    {
                        ScheduleGames = 0
                    };
                    teamSeasonScheduleTotalsEnumerable.Add(teamSeasonScheduleTotalsItem);

                    var teamSeasonScheduleAveragesItem = new GetTeamSeasonScheduleAverages_Result
                    {
                        PointsFor = offensiveAverage,
                        PointsAgainst = defensiveAverage
                    };
                    teamSeasonScheduleAveragesEnumerable.Add(teamSeasonScheduleAveragesItem);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntities()).Returns(teamSeasons);

            _dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            _dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);

            var teamName = "Team 2";
            var teamSeason = teamSeasons.Find(ts => ts.TeamName == teamName && ts.SeasonID == selectedSeason);

            var ex = new Exception("Message");
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(teamSeason.TeamName, teamSeason.SeasonID))
                .Throws(ex);

            // Act
            service.RunWeeklyUpdate();

            // Assert
            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .MustHaveHappenedOnceExactly();

            #region UpdateLeagueSeason

            A.CallTo(() => _leagueSeasonRepository.FindEntity("APFA", selectedSeason)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals("APFA", selectedSeason))
                .MustHaveHappenedOnceExactly();

            var leagueSeasonTotalsRow = leagueSeasonTotalsEnumerable.FirstOrDefault();
            A.CallTo(() => _calculator.Divide((double)leagueSeasonTotalsRow.TotalPoints,
                (double)leagueSeasonTotalsRow.TotalGames)).MustHaveHappenedOnceExactly();

            Assert.AreEqual(leagueSeasonAveragePoints, leagueSeason.AveragePoints);

            #endregion UpdateLeagueSeason

            #region UpdateWeekCount

            A.CallTo(() => _gameRepository.GetEntities()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _weekCountRepository.FindEntity(selectedSeason)).MustHaveHappenedOnceExactly();
            var srcWeekCount = games.Where(g => g.SeasonID == selectedSeason).Select(g => g.Week).Max();
            var destWeekCount = weekCounts.FirstOrDefault(wc => wc.SeasonID == selectedSeason);
            Assert.AreEqual(destWeekCount.Count, srcWeekCount);

            #endregion UpdateWeekCount

            #region UpdateRankings

            A.CallTo(() => _teamSeasonRepository.GetEntities()).MustHaveHappenedOnceExactly();

            #region UpdateRankingsByTeamSeason

            A.CallTo(() => _sharedService.ShowExceptionMessage(ex.InnerException, $"Exception: {teamSeason.TeamName}"))
                .MustHaveHappenedOnceExactly();

            #endregion UpdateRankingsByTeamSeason

            #endregion UpdateRankings

            #region SaveChanges

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).MustHaveHappenedTwiceExactly();

            #endregion SaveChanges

            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void RunWeeklyUpdate_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Set up needed infrastructure of class under test.
            WpfGlobals.SelectedSeason = 1920;

            A.CallTo(() =>
                    _sharedService.ShowMessageBox(
                        "This operation may make the UI unresponsive for a minute or two. Are you sure you want to continue?",
                        WpfGlobals.Constants.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question))
                .Returns(MessageBoxResult.Yes);

            var leagueName = "League";
            var seasonID = (int)WpfGlobals.SelectedSeason;
            var leagueSeason = new LeagueSeason
            {
                LeagueName = leagueName,
                SeasonID = seasonID,
                AveragePoints = 24.5
            };
            A.CallTo(() => _leagueSeasonRepository.FindEntity(A<string>.Ignored, A<int>.Ignored)).Returns(leagueSeason);

            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = 20.00
                }
            };
            _dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueSeasonTotals = _dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            A.CallTo(() => _storedProcedureRepository.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotals);

            var leagueSeasonAveragePoints = 20;
            A.CallTo(() => _calculator.Divide(A<double>.Ignored, A<double>.Ignored)).Returns(leagueSeasonAveragePoints);

            var seasonCount = 3;
            var weekCountPerSeason = 3;
            var games = new List<Game>(seasonCount * weekCountPerSeason);
            for (int i = 0; i < seasonCount; i++)
            {
                for (int j = 1; j <= weekCountPerSeason; j++)
                {
                    var game = new Game
                    {
                        SeasonID = 1922 - i,
                        Week = j
                    };
                    games.Add(game);
                }
            }
            A.CallTo(() => _gameRepository.GetEntities()).Returns(games);

            var weekCounts = new List<WeekCount>(weekCountPerSeason);
            for (int i = 0; i < weekCountPerSeason; i++)
            {
                var weekCount = new WeekCount { Count = i };
                weekCounts.Add(weekCount);
            }
            A.CallTo(() => _weekCountRepository.GetEntities()).Returns(weekCounts);

            A.CallTo(() => _sharedService.SaveChanges(_dbContext)).Throws<Exception>();

            // Act
            Assert.Throws<Exception>(() => service.RunWeeklyUpdate());

            // Assert
            A.CallTo(() => _sharedService.ShowMessageBox("Weekly update completed.", WpfGlobals.Constants.AppName,
                MessageBoxButton.OK, MessageBoxImage.Information)).MustNotHaveHappened();
        }

        [TestCase]
        public void ShowGames_HappyPath()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Define argument variables of method under test.
            var gamesWindow = A.Fake<IGamesWindow>();

            // Act
            service.ShowGames(gamesWindow);

            // Assert
            A.CallTo(() => gamesWindow.ShowDialog()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void ShowGames_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // Define argument variables of method under test.
            var gamesWindow = A.Fake<IGamesWindow>();
            A.CallTo(() => gamesWindow.ShowDialog()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => service.ShowGames(gamesWindow));
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new MainWindowService(_sharedService, _gameRepository, _seasonRepository,
                _leagueSeasonRepository, _teamSeasonRepository, _weekCountRepository, _storedProcedureRepository,
                _dbContext, _calculator, _gamePredictorWindow);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }

        #endregion Test Cases
    }
}
