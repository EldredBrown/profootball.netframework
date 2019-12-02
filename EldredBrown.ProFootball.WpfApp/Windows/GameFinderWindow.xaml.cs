using System.Windows;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;

namespace EldredBrown.ProFootball.WpfApp.Windows
{
    public interface IGameFinderWindow
    {
        object DataContext { get; set; }

        void InitializeComponent();
        bool? ShowDialog();
    }

    /// <summary>
    /// Interaction logic for GameFinder.xaml
    /// </summary>
    public partial class GameFinderWindow : Window, IGameFinderWindow
    {
        #region Member Fields

        private readonly ISharedService _sharedService;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the GameFinderWindow class
        /// </summary>
        public GameFinderWindow()
		{
			InitializeComponent();

		    _sharedService = WpfGlobals.Container.Resolve<ISharedService>();

		    DataContext = WpfGlobals.Container.Resolve<IGameFinderWindowViewModel>();
		}

        #endregion Constructors & Finalizers

        #region Methods

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed
        /// </summary>
        /// <returns></returns>
        public new bool? ShowDialog()
        {
            return base.ShowDialog();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event on the OK button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (DataContext as GameFinderWindowViewModel);

            try
            {
                // The GuestName and HostName properties in the underlying ViewModel are not updated automatically when a press
                // of the Enter key clicks the OK button automatically, so we need to update these directly as follows: 
                vm.GuestName = GuestTextBox.Text;
                vm.HostName = HostTextBox.Text;
                vm.ValidateDataEntry();
            }
            catch (DataValidationException ex)
            {
                _sharedService.ShowExceptionMessage(ex);
                GuestTextBox.Focus();
                return;
            }

            DialogResult = true;
        }

        /// <summary>
        /// Handles the Click event on the Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion Event Handlers
    }
}
