using EldredBrown.ProFootballApplicationWPF.Models.Data;

namespace EldredBrown.ProFootballApplicationWPF
{
    public class DataAccess : IDataAccess
    {
        private static DataAccess _uniqueInstance;

        private DataAccess()
        {
            DbContext = new ProFootballDbContext();
        }

        public static DataAccess Instance
        {
            get
            {
                if (_uniqueInstance == null)
                {
                    _uniqueInstance = new DataAccess();
                }

                return _uniqueInstance;
            }
        }

        public ProFootballDbContext DbContext { get; }
    }
}