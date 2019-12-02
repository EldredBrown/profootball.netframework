using System.Collections.Generic;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View
{
    public class TeamSeasonDetailsViewModel
    {
        public TeamSeasonViewModel TeamSeason { get; set; }
        public IEnumerable<TeamSeasonScheduleProfileViewModel> TeamSeasonScheduleProfile { get; set; }
        public TeamSeasonScheduleTotalsViewModel TeamSeasonScheduleTotals { get; set; }
        public TeamSeasonScheduleAveragesViewModel TeamSeasonScheduleAverages { get; set; }
    }
}
