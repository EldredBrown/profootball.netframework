using EldredBrown.ProFootballApplicationWeb.Models;
using EldredBrown.ProFootballApplicationWeb.Models.ViewModels;
using EldredBrown.ProFootballApplicationWeb.Services;
using NUnit.Framework;

namespace EldredBrown.ProFootballApplicationWeb.Tests
{
    [TestFixture]
    public class DataMapperTest
    {
        [TestCase]
        public void MapToGame()
        {
            // Arrange
            var gameViewModel = new GameViewModel
            {
                ID = 1,
                SeasonID = 2017,
                Week = 1,
                GuestName = "Guest",
                GuestScore = 2,
                HostName = "Host",
                HostScore = 3,
                IsPlayoffGame = false,
                Notes = "Notes"
            };

            // Act
            var mapper = new DataMapper();
            var result = mapper.MapToGame(gameViewModel);

            // Assert
            Assert.IsInstanceOf(typeof(Game), result);
            Assert.AreEqual(gameViewModel.ID, result.ID);
            Assert.AreEqual(gameViewModel.SeasonID, result.SeasonID);
            Assert.AreEqual(gameViewModel.Week, result.Week);
            Assert.AreEqual(gameViewModel.GuestName, result.GuestName);
            Assert.AreEqual(gameViewModel.GuestScore, result.GuestScore);
            Assert.AreEqual(gameViewModel.HostName, result.HostName);
            Assert.AreEqual(gameViewModel.HostScore, result.HostScore);
            Assert.AreEqual(gameViewModel.IsPlayoffGame, result.IsPlayoffGame);
            Assert.AreEqual(gameViewModel.Notes, result.Notes);
        }

        [TestCase]
        public void MapToGameViewModel()
        {
            // Arrange
            var game = new Game
            {
                ID = 1,
                SeasonID = 2017,
                Week = 1,
                GuestName = "Guest",
                GuestScore = 2,
                HostName = "Host",
                HostScore = 3,
                IsPlayoffGame = false,
                Notes = "Notes"
            };

            // Act
            var mapper = new DataMapper();
            var result = mapper.MapToGameViewModel(game);

            // Assert
            Assert.IsInstanceOf(typeof(GameViewModel), result);
            Assert.AreEqual(game.ID, result.ID);
            Assert.AreEqual(game.SeasonID, result.SeasonID);
            Assert.AreEqual(game.Week, result.Week);
            Assert.AreEqual(game.GuestName, result.GuestName);
            Assert.AreEqual(game.GuestScore, result.GuestScore);
            Assert.AreEqual(game.HostName, result.HostName);
            Assert.AreEqual(game.HostScore, result.HostScore);
            Assert.AreEqual(game.IsPlayoffGame, result.IsPlayoffGame);
            Assert.AreEqual(game.Notes, result.Notes);
        }

        [TestCase]
        public void MapToSeasonStandingsResultViewModel()
        {
            // Arrange
            var seasonStandings = new usp_GetSeasonStandings_Result
            {
                Team = "Team",
                Conference = "Conference",
                Division = "Division",
                Wins = 1d,
                Losses = 1d,
                Ties = 1d,
                WinningPercentage = 0.5,
                PointsFor = 9d,
                PointsAgainst = 9d,
                AvgPointsFor = 3d,
                AvgPointsAgainst = 3d
            };

            // Act
            var mapper = new DataMapper();
            var result = mapper.MapToSeasonStandingsResultViewModel(seasonStandings);

            // Assert
            Assert.IsInstanceOf(typeof(SeasonStandingsResultViewModel), result);
            Assert.AreEqual(seasonStandings.Team, result.Team);
            Assert.AreEqual(seasonStandings.Conference, result.Conference);
            Assert.AreEqual(seasonStandings.Division, result.Division);
            Assert.AreEqual(seasonStandings.Wins, result.Wins);
            Assert.AreEqual(seasonStandings.Losses, result.Losses);
            Assert.AreEqual(seasonStandings.Ties, result.Ties);
            Assert.AreEqual(seasonStandings.WinningPercentage, result.WinningPercentage);
            Assert.AreEqual(seasonStandings.PointsFor, result.PointsFor);
            Assert.AreEqual(seasonStandings.PointsAgainst, result.PointsAgainst);
            Assert.AreEqual(seasonStandings.AvgPointsFor, result.AvgPointsFor);
            Assert.AreEqual(seasonStandings.AvgPointsAgainst, result.AvgPointsAgainst);
        }

