using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EldredBrown.ProFootball.Shared;
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
    public class TeamSeasonsServiceTest
    {
        private ISharedService _sharedService;
        private IDataMapper _dataMapper;
        private IRepository<LeagueSeason> _leagueSeasonRepository;
        private IRepository<TeamSeason> _teamSeasonRepository;
        private IStoredProcedureRepository _storedProcedureRepository;
        private ICalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _dataMapper = A.Fake<IDataMapper>();
            _leagueSeasonRepository = A.Fake<IRepository<LeagueSeason>>();
            _teamSeasonRepository = A.Fake<IRepository<TeamSeason>>();
            _storedProcedureRepository = A.Fake<IStoredProcedureRepository>();
            _calculator = A.Fake<ICalculator>();
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsTeamAsc_OrderByTeamNameAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "team_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);
            
            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.TeamName, currItem.TeamName);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsTeamDesc_OrderByTeamNameDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "team_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.TeamName, currItem.TeamName);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsWinsAsc_OrderByWinsAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "wins_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        Wins = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        Wins = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.Wins, currItem.Wins);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsWinsDesc_OrderByWinsDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "wins_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        Wins = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        Wins = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.Wins, currItem.Wins);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsLossesAsc_OrderByLossesAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "losses_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        Losses = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        Losses = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.Losses, currItem.Losses);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsLossesDesc_OrderByLossesDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "losses_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        Losses = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        Losses = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.Losses, currItem.Losses);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsTiesAsc_OrderByTiesAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "ties_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        Ties = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        Ties = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.Ties, currItem.Ties);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsTiesDesc_OrderByTiesDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "ties_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        Ties = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        Ties = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.Ties, currItem.Ties);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsWinPctAsc_OrderByWinPctAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "win_pct_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;
                    var winPct = (double)j / 3;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        WinningPercentage = winPct
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        WinningPercentage = winPct
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.WinningPercentage, currItem.WinningPercentage);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsWinPctDesc_OrderByWinPctDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "win_pct_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;
                    var winPct = (double)j / 3;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        WinningPercentage = winPct
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        WinningPercentage = winPct
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.WinningPercentage, currItem.WinningPercentage);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsPointsForAsc_OrderByPointsForAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "pf_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        PointsFor = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        PointsFor = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.PointsFor, currItem.PointsFor);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsPointsForDesc_OrderByPointsForDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "pf_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        PointsFor = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        PointsFor = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.PointsFor, currItem.PointsFor);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsPointsAgainstAsc_OrderByPointsAgainstAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "pa_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        PointsAgainst = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        PointsAgainst = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.PointsAgainst, currItem.PointsAgainst);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsPointsAgainstDesc_OrderByPointsAgainstDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "pa_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        PointsAgainst = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        PointsAgainst = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.PointsAgainst, currItem.PointsAgainst);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsPythagoreanWinsAsc_OrderByPythagoreanWinsAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "pyth_wins_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        PythagoreanWins = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        PythagoreanWins = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.PythagoreanWins, currItem.PythagoreanWins);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsPythagoreanWinsDesc_OrderByPythagoreanWinsDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "pyth_wins_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        PythagoreanWins = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        PythagoreanWins = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.PythagoreanWins, currItem.PythagoreanWins);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsPythagoreanLossesAsc_OrderByPythagoreanLossesAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "pyth_losses_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        PythagoreanLosses = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        PythagoreanLosses = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.PythagoreanLosses, currItem.PythagoreanLosses);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsPythagoreanLossesDesc_OrderByPythagoreanLossesDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "pyth_losses_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        PythagoreanLosses = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        PythagoreanLosses = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.PythagoreanLosses, currItem.PythagoreanLosses);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsOffensiveAverageAsc_OrderByOffensiveAverageAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "off_avg_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveAverage = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveAverage = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.OffensiveAverage, currItem.OffensiveAverage);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsOffensiveAverageDesc_OrderByOffensiveAverageDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "off_avg_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveAverage = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveAverage = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.OffensiveAverage, currItem.OffensiveAverage);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsOffensiveFactorAsc_OrderByOffensiveFactorAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "off_factor_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveFactor = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveFactor = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.OffensiveFactor, currItem.OffensiveFactor);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsOffensiveFactorDesc_OrderByOffensiveFactorDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "off_factor_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveFactor = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveFactor = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.OffensiveFactor, currItem.OffensiveFactor);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsOffensiveIndexAsc_OrderByOffensiveIndexAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "off_index_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveIndex = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveIndex = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.OffensiveIndex, currItem.OffensiveIndex);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsOffensiveIndexDesc_OrderByOffensiveIndexDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "off_index_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveIndex = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        OffensiveIndex = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.OffensiveIndex, currItem.OffensiveIndex);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsDefensiveAverageAsc_OrderByDefensiveAverageAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "def_avg_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveAverage = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveAverage = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.DefensiveAverage, currItem.DefensiveAverage);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsDefensiveAverageDesc_OrderByDefensiveAverageDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "def_avg_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveAverage = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveAverage = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.DefensiveAverage, currItem.DefensiveAverage);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsDefensiveFactorAsc_OrderByDefensiveFactorAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "def_factor_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveFactor = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveFactor = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.DefensiveFactor, currItem.DefensiveFactor);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsDefensiveFactorDesc_OrderByDefensiveFactorDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "def_factor_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveFactor = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveFactor = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.DefensiveFactor, currItem.DefensiveFactor);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsDefensiveIndexAsc_OrderByDefensiveIndexAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "def_index_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveIndex = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveIndex = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.DefensiveIndex, currItem.DefensiveIndex);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsDefensiveIndexDesc_OrderByDefensiveIndexDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "def_index_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveIndex = j
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        DefensiveIndex = j
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.DefensiveIndex, currItem.DefensiveIndex);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsFinalPythWinPctAsc_OrderByFinalPythWinPctAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "fin_pyth_pct_asc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;
                    var winPct = (double)j / 3;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        FinalPythagoreanWinningPercentage = winPct
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        FinalPythagoreanWinningPercentage = winPct
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.FinalPythagoreanWinningPercentage,
                        currItem.FinalPythagoreanWinningPercentage);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderIsFinalPythWinPctDesc_OrderByFinalPythWinPctDescending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = "fin_pyth_pct_desc";
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = 0; j < teamCountPerSeason; j++)
                {
                    var name = "Team" + j;
                    var winPct = (double)j / 3;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id,
                        FinalPythagoreanWinningPercentage = winPct
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id,
                        FinalPythagoreanWinningPercentage = winPct
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Less(nextItem.FinalPythagoreanWinningPercentage, currItem.FinalPythagoreanWinningPercentage);
                }
            }
        }

        [TestCase]
        public async Task GetEntitiesOrderedAsync_SortOrderNotSelected_OrderByTeamNameAscending()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasonID = 2017;
            var sortOrder = string.Empty;
            var dbContext = A.Fake<ProFootballEntities>();

            var seasonCount = 3;
            var teamCountPerSeason = 3;
            var teamSeasonCount = seasonCount * teamCountPerSeason;

            var teamSeasons = new List<TeamSeason>(teamSeasonCount);
            for (int i = seasonCount; i > 0; i--)
            {
                var id = 2014 + i;

                for (int j = teamCountPerSeason; j > 0; j--)
                {
                    var name = "Team" + j;

                    var teamSeason = new TeamSeason
                    {
                        TeamName = name,
                        SeasonID = id
                    };
                    teamSeasons.Add(teamSeason);

                    var teamSeasonViewModel = new TeamSeasonViewModel
                    {
                        TeamName = name,
                        SeasonID = id
                    };
                    A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(teamSeason)).Returns(teamSeasonViewModel);
                }
            }
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            // Act
            var result = await service.GetEntitiesOrderedAsync(seasonID, sortOrder, dbContext);

            // Assert
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _dataMapper.MapToTeamSeasonViewModel(A<TeamSeason>.That.IsNotNull()))
                .MustHaveHappened(teamCountPerSeason, Times.Exactly);

            // Verify that result is of correct type.
            Assert.IsInstanceOf<IEnumerable<TeamSeasonViewModel>>(result);
            var resultToList = result.ToList();

            // Verify correct TeamSeasonViewModel count in result.
            Assert.AreEqual(teamCountPerSeason, resultToList.Count);

            // Verify TeamSeasonViewModels all from correct season and sorted in correct order.
            for (int i = 0; i < resultToList.Count; i++)
            {
                var currItem = resultToList.ElementAt(i);
                Assert.AreEqual(seasonID, currItem.SeasonID);

                if (i < resultToList.Count - 1)
                {
                    var nextItem = resultToList.ElementAt(i + 1);
                    Assert.Greater(nextItem.TeamName, currItem.TeamName);
                }
            }
        }

        [TestCase]
        public async Task GetTeamSeasonDetailsAsync()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var teamName = "Team";
            var seasonID = 2017;
            var dbContext = A.Fake<ProFootballEntities>();

            var teamSeason = new TeamSeasonViewModel();
            A.CallTo(() => _sharedService.FindEntityAsync(A<string>.Ignored, A<int>.Ignored, dbContext))
                .Returns(teamSeason);

            var count = 3;
            var teamSeasonScheduleProfile = new List<GetTeamSeasonScheduleProfile_Result>(count);
            for (int i = 1; i <= count; i++)
            {
                var teamSeasonScheduleProfileResult = new GetTeamSeasonScheduleProfile_Result
                {
                    Opponent = "Opponent" + i
                };
                teamSeasonScheduleProfile.Add(teamSeasonScheduleProfileResult);
            }
            dbContext.SetUpFakeTeamSeasonScheduleProfile(teamSeasonScheduleProfile);

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleProfile(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleProfile(teamName, seasonID));

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID));

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID));

            // Act
            var result = await service.GetTeamSeasonDetailsAsync(teamName, seasonID, dbContext);

            // Assert
            #region TeamSeason = await _sharedService.FindEntityAsync(teamName, seasonID)

            A.CallTo(() => _sharedService.FindEntityAsync(teamName, seasonID, dbContext))
                .MustHaveHappenedOnceExactly();

            Assert.AreSame(teamSeason, result.TeamSeason);

            #endregion TeamSeason = await _sharedService.FindEntityAsync(teamName, seasonID)

            #region TeamSeasonScheduleProfile = GetTeamSeasonScheduleProfile(teamName, seasonID);

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleProfile(dbContext, teamName, seasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _dataMapper.MapToTeamSeasonScheduleProfileViewModel(A<GetTeamSeasonScheduleProfile_Result>
                        .That.IsNotNull()))
                .MustHaveHappened(count, Times.Exactly);

            Assert.IsInstanceOf<IEnumerable<TeamSeasonScheduleProfileViewModel>>(result.TeamSeasonScheduleProfile);

            #endregion TeamSeasonScheduleProfile = GetTeamSeasonScheduleProfile(teamName, seasonID);
            
            #region TeamSeasonScheduleTotals = GetTeamSeasonScheduleTotals(teamName, seasonID);

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, teamName, seasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _dataMapper.MapToTeamSeasonScheduleTotalsViewModel(
                        A<GetTeamSeasonScheduleTotals_Result>.Ignored))
                .MustHaveHappenedOnceExactly();

            #endregion TeamSeasonScheduleTotals = GetTeamSeasonScheduleTotals(teamName, seasonID);

            #region TeamSeasonScheduleAverages = GetTeamSeasonScheduleAverages(teamName, seasonID);

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() =>
                _dataMapper.MapToTeamSeasonScheduleAveragesViewModel(
                    A<GetTeamSeasonScheduleAverages_Result>.Ignored)).MustHaveHappenedOnceExactly();

            #endregion TeamSeasonScheduleAverages = GetTeamSeasonScheduleAverages(teamName, seasonID);

            Assert.IsInstanceOf<TeamSeasonDetailsViewModel>(result);
            Assert.AreSame(teamSeason, result.TeamSeason);
            Assert.IsInstanceOf<IEnumerable<TeamSeasonScheduleProfileViewModel>>(result.TeamSeasonScheduleProfile);
            Assert.IsInstanceOf<TeamSeasonScheduleTotalsViewModel>(result.TeamSeasonScheduleTotals);
            Assert.IsInstanceOf<TeamSeasonScheduleAveragesViewModel>(result.TeamSeasonScheduleAverages);
        }

        [TestCase]
        public void SetSelectedSeason_SeasonIDNullAndSortOrderNullAndGlobalsSelectedSeasonNull_FirstSeasonFromListSelected()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasons = new List<SeasonViewModel>
            {
                new SeasonViewModel { ID = 2017 },
                new SeasonViewModel { ID = 2016 },
                new SeasonViewModel { ID = 2014 },
            };
            int? seasonID = null;
            string sortOrder = null;

            TeamSeasonsService.SelectedSeason = 0;
            WebGlobals.SelectedSeason = null;

            // Act
            service.SetSelectedSeason(seasons, seasonID, sortOrder);

            // Assert
            Assert.AreEqual(seasons.First().ID, TeamSeasonsService.SelectedSeason);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, WebGlobals.SelectedSeason);
        }

        [TestCase]
        public void SetSelectedSeason_SeasonIDNullAndSortOrderNullAndGlobalsSelectedSeasonNotNull_GlobalsSelectedSeasonSelected()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasons = new List<SeasonViewModel>
            {
                new SeasonViewModel { ID = 2017 },
                new SeasonViewModel { ID = 2016 },
                new SeasonViewModel { ID = 2014 },
            };
            int? seasonID = null;
            string sortOrder = null;

            WebGlobals.SelectedSeason = 2017;

            // Act
            service.SetSelectedSeason(seasons, seasonID, sortOrder);

            // Assert
            Assert.AreEqual((int)WebGlobals.SelectedSeason, TeamSeasonsService.SelectedSeason);
        }

        [TestCase]
        public void SetSelectedSeason_SeasonIDNullAndSortOrderEmptyAndGlobalsSelectedSeasonNotNull_GlobalsSelectedSeasonSelected()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasons = new List<SeasonViewModel>
            {
                new SeasonViewModel { ID = 2017 },
                new SeasonViewModel { ID = 2016 },
                new SeasonViewModel { ID = 2014 },
            };
            int? seasonID = null;
            string sortOrder = string.Empty;

            TeamSeasonsService.SelectedSeason = 0;
            WebGlobals.SelectedSeason = 2017;

            // Act
            service.SetSelectedSeason(seasons, seasonID, sortOrder);

            // Assert
            Assert.AreEqual((int)WebGlobals.SelectedSeason, TeamSeasonsService.SelectedSeason);
        }

        [TestCase]
        public void SetSelectedSeason_SeasonIDNullAndSortOrderNeitherNullNorEmpty_NoSeasonSelected()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasons = new List<SeasonViewModel>
            {
                new SeasonViewModel { ID = 2017 },
                new SeasonViewModel { ID = 2016 },
                new SeasonViewModel { ID = 2014 },
            };
            int? seasonID = null;
            string sortOrder = "team_asc";

            TeamSeasonsService.SelectedSeason = 0;
            WebGlobals.SelectedSeason = 2017;

            // Act
            service.SetSelectedSeason(seasons, seasonID, sortOrder);

            // Assert
            Assert.AreEqual(0, TeamSeasonsService.SelectedSeason);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, WebGlobals.SelectedSeason);
        }

        [TestCase]
        public void SetSelectedSeason_SeasonIDNotNull_SeasonIdSelected()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var seasons = new List<SeasonViewModel>
            {
                new SeasonViewModel { ID = 2017 },
                new SeasonViewModel { ID = 2016 },
                new SeasonViewModel { ID = 2014 },
            };
            int? seasonID = 2017;
            string sortOrder = "team_asc";

            TeamSeasonsService.SelectedSeason = 0;
            WebGlobals.SelectedSeason = 2017;

            // Act
            service.SetSelectedSeason(seasons, seasonID, sortOrder);

            // Assert
            Assert.AreEqual((int)seasonID, TeamSeasonsService.SelectedSeason);
            Assert.AreEqual(TeamSeasonsService.SelectedSeason, WebGlobals.SelectedSeason);
        }

        [TestCase]
        public async Task UpdateRankings_TeamSeasonScheduleTotalsNull_UpdateRankingsByTeamSeasonAborts()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var dbContext = A.Fake<ProFootballEntities>();

            TeamSeasonsService.SelectedSeason = 2017;

            double avgPoints = 20;
            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = avgPoints
                }
            };
            dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueName = "NFL";
            var seasonID = 2017;

            var leagueSeasonTotalsResult = dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            A.CallTo(() =>
                    _storedProcedureRepository.GetLeagueSeasonTotals(dbContext, A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotalsResult);

            var leagueSeasons = new List<LeagueSeason>
            {
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 1,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 2,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                }
            };
            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).Returns(leagueSeasons);

            var teamName = "Team";
            double pointsFor = 21;
            double pointsAgainst = 19;
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    DefensiveAverage = pointsAgainst
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 1,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    DefensiveAverage = pointsAgainst
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 2,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    DefensiveAverage = pointsAgainst
                }
            };
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            var teamSeasonScheduleTotals = new List<GetTeamSeasonScheduleTotals_Result>();
            dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotals);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID));

            var teamSeasonScheduleAverages = new List<GetTeamSeasonScheduleAverages_Result>
            {
                new GetTeamSeasonScheduleAverages_Result
                {
                    PointsFor = 0,
                    PointsAgainst = 0,
                    SchedulePointsFor = 0,
                    SchedulePointsAgainst = 0
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAverages);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID));

            // Act
            await service.UpdateRankings(dbContext);

            // Assert
            A.CallTo(() => _storedProcedureRepository
                    .GetLeagueSeasonTotals(dbContext, leagueName, TeamSeasonsService.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            // Verify correct values assigned to LeagueSeason object.
            var leagueSeasonTotals = leagueSeasonTotalsEnumerable.FirstOrDefault();
            var leagueSeason = leagueSeasons.FirstOrDefault();
            Assert.AreEqual((double)leagueSeasonTotals.TotalGames, leagueSeason.TotalGames);
            Assert.AreEqual((double)leagueSeasonTotals.TotalPoints, leagueSeason.TotalPoints);
            Assert.AreEqual((double)leagueSeasonTotals.AveragePoints, leagueSeason.AveragePoints);

            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            #region await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            var teamSeason = teamSeasons.First();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, teamSeason.TeamName,
                        teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, teamSeason.TeamName,
                        teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            #endregion await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedTwiceExactly();
        }

        [TestCase]
        public async Task UpdateRankings_TeamSeasonScheduleAveragesNull_UpdateRankingsByTeamSeasonAborts()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var dbContext = A.Fake<ProFootballEntities>();

            TeamSeasonsService.SelectedSeason = 2017;

            double avgPoints = 20;
            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = avgPoints
                }
            };
            dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueName = "NFL";
            var seasonID = 2017;

            var leagueSeasonTotalsResult = dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            A.CallTo(() =>
                    _storedProcedureRepository.GetLeagueSeasonTotals(dbContext, A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotalsResult);

            var leagueSeasons = new List<LeagueSeason>
            {
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 1,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 2,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                }
            };
            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).Returns(leagueSeasons);

            var teamName = "Team";
            double pointsFor = 21;
            double pointsAgainst = 19;
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    DefensiveAverage = pointsAgainst
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 1,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    DefensiveAverage = pointsAgainst
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 2,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    DefensiveAverage = pointsAgainst
                }
            };
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            var teamSeasonScheduleTotals = new List<GetTeamSeasonScheduleTotals_Result>
            {
                new GetTeamSeasonScheduleTotals_Result
                {
                    ScheduleGames = 0
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotals);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID));

            var teamSeasonScheduleAverages = new List<GetTeamSeasonScheduleAverages_Result>();
            dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAverages);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID));

            // Act
            await service.UpdateRankings(dbContext);

            // Assert
            A.CallTo(() => _storedProcedureRepository
                    .GetLeagueSeasonTotals(dbContext, leagueName, TeamSeasonsService.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            // Verify correct values assigned to LeagueSeason object.
            var leagueSeasonTotals = leagueSeasonTotalsEnumerable.FirstOrDefault();
            var leagueSeason = leagueSeasons.FirstOrDefault();
            Assert.AreEqual((double)leagueSeasonTotals.TotalGames, leagueSeason.TotalGames);
            Assert.AreEqual((double)leagueSeasonTotals.TotalPoints, leagueSeason.TotalPoints);
            Assert.AreEqual((double)leagueSeasonTotals.AveragePoints, leagueSeason.AveragePoints);

            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            #region await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            var teamSeason = teamSeasons.First();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, teamSeason.TeamName,
                        teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, teamSeason.TeamName,
                        teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            #endregion await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedTwiceExactly();
        }

        [TestCase]
        public async Task UpdateRankings_TeamSeasonScheduleTotalsScheduleGamesNull_UpdateRankingsByTeamSeasonAborts()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var dbContext = A.Fake<ProFootballEntities>();

            TeamSeasonsService.SelectedSeason = 2017;

            double avgPoints = 20;
            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = avgPoints
                }
            };
            dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueName = "NFL";
            var seasonID = 2017;

            var leagueSeasonTotalsResult = dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            A.CallTo(() =>
                    _storedProcedureRepository.GetLeagueSeasonTotals(dbContext, A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotalsResult);

            var leagueSeasons = new List<LeagueSeason>
            {
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 1,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 2,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                }
            };
            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).Returns(leagueSeasons);

            var teamName = "Team";
            double pointsFor = 21;
            double pointsAgainst = 19;
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    DefensiveAverage = pointsAgainst
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 1,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    DefensiveAverage = pointsAgainst
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 2,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    DefensiveAverage = pointsAgainst
                }
            };
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            var teamSeasonScheduleTotals = new List<GetTeamSeasonScheduleTotals_Result>
            {
                new GetTeamSeasonScheduleTotals_Result()
            };
            dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotals);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID));

            var teamSeasonScheduleAverages = new List<GetTeamSeasonScheduleAverages_Result>
            {
                new GetTeamSeasonScheduleAverages_Result
                {
                    PointsFor = 0,
                    PointsAgainst = 0,
                    SchedulePointsFor = 0,
                    SchedulePointsAgainst = 0
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAverages);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID));

            // Act
            await service.UpdateRankings(dbContext);

            // Assert
            A.CallTo(() => _storedProcedureRepository
                    .GetLeagueSeasonTotals(dbContext, leagueName, TeamSeasonsService.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            // Verify correct values assigned to LeagueSeason object.
            var leagueSeasonTotals = leagueSeasonTotalsEnumerable.FirstOrDefault();
            var leagueSeason = leagueSeasons.FirstOrDefault();
            Assert.AreEqual((double)leagueSeasonTotals.TotalGames, leagueSeason.TotalGames);
            Assert.AreEqual((double)leagueSeasonTotals.TotalPoints, leagueSeason.TotalPoints);
            Assert.AreEqual((double)leagueSeasonTotals.AveragePoints, leagueSeason.AveragePoints);

            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            #region await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            var teamSeason = teamSeasons.First();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, teamSeason.TeamName,
                        teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, teamSeason.TeamName,
                        teamSeason.SeasonID))
                .MustHaveHappenedOnceExactly();

            #endregion await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedTwiceExactly();
        }

        [TestCase]
        public async Task UpdateRankings_NoStoredProcedureResultsNull_UpdateRankingsByTeamSeasonCompletes()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var dbContext = A.Fake<ProFootballEntities>();

            TeamSeasonsService.SelectedSeason = 2017;

            double avgPoints = 20;
            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = avgPoints
                }
            };
            dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueName = "NFL";
            var seasonID = 2017;

            var leagueSeasonTotalsResult = dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            A.CallTo(() =>
                    _storedProcedureRepository.GetLeagueSeasonTotals(dbContext, A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotalsResult);

            var leagueSeasons = new List<LeagueSeason>
            {
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 1,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 2,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                }
            };
            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).Returns(leagueSeasons);

            var teamName = "Team";
            double pointsFor = 21;
            double pointsAgainst = 19;
            double factor = 1;
            double pythWP = 0.5;
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID,
                    LeagueName = leagueName,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    OffensiveFactor = factor,
                    DefensiveAverage = pointsAgainst,
                    DefensiveFactor = factor,
                    FinalPythagoreanWinningPercentage = pythWP
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 1,
                    LeagueName = leagueName,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    OffensiveFactor = factor,
                    DefensiveAverage = pointsAgainst,
                    DefensiveFactor = factor,
                    FinalPythagoreanWinningPercentage = pythWP
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 2,
                    LeagueName = leagueName,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    OffensiveFactor = factor,
                    DefensiveAverage = pointsAgainst,
                    DefensiveFactor = factor,
                    FinalPythagoreanWinningPercentage = pythWP
                }
            };
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            var teamSeasonScheduleTotalsEnumerable = new List<GetTeamSeasonScheduleTotals_Result>
            {
                new GetTeamSeasonScheduleTotals_Result
                {
                    ScheduleGames = 0
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID));

            var teamSeasonScheduleAveragesEnumerable = new List<GetTeamSeasonScheduleAverages_Result>
            {
                new GetTeamSeasonScheduleAverages_Result
                {
                    PointsFor = 0,
                    PointsAgainst = 0,
                    SchedulePointsFor = 0,
                    SchedulePointsAgainst = 0
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID));

            var teamSeason = teamSeasons.First();

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games)).Returns(pointsFor);
            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games)).Returns(pointsAgainst);

            var teamSeasonScheduleAverages = teamSeasonScheduleAveragesEnumerable.FirstOrDefault();
            A.CallTo(() => _calculator.Divide((double) teamSeason.OffensiveAverage,
                (double) teamSeasonScheduleAverages.PointsAgainst)).Returns(factor);
            A.CallTo(() => _calculator.Divide((double)teamSeason.DefensiveAverage,
                (double)teamSeasonScheduleAverages.PointsFor)).Returns(factor);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason)).Returns(pythWP);

            // Act
            await service.UpdateRankings(dbContext);

            // Assert
            A.CallTo(() => _storedProcedureRepository
                    .GetLeagueSeasonTotals(dbContext, leagueName, TeamSeasonsService.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            // Verify correct values assigned to LeagueSeason object.
            var leagueSeasonTotals = leagueSeasonTotalsEnumerable.FirstOrDefault();
            var leagueSeason = leagueSeasons.FirstOrDefault();
            Assert.AreEqual((double)leagueSeasonTotals.TotalGames, leagueSeason.TotalGames);
            Assert.AreEqual((double)leagueSeasonTotals.TotalPoints, leagueSeason.TotalPoints);
            Assert.AreEqual((double)leagueSeasonTotals.AveragePoints, leagueSeason.AveragePoints);

            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            #region await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, teamName, seasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(pointsFor, teamSeason.OffensiveAverage);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(pointsAgainst, teamSeason.DefensiveAverage);

            A.CallTo(() => _calculator.Divide((double)teamSeason.OffensiveAverage,
                (double)teamSeasonScheduleAverages.PointsAgainst)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(factor, teamSeason.OffensiveFactor);

            A.CallTo(() => _calculator.Divide((double)teamSeason.DefensiveAverage,
                (double)teamSeasonScheduleAverages.PointsFor)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(factor, teamSeason.DefensiveFactor);

            Assert.AreEqual((teamSeason.OffensiveAverage + teamSeason.OffensiveFactor * leagueSeason.AveragePoints) / 2,
                teamSeason.OffensiveIndex);
            Assert.AreEqual((teamSeason.DefensiveAverage + teamSeason.DefensiveFactor * leagueSeason.AveragePoints) / 2,
                teamSeason.DefensiveIndex);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(pythWP, teamSeason.FinalPythagoreanWinningPercentage);

            #endregion await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedTwiceExactly();
        }

        [TestCase]
        public async Task UpdateRankings_SaveThrowsException_MethodCompletes()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var dbContext = A.Fake<ProFootballEntities>();

            TeamSeasonsService.SelectedSeason = 2017;

            double avgPoints = 20;
            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = avgPoints
                }
            };
            dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueName = "NFL";
            var seasonID = 2017;

            var leagueSeasonTotalsResult = dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            A.CallTo(() =>
                    _storedProcedureRepository.GetLeagueSeasonTotals(dbContext, A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotalsResult);

            var leagueSeasons = new List<LeagueSeason>
            {
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 1,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 2,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                }
            };
            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).Returns(leagueSeasons);

            A.CallTo(() => dbContext.SaveChangesAsync()).Throws<Exception>();

            var teamName = "Team";
            double pointsFor = 21;
            double pointsAgainst = 19;
            double factor = 1;
            double pythWP = 0.5;
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID,
                    LeagueName = leagueName,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    OffensiveFactor = factor,
                    DefensiveAverage = pointsAgainst,
                    DefensiveFactor = factor,
                    FinalPythagoreanWinningPercentage = pythWP
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 1,
                    LeagueName = leagueName,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    OffensiveFactor = factor,
                    DefensiveAverage = pointsAgainst,
                    DefensiveFactor = factor,
                    FinalPythagoreanWinningPercentage = pythWP
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 2,
                    LeagueName = leagueName,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    OffensiveFactor = factor,
                    DefensiveAverage = pointsAgainst,
                    DefensiveFactor = factor,
                    FinalPythagoreanWinningPercentage = pythWP
                }
            };
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            var teamSeasonScheduleTotalsEnumerable = new List<GetTeamSeasonScheduleTotals_Result>
            {
                new GetTeamSeasonScheduleTotals_Result
                {
                    ScheduleGames = 0
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotalsEnumerable);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID));

            var teamSeasonScheduleAveragesEnumerable = new List<GetTeamSeasonScheduleAverages_Result>
            {
                new GetTeamSeasonScheduleAverages_Result
                {
                    PointsFor = 0,
                    PointsAgainst = 0,
                    SchedulePointsFor = 0,
                    SchedulePointsAgainst = 0
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAveragesEnumerable);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID));

            var teamSeason = teamSeasons.First();

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games)).Returns(pointsFor);
            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games)).Returns(pointsAgainst);

            var teamSeasonScheduleAverages = teamSeasonScheduleAveragesEnumerable.FirstOrDefault();
            A.CallTo(() => _calculator.Divide((double)teamSeason.OffensiveAverage,
                (double)teamSeasonScheduleAverages.PointsAgainst)).Returns(factor);
            A.CallTo(() => _calculator.Divide((double)teamSeason.DefensiveAverage,
                (double)teamSeasonScheduleAverages.PointsFor)).Returns(factor);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason)).Returns(pythWP);

            // Act
            await service.UpdateRankings(dbContext);

            // Assert
            A.CallTo(() => _storedProcedureRepository
                    .GetLeagueSeasonTotals(dbContext, leagueName, TeamSeasonsService.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            // Verify correct values assigned to LeagueSeason object.
            var leagueSeasonTotals = leagueSeasonTotalsEnumerable.FirstOrDefault();
            var leagueSeason = leagueSeasons.FirstOrDefault();
            Assert.AreEqual((double)leagueSeasonTotals.TotalGames, leagueSeason.TotalGames);
            Assert.AreEqual((double)leagueSeasonTotals.TotalPoints, leagueSeason.TotalPoints);
            Assert.AreEqual((double)leagueSeasonTotals.AveragePoints, leagueSeason.AveragePoints);

            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            #region await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, teamName, seasonID))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, teamName, seasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _calculator.Divide(teamSeason.PointsFor, teamSeason.Games)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(pointsFor, teamSeason.OffensiveAverage);

            A.CallTo(() => _calculator.Divide(teamSeason.PointsAgainst, teamSeason.Games))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(pointsAgainst, teamSeason.DefensiveAverage);

            A.CallTo(() => _calculator.Divide((double)teamSeason.OffensiveAverage,
                (double)teamSeasonScheduleAverages.PointsAgainst)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(factor, teamSeason.OffensiveFactor);

            A.CallTo(() => _calculator.Divide((double)teamSeason.DefensiveAverage,
                (double)teamSeasonScheduleAverages.PointsFor)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(factor, teamSeason.DefensiveFactor);

            Assert.AreEqual((teamSeason.OffensiveAverage + teamSeason.OffensiveFactor * leagueSeason.AveragePoints) / 2,
                teamSeason.OffensiveIndex);
            Assert.AreEqual((teamSeason.DefensiveAverage + teamSeason.DefensiveFactor * leagueSeason.AveragePoints) / 2,
                teamSeason.DefensiveIndex);

            A.CallTo(() => _calculator.CalculatePythagoreanWinningPercentage(teamSeason)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(pythWP, teamSeason.FinalPythagoreanWinningPercentage);

            #endregion await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedTwiceExactly();
        }

        [TestCase]
        public async Task UpdateRankings_ArgumentNullExceptionDuringGetOfTeamSeasonEntities_MethodAborts()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var dbContext = A.Fake<ProFootballEntities>();

            TeamSeasonsService.SelectedSeason = 2017;

            double avgPoints = 20;
            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = avgPoints
                }
            };
            dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueName = "NFL";
            var seasonID = 2017;

            var leagueSeasonTotalsResult = dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            A.CallTo(() =>
                    _storedProcedureRepository.GetLeagueSeasonTotals(dbContext, A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotalsResult);

            var leagueSeasons = new List<LeagueSeason>
            {
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 1,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 2,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                }
            };
            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).Returns(leagueSeasons);

            var teamName = "Team";
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Throws<ArgumentNullException>();

            var teamSeasonScheduleTotals = new List<GetTeamSeasonScheduleTotals_Result>();
            dbContext.SetUpFakeTeamSeasonScheduleTotals(teamSeasonScheduleTotals);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleTotals(teamName, seasonID));

            var teamSeasonScheduleAverages = new List<GetTeamSeasonScheduleAverages_Result>
            {
                new GetTeamSeasonScheduleAverages_Result
                {
                    PointsFor = 0,
                    PointsAgainst = 0,
                    SchedulePointsFor = 0,
                    SchedulePointsAgainst = 0
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAverages);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID));

            // Act
            await service.UpdateRankings(dbContext);

            // Assert
            A.CallTo(() => _storedProcedureRepository
                    .GetLeagueSeasonTotals(dbContext, leagueName, TeamSeasonsService.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            // Verify correct values assigned to LeagueSeason object.
            var leagueSeasonTotals = leagueSeasonTotalsEnumerable.FirstOrDefault();
            var leagueSeason = leagueSeasons.FirstOrDefault();
            Assert.AreEqual((double)leagueSeasonTotals.TotalGames, leagueSeason.TotalGames);
            Assert.AreEqual((double)leagueSeasonTotals.TotalPoints, leagueSeason.TotalPoints);
            Assert.AreEqual((double)leagueSeasonTotals.AveragePoints, leagueSeason.AveragePoints);

            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedOnceExactly();

            #region await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .MustNotHaveHappened();

            #endregion await UpdateRankingsByTeamSeason(dbContext, SelectedSeason, teamSeason);
        }

        [TestCase]
        public async Task UpdateRankings_ArgumentNullExceptionDuringUpdateRankingsByTeamSeason_MethodAborts()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            var dbContext = A.Fake<ProFootballEntities>();

            TeamSeasonsService.SelectedSeason = 2017;

            double avgPoints = 20;
            var leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
            {
                new GetLeagueSeasonTotals_Result
                {
                    TotalGames = 256,
                    TotalPoints = 5120,
                    AveragePoints = avgPoints
                }
            };
            dbContext.SetUpFakeLeagueSeasonTotals(leagueSeasonTotalsEnumerable);

            var leagueName = "NFL";
            var seasonID = 2017;

            var leagueSeasonTotalsResult = dbContext.GetLeagueSeasonTotals(leagueName, seasonID);
            A.CallTo(() =>
                    _storedProcedureRepository.GetLeagueSeasonTotals(dbContext, A<string>.Ignored, A<int>.Ignored))
                .Returns(leagueSeasonTotalsResult);

            var leagueSeasons = new List<LeagueSeason>
            {
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 1,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                },
                new LeagueSeason
                {
                    LeagueName = leagueName,
                    SeasonID = seasonID - 2,
                    TotalGames = 0,
                    TotalPoints = 0,
                    AveragePoints = 0
                }
            };
            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).Returns(leagueSeasons);

            var teamName = "Team";
            double pointsFor = 21;
            double pointsAgainst = 19;
            double factor = 1;
            double pythWP = 0.5;
            var teamSeasons = new List<TeamSeason>
            {
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID,
                    LeagueName = leagueName,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    OffensiveFactor = factor,
                    DefensiveAverage = pointsAgainst,
                    DefensiveFactor = factor,
                    FinalPythagoreanWinningPercentage = pythWP
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 1,
                    LeagueName = leagueName,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    OffensiveFactor = factor,
                    DefensiveAverage = pointsAgainst,
                    DefensiveFactor = factor,
                    FinalPythagoreanWinningPercentage = pythWP
                },
                new TeamSeason
                {
                    TeamName = teamName,
                    SeasonID = seasonID - 2,
                    LeagueName = leagueName,
                    Games = 1,
                    PointsFor = pointsFor,
                    PointsAgainst = pointsAgainst,
                    OffensiveAverage = pointsFor,
                    OffensiveFactor = factor,
                    DefensiveAverage = pointsAgainst,
                    DefensiveFactor = factor,
                    FinalPythagoreanWinningPercentage = pythWP
                }
            };
            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).Returns(teamSeasons);

            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleTotals(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Throws<ArgumentNullException>();

            var teamSeasonScheduleAverages = new List<GetTeamSeasonScheduleAverages_Result>
            {
                new GetTeamSeasonScheduleAverages_Result
                {
                    PointsFor = 0,
                    PointsAgainst = 0,
                    SchedulePointsFor = 0,
                    SchedulePointsAgainst = 0
                }
            };
            dbContext.SetUpFakeTeamSeasonScheduleAverages(teamSeasonScheduleAverages);
            A.CallTo(() =>
                    _storedProcedureRepository.GetTeamSeasonScheduleAverages(dbContext, A<string>.Ignored,
                        A<int>.Ignored))
                .Returns(dbContext.GetTeamSeasonScheduleAverages(teamName, seasonID));

            // Act
            await service.UpdateRankings(dbContext);

            // Assert
            A.CallTo(() => _storedProcedureRepository
                    .GetLeagueSeasonTotals(dbContext, leagueName, TeamSeasonsService.SelectedSeason))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _leagueSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();

            // Verify correct values assigned to LeagueSeason object.
            var leagueSeasonTotals = leagueSeasonTotalsEnumerable.FirstOrDefault();
            var leagueSeason = leagueSeasons.FirstOrDefault();
            Assert.AreEqual((double)leagueSeasonTotals.TotalGames, leagueSeason.TotalGames);
            Assert.AreEqual((double)leagueSeasonTotals.TotalPoints, leagueSeason.TotalPoints);
            Assert.AreEqual((double)leagueSeasonTotals.AveragePoints, leagueSeason.AveragePoints);

            A.CallTo(() => dbContext.SaveChangesAsync()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _teamSeasonRepository.GetEntitiesAsync(dbContext)).MustHaveHappenedOnceExactly();
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new TeamSeasonsService(_sharedService, _dataMapper, _leagueSeasonRepository,
                _teamSeasonRepository, _storedProcedureRepository, _calculator);

            // Act

            // Assert
        }
    }
}
