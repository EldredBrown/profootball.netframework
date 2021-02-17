using System.Windows.Controls;
using EldredBrown.ProFootball.WpfApp.ViewModels;

namespace EldredBrown.ProFootball.WpfApp.UserControls
{
    public interface IRankingsControl
    {
        void InitializeComponent();
        void Refresh();
    }

    /// <summary>
    /// Interaction logic for RankingsControl.xaml
    /// </summary>
    public partial class RankingsControl : UserControl, IRankingsControl
    {
        /// <summary>
        /// Initializes a new instance of the RankingsControl class
        /// </summary>
        public RankingsControl()
		{
			InitializeComponent();

            DataContext = WpfGlobals.Container.Resolve<IRankingsControlViewModel>();
        }

        /// <summary>
        /// Refreshes the view of this RankingsControl object
        /// </summary>
        public void Refresh()
        {
            (DataContext as IRankingsControlViewModel).ViewRankingsCommand.Execute(null);
        }
    }
}
