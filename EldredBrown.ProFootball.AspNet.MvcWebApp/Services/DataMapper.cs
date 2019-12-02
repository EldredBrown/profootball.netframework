using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Services
{
    public interface IDataMapper
    {
        Game MapToGame(GameViewModel gameViewModel);
        GameViewModel MapToGameViewModel(Game game);
        SeasonStandingsResultViewModel MapToSeasonStandingsResultViewModel(GetSeasonStandings_Result result);
        SeasonViewModel MapToSeasonViewModel(Season season);

        TeamSeasonScheduleAveragesViewModel MapToTeamSeasonScheduleAveragesViewModel(
            GetTeamSeasonScheduleAverages_Result result);

        TeamSeasonScheduleProfileViewModel MapToTeamSeasonScheduleProfileViewModel(
            GetTeamSeasonScheduleProfile_Result result);

        TeamSeasonScheduleTotalsViewModel MapToTeamSeasonScheduleTotalsViewModel(
            GetTeamSeasonScheduleTotals_Result result);

        TeamSeasonViewModel MapToTeamSeasonViewModel(TeamSeason teamSeason);
        TeamViewModel MapToTeamViewModel(Team team);
    }

    /// <summary>
    /// Class used to map data from data entity objects to ViewModel objects and vice versa
    /// </summary>
    public class DataMapper : IDataMapper
    {
        public Game MapToGame(GameViewModel gameViewModel)
        {
            return new Game
            {
                ID = gameViewModel.ID,
                SeasonID = gameViewModel.SeasonID,
                Week = gameViewModel.Week,
                GuestName = gameViewModel.GuestName,
                GuestScore = gameViewModel.GuestScore,
                HostName = gameViewModel.HostName,
                HostScore = gameViewModel.HostScore,
                IsPlayoffGame = gameViewModel.IsPlayoffGame,
                Notes = gameViewModel.Notes
            };
        }

        public GameViewModel MapToGameViewModel(Game game)
        {
            return new GameViewModel
            {
                ID = game.ID,
                SeasonID = game.SeasonID,
                Week = game.Week,
                GuestName = game.GuestName,
                GuestScore = game.GuestScore,
                HostName = game.HostName,
                HostScore = game.HostScore,
                IsPlayoffGame = game.IsPlayoffGame,
                Notes = game.Notes
            };
        }

        public SeasonStandingsResultViewModel MapToSeasonStandingsResultViewModel(GetSeasonStandings_Result result)
        {
            return new SeasonStandingsResultViewModel
            {
                Team = result.Team,
                Conference = result.Conference,
                Division = result.Division,
                Wins = result.Wins,
                Losses = result.Losses,
                Ties = result.Ties,
                WinningPercentage = result.WinningPercentage,
                PointsFor = result.PointsFor,
                PointsAgainst = result.PointsAgainst,
                AvgPointsFor = result.AvgPointsFor,
                AvgPointsAgainst = result.AvgPointsAgainst
            };
        }

        public SeasonViewModel MapToSeasonViewModel(Season season)
        {
            return new SeasonViewModel
            {
                ID = season.ID
            };
        }

        public TeamSeasonScheduleAveragesViewModel MapToTeamSeasonScheduleAveragesViewModel(
            GetTeamSeasonScheduleAverages_Result result)
        {
            return new TeamSeasonScheduleAveragesViewModel
            {
                PointsFor = result.PointsFor,
                PointsAgainst = result.PointsAgainst,
                SchedulePointsFor = result.SchedulePointsFor,
                SchedulePointsAgainst = result.SchedulePointsAgainst
            };
        }

        public TeamSeasonScheduleProfileViewModel MapToTeamSeasonScheduleProfileViewModel(
            GetTeamSeasonScheduleProfile_Result result)
        {
            return new TeamSeasonScheduleProfileViewModel
            {
                Opponent = result.Opponent,
                GamePointsFor = result.GamePointsFor,
                GamePointsAgainst = result.GamePointsAgainst,
                OpponentWins = result.OpponentWins,
                OpponentLosses = result.OpponentLosses,
                OpponentTies = result.OpponentTies,
                OpponentWinningPercentage = result.OpponentWinningPercentage,
                OpponentWeightedGames = result.OpponentWeightedGames,
                OpponentWeightedPointsFor = result.OpponentWeightedPointsFor,
                OpponentWeightedPointsAgainst = result.OpponentWeightedPointsAgainst
            };
        }

        public TeamSeasonScheduleTotalsViewModel MapToTeamSeasonScheduleTotalsViewModel(
            GetTeamSeasonScheduleTotals_Result result)
        {
            return new TeamSeasonScheduleTotalsViewModel
            {
                Games = result.Games,
                PointsFor = result.PointsFor,
                PointsAgainst = result.PointsAgainst,
                ScheduleWins = result.ScheduleWins,
                ScheduleLosses = result.ScheduleLosses,
                ScheduleTies = result.ScheduleTies,
                ScheduleWinningPercentage = result.ScheduleWinningPercentage,
                ScheduleGames = result.ScheduleGames,
                SchedulePointsFor = result.SchedulePointsFor,
                SchedulePointsAgainst = result.SchedulePointsAgainst
            };
        }

        public TeamSeasonViewModel MapToTeamSeasonViewModel(TeamSeason teamSeason)
        {
            return new TeamSeasonViewModel
            {
                TeamName = teamSeason.TeamName,
                SeasonID = teamSeason.SeasonID,
                LeagueName = teamSeason.LeagueName,
                ConferenceName = teamSeason.ConferenceName,
                DivisionName = teamSeason.DivisionName,
                Games = teamSeason.Games,
                Wins = teamSeason.Wins,
                Losses = teamSeason.Losses,
                Ties = teamSeason.Ties,
                WinningPercentage = teamSeason.WinningPercentage,
                PointsFor = teamSeason.PointsFor,
                PointsAgainst = teamSeason.PointsAgainst,
                PythagoreanWins = teamSeason.PythagoreanWins,
                PythagoreanLosses = teamSeason.PythagoreanLosses,
                OffensiveAverage = teamSeason.OffensiveAverage,
                OffensiveFactor = teamSeason.OffensiveFactor,
                OffensiveIndex = teamSeason.OffensiveIndex,
                DefensiveAverage = teamSeason.DefensiveAverage,
                DefensiveFactor = teamSeason.DefensiveFactor,
                DefensiveIndex = teamSeason.DefensiveIndex,
                FinalPythagoreanWinningPercentage = teamSeason.FinalPythagoreanWinningPercentage
            };
        }

        public TeamViewModel MapToTeamViewModel(Team team)
        {
            return new TeamViewModel
            {
                Name = team.Name
            };
        }
    }
}
