using EldredBrown.ProFootballApplicationWPF.Models.Data;

namespace EldredBrown.ProFootballApplicationWPF
{
    public interface IDataAccess
    {
        ProFootballDbContext DbContext { get; }
    }
}
