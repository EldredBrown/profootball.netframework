using System;
using System.Collections.ObjectModel;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootballWPF.Tests.ViewModels
{
    [TestFixture]
    public class MainWindowViewModelTest
    {
        #region Member Fields

        private IMainWindowService _windowService;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _windowService = A.Fake<IMainWindowService>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void PredictGameScoreCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new MainWindowViewModel(_windowService);

            // Act
            // Call method under test.
            viewModel.PredictGameScoreCommand.Execute(null);

            // Assert
            // Assert results of call to method under test.
            A.CallTo(() => _windowService.PredictGameScore()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void PredictGameScoreCommand_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new MainWindowViewModel(_windowService);

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _windowService.PredictGameScore()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => viewModel.PredictGameScoreCommand.Execute(null));
        }

        [TestCase]
        public void WeeklyUpdateCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new MainWindowViewModel(_windowService);

            // Act
            // Call method under test.
            viewModel.WeeklyUpdateCommand.Execute(null);

            // Assert
            // Assert results of call to method under test.
            A.CallTo(() => _windowService.RunWeeklyUpdate()).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void WeeklyUpdateCommand_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new MainWindowViewModel(_windowService);

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _windowService.RunWeeklyUpdate()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => viewModel.WeeklyUpdateCommand.Execute(null));
        }

        [TestCase]
        public void ViewSeasonsCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new MainWindowViewModel(_windowService);

            // Act
            viewModel.ViewSeasonsCommand.Execute(null);

            // Assert
            A.CallTo(() => _windowService.GetAllSeasonIds()).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<int>>(viewModel.Seasons);
            Assert.AreEqual(1920, viewModel.SelectedSeason);
        }

        [TestCase]
        public void ViewSeasonsCommand_ExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new MainWindowViewModel(_windowService);

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _windowService.GetAllSeasonIds()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => viewModel.ViewSeasonsCommand.Execute(null));
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            // TODO: Instantiate class under test.

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }

        #endregion Test Cases
    }
}
