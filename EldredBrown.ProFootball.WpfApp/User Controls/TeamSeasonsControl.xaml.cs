using System.Windows.Controls;
using EldredBrown.ProFootball.WpfApp.ViewModels;

namespace EldredBrown.ProFootball.WpfApp.UserControls
{
    public interface ITeamSeasonsControl
    {
        void InitializeComponent();
        void Refresh();
    }

    /// <summary>
    /// Interaction logic for TeamsControl.xaml
    /// </summary>
    public partial class TeamSeasonsControl : UserControl, ITeamSeasonsControl
    {
        /// <summary>
        /// Initializes a new instance of the TeamsControl class
        /// </summary>
        public TeamSeasonsControl()
		{
            InitializeComponent();

            DataContext = WpfGlobals.Container.Resolve<ITeamSeasonsControlViewModel>();
        }

        /// <summary>
        /// Refreshes the view of this TeamsControl object
        /// </summary>
        public void Refresh()
        {
            (DataContext as ITeamSeasonsControlViewModel).ViewTeamsCommand.Execute(null);
        }
    }
}
