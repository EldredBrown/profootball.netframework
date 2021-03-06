using System.Windows.Controls;
using EldredBrown.ProFootball.WpfApp.ViewModels;

namespace EldredBrown.ProFootball.WpfApp.UserControls
{
    public interface ISeasonStandingsControl
    {
        void InitializeComponent();
        void Refresh();
    }

    /// <summary>
    /// Interaction logic for StandingsControl.xaml
    /// </summary>
    public partial class SeasonStandingsControl : UserControl, ISeasonStandingsControl
    {
        /// <summary>
        /// Initializes a new instance of the StandingsControl class
        /// </summary>
        public SeasonStandingsControl()
		{
			InitializeComponent();

            DataContext = WpfGlobals.Container.Resolve<ISeasonStandingsControlViewModel>();
        }

        /// <summary>
        /// Refreshes the view of this StandingsControl object
        /// </summary>
        public void Refresh()
        {
            (DataContext as ISeasonStandingsControlViewModel).ViewStandingsCommand.Execute(null);
        }
    }
}
