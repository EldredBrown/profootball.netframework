using System.Windows;
using System.Windows.Controls;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;

namespace EldredBrown.ProFootball.WpfApp
{
    public interface IMainWindow
    {
        void InitializeComponent();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        #region Member Fields

        private readonly IMainWindowService _service;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
		{
		    WpfGlobals.RegisterDependencies();

            InitializeComponent();

		    // Resolve objects of needed types (ask the WpfGlobals.Container for an instance).
		    // This is analagous to calling new() in a non-IoC application.
            _service = WpfGlobals.Container.Resolve<IMainWindowService>();

            DataContext = WpfGlobals.Container.Resolve<IMainWindowViewModel>();
        }

        #endregion Constructors & Finalizers

        #region Private Event Handlers

        /// <summary>
        /// Handles the SelectionChanged event on the SeasonsComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeasonsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TeamSeasonsControl.Refresh();
            SeasonStandingsControl.Refresh();
            RankingsControl.Refresh();
        }

        /// <summary>
        /// Handles the Click event on the ShowGamesButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowGamesButton_Click(object sender, RoutedEventArgs e)
        {
            _service.ShowGames();
            this.TeamSeasonsControl.Refresh();
        }

        #endregion Private Event Handlers
    }
}
