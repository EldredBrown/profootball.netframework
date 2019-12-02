using System;
using System.ComponentModel.DataAnnotations;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View
{
    public class TeamSeasonScheduleProfileViewModel
    {
        public string Opponent { get; set; }
        public Nullable<double> GamePointsFor { get; set; }
        public Nullable<double> GamePointsAgainst { get; set; }
        public Nullable<double> OpponentWins { get; set; }
        public Nullable<double> OpponentLosses { get; set; }
        public Nullable<double> OpponentTies { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.000}")]
        public Nullable<double> OpponentWinningPercentage { get; set; }

        public Nullable<double> OpponentWeightedGames { get; set; }
        public Nullable<double> OpponentWeightedPointsFor { get; set; }
        public Nullable<double> OpponentWeightedPointsAgainst { get; set; }
    }
}
