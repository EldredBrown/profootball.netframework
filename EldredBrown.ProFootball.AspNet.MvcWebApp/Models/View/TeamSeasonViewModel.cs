using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View
{
    public class TeamSeasonViewModel
    {
        [DisplayName("Team")]
        public string TeamName { get; set; }

        [DisplayName("Season")]
        public int SeasonID { get; set; }

        public string LeagueName { get; set; }
        public string ConferenceName { get; set; }
        public string DivisionName { get; set; }
        public double Games { get; set; }
        public double Wins { get; set; }
        public double Losses { get; set; }
        public double Ties { get; set; }

        [DisplayName("W-Pct.")]
        [DisplayFormat(DataFormatString = "{0:#.000}")]
        public Nullable<double> WinningPercentage { get; set; }

        public double PointsFor { get; set; }
        public double PointsAgainst { get; set; }

        [DisplayName("Pyth W")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public double PythagoreanWins { get; set; }

        [DisplayName("Pyth L")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public double PythagoreanLosses { get; set; }

        [DisplayName("OA")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<double> OffensiveAverage { get; set; }

        [DisplayName("OF")]
        [DisplayFormat(DataFormatString = "{0:#.000}")]
        public Nullable<double> OffensiveFactor { get; set; }

        [DisplayName("OI")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<double> OffensiveIndex { get; set; }

        [DisplayName("DA")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<double> DefensiveAverage { get; set; }

        [DisplayName("DF")]
        [DisplayFormat(DataFormatString = "{0:#.000}")]
        public Nullable<double> DefensiveFactor { get; set; }

        [DisplayName("DI")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<double> DefensiveIndex { get; set; }

        [DisplayName("FinPythW")]
        [DisplayFormat(DataFormatString = "{0:#.000}")]
        public Nullable<double> FinalPythagoreanWinningPercentage { get; set; }
    }
}
