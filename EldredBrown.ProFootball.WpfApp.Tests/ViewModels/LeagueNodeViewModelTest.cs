using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Services;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootballWPF.Tests.ViewModels
{
    [TestFixture]
    public class LeagueNodeViewModelTest
    {
        #region Member Fields

        private ILeaguesTreeViewViewModel _parentControl;
        private ISeasonStandingsControlService _controlService;
        private League _league;
        private IRepository<Conference> _conferenceRepository;
        private ISharedService _sharedService;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _parentControl = A.Fake<ILeaguesTreeViewViewModel>();
            _controlService = A.Fake<ISeasonStandingsControlService>();
            _league = new League();
            _conferenceRepository = A.Fake<IRepository<Conference>>();
            _sharedService = A.Fake<ISharedService>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void IsExpanded_HasDummyChildAndConferencesCountGreaterThanZero_RemovesDummyChildAndLoadsChildren()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new LeagueNodeViewModel(_parentControl, _controlService, _league, _conferenceRepository);

            // Set up needed infrastructure of class under test.
            var seasonID = 1920;

            var conferenceCount = 3;
            var conferences = new List<Conference>(conferenceCount);
            for (int i = 1; i <= conferenceCount; i++)
            {
                var conference = new Conference
                {
                    Name = $"Conference {i}"
                };
                conferences.Add(conference);
            }
            A.CallTo(() => _controlService.GetConferencesByLeagueAndSeason(A<string>.Ignored, A<int>.Ignored))
                .Returns(conferences);

            // Act
            viewModel.IsExpanded = true;

            // Assert
            A.CallTo(() => _controlService.GetConferencesByLeagueAndSeason(_league.Name, seasonID))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ObservableCollection<ITreeViewItemViewModel>>(viewModel.Children);
            Assert.AreEqual(conferenceCount, viewModel.Children.Count);
            foreach (var child in viewModel.Children)
            {
                Assert.IsInstanceOf<ConferenceNodeViewModel>(child);
            }
        }

        [TestCase]
        public void IsExpanded_HasDummyChildAndConferencesCountEqualZeroAndDivisionsCountGreaterThanZero_RemovesDummyChildAndLoadsChildren()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new LeagueNodeViewModel(_parentControl, _controlService, _league, _conferenceRepository);

            // Set up needed infrastructure of class under test.
            var seasonID = 1920;

            var conferences = new List<Conference>();
            A.CallTo(() => _controlService.GetConferencesByLeagueAndSeason(A<string>.Ignored, A<int>.Ignored))
                .Returns(conferences);

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
            A.CallTo(() => _controlService.GetDivisionsByLeagueAndSeason(A<string>.Ignored, A<int>.Ignored))
                .Returns(divisions);

            // Act
            viewModel.IsExpanded = true;

            // Assert
            A.CallTo(() => _controlService.GetConferencesByLeagueAndSeason(_league.Name, seasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _controlService.GetDivisionsByLeagueAndSeason(_league.Name, seasonID))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ObservableCollection<ITreeViewItemViewModel>>(viewModel.Children);
            Assert.AreEqual(divisionCount, viewModel.Children.Count);
            foreach (var child in viewModel.Children)
            {
                Assert.IsInstanceOf<DivisionNodeViewModel>(child);
            }
        }

        [TestCase]
        public void IsExpanded_HasDummyChildAndConferencesCountEqualZeroAndDivisionsCountEqualZero_RemovesDummyChildAndLoadsChildren()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new LeagueNodeViewModel(_parentControl, _controlService, _league, _conferenceRepository);

            // Set up needed infrastructure of class under test.
            var seasonID = 1920;

            var conferences = new List<Conference>();
            A.CallTo(() => _controlService.GetConferencesByLeagueAndSeason(A<string>.Ignored, A<int>.Ignored))
                .Returns(conferences);

            var divisions = new List<Division>();
            A.CallTo(() => _controlService.GetDivisionsByLeagueAndSeason(A<string>.Ignored, A<int>.Ignored))
                .Returns(divisions);

            // Act
            viewModel.IsExpanded = true;

            // Assert
            A.CallTo(() => _controlService.GetConferencesByLeagueAndSeason(_league.Name, seasonID))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _controlService.GetDivisionsByLeagueAndSeason(_league.Name, seasonID))
                .MustHaveHappenedOnceExactly();

            Assert.IsNull(viewModel.Children);
        }

        [TestCase]
        public void IsExpanded_HasDummyChildAndLoadChildrenCatchesException_ShowsExceptionMessage()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new LeagueNodeViewModel(_parentControl, _controlService, _league, _conferenceRepository,
                _sharedService);

            // Set up needed infrastructure of class under test.
            var ex = new Exception();
            A.CallTo(() => _controlService.GetConferencesByLeagueAndSeason(A<string>.Ignored, A<int>.Ignored))
                .Throws(ex);

            // Act
            viewModel.IsExpanded = true;

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex, "Exception")).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<ObservableCollection<ITreeViewItemViewModel>>(viewModel.Children);
            Assert.IsEmpty(viewModel.Children);
        }

        [TestCase]
        public void IsExpanded_HasNoDummyChild_DoesNotLoadChildren()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new LeagueNodeViewModel(_parentControl, _controlService, _league, _conferenceRepository,
                lazyLoadChildren: false);

            // Act
            viewModel.IsExpanded = true;

            // Assert
            #region LoadChildren()

            A.CallTo(() => _controlService.GetConferencesByLeagueAndSeason(A<string>.Ignored, A<int>.Ignored))
                .MustNotHaveHappened();

            #endregion LoadChildren()
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            // Instantiate class under test.
            var viewModel = new LeagueNodeViewModel(_parentControl, _controlService, _league, _conferenceRepository);

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
