using System;
using System.Data.Entity;
using EldredBrown.ProFootball.Shared;

namespace EldredBrown.ProFootball.WpfApp.Models
{
    /// <summary>
    /// Interface to allow for similar usage of the League, Conference, and Division classes
    /// </summary>
    public interface IAssociation
    {
        string Name { get; set; }
        int FirstSeasonID { get; set; }
        Nullable<int> LastSeasonID { get; set; }

        bool AssociationExists(int seasonID);
    }

    public partial class League : IAssociation
    {
        public virtual bool AssociationExists(int seasonID)
        {
            return FirstSeasonID <= seasonID && (LastSeason == null || LastSeasonID >= seasonID);
        }
    }

    public partial class Conference : IAssociation
    {
        public virtual bool AssociationExists(int seasonID)
        {
            return FirstSeasonID <= seasonID && (LastSeason == null || LastSeasonID >= seasonID);
        }
    }

    public partial class Division : IAssociation
    {
        public virtual bool AssociationExists(int seasonID)
        {
            return FirstSeasonID <= seasonID && (LastSeason == null || LastSeasonID >= seasonID);
        }
    }

    public partial class Game
    {
        /// <summary>
        /// Override of Object.Equals method for equality testing
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                var g = obj as Game;

                var result = true;
                result &= (this.ID == g.ID);
                result &= (this.SeasonID == g.SeasonID);
                result &= (this.Week == g.Week);
                result &= (this.GuestName == g.GuestName);
                result &= (this.GuestScore == g.GuestScore);
                result &= (this.HostName == g.HostName);
                result &= (this.HostScore == g.HostScore);
                result &= (this.IsPlayoffGame == g.IsPlayoffGame);
                result &= (this.Notes == g.Notes);
                return result;
            }
        }

        /// <summary>
        /// To prevent build warning, override GetHashCode() when overriding Equals(Object obj).
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Makes the TeamSeason model an implementation of the ITeamSeason interface declared in the shared project
    /// </summary>
    public partial class TeamSeason : ITeamSeason
    {
    }

    public partial class ProFootballEntities
    {
        /// <summary>
        /// Allows for the stubbing that is necessary to unit test data store update functionality used in many of this
        /// app's data models
        /// </summary>
        /// <param name="entity"></param>
        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
