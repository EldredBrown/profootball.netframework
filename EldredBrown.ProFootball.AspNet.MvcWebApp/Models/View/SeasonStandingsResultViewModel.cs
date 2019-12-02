using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View
{
    public class SeasonStandingsResultViewModel
    {
        public string Team { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public Nullable<double> Wins { get; set; }
        public Nullable<double> Losses { get; set; }
        public Nullable<double> Ties { get; set; }

        [DisplayName("W-Pct.")]
        [DisplayFormat(DataFormatString = "{0:#.000}")]
        public Nullable<double> WinningPercentage { get; set; }

        public Nullable<double> PointsFor { get; set; }
        public Nullable<double> PointsAgainst { get; set; }

        [DisplayName("OA")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public Nullable<double> AvgPointsFor { get; set; }

        [DisplayName("OA")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public Nullable<double> AvgPointsAgainst { get; set; }
    }
}
