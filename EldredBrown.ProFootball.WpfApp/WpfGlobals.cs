using System;
using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using EldredBrown.ProFootball.WpfApp.Windows;

namespace EldredBrown.ProFootball.WpfApp
{
    /// <summary>
    /// All globals used by the WPF app
    /// </summary>
    public class WpfGlobals
    {
        // This hard-coded value is a bit of a hack at this time, but I intend to tie this to the current year in the 
        // future.
        public static int? SelectedSeason = 1920;

        public static IWindsorContainer Container;
        public static void RegisterDependencies()
        {
            Container = new WindsorContainer();

            // Register types with the container.
            Container.Register(Component.For<IMainWindowViewModel>()
                .ImplementedBy<MainWindowViewModel>());
            Container.Register(Component.For<ITeamSeasonsControlViewModel>()
                .ImplementedBy<TeamSeasonsControlViewModel>());
            Container.Register(Component.For<ISeasonStandingsControlViewModel>()
                .ImplementedBy<SeasonStandingsControlViewModel>());
            Container.Register(Component.For<IRankingsControlViewModel>()
                .ImplementedBy<RankingsControlViewModel>());
            Container.Register(Component.For<IGamesWindowViewModel>()
                .ImplementedBy<GamesWindowViewModel>());
            Container.Register(Component.For<IGameFinderWindow>()
                .ImplementedBy<GameFinderWindow>());
            Container.Register(Component.For<IGameFinderWindowViewModel>()
                .ImplementedBy<GameFinderWindowViewModel>());
            Container.Register(Component.For<IGamePredictorWindow>()
                .ImplementedBy<GamePredictorWindow>());
            Container.Register(Component.For<IGamePredictorWindowViewModel>()
                .ImplementedBy<GamePredictorWindowViewModel>());
            Container.Register(Component.For<ISharedService>()
                .ImplementedBy<SharedService>());
            Container.Register(Component.For<IMainWindowService>()
                .ImplementedBy<MainWindowService>());
            Container.Register(Component.For<ITeamSeasonsControlService>()
                .ImplementedBy<TeamSeasonsControlService>());
            Container.Register(Component.For<ISeasonStandingsControlService>()
                .ImplementedBy<SeasonStandingsControlService>());
            Container.Register(Component.For<IRankingsControlService>()
                .ImplementedBy<RankingsControlService>());
            Container.Register(Component.For<IGamesWindowService>()
                .ImplementedBy<GamesWindowService>());
            Container.Register(Component.For<IGamePredictorWindowService>()
                .ImplementedBy<GamePredictorWindowService>());
            Container.Register(Component.For<IRepository<League>>()
                .ImplementedBy<LeagueRepository>());
            Container.Register(Component.For<IRepository<Conference>>()
                .ImplementedBy<ConferenceRepository>());
            Container.Register(Component.For<IRepository<Division>>()
                .ImplementedBy<DivisionRepository>());
            Container.Register(Component.For<IRepository<Team>>()
                .ImplementedBy<TeamRepository>());
            Container.Register(Component.For<IRepository<Game>>()
                .ImplementedBy<GameRepository>());
            Container.Register(Component.For<IRepository<Season>>()
                .ImplementedBy<SeasonRepository>());
            Container.Register(Component.For<IRepository<LeagueSeason>>()
                .ImplementedBy<LeagueSeasonRepository>());
            Container.Register(Component.For<IRepository<TeamSeason>>()
                .ImplementedBy<TeamSeasonRepository>());
            Container.Register(Component.For<IRepository<WeekCount>>()
                .ImplementedBy<WeekCountRepository>());
            Container.Register(Component.For<IStoredProcedureRepository>()
                .ImplementedBy<StoredProcedureRepository>());
            //Container.Register(Component.For<IProFootballDbContext>()
            //    .ImplementedBy<ProFootballEntities>());
            Container.Register(Component.For<ProFootballEntities>()
                .ImplementedBy<ProFootballEntities>());
            Container.Register(Component.For<ICalculator>()
                .ImplementedBy<Calculator>());
        }

        internal struct Constants
        {
            internal const string AppName = "Football Application";

            internal const string AdjustedScoresErrorMessage = 
                "Unless the game is a forfeit, the winning team must be awarded a higher adjusted score than the losing team.";
            internal const string BothTeamsNeededErrorMessage = 
                "Please enter names for both teams.";
            internal const string DifferentTeamsNeededErrorMessage = 
                "Please enter a different name for each team.";
            internal const string TeamNotInDatabaseMessage = 
                "Please make sure that both teams are in the NFL and that both team names are spelled correctly.";
            internal const string TieScoresNotPermittedErrorMessage = 
                "Tie scores are not permitted for playoff games.";
        }

        /// <summary>
        /// Shows a DataValidationException message.
        /// </summary>
        /// <param name = "message"></param>
        public virtual void ShowExceptionMessage(DataValidationException ex)
        {
            ShowExceptionMessage(ex, "DataValidationException");
        }

        /// <summary>
        /// Shows the message for any exception of a type that doesn't have its own specific ShowMessage method.
        /// </summary>
        /// <param name = "message"></param>
        /// <param name = "caption"></param>
        public virtual void ShowExceptionMessage(Exception ex, string caption = "Exception")
        {
            MessageBox.Show(ex.Message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
