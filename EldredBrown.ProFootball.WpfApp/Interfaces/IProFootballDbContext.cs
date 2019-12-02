using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using EldredBrown.ProFootballApplication.WPF.Models;

namespace EldredBrown.ProFootballApplication.WPF.Interfaces
{
    /// <summary>
    /// Interface to allow for stubbing of the DbContext in unit tests
    /// </summary>
    public interface IProFootballDbContext : IDisposable
    {
        DbSet<Conference> Conferences { get; set; }
        DbSet<Division> Divisions { get; set; }
        DbSet<Game> Games { get; set; }
        DbSet<League> Leagues { get; set; }
        DbSet<LeagueSeason> LeagueSeasons { get; set; }
        DbSet<Season> Seasons { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<TeamSeason> TeamSeasons { get; set; }
        DbSet<WeekCount> WeekCounts { get; set; }

        int SaveChanges();
        void SetModified(object entity);

        ObjectResult<GetLeagueSeasonTotals_Result> GetLeagueSeasonTotals(string leagueName, Nullable<int> seasonID);

        ObjectResult<GetRankingsOffensive_Result> GetRankingsOffensive(Nullable<int> seasonID);
        ObjectResult<GetRankingsDefensive_Result> GetRankingsDefensive(Nullable<int> seasonID);
        ObjectResult<GetRankingsTotal_Result> GetRankingsTotal(Nullable<int> seasonID);

        ObjectResult<GetSeasonStandingsForConference_Result> GetSeasonStandingsForConference(
            Nullable<int> seasonID, string conferenceName);

        ObjectResult<GetSeasonStandingsForDivision_Result> GetSeasonStandingsForDivision(Nullable<int> seasonID,
            string divisionName);

        ObjectResult<GetTeamSeasonScheduleProfile_Result> GetTeamSeasonScheduleProfile(string teamName,
            Nullable<int> seasonID);

        ObjectResult<GetTeamSeasonScheduleTotals_Result> GetTeamSeasonScheduleTotals(string teamName,
            Nullable<int> seasonID);

        ObjectResult<GetTeamSeasonScheduleAverages_Result> GetTeamSeasonScheduleAverages(string teamName,
            Nullable<int> seasonID);
    }
}
