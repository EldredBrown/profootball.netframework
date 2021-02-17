using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using EldredBrown.ProFootball.WpfApp.ViewModels;

namespace EldredBrown.ProFootball.WpfApp.Windows
{
    public interface IGamesWindow
    {
        void InitializeComponent();
        bool? ShowDialog();
    }

    /// <summary>
    /// Interaction logic for Games.xaml
    /// </summary>
    public partial class GamesWindow : Window, IGamesWindow
    {
        /// <summary>
        /// Initializes a new instance of the GamesWindow class
        /// </summary>
        public GamesWindow()
		{
			InitializeComponent();

		    DataContext = WpfGlobals.Container.Resolve<IGamesWindowViewModel>();
		}

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed
        /// </summary>
        /// <returns></returns>
        public new bool? ShowDialog()
        {
            return base.ShowDialog();
        }

        /// <summary>
        /// Handles the SelectionChanged event for the GamesDataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GamesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GamesDataGrid.SelectedItem == CollectionView.NewItemPlaceholder)
            {
                // Prepare to add a new game.
                (DataContext as GamesWindowViewModel).SelectedGame = null;
            }
        }
    }
}
