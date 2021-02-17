using System;
using System.Windows;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using log4net;

namespace EldredBrown.ProFootball.WpfApp.Services
{
    public interface ISharedService
    {
        TeamSeason FindTeamSeason(string teamName, int seasonID);

        void SaveChanges(ProFootballEntities dbContext);

        void ShowExceptionMessage(ArgumentNullException ex);
        void ShowExceptionMessage(DataValidationException ex);
        void ShowExceptionMessage(Exception ex, string caption = "Exception");

        MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button,
            MessageBoxImage icon);
    }

    /// <summary>
    /// Service class shared by many control objects
    /// </summary>
    public class SharedService : ISharedService
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IRepository<TeamSeason> _teamSeasonRepository;

        /// <summary>
        /// Initializes a new instance of the SharedService class
        /// </summary>
        /// <param name="teamSeasonRepository"></param>
        public SharedService(IRepository<TeamSeason> teamSeasonRepository)
        {
            _teamSeasonRepository = teamSeasonRepository;
        }

        /// <summary>
        /// Finds a TeamSeason by TeamName and SeasonID
        /// </summary>
        /// <param name="teamName">The TeamName of the TeamSeason object to be found</param>
        /// <param name="seasonID">The SeasonID of the TeamSeason object to be found</param>
        /// <returns>The TeamSeason object with the matching TeamName and SeasonID</returns>
        public TeamSeason FindTeamSeason(string teamName, int seasonID)
        {
            try
            {
                return _teamSeasonRepository.FindEntity(teamName, seasonID);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Save all changes to the data store
        /// </summary>
        /// <param name="dbContext">The data store object representing the database to which changes will be saved</param>
        public void SaveChanges(ProFootballEntities dbContext)
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Shows an ArgumentNullException message.
        /// </summary>
        /// <param name="ex">The exception for which a message will be shown</param>
        public void ShowExceptionMessage(ArgumentNullException ex)
        {
            ShowExceptionMessage(ex, "ArgumentNullException");
        }

        /// <summary>
        /// Shows a DataValidationException message.
        /// </summary>
        /// <param name="ex">The exception for which a message will be shown</param>
        public void ShowExceptionMessage(DataValidationException ex)
        {
            ShowExceptionMessage(ex, "DataValidationException");
        }

        /// <summary>
        /// Shows the message for any exception of a type that doesn't have its own specific ShowMessage method.
        /// </summary>
        /// <param name="ex">The exception for which a message will be shown</param>
        /// <param name="caption">The MessageBox caption</param>
        public void ShowExceptionMessage(Exception ex, string caption = "Exception")
        {
            MessageBox.Show(ex.Message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Shows a message box with the specified arguments
        /// </summary>
        /// <param name="messageBoxText">Message box text</param>
        /// <param name="caption">Message box caption</param>
        /// <param name="button">Message box button</param>
        /// <param name="icon">Message box icon</param>
        /// <returns></returns>
        public MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button,
            MessageBoxImage icon)
        {
            return MessageBox.Show(messageBoxText, caption, button, icon);
        }
    }
}
