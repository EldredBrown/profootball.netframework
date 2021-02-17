using System;
using System.Collections.ObjectModel;
using EldredBrown.ProFootball.WpfApp;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootballWPF.Tests.ViewModels
{
    [TestFixture]
    public class DivisionNodeViewModelTest
    {
        private ITreeViewItemViewModel _parent;
        private ISeasonStandingsControlService _controlService;
        private Division _division;
        private ISharedService _sharedService;

        [SetUp]
        public void SetUp()
        {
            _parent = A.Fake<TreeViewItemViewModel>();
            _controlService = A.Fake<ISeasonStandingsControlService>();
            _division = new Division();
            _sharedService = A.Fake<ISharedService>();
        }

        [TestCase]
        public void IsSelected_ShowStandingsHappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new DivisionNodeViewModel(_parent, _controlService, _division);

            // Act
            viewModel.IsSelected = true;

            // Assert
            #region ShowStandings()

            A.CallTo(() => _controlService.GetSeasonStandingsForDivision(
                (int)WpfGlobals.SelectedSeason, _division.Name)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<GetSeasonStandingsForAssociation_Result>>(
                viewModel.Parent.Standings);

            #endregion ShowStandings()
        }

        [TestCase]
        public void IsSelected_ShowStandingsCatchesException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new DivisionNodeViewModel(_parent, _controlService, _division, _sharedService);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _controlService.GetSeasonStandingsForDivision(A<int>.Ignored, A<string>.Ignored)).Throws(ex);

            // Act
            viewModel.IsSelected = true;

            // Assert
            #region ShowStandings()

            Assert.IsNull(viewModel.Parent.Standings);

            A.CallTo(() => _sharedService.ShowExceptionMessage(ex.InnerException, "Exception"))
                .MustHaveHappenedOnceExactly();

            #endregion ShowStandings()
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new DivisionNodeViewModel(_parent, _controlService, _division);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }
    }
}
