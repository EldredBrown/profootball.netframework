using System.Windows;
using EldredBrown.ProFootball.WpfApp.ViewModels;

namespace EldredBrown.ProFootball.WpfApp.Windows
{
    public interface IGamePredictorWindow
    {
        void InitializeComponent();
        void Show();
    }

    /// <summary>
    /// Interaction logic for GamePredictorWindow.xaml
    /// </summary>
    public partial class GamePredictorWindow : Window, IGamePredictorWindow
    {
        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the GamePredictorWindow class
        /// </summary>
        public GamePredictorWindow()
        {
            InitializeComponent();

            DataContext = WpfGlobals.Container.Resolve<IGamePredictorWindowViewModel>();
        }

        #endregion Constructors & Finalizers
    }
}
