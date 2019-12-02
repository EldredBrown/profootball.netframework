//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EldredBrown.ProFootball.WpfApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TeamSeason
    {
        public string TeamName { get; set; }
        public int SeasonID { get; set; }
        public string LeagueName { get; set; }
        public string ConferenceName { get; set; }
        public string DivisionName { get; set; }
        public double Games { get; set; }
        public double Wins { get; set; }
        public double Losses { get; set; }
        public double Ties { get; set; }
        public Nullable<double> WinningPercentage { get; set; }
        public double PointsFor { get; set; }
        public double PointsAgainst { get; set; }
        public double PythagoreanWins { get; set; }
        public double PythagoreanLosses { get; set; }
        public Nullable<double> OffensiveAverage { get; set; }
        public Nullable<double> OffensiveFactor { get; set; }
        public Nullable<double> OffensiveIndex { get; set; }
        public Nullable<double> DefensiveAverage { get; set; }
        public Nullable<double> DefensiveFactor { get; set; }
        public Nullable<double> DefensiveIndex { get; set; }
        public Nullable<double> FinalPythagoreanWinningPercentage { get; set; }
    
        public virtual Team Team { get; set; }
        public virtual Season Season { get; set; }
        public virtual League League { get; set; }
        public virtual Conference Conference { get; set; }
        public virtual Division Division { get; set; }
    }
}