using System;

namespace EldredBrown.ProFootball.Shared
{
    public interface ITeamSeason
    {
        string TeamName { get; set; }
        int SeasonID { get; set; }
        string LeagueName { get; set; }
        string ConferenceName { get; set; }
        string DivisionName { get; set; }
        double Games { get; set; }
        double Wins { get; set; }
        double Losses { get; set; }
        double Ties { get; set; }
        Nullable<double> WinningPercentage { get; set; }
        double PointsFor { get; set; }
        double PointsAgainst { get; set; }
        double PythagoreanWins { get; set; }
        double PythagoreanLosses { get; set; }
        Nullable<double> OffensiveAverage { get; set; }
        Nullable<double> OffensiveFactor { get; set; }
        Nullable<double> OffensiveIndex { get; set; }
        Nullable<double> DefensiveAverage { get; set; }
        Nullable<double> DefensiveFactor { get; set; }
        Nullable<double> DefensiveIndex { get; set; }
        Nullable<double> FinalPythagoreanWinningPercentage { get; set; }
    }
}
