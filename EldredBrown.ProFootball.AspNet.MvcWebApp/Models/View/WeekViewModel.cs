using System;
using System.ComponentModel;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View
{
    public class WeekViewModel
    {
        public WeekViewModel(int? week)
        {
            if (week == null)
            {
                this.ID = string.Empty;
            }
            else
            {
                this.ID = week.ToString();
            }
        }

        [DisplayName()]
        public string ID { get; set; }
    }
}
