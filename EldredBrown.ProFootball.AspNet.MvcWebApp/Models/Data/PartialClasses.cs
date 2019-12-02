using System.Data.Entity;
using EldredBrown.ProFootball.Shared;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data
{
    /// <summary>
    /// Makes the ProFootballEntities an implementation of the IProFootballDbContext interface so it can be stubbed in 
    /// unit tests
    /// </summary>
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

    /// <summary>
    /// Makes the TeamSeason model an implementation of the ITeamSeason interface declared in the shared project
    /// </summary>
    public partial class TeamSeason: ITeamSeason
    {
    }
}
