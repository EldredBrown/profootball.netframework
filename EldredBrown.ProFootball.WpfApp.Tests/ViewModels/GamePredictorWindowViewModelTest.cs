using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EldredBrown.ProFootball.Shared;
using EldredBrown.ProFootball.WpfApp;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootballWPF.Tests.ViewModels
{
    [TestFixture]
    public class GamePredictorWindowViewModelTest
    {
        #region Member Fields

        private ISharedService _sharedService;
        private IGamePredictorWindowService _windowService;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _windowService = A.Fake<IGamePredictorWindowService>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void ViewSeasonsCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService);

            // Set up needed infrastructure of class under test.
            var seasonCount = 3;
            var lastSeason = 1920;
            var seasonIDs = new List<int>(seasonCount);
            for (int i = lastSeason; i > lastSeason - seasonCount; i--)
            {
                var seasonID = i;
                seasonIDs.Add(seasonID);
            }
            A.CallTo(() => _windowService.GetSeasonIDs()).Returns(seasonIDs);

            // Act
            viewModel.ViewSeasonsCommand.Execute(null);

            // Assert
            A.CallTo(() => _windowService.GetSeasonIDs()).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<int>>(viewModel.GuestSeasons);
            Assert.IsInstanceOf<ReadOnlyCollection<int>>(viewModel.HostSeasons);

            Assert.AreEqual(lastSeason, viewModel.GuestSelectedSeason);
            Assert.AreEqual(lastSeason, viewModel.HostSelectedSeason);
        }

        [TestCase]
        public void ViewSeasonsCommand_ExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _windowService.GetSeasonIDs()).Throws(ex);

            // Act
            viewModel.ViewSeasonsCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();

            Assert.IsNull(viewModel.GuestSeasons);
            Assert.IsNull(viewModel.HostSeasons);

            Assert.AreEqual(0, viewModel.GuestSelectedSeason);
            Assert.AreEqual(0, viewModel.HostSelectedSeason);
        }

        [TestCase]
        public void CalculatePredictionCommand_HappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = "Guest",
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            var guestSeason = new TeamSeason
            {
                TeamName = "Guest",
                SeasonID = 2017,
                OffensiveFactor = 1.1,
                OffensiveAverage = 22,
                DefensiveFactor = 0.9,
                DefensiveAverage = 20
            };
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(guestSeason);

            var hostSeason = new TeamSeason
            {
                TeamName = "Host",
                SeasonID = 2017,
                OffensiveFactor = 1.2,
                OffensiveAverage = 23,
                DefensiveFactor = 0.8,
                DefensiveAverage = 19
            };
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(hostSeason);

            // Act
            viewModel.CalculatePredictionCommand.Execute(null);

            // Assert
            #region ValidateDataEntry()

            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            #endregion ValidateDataEntry()

            Assert.AreEqual((int)((guestSeason.OffensiveFactor * hostSeason.DefensiveAverage +
                hostSeason.DefensiveFactor * guestSeason.OffensiveAverage) / 2d), viewModel.GuestScore);
            Assert.AreEqual((int)((hostSeason.OffensiveFactor * guestSeason.DefensiveAverage +
                guestSeason.DefensiveFactor * hostSeason.OffensiveAverage) / 2d), viewModel.HostScore);
        }

        [TestCase]
        public void CalculatePredictionCommand_DataValidationExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = null,
                HostName = "Host"
            };

            // Act
            viewModel.CalculatePredictionCommand.Execute(null);

            // Assert
            #region ValidateDataEntry()

            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            #endregion ValidateDataEntry()

            A.CallTo(() => _sharedService.ShowExceptionMessage(A<DataValidationException>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void CalculatePredictionCommand_GenericExceptionCaught_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _sharedService.FindTeamSeason(A<string>.Ignored, A<int>.Ignored)).Throws(ex);

            // Act
            viewModel.CalculatePredictionCommand.Execute(null);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();
        }

        [TestCase]
        public void ValidateDataEntry_GuestNameIsNull_ThrowsDataValidationException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = null,
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(new TeamSeason());
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(new TeamSeason());

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_GuestNameIsEmpty_ThrowsDataValidationException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = String.Empty,
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(new TeamSeason());
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(new TeamSeason());

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_GuestNameIsWhitespace_ThrowsDataValidationException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = " ",
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(new TeamSeason());
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(new TeamSeason());

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_HostNameIsNull_ThrowsDataValidationException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = "Guest",
                HostName = null
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(new TeamSeason());
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(new TeamSeason());

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_HostNameIsEmpty_ThrowsDataValidationException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = "Guest",
                HostName = String.Empty
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(new TeamSeason());
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(new TeamSeason());

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_HostNameIsWhitespace_ThrowsDataValidationException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = "Guest",
                HostName = " "
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(new TeamSeason());
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(new TeamSeason());

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_GuestSeasonIsNull_ThrowsDataValidationException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = "Guest",
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(null);
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(new TeamSeason());

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual(WpfGlobals.Constants.TeamNotInDatabaseMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_HostSeasonIsNull_ThrowsDataValidationException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = "Guest",
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(new TeamSeason());
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(null);

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.AreEqual(WpfGlobals.Constants.TeamNotInDatabaseMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_GuestSeasonAndHostSeasonAreNotNull_ThrowsNoException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService)
            {
                GuestName = "Guest",
                HostName = "Host"
            };

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .Returns(new TeamSeason());
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .Returns(new TeamSeason());

            // Act
            var result = viewModel.ValidateDataEntry();

            // Assert
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.GuestName, viewModel.GuestSelectedSeason))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _sharedService.FindTeamSeason(viewModel.HostName, viewModel.HostSelectedSeason))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<Matchup>(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GamePredictorWindowViewModel(_sharedService, _windowService);

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