        [TestCase]
        public void MapToSeasonViewModel()
        {
            // Arrange
            var season = new Season { ID = 2017 };

            // Act
            var mapper = new DataMapper();
            var result = mapper.MapToSeasonViewModel(season);

            // Assert
            Assert.IsInstanceOf(typeof(SeasonViewModel), result);
            Assert.AreEqual(season.ID, result.ID);
        }

        [TestCase]
        public void MapToTeamSeasonScheduleAveragesViewModel()
        {
            // Arrange
            var teamSeasonScheduleAverages = new usp_GetTeamSeasonScheduleAverages_Result();

            // Act
            var mapper = new DataMapper();
            var result = mapper.MapToTeamSeasonScheduleAveragesViewModel(teamSeasonScheduleAverages);

            // Assert
            Assert.IsInstanceOf(typeof(TeamSeasonScheduleAveragesViewModel), result);
            Assert.AreEqual(teamSeasonScheduleAverages.PointsFor, result.PointsFor);
            Assert.AreEqual(teamSeasonScheduleAverages.PointsAgainst, result.PointsAgainst);
            Assert.AreEqual(teamSeasonScheduleAverages.SchedulePointsFor, result.SchedulePointsFor);
            Assert.AreEqual(teamSeasonScheduleAverages.SchedulePointsAgainst, result.SchedulePointsAgainst);
        }

        [TestCase]
        public void MapToTeamSeasonScheduleProfileViewModel()
        {
            // Arrange
            var teamSeasonScheduleProfile = new usp_GetTeamSeasonScheduleProfile_Result
            {
                Opponent = "Opponent",
                GamePointsFor = 9d,
                GamePointsAgainst = 9d,
                OpponentWins = 2d,
                OpponentLosses = 2d,
                OpponentTies = 2d,
                OpponentWinningPercentage = 0.5,
                OpponentWeightedGames = 3d,
                OpponentWeightedPointsFor = 9d,
                OpponentWeightedPointsAgainst = 9d
            };

            // Act
            var mapper = new DataMapper();
            var result = mapper.MapToTeamSeasonScheduleProfileViewModel(teamSeasonScheduleProfile);

            // Assert
            Assert.IsInstanceOf(typeof(TeamSeasonScheduleProfileViewModel), result);
            Assert.AreEqual(teamSeasonScheduleProfile.Opponent, result.Opponent);
            Assert.AreEqual(teamSeasonScheduleProfile.GamePointsFor, result.GamePointsFor);
            Assert.AreEqual(teamSeasonScheduleProfile.GamePointsAgainst, result.GamePointsAgainst);
            Assert.AreEqual(teamSeasonScheduleProfile.OpponentWins, result.OpponentWins);
            Assert.AreEqual(teamSeasonScheduleProfile.OpponentLosses, result.OpponentLosses);
            Assert.AreEqual(teamSeasonScheduleProfile.OpponentTies, result.OpponentTies);
            Assert.AreEqual(teamSeasonScheduleProfile.OpponentWinningPercentage, result.OpponentWinningPercentage);
            Assert.AreEqual(teamSeasonScheduleProfile.OpponentWeightedGames, result.OpponentWeightedGames);
            Assert.AreEqual(teamSeasonScheduleProfile.OpponentWeightedPointsFor, result.OpponentWeightedPointsFor);
            Assert.AreEqual(teamSeasonScheduleProfile.OpponentWeightedPointsAgainst, result.OpponentWeightedPointsAgainst);
        }

