using System;
using System.Collections.ObjectModel;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Services;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public interface IMainWindowViewModel : IViewModelBase
    {
        DelegateCommand PredictGameScoreCommand { get; }
        ReadOnlyCollection<int> Seasons { get; set; }
        int SelectedSeason { get; set; }
        DelegateCommand ViewSeasonsCommand { get; }
        DelegateCommand WeeklyUpdateCommand { get; }
    }

    /// <summary>
    /// ViewModel logic for the MainWindow class.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        #region Member Fields

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMainWindowService _windowService;

        #endregion Member Fields

        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel(IMainWindowService windowService)
            :base(null)
		{
            _windowService = windowService;
        }

        #endregion Constructors & Finalizers

        #region Properties

        /// <summary>
        /// Gets or sets this window's seasons collection.
        /// </summary>
        private ReadOnlyCollection<int> _seasons;
        public ReadOnlyCollection<int> Seasons
        {
            get
            {
                return _seasons;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Seasons");
                }
                else if (value != _seasons)
                {
                    _seasons = value;
                    OnPropertyChanged("Seasons");
                    RequestUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets this window's selected season.
        /// </summary>
        public int SelectedSeason
        {
            get
            {
                return (int)WpfGlobals.SelectedSeason;
            }
            set
            {
                if (value != WpfGlobals.SelectedSeason)
                {
                    WpfGlobals.SelectedSeason = value;
                    OnPropertyChanged("SelectedSeason");
                }
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Opens the GamePredictorWindow.
        /// </summary>
        private DelegateCommand _predictGameScoreCommand;
        public DelegateCommand PredictGameScoreCommand
        {
            get
            {
                if (_predictGameScoreCommand == null)
                {
                    _predictGameScoreCommand = new DelegateCommand(param => PredictGameScore());
                }
                return _predictGameScoreCommand;
            }
        }
        private void PredictGameScore()
        {
            try
            {
                _windowService.PredictGameScore();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Runs a weekly update.
        /// </summary>
        private DelegateCommand _weeklyUpdateCommand;
        public DelegateCommand WeeklyUpdateCommand
        {
            get
            {
                if (_weeklyUpdateCommand == null)
                {
                    _weeklyUpdateCommand = new DelegateCommand(param => RunWeeklyUpdate(),
                         param => { return _windowService.GetGameCount(WpfGlobals.SelectedSeason) > 0; });
                }
                return _weeklyUpdateCommand;
            }
        }
        private void RunWeeklyUpdate()
        {
            try
            {
                _windowService.RunWeeklyUpdate();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Loads the DataModel's Seasons table.
        /// </summary>
        private DelegateCommand _viewSeasonsCommand;
        public DelegateCommand ViewSeasonsCommand
        {
            get
            {
                if (_viewSeasonsCommand == null)
                {
                    _viewSeasonsCommand = new DelegateCommand(param => ViewSeasons());
                }
                return _viewSeasonsCommand;
            }
        }
        private void ViewSeasons()
        {
            try
            {
                Seasons = new ReadOnlyCollection<int>(_windowService.GetAllSeasonIds().ToList());
                SelectedSeason = 1920;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                throw;
            }
        }

        #endregion Commands
    }
}
