using System;
using System.Collections.Generic;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Interfaces;
using EldredBrown.ProFootball.WpfApp.Models;
using EldredBrown.ProFootball.WpfApp.Repositories;
using EldredBrown.ProFootball.WpfApp.Services;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.WpfApp.Tests.Services
{
    [TestFixture]
    public class SeasonStandingsControlServiceTest
    {
        #region Member Fields

        private ISharedService _sharedService;
        private IRepository<League> _leagueRepository;
        private IRepository<Conference> _conferenceRepository;
        private IRepository<Division> _divisionRepository;
        private IStoredProcedureRepository _storedProcedureRepository;

        #endregion Member Fields

        #region SetUp & TearDown

        [SetUp]
        public void SetUp()
        {
            _sharedService = A.Fake<ISharedService>();
            _leagueRepository = A.Fake<IRepository<League>>();
            _conferenceRepository = A.Fake<IRepository<Conference>>();
            _divisionRepository = A.Fake<IRepository<Division>>();
            _storedProcedureRepository = A.Fake<IStoredProcedureRepository>();
        }

        #endregion SetUp & TearDown

        #region Test Cases

        [TestCase]
        public void GetLeaguesBySeason_HappyPath()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var leagueCount = 3;
            var leagues = new List<League>(leagueCount);
            for (int i = leagueCount; i > 0; i--)
            {
                var league = new League
                {
                    Name = $"League {i}"
                };
                leagues.Add(league);
            }
            A.CallTo(() => _leagueRepository.GetEntities()).Returns(leagues);

            // Act
            var result = service.GetLeaguesBySeason(seasonID);

            // Assert
            A.CallTo(() => _leagueRepository.GetEntities()).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<League>>(result);

            var resultToList = result.ToList();
            Assert.AreEqual(leagueCount, resultToList.Count);

            // Verify correct order: Order by Name ascending.
            for (int i = 1; i < resultToList.Count; i++)
            {
                Assert.Greater(resultToList[i].Name, resultToList[i - 1].Name);
            }
        }

        [TestCase]
        public void GetLeaguesBySeason_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _leagueRepository.GetEntities()).Throws(ex);

            // Act
            var result = service.GetLeaguesBySeason(seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<League>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetLeaguesBySeason_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _leagueRepository.GetEntities()).Throws<Exception>();

            // Act
            IEnumerable<League> result = null;
            Assert.Throws<Exception>(() => result = service.GetLeaguesBySeason(seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetConferencesByLeagueAndSeason_HappyPath()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var leagueName = "League 1";
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var leagueCount = 3;
            var conferenceCountPerLeague = 3;
            var conferences = new List<Conference>(leagueCount * conferenceCountPerLeague);
            for (int i = leagueCount; i > 0; i--)
            {
                for (int j = conferenceCountPerLeague; j > 0; j--)
                {
                    var conference = new Conference
                    {
                        Name = $"Conference {j}",
                        LeagueName = $"League {i}"
                    };
                    conferences.Add(conference);
                }
            }
            A.CallTo(() => _conferenceRepository.GetEntities()).Returns(conferences);

            // Act
            var result = service.GetConferencesByLeagueAndSeason(leagueName, seasonID);

            // Assert
            A.CallTo(() => _conferenceRepository.GetEntities()).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<Conference>>(result);

            var resultToList = result.ToList();
            Assert.AreEqual(conferenceCountPerLeague, resultToList.Count);

            // Verify correct order: Order by Name ascending.
            for (int i = 1; i < resultToList.Count; i++)
            {
                Assert.Greater(resultToList[i].Name, resultToList[i - 1].Name);
            }
        }

        [TestCase]
        public void GetConferencesByLeagueAndSeason_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var leagueName = "League 1";
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _conferenceRepository.GetEntities()).Throws(ex);

            // Act
            var result = service.GetConferencesByLeagueAndSeason(leagueName, seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<Conference>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetConferencesByLeagueAndSeason_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var leagueName = "League 1";
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _conferenceRepository.GetEntities()).Throws<Exception>();

            // Act
            IEnumerable<Conference> result = null;
            Assert.Throws<Exception>(() => result = service.GetConferencesByLeagueAndSeason(leagueName, seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetDivisionsByLeagueAndSeason_HappyPath()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var leagueName = "League 1";
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var leagueCount = 3;
            var divisionCountPerLeague = 3;
            var divisions = new List<Division>(leagueCount * divisionCountPerLeague);
            for (int i = leagueCount; i > 0; i--)
            {
                for (int j = divisionCountPerLeague; j > 0; j--)
                {
                    var division = new Division
                    {
                        Name = $"Division {j}",
                        LeagueName = $"League {i}"
                    };
                    divisions.Add(division);
                }
            }
            A.CallTo(() => _divisionRepository.GetEntities()).Returns(divisions);

            // Act
            var result = service.GetDivisionsByLeagueAndSeason(leagueName, seasonID);

            // Assert
            A.CallTo(() => _divisionRepository.GetEntities()).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<Division>>(result);

            var resultToList = result.ToList();
            Assert.AreEqual(divisionCountPerLeague, resultToList.Count);

            // Verify correct order: Order by Name ascending.
            for (int i = 1; i < resultToList.Count; i++)
            {
                Assert.Greater(resultToList[i].Name, resultToList[i - 1].Name);
            }
        }

        [TestCase]
        public void GetDivisionsByLeagueAndSeason_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var leagueName = "League 1";
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _divisionRepository.GetEntities()).Throws(ex);

            // Act
            var result = service.GetDivisionsByLeagueAndSeason(leagueName, seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<Division>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetDivisionsByLeagueAndSeason_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var leagueName = "League 1";
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _divisionRepository.GetEntities()).Throws<Exception>();

            // Act
            IEnumerable<Division> result = null;
            Assert.Throws<Exception>(() => result = service.GetDivisionsByLeagueAndSeason(leagueName, seasonID));

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetDivisionsByConferenceAndSeason_HappyPath()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var conferenceName = "Conference 1";
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var conferenceCount = 3;
            var divisionCountPerConference = 3;
            var divisions = new List<Division>(conferenceCount * divisionCountPerConference);
            for (int i = conferenceCount; i > 0; i--)
            {
                for (int j = divisionCountPerConference; j > 0; j--)
                {
                    var division = new Division
                    {
                        Name = $"Division {j}",
                        ConferenceName = $"Conference {i}"
                    };
                    divisions.Add(division);
                }
            }
            A.CallTo(() => _divisionRepository.GetEntities()).Returns(divisions);

            // Act
            var result = service.GetDivisionsByConferenceAndSeason(conferenceName, seasonID);

            // Assert
            A.CallTo(() => _divisionRepository.GetEntities()).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<Division>>(result);

            var resultToList = result.ToList();
            Assert.AreEqual(divisionCountPerConference, resultToList.Count);

            // Verify correct order: Order by Name ascending.
            for (int i = 1; i < resultToList.Count; i++)
            {
                Assert.Greater(resultToList[i].Name, resultToList[i - 1].Name);
            }
        }

        [TestCase]
        public void GetDivisionsByConferenceAndSeason_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var conferenceName = "Conference 1";
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(() => _divisionRepository.GetEntities()).Throws(ex);

            // Act
            var result = service.GetDivisionsByConferenceAndSeason(conferenceName, seasonID);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<Division>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetDivisionsByConferenceAndSeason_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var conferenceName = "Conference 1";
            var seasonID = 2017;

            // Set up needed infrastructure of class under test.
            A.CallTo(() => _divisionRepository.GetEntities()).Throws<Exception>();

            // Act
            IEnumerable<Division> result = null;
            Assert.Throws<Exception>(
                () => { result = service.GetDivisionsByConferenceAndSeason(conferenceName, seasonID); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetSeasonStandingsForConference_HappyPath()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var conferenceName = "Conference";

            // Set up needed infrastructure of class under test.
            var dbContext = A.Fake<ProFootballEntities>();

            var count = 3;
            var seasonStandingsForConferenceEnumerable = new List<GetSeasonStandingsForConference_Result>(count);
            for (int i = 0; i < count; i++)
            {
                var item = new GetSeasonStandingsForConference_Result();
                seasonStandingsForConferenceEnumerable.Add(item);
            }
            dbContext.SetUpFakeSeasonStandingsForConference(seasonStandingsForConferenceEnumerable);
            
            var seasonStandingsForConference = dbContext.GetSeasonStandingsForConference(seasonID, conferenceName);
            A.CallTo(
                    () => _storedProcedureRepository.GetSeasonStandingsForConference(A<int>.Ignored, A<string>.Ignored))
                .Returns(seasonStandingsForConference);

            // Act
            var result = service.GetSeasonStandingsForConference(seasonID, conferenceName);

            // Assert
            A.CallTo(() => _storedProcedureRepository.GetSeasonStandingsForConference(seasonID, conferenceName))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetSeasonStandingsForConference_Result>>(result);
            Assert.AreEqual(count, result.Count());
        }

        [TestCase]
        public void GetSeasonStandingsForLeague_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var leagueName = "League";

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(
                    () => _storedProcedureRepository.GetSeasonStandingsForLeague(A<int>.Ignored, A<string>.Ignored))
                .Throws(ex);

            // Act
            var result = service.GetSeasonStandingsForLeague(seasonID, leagueName);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetSeasonStandingsForLeague_Result>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetSeasonStandingsForLeague_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var leagueName = "League";

            // Set up needed infrastructure of class under test.
            A.CallTo(
                    () => _storedProcedureRepository.GetSeasonStandingsForLeague(A<int>.Ignored, A<string>.Ignored))
                .Throws<Exception>();

            // Act
            IEnumerable<GetSeasonStandingsForLeague_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = service.GetSeasonStandingsForLeague(seasonID, leagueName); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetSeasonStandingsForConference_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var conferenceName = "Conference";

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(
                    () => _storedProcedureRepository.GetSeasonStandingsForConference(A<int>.Ignored, A<string>.Ignored))
                .Throws(ex);

            // Act
            var result = service.GetSeasonStandingsForConference(seasonID, conferenceName);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetSeasonStandingsForConference_Result>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetSeasonStandingsForConference_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var conferenceName = "Conference";

            // Set up needed infrastructure of class under test.
            A.CallTo(
                    () => _storedProcedureRepository.GetSeasonStandingsForConference(A<int>.Ignored, A<string>.Ignored))
                .Throws<Exception>();

            // Act
            IEnumerable<GetSeasonStandingsForConference_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = service.GetSeasonStandingsForConference(seasonID, conferenceName); });

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void GetSeasonStandingsForDivision_HappyPath()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var divisionName = "Division";

            // Set up needed infrastructure of class under test.
            var dbContext = A.Fake<ProFootballEntities>();

            var count = 3;
            var seasonStandingsForDivisionEnumerable = new List<GetSeasonStandingsForDivision_Result>(count);
            for (int i = 0; i < count; i++)
            {
                var item = new GetSeasonStandingsForDivision_Result();
                seasonStandingsForDivisionEnumerable.Add(item);
            }
            dbContext.SetUpFakeSeasonStandingsForDivision(seasonStandingsForDivisionEnumerable);

            var seasonStandingsForDivision = dbContext.GetSeasonStandingsForDivision(seasonID, divisionName);
            A.CallTo(
                    () => _storedProcedureRepository.GetSeasonStandingsForDivision(A<int>.Ignored, A<string>.Ignored))
                .Returns(seasonStandingsForDivision);

            // Act
            var result = service.GetSeasonStandingsForDivision(seasonID, divisionName);

            // Assert
            A.CallTo(() => _storedProcedureRepository.GetSeasonStandingsForDivision(seasonID, divisionName))
                .MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetSeasonStandingsForDivision_Result>>(result);
            Assert.AreEqual(count, result.Count());
        }

        [TestCase]
        public void GetSeasonStandingsForDivision_ArgumentNullExceptionCaught_ShowsExceptionMessageAndReturnsEmptyCollection()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var divisionName = "Division";

            // Set up needed infrastructure of class under test.
            var ex = new ArgumentNullException();
            A.CallTo(
                    () => _storedProcedureRepository.GetSeasonStandingsForDivision(A<int>.Ignored, A<string>.Ignored))
                .Throws(ex);

            // Act
            var result = service.GetSeasonStandingsForDivision(seasonID, divisionName);

            // Assert
            A.CallTo(() => _sharedService.ShowExceptionMessage(ex)).MustHaveHappenedOnceExactly();

            Assert.IsInstanceOf<IEnumerable<GetSeasonStandingsForDivision_Result>>(result);
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void GetSeasonStandingsForDivision_GenericExceptionCaught_LogsAndRethrowsException()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

            // Define argument variables of method under test.
            var seasonID = 2017;
            var divisionName = "Division";

            // Set up needed infrastructure of class under test.
            A.CallTo(
                    () => _storedProcedureRepository.GetSeasonStandingsForDivision(A<int>.Ignored, A<string>.Ignored))
                .Throws<Exception>();

            // Act
            IEnumerable<GetSeasonStandingsForDivision_Result> result = null;
            Assert.Throws<Exception>(
                () => { result = service.GetSeasonStandingsForDivision(seasonID, divisionName); });

            // Assert
            Assert.IsNull(result);
        }

        //[TestCase]
        public void TestCase1()
        {
            // Arrange
            var service = new SeasonStandingsControlService(_sharedService, _leagueRepository, _conferenceRepository,
                _divisionRepository, _storedProcedureRepository);

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