        [TestCase]
        public void MapToTeamSeasonScheduleTotalsViewModel()
        {
            // Arrange
            var teamSeasonScheduleTotalsProfile = new usp_GetTeamSeasonScheduleTotals_Result
            {
                Games = 3,
                PointsFor = 9d,
                PointsAgainst = 9d,
                ScheduleWins = 2d,
                ScheduleLosses = 2d,
                ScheduleTies = 2d,
                ScheduleWinningPercentage = 0.5,
                ScheduleGames = 3d,
                SchedulePointsFor = 9d,
                SchedulePointsAgainst = 9d
            };

            // Act
            var mapper = new DataMapper();
            var result = mapper.MapToTeamSeasonScheduleTotalsViewModel(teamSeasonScheduleTotalsProfile);

            // Assert
            Assert.IsInstanceOf(typeof(TeamSeasonScheduleTotalsViewModel), result);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.Games, result.Games);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.PointsFor, result.PointsFor);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.PointsAgainst, result.PointsAgainst);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.ScheduleWins, result.ScheduleWins);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.ScheduleLosses, result.ScheduleLosses);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.ScheduleTies, result.ScheduleTies);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.ScheduleWinningPercentage, result.ScheduleWinningPercentage);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.ScheduleGames, result.ScheduleGames);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.SchedulePointsFor, result.SchedulePointsFor);
            Assert.AreEqual(teamSeasonScheduleTotalsProfile.SchedulePointsAgainst, result.SchedulePointsAgainst);
        }

        [TestCase]
        public void MapToTeamSeasonViewModel()
        {
            // Arrange
            var teamSeason = new TeamSeason
            {
                TeamName = "Team",
                SeasonID = 2017,
                LeagueName = "League",
                ConferenceName = "Conference",
                DivisionName = "DivisionName",
                Games = 3d,
                Wins = 1d,
                Losses = 1d,
                Ties = 1d,
                WinningPercentage = 0.5,
                PointsFor = 9d,
                PointsAgainst = 9d,
                PythagoreanWins = 1.5,
                PythagoreanLosses = 1.5,
                OffensiveAverage = 3d,
                OffensiveFactor = 0.5,
                OffensiveIndex = 3d,
                DefensiveAverage = 3d,
                DefensiveFactor = 0.5,
                DefensiveIndex = 3d,
                FinalPythagoreanWinningPercentage = 0.5
            };

            // Act
            var mapper = new DataMapper();
            var result = mapper.MapToTeamSeasonViewModel(teamSeason);

            // Assert
            Assert.IsInstanceOf(typeof(TeamSeasonViewModel), result);
            Assert.AreEqual(teamSeason.TeamName, result.TeamName);
            Assert.AreEqual(teamSeason.SeasonID, result.SeasonID);
            Assert.AreEqual(teamSeason.LeagueName, result.LeagueName);
            Assert.AreEqual(teamSeason.ConferenceName, result.ConferenceName);
            Assert.AreEqual(teamSeason.DivisionName, result.DivisionName);
            Assert.AreEqual(teamSeason.Games, result.Games);
            Assert.AreEqual(teamSeason.Wins, result.Wins);
            Assert.AreEqual(teamSeason.Losses, result.Losses);
            Assert.AreEqual(teamSeason.Ties, result.Ties);
            Assert.AreEqual(teamSeason.WinningPercentage, result.WinningPercentage);
            Assert.AreEqual(teamSeason.PointsFor, result.PointsFor);
            Assert.AreEqual(teamSeason.PointsAgainst, result.PointsAgainst);
            Assert.AreEqual(teamSeason.PythagoreanWins, result.PythagoreanWins);
            Assert.AreEqual(teamSeason.PythagoreanLosses, result.PythagoreanLosses);
            Assert.AreEqual(teamSeason.OffensiveAverage, result.OffensiveAverage);
            Assert.AreEqual(teamSeason.OffensiveFactor, result.OffensiveFactor);
            Assert.AreEqual(teamSeason.OffensiveIndex, result.OffensiveIndex);
            Assert.AreEqual(teamSeason.DefensiveAverage, result.DefensiveAverage);
            Assert.AreEqual(teamSeason.DefensiveFactor, result.DefensiveFactor);
            Assert.AreEqual(teamSeason.DefensiveIndex, result.DefensiveIndex);
            Assert.AreEqual(teamSeason.FinalPythagoreanWinningPercentage, result.FinalPythagoreanWinningPercentage);
        }

        [TestCase]
        public void MapToTeamViewModel()
        {
            // Arrange
            var team = new Team
            {
                Name = "Team"
            };

            // Act
            var mapper = new DataMapper();
            var result = mapper.MapToTeamViewModel(team);

            // Assert
            Assert.IsInstanceOf(typeof(TeamViewModel), result);
            Assert.AreEqual(team.Name, result.Name);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange

            // Act
            var mapper = new DataMapper();

            // Assert
        }
    }
}
