using System;
using System.ComponentModel.DataAnnotations;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View
{
    public class TeamSeasonScheduleAveragesViewModel
    {
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<double> PointsFor { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<double> PointsAgainst { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<double> SchedulePointsFor { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<double> SchedulePointsAgainst { get; set; }
    }
}
