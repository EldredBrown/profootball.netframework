using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using EldredBrown.ProFootball.WpfApp;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using EldredBrown.ProFootball.WpfApp.Windows;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootballWPF.Tests.ViewModels
{
    [TestFixture]
    public class GamesWindowViewModelTest
    {
        #region Member Fields

        private ISharedService _sharedService;
        private IGamesWindowService _controlService;
        private IGameFinderWindow _gameFinderWindow;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _controlService = A.Fake<IGamesWindowService>();

            _gameFinderWindow = A.Fake<IGameFinderWindow>();
            _gameFinderWindow.DataContext = A.Fake<IGameFinderWindowViewModel>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void SelectedGame_ValueEqualsNull_ClearsDataEntryControls()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Act
            viewModel.SelectedGame = null;

            // Assert
            #region ClearDataEntryControls();

            Assert.AreEqual(String.Empty, viewModel.GuestName);
            Assert.AreEqual(0, viewModel.GuestScore);
            Assert.AreEqual(String.Empty, viewModel.HostName);
            Assert.AreEqual(0, viewModel.HostScore);
            Assert.IsFalse(viewModel.IsPlayoffGame);
            Assert.AreEqual(String.Empty, viewModel.Notes);

            Assert.AreEqual(Visibility.Visible, viewModel.AddGameControlVisibility);
            Assert.AreEqual(Visibility.Hidden, viewModel.EditGameControlVisibility);
            Assert.AreEqual(Visibility.Hidden, viewModel.DeleteGameControlVisibility);

            #endregion ClearDataEntryControls();
        }

        [TestCase]
        public void SelectedGame_ValueDoesNotEqualNull_PopulatesDataEntryControls()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Act
            viewModel.SelectedGame = new Game();

            // Assert
            #region PopulateDataEntryControls();

            var selectedGame = viewModel.SelectedGame;
            Assert.AreEqual(selectedGame.Week, viewModel.Week);
            Assert.AreEqual(selectedGame.GuestName, viewModel.GuestName);
            Assert.AreEqual(selectedGame.GuestScore, viewModel.GuestScore);
            Assert.AreEqual(selectedGame.HostName, viewModel.HostName);
            Assert.AreEqual(selectedGame.HostScore, viewModel.HostScore);
            Assert.AreEqual(selectedGame.IsPlayoffGame, viewModel.IsPlayoffGame);
            Assert.AreEqual(selectedGame.Notes, viewModel.Notes);

            Assert.AreEqual(Visibility.Hidden, viewModel.AddGameControlVisibility);
            Assert.AreEqual(Visibility.Visible, viewModel.EditGameControlVisibility);
            Assert.AreEqual(Visibility.Visible, viewModel.DeleteGameControlVisibility);

            #endregion PopulateDataEntryControls();
        }

        [TestCase]
        public void AddGameCommand_GuestNameIsNull_ShowsExceptionMessageAndAborts()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                GuestName = null,
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(null);
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(null);

            // Act
            viewModel.AddGameCommand.Execute(null);

            // Assert
            var ex = new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsEqualTo(ex)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _controlService.AddGame(A<Game>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null)).MustNotHaveHappened();
        }

        [TestCase]
        public void AddGameCommand_GuestNameIsEmpty_ShowsExceptionMessageAndAborts()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                GuestName = String.Empty,
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(null);
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(null);

            // Act
            viewModel.AddGameCommand.Execute(null);

            // Assert
            var ex = new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsEqualTo(ex)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _controlService.AddGame(A<Game>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null)).MustNotHaveHappened();
        }

        [TestCase]
        public void AddGameCommand_GuestNameIsWhitespace_ShowsExceptionMessageAndAborts()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                GuestName = "   ",
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(null);
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(null);

            // Act
            viewModel.AddGameCommand.Execute(null);

            // Assert
            var ex = new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsEqualTo(ex)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _controlService.AddGame(A<Game>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null)).MustNotHaveHappened();
        }

        [TestCase]
        public void AddGameCommand_HostNameIsNull_ShowsExceptionMessageAndAborts()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                GuestName = "Guest",
                HostName = null
            };

            // Set up needed infrastructure of class under test.
            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(null);
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(null);

            // Act
            viewModel.AddGameCommand.Execute(null);

            // Assert
            var ex = new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsEqualTo(ex)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _controlService.AddGame(A<Game>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null)).MustNotHaveHappened();
        }

        [TestCase]
        public void AddGameCommand_HostNameIsEmpty_ShowsExceptionMessageAndAborts()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                GuestName = "Guest",
                HostName = String.Empty
            };

            // Set up needed infrastructure of class under test.
            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(null);
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(null);

            // Act
            viewModel.AddGameCommand.Execute(null);

            // Assert
            var ex = new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsEqualTo(ex)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _controlService.AddGame(A<Game>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null)).MustNotHaveHappened();
        }

        [TestCase]
        public void AddGameCommand_HostNameIsWhitespace_ShowsExceptionMessageAndAborts()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                GuestName = "Guest",
                HostName = "   "
            };

            // Set up needed infrastructure of class under test.
            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(null);
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(null);

            // Act
            viewModel.AddGameCommand.Execute(null);

            // Assert
            var ex = new DataValidationException(WpfGlobals.Constants.BothTeamsNeededErrorMessage);
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsEqualTo(ex)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _controlService.AddGame(A<Game>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null)).MustNotHaveHappened();
        }

        [TestCase]
        public void AddGameCommand_GuestNameEqualsHostName_ShowsExceptionMessageAndAborts()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                GuestName = "Team",
                HostName = "Team"
            };

            // Set up needed infrastructure of class under test.
            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(null);
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(null);

            // Act
            viewModel.AddGameCommand.Execute(null);

            // Assert
            var ex = new DataValidationException(WpfGlobals.Constants.DifferentTeamsNeededErrorMessage);
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsEqualTo(ex)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _controlService.AddGame(A<Game>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null)).MustNotHaveHappened();
        }

        //[TestCase]
        //public void AddGameCommand_GuestSeasonIsNull_ShowsExceptionMessageAndAborts()
        //{
        //    // Arrange
        //    // Instantiate class under test.
        //    var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
        //    {
        //        // Set up needed infrastructure of class under test.
        //        GuestName = "Guest",
        //        HostName = "Host"
        //    };

        //    // Set up needed infrastructure of class under test.
        //    var seasonID = (int)WpfGlobals.SelectedSeason;
        //    A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(null);
        //    A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(new TeamSeason());

        //    // Act
        //    viewModel.AddGameCommand.Execute(null);

        //    // Assert
        //    var ex = new DataValidationException(WpfGlobals.Constants.TeamNotInDatabaseMessage);
        //    A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsEqualTo(ex)))
        //        .MustHaveHappenedOnceExactly();

        //    A.CallTo(() => _controlService.AddGame(A<Game>.Ignored)).MustNotHaveHappened();
        //    A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null)).MustNotHaveHappened();
        //}

        //[TestCase]
        //public void AddGameCommand_HostSeasonIsNull_ShowsExceptionMessageAndAborts()
        //{
        //    // Arrange
        //    // Instantiate class under test.
        //    var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
        //    {
        //        // Set up needed infrastructure of class under test.
        //        GuestName = "Guest",
        //        HostName = "Host"
        //    };

        //    // Set up needed infrastructure of class under test.
        //    var seasonID = (int)WpfGlobals.SelectedSeason;
        //    A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(new TeamSeason());
        //    A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(null);

        //    // Act
        //    viewModel.AddGameCommand.Execute(null);

        //    // Assert
        //    var ex = new DataValidationException(WpfGlobals.Constants.TeamNotInDatabaseMessage);
        //    A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsEqualTo(ex)))
        //        .MustHaveHappenedOnceExactly();

        //    A.CallTo(() => _controlService.AddGame(A<Game>.Ignored)).MustNotHaveHappened();
        //    A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null)).MustNotHaveHappened();
        //}

        [TestCase]
        public void AddGameCommand_NoDataValidationExceptionCaught_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                GuestName = "Guest",
                HostName = "Host",
                SelectedGame = new Game
                {
                    SeasonID = (int)WpfGlobals.SelectedSeason,
                    Week = 1,
                    GuestName = "Guest",
                    GuestScore = 0,
                    HostName = "Host",
                    HostScore = 0,
                    IsPlayoffGame = false,
                    Notes = "Notes"
                }
            };

            // Set up needed infrastructure of class under test.
            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).Returns(new TeamSeason());
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).Returns(new TeamSeason());

            var selectedGame = viewModel.SelectedGame;
            var newGame = new Game
            {
                SeasonID = (int)WpfGlobals.SelectedSeason,
                Week = selectedGame.Week,
                GuestName = selectedGame.GuestName,
                GuestScore = selectedGame.GuestScore,
                HostName = selectedGame.HostName,
                HostScore = selectedGame.HostScore,
                IsPlayoffGame = selectedGame.IsPlayoffGame,
                Notes = selectedGame.Notes
            };

            // Act
            viewModel.AddGameCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.Ignored))
                .MustNotHaveHappened();
            A.CallTo(() => _controlService.AddGame(A<Game>.That.IsEqualTo(newGame))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<Game>>(viewModel.Games);
            Assert.IsNull(viewModel.SelectedGame);
        }

        [TestCase]
        public void AddGameCommand_GenericExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // TODO: Define argument variables of method under test.

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _sharedService.FindTeamSeason(A<string>.Ignored, A<int>.Ignored)).Throws(ex);

            // Act
            viewModel.AddGameCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Message"));
        }

        [TestCase]
        public void DeleteGameCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                SelectedGame = new Game
                {
                    SeasonID = (int)WpfGlobals.SelectedSeason,
                    Week = 1,
                    GuestName = "Guest",
                    GuestScore = 0,
                    HostName = "Host",
                    HostScore = 0,
                    IsPlayoffGame = false,
                    Notes = "Notes"
                }
            };

            // Set up needed infrastructure of class under test.
            var selectedGame = viewModel.SelectedGame;
            var oldGame = new Game
            {
                ID = selectedGame.ID,
                SeasonID = (int)WpfGlobals.SelectedSeason,
                Week = selectedGame.Week,
                GuestName = selectedGame.GuestName,
                GuestScore = selectedGame.GuestScore,
                HostName = selectedGame.HostName,
                HostScore = selectedGame.HostScore,
                IsPlayoffGame = selectedGame.IsPlayoffGame,
                Notes = selectedGame.Notes
            };

            // Act
            viewModel.DeleteGameCommand.Execute(null);

            // Assert
            A.CallTo(() => _controlService.DeleteGame(A<Game>.That.IsEqualTo(oldGame))).MustHaveHappenedOnceExactly();

            Assert.IsNull(viewModel.SelectedGame);

            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<Game>>(viewModel.Games);
        }

        [TestCase]
        public void DeleteGameCommand_ExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                SelectedGame = new Game
                {
                    SeasonID = (int)WpfGlobals.SelectedSeason,
                    Week = 1,
                    GuestName = "Guest",
                    GuestScore = 0,
                    HostName = "Host",
                    HostScore = 0,
                    IsPlayoffGame = false,
                    Notes = "Notes"
                }
            };

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _controlService.DeleteGame(A<Game>.Ignored)).Throws(ex);

            // Act
            viewModel.DeleteGameCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void EditGameCommand_DataValidationExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                GuestName = null,
                HostName = null
            };

            // Act
            viewModel.EditGameCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.That.IsNotNull()));
        }

        [TestCase]
        public void EditGameCommand_IsFindGameFilterApplied_CallsApplyFindGameFilter()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                IsFindGameFilterApplied = true,
                SelectedGame = new Game
                {
                    SeasonID = (int)WpfGlobals.SelectedSeason,
                    Week = 1,
                    GuestName = "Guest",
                    GuestScore = 0,
                    HostName = "Host",
                    HostScore = 0,
                    IsPlayoffGame = false,
                    Notes = "Notes"
                }
            };

            // Set up needed infrastructure of class under test.
            var selectedGame = viewModel.SelectedGame;

            var oldGame = new Game
            {
                ID = selectedGame.ID,
                SeasonID = (int)WpfGlobals.SelectedSeason,
                Week = selectedGame.Week,
                GuestName = selectedGame.GuestName,
                GuestScore = selectedGame.GuestScore,
                HostName = selectedGame.HostName,
                HostScore = selectedGame.HostScore,
                IsPlayoffGame = selectedGame.IsPlayoffGame,
                Notes = selectedGame.Notes
            };

            var newGame = new Game
            {
                SeasonID = (int)WpfGlobals.SelectedSeason,
                Week = selectedGame.Week,
                GuestName = selectedGame.GuestName,
                GuestScore = selectedGame.GuestScore,
                HostName = selectedGame.HostName,
                HostScore = selectedGame.HostScore,
                IsPlayoffGame = selectedGame.IsPlayoffGame,
                Notes = selectedGame.Notes
            };

            // Act
            viewModel.EditGameCommand.Execute(null);

            // Assert
            #region ValidateDataEntry();

            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).MustHaveHappenedOnceExactly();

            #endregion ValidateDataEntry();

            A.CallTo(() => _controlService.EditGame(A<Game>.That.IsEqualTo(oldGame), A<Game>.That.IsEqualTo(newGame)))
                .MustHaveHappenedOnceExactly();

            #region ApplyFindGameFilter();

            var dataContext = viewModel.GameFinder.DataContext as IGameFinderWindowViewModel;

            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, dataContext.GuestName,
                dataContext.HostName)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<Game>>(viewModel.Games);
            Assert.IsTrue(viewModel.IsFindGameFilterApplied);

            #endregion ApplyFindGameFilter();
        }

        [TestCase]
        public void EditGameCommand_NotIsFindGameFilterApplied_SetsGamesProperty()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                // Set up needed infrastructure of class under test.
                IsFindGameFilterApplied = false,
                SelectedGame = new Game
                {
                    SeasonID = (int)WpfGlobals.SelectedSeason,
                    Week = 1,
                    GuestName = "Guest",
                    GuestScore = 0,
                    HostName = "Host",
                    HostScore = 0,
                    IsPlayoffGame = false,
                    Notes = "Notes"
                }
            };

            // Set up needed infrastructure of class under test.
            var selectedGame = viewModel.SelectedGame;

            var oldGame = new Game
            {
                ID = selectedGame.ID,
                SeasonID = (int)WpfGlobals.SelectedSeason,
                Week = selectedGame.Week,
                GuestName = selectedGame.GuestName,
                GuestScore = selectedGame.GuestScore,
                HostName = selectedGame.HostName,
                HostScore = selectedGame.HostScore,
                IsPlayoffGame = selectedGame.IsPlayoffGame,
                Notes = selectedGame.Notes
            };

            var newGame = new Game
            {
                SeasonID = (int)WpfGlobals.SelectedSeason,
                Week = selectedGame.Week,
                GuestName = selectedGame.GuestName,
                GuestScore = selectedGame.GuestScore,
                HostName = selectedGame.HostName,
                HostScore = selectedGame.HostScore,
                IsPlayoffGame = selectedGame.IsPlayoffGame,
                Notes = selectedGame.Notes
            };

            // Act
            viewModel.EditGameCommand.Execute(null);

            // Assert
            #region ValidateDataEntry();

            var seasonID = (int)WpfGlobals.SelectedSeason;
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, seasonID)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, seasonID)).MustHaveHappenedOnceExactly();

            #endregion ValidateDataEntry();

            A.CallTo(() => _controlService.EditGame(A<Game>.That.IsEqualTo(oldGame), A<Game>.That.IsEqualTo(newGame)))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<Game>>(viewModel.Games);
        }

        [TestCase]
        public void EditGameCommand_GenericExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _sharedService.FindTeamSeason(A<string>.Ignored, A<int>.Ignored)).Throws(ex);

            // Act
            viewModel.EditGameCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception"));
        }

        [TestCase]
        public void FindGameCommand_DialogResultIsTrueAndGamesCountIsZero_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Set up needed infrastructure of class under test.
            A.CallTo(() => viewModel.GameFinder.ShowDialog()).Returns(true);

            viewModel.Games = new ReadOnlyCollection<Game>(new List<Game>());
            viewModel.SelectedGame = new Game();

            // Act
            viewModel.FindGameCommand.Execute(null);

            // Assert
            A.CallTo(() => viewModel.GameFinder.ShowDialog()).MustHaveHappenedOnceExactly();

            #region ApplyFindGameFilter();

            var dataContext = viewModel.GameFinder.DataContext as IGameFinderWindowViewModel;

            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, dataContext.GuestName,
                dataContext.HostName)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<Game>>(viewModel.Games);
            Assert.IsTrue(viewModel.IsFindGameFilterApplied);

            #endregion ApplyFindGameFilter();

            Assert.IsTrue(viewModel.IsGamesReadOnly);
            Assert.IsTrue(viewModel.IsShowAllGamesEnabled);
            Assert.IsNull(viewModel.SelectedGame);
            Assert.AreEqual(Visibility.Hidden, viewModel.AddGameControlVisibility);
        }

        [TestCase]
        public void FindGameCommand_DialogResultIsTrueAndGamesCountIsNotZero_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Set up needed infrastructure of class under test.
            A.CallTo(() => viewModel.GameFinder.ShowDialog()).Returns(true);

            var games = new List<Game>
            {
                new Game()
            };
            viewModel.Games = new ReadOnlyCollection<Game>(games);

            A.CallTo(() => _controlService.GetGames(
                (int)WpfGlobals.SelectedSeason, A<string>.Ignored, A<string>.Ignored)).Returns(games);

            var game = new Game();
            viewModel.SelectedGame = game;

            // Act
            viewModel.FindGameCommand.Execute(null);

            // Assert
            A.CallTo(() => viewModel.GameFinder.ShowDialog()).MustHaveHappenedOnceExactly();

            #region ApplyFindGameFilter();

            var dataContext = viewModel.GameFinder.DataContext as IGameFinderWindowViewModel;

            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, dataContext.GuestName,
                dataContext.HostName)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<Game>>(viewModel.Games);
            Assert.IsTrue(viewModel.IsFindGameFilterApplied);

            #endregion ApplyFindGameFilter();

            Assert.IsTrue(viewModel.IsGamesReadOnly);
            Assert.IsTrue(viewModel.IsShowAllGamesEnabled);
            Assert.AreSame(game, viewModel.SelectedGame);
            Assert.AreEqual(Visibility.Hidden, viewModel.AddGameControlVisibility);
        }

        [TestCase]
        public void FindGameCommand_DialogResultIsFalse_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow)
            {
                AddGameControlVisibility = Visibility.Visible,
                SelectedGame = null
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => viewModel.GameFinder.ShowDialog()).Returns(false);

            var game = new Game();
            viewModel.SelectedGame = game;

            // Act
            viewModel.FindGameCommand.Execute(null);

            // Assert
            A.CallTo(() => viewModel.GameFinder.ShowDialog()).MustHaveHappenedOnceExactly();

            Assert.IsFalse(viewModel.IsGamesReadOnly);
            Assert.IsFalse(viewModel.IsShowAllGamesEnabled);
            Assert.AreSame(game, viewModel.SelectedGame);
        }

        [TestCase]
        public void FindGameCommand_ExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => viewModel.GameFinder.ShowDialog()).Throws(ex);

            // Act
            viewModel.FindGameCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void ShowAllGamesCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Set up needed infrastructure of class under test.
            var weekCount = 1;
            A.CallTo(() => _controlService.GetWeekCount()).Returns(weekCount);

            // Act
            viewModel.ShowAllGamesCommand.Execute(null);

            // Assert
            #region ViewGames();

            // Load the DataModel's Games table.
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<Game>>(viewModel.Games);
            Assert.IsNull(viewModel.SelectedGame);

            A.CallTo(() => _controlService.GetWeekCount()).MustHaveHappenedOnceExactly();

            Assert.AreEqual(weekCount, viewModel.Week);

            #endregion ViewGames();

            Assert.IsFalse(viewModel.IsFindGameFilterApplied);
            Assert.IsFalse(viewModel.IsGamesReadOnly);
            Assert.IsFalse(viewModel.IsShowAllGamesEnabled);
            Assert.IsNull(viewModel.SelectedGame);
        }

        [TestCase]
        public void ShowAllGamesCommand_ExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _sharedService.ShowExceptionMessage(A<Exception>.Ignored, "Exception")).Throws(ex);

            // Act
            viewModel.ShowAllGamesCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception"));
        }

        [TestCase]
        public void ViewGamesCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Set up needed infrastructure of class under test.
            var week = 1;
            A.CallTo(() => _controlService.GetWeekCount()).Returns(week);

            // Act
            viewModel.ViewGamesCommand.Execute(null);

            // Assert
            A.CallTo(() => _controlService.GetGames((int)WpfGlobals.SelectedSeason, null, null))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<Game>>(viewModel.Games);
            Assert.IsNull(viewModel.SelectedGame);

            A.CallTo(() => _controlService.GetWeekCount()).MustHaveHappenedOnceExactly();

            Assert.AreEqual(week, viewModel.Week);
        }

        [TestCase]
        public void ViewGamesCommand_ExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _controlService.GetGames(A<int>.Ignored, null, null)).Throws(ex);

            // Act
            viewModel.ViewGamesCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamesWindowViewModel(_sharedService, _controlService, _gameFinderWindow);

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
