using System;
using System.Collections.Generic;
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
    public class ConferenceNodeViewModelTest
    {
        private ITreeViewItemViewModel _parent;
        private ISeasonStandingsControlService _controlService;
        private Conference _conference;
        private ISharedService _sharedService;

        [SetUp]
        public void SetUp()
        {
            _parent = A.Fake<ITreeViewItemViewModel>();
            _controlService = A.Fake<ISeasonStandingsControlService>();
            _conference = new Conference();
            _sharedService = A.Fake<ISharedService>();
        }

        [TestCase]
        public void IsExpanded_HasDummyChildAndDivisionsCountGreaterThanZero_RemovesDummyChildAndLoadsChildren()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new ConferenceNodeViewModel(_parent, _controlService, _conference);

            // Set up needed infrastructure of class under test.
            // viewModel object already has a dummy child created by the constructor.

            var divisionCount = 3;
            var divisions = new List<Division>(divisionCount);
            for (int i = 1; i <= divisionCount; i++)
            {
                var division = new Division
                {
                    Name = $"Division {i}"
                };
                divisions.Add(division);
            }
            A.CallTo(() => _controlService.GetDivisionsByConferenceAndSeason(A<string>.Ignored, A<int>.Ignored))
                .Returns(divisions);

            // Act
            viewModel.IsExpanded = true;

            // Assert
            #region LoadChildren()

            A.CallTo(() => _controlService.GetDivisionsByConferenceAndSeason(_conference.Name,
                (int)WpfGlobals.SelectedSeason)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ObservableCollection<ITreeViewItemViewModel>>(viewModel.Children);
            foreach (var child in viewModel.Children)
            {
                Assert.IsInstanceOf<DivisionNodeViewModel>(child);
            }

            #endregion LoadChildren()
        }

        [TestCase]
        public void IsExpanded_HasDummyChildAndDivisionsCountZero_RemovesDummyChildAndLoadsChildren()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new ConferenceNodeViewModel(_parent, _controlService, _conference);

            // Set up needed infrastructure of class under test.
            // viewModel object already has a dummy child created by the constructor.

            var divisions = new List<Division>();
            A.CallTo(() => _controlService.GetDivisionsByConferenceAndSeason(A<string>.Ignored, A<int>.Ignored))
                .Returns(divisions);

            // Act
            viewModel.IsExpanded = true;

            // Assert
            #region LoadChildren()

            A.CallTo(() => _controlService.GetDivisionsByConferenceAndSeason(_conference.Name,
                (int)WpfGlobals.SelectedSeason)).MustHaveHappenedOnceExactly();

            Assert.IsNull(viewModel.Children);

            #endregion LoadChildren()
        }

        [TestCase]
        public void IsExpanded_HasDummyChildAndLoadChildrenCatchesException_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new ConferenceNodeViewModel(_parent, _controlService, _conference, _sharedService);

            // Set up needed infrastructure of class under test.
            // viewModel object already has a dummy child created by the constructor.

            var ex = new Exception();
            A.CallTo(() => _controlService.GetDivisionsByConferenceAndSeason(A<string>.Ignored, A<int>.Ignored))
                .Throws(ex);

            // Act
            viewModel.IsExpanded = true;

            // Assert
            #region LoadChildren()

            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();

            Assert.IsEmpty(viewModel.Children);

            #endregion LoadChildren()
        }

        [TestCase]
        public void IsExpanded_HasNoDummyChild_DoesNotLoadChildren()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new ConferenceNodeViewModel(_parent, _controlService, _conference, lazyLoadChildren: false);

            // Act
            viewModel.IsExpanded = true;

            // Assert
            #region LoadChildren()

            A.CallTo(() => _controlService.GetDivisionsByConferenceAndSeason(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            #endregion LoadChildren()
        }

        [TestCase]
        public void IsSelected_ShowStandingsHappyPath()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new ConferenceNodeViewModel(_parent, _controlService, _conference);

            // Act
            viewModel.IsSelected = true;

            // Assert
            #region ShowStandings()

            A.CallTo(() => _controlService.GetSeasonStandingsForConference(
                (int)WpfGlobals.SelectedSeason, _conference.Name)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ReadOnlyCollection<GetSeasonStandingsForAssociation_Result>>(
                viewModel.Parent.Standings);

            #endregion ShowStandings()
        }

        [TestCase]
        public void IsSelected_ShowStandingsCatchesException()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new ConferenceNodeViewModel(_parent, _controlService, _conference, _sharedService);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _controlService.GetSeasonStandingsForConference(A<int>.Ignored, A<string>.Ignored))
                .Throws(ex);

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
            var viewModel = new ConferenceNodeViewModel(_parent, _controlService, _conference);

            // TODO: Define argument variables of method under test.

            // TODO: Set up needed infrastructure of class under test.

            // Act
            // TODO: Call method under test.

            // Assert
            // TODO: Assert results of call to method under test.
        }
    }
}
