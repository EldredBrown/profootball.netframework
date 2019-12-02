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
    
    public partial class League
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public League()
        {
            this.Conferences = new HashSet<Conference>();
            this.Divisions = new HashSet<Division>();
            this.LeagueSeasons = new HashSet<LeagueSeason>();
            this.TeamSeasons = new HashSet<TeamSeason>();
        }
    
        public string Name { get; set; }
        public string LongName { get; set; }
        public int FirstSeasonID { get; set; }
        public Nullable<int> LastSeasonID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Conference> Conferences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Division> Divisions { get; set; }
        public virtual Season FirstSeason { get; set; }
        public virtual Season LastSeason { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeagueSeason> LeagueSeasons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeamSeason> TeamSeasons { get; set; }
    }
}
