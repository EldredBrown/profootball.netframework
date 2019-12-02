using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.DAL
{
    public class FootballConfiguration : DbConfiguration
    {
        public FootballConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}
