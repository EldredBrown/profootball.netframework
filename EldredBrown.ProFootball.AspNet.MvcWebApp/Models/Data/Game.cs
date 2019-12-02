//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Game
    {
        public int ID { get; set; }
        public int SeasonID { get; set; }
        public int Week { get; set; }
        public string GuestName { get; set; }
        public double GuestScore { get; set; }
        public string HostName { get; set; }
        public double HostScore { get; set; }
        public string WinnerName { get; set; }
        public Nullable<double> WinnerScore { get; set; }
        public string LoserName { get; set; }
        public Nullable<double> LoserScore { get; set; }
        public bool IsPlayoffGame { get; set; }
        public string Notes { get; set; }
    
        public virtual Season Season { get; set; }
        public virtual Team Guest { get; set; }
        public virtual Team Host { get; set; }
        public virtual Team Winner { get; set; }
        public virtual Team Loser { get; set; }
    }
}
