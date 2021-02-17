using System;
using EldredBrown.ProFootball.WpfApp;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootballWPF.Tests.ViewModels
{
    [TestFixture]
    public class GameFinderWindowViewModelTest
    {
        private ISharedService _sharedService;

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
        }

        [TestCase]
        public void ValidateDataEntry_GuestNameIsNull_ThrowsDataValidationException()
        {
            // Arrange
            var viewModel = new GameFinderWindowViewModel(_sharedService)
            {
                GuestName = null
            };

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_GuestNameIsEmpty_ThrowsDataValidationException()
        {
            // Arrange
            var viewModel = new GameFinderWindowViewModel(_sharedService)
            {
                GuestName = string.Empty
            };

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_GuestNameIsAllWhitespace_ThrowsDataValidationException()
        {
            // Arrange
            var viewModel = new GameFinderWindowViewModel(_sharedService)
            {
                GuestName = " "
            };

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_HostNameIsNull_ThrowsDataValidationException()
        {
            // Arrange
            var viewModel = new GameFinderWindowViewModel(_sharedService)
            {
                GuestName = "Team",
                HostName = null
            };

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_HostNameIsEmpty_ThrowsDataValidationException()
        {
            // Arrange
            var viewModel = new GameFinderWindowViewModel(_sharedService)
            {
                GuestName = "Team",
                HostName = string.Empty
            };

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_HostNameIsAllWhitespace_ThrowsDataValidationException()
        {
            // Arrange
            var viewModel = new GameFinderWindowViewModel(_sharedService)
            {
                GuestName = "Team",
                HostName = " "
            };

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            Assert.AreEqual(WpfGlobals.Constants.BothTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_GuestNameAndHostNameAreEqual_ThrowsDataValidationException()
        {
            // Arrange
            var viewModel = new GameFinderWindowViewModel(_sharedService)
            {
                GuestName = "Team",
                HostName = "Team"
            };

            // Act
            var ex = Assert.Throws<DataValidationException>(() => viewModel.ValidateDataEntry());

            // Assert
            Assert.AreEqual(WpfGlobals.Constants.DifferentTeamsNeededErrorMessage, ex.Message);
        }

        [TestCase]
        public void ValidateDataEntry_GuestAndHostDifferent_DoesNotThrowException()
        {
            // Arrange
            var viewModel = new GameFinderWindowViewModel(_sharedService)
            {
                GuestName = "Guest",
                HostName = "Host"
            };

            // Act & Assert
            Assert.DoesNotThrow(() => viewModel.ValidateDataEntry());
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new GameFinderWindowViewModel(_sharedService);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }
    }
}
