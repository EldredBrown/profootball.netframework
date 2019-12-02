using System.Reflection;
using System.Web.Mvc;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.App_Start
{
    /// <summary>
    /// Class to handle all dependency injection for this web app
    /// </summary>
    public static class DependencyInjectionConfig
    {
        /// <summary>
        /// Registers types for dependency injection
        /// </summary>
        public static void Register()
        {
            // Create the container as usual.
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register your types, for instance:
            container.Register<ITeamSeasonsService, TeamSeasonsService>(Lifestyle.Scoped);
            container.Register<IGamesService, GamesService>(Lifestyle.Scoped);
            container.Register<ISeasonStandingsService, SeasonStandingsService>(Lifestyle.Scoped);
            container.Register<IGamePredictorService, GamePredictorService>(Lifestyle.Scoped);
            container.Register<ISharedService, SharedService>(Lifestyle.Scoped);
            container.Register<IDataMapper, DataMapper>(Lifestyle.Scoped);
            container.Register<IRepository<Game>, GameRepository>(Lifestyle.Scoped);
            container.Register<IRepository<LeagueSeason>, LeagueSeasonRepository>(Lifestyle.Scoped);
            container.Register<IRepository<Season>, SeasonRepository>(Lifestyle.Scoped);
            container.Register<IStoredProcedureRepository, StoredProcedureRepository>(Lifestyle.Scoped);
            container.Register<IRepository<Team>, TeamRepository>(Lifestyle.Scoped);
            container.Register<IRepository<TeamSeason>, TeamSeasonRepository>(Lifestyle.Scoped);
            container.Register<IRepository<WeekCount>, WeekCountRepository>(Lifestyle.Scoped);
            container.Register<ICalculator, Calculator>(Lifestyle.Scoped);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
