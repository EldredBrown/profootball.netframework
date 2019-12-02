using System;
using System.ComponentModel.DataAnnotations;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View
{
    public class TeamSeasonScheduleTotalsViewModel
    {
        public Nullable<double> Games { get; set; }
        public Nullable<double> PointsFor { get; set; }
        public Nullable<double> PointsAgainst { get; set; }
        public Nullable<double> ScheduleWins { get; set; }
        public Nullable<double> ScheduleLosses { get; set; }
        public Nullable<double> ScheduleTies { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.000}")]
        public Nullable<double> ScheduleWinningPercentage { get; set; }

        public Nullable<double> ScheduleGames { get; set; }
        public Nullable<double> SchedulePointsFor { get; set; }
        public Nullable<double> SchedulePointsAgainst { get; set; }
    }
}
