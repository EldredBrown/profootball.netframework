using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using EldredBrown.ProFootball.WpfApp.Models;
using FakeItEasy;

namespace EldredBrown.ProFootball.WpfApp.Tests.Services
{
    public static class TestService
    {
        public static void SetUpFakeLeagueSeasons(this ProFootballEntities dbContext,
            IEnumerable<LeagueSeason> leagueSeasonsEnumerable = null)
        {
            if (leagueSeasonsEnumerable == null)
            {
                leagueSeasonsEnumerable = new List<LeagueSeason>
                {
                    new LeagueSeason
                    {
                        LeagueName = "APFA",
                        SeasonID = 2017,
                        TotalGames = 256,
                        TotalPoints = 5120,
                        AveragePoints = 20
                    }
                };
            }
            var leagueSeasonsQueryable = leagueSeasonsEnumerable.AsQueryable();

            var fakeDbSet = A.Fake<DbSet<LeagueSeason>>(d => d.Implements(typeof(IQueryable<LeagueSeason>)));

            // Setup all IQueryable methods using what you have from "LeagueSeasons"
            A.CallTo(() => (fakeDbSet as IQueryable<LeagueSeason>).ElementType)
                .Returns(leagueSeasonsQueryable.ElementType);
            A.CallTo(() => (fakeDbSet as IQueryable<LeagueSeason>).Expression)
                .Returns(leagueSeasonsQueryable.Expression);
            A.CallTo(() => (fakeDbSet as IQueryable<LeagueSeason>).Provider).Returns(leagueSeasonsQueryable.Provider);
            A.CallTo(() => (fakeDbSet as IQueryable<LeagueSeason>).GetEnumerator())
                .Returns(leagueSeasonsQueryable.GetEnumerator());

            // Do the wiring between DbContext and DbSet
            A.CallTo(() => dbContext.LeagueSeasons).Returns(fakeDbSet);
        }

        public static void SetUpFakeLeagueSeasonTotals(this ProFootballEntities dbContext,
            IEnumerable<GetLeagueSeasonTotals_Result> leagueSeasonTotalsEnumerable = null)
        {
            if (leagueSeasonTotalsEnumerable == null)
            {
                leagueSeasonTotalsEnumerable = new List<GetLeagueSeasonTotals_Result>
                {
                    new GetLeagueSeasonTotals_Result
                    {
                        TotalGames = 256,
                        TotalPoints = 5120,
                        AveragePoints = 20.00
                    }
                };
            }
            var leagueSeasonTotalsQueryable = leagueSeasonTotalsEnumerable.AsQueryable();

            var fakeObjectResult = A.Fake<ObjectResult<GetLeagueSeasonTotals_Result>>(d =>
                d.Implements(typeof(IEnumerable<GetLeagueSeasonTotals_Result>)));

            // Setup all IEnumerable methods using what you have from "leagueSeasonTotals"
            A.CallTo(() => (fakeObjectResult as IEnumerable<GetLeagueSeasonTotals_Result>).GetEnumerator())
                .Returns(leagueSeasonTotalsQueryable.GetEnumerator());

            // Do the wiring between DbContext and ObjectResult
            A.CallTo(() => dbContext.GetLeagueSeasonTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(fakeObjectResult);
        }

        public static void SetUpFakeRankingsOffensive(this ProFootballEntities dbContext,
            IEnumerable<GetRankingsOffensive_Result> offensiveRankingsEnumerable = null)
        {
            if (offensiveRankingsEnumerable == null)
            {
                offensiveRankingsEnumerable = new List<GetRankingsOffensive_Result>();
            }
            var offensiveRankingsQueryable = offensiveRankingsEnumerable.AsQueryable();

            var fakeObjectResult = A.Fake<ObjectResult<GetRankingsOffensive_Result>>(d =>
                d.Implements(typeof(IEnumerable<GetRankingsOffensive_Result>)));

            // Setup all IEnumerable methods using what you have from "seasonStandings"
            A.CallTo(
                    () => (fakeObjectResult as IEnumerable<GetRankingsOffensive_Result>).GetEnumerator())
                .Returns(offensiveRankingsQueryable.GetEnumerator());

            // Do the wiring between DbContext and ObjectResult
            A.CallTo(() => dbContext.GetRankingsOffensive(A<int>.Ignored)).Returns(fakeObjectResult);
        }

        public static void SetUpFakeRankingsDefensive(this ProFootballEntities dbContext,
            IEnumerable<GetRankingsDefensive_Result> defensiveRankingsEnumerable = null)
        {
            if (defensiveRankingsEnumerable == null)
            {
                defensiveRankingsEnumerable = new List<GetRankingsDefensive_Result>();
            }
            var defensiveRankingsQueryable = defensiveRankingsEnumerable.AsQueryable();

            var fakeObjectResult = A.Fake<ObjectResult<GetRankingsDefensive_Result>>(d =>
                d.Implements(typeof(IEnumerable<GetRankingsDefensive_Result>)));

            // Setup all IEnumerable methods using what you have from "seasonStandings"
            A.CallTo(
                    () => (fakeObjectResult as IEnumerable<GetRankingsDefensive_Result>).GetEnumerator())
                .Returns(defensiveRankingsQueryable.GetEnumerator());

            // Do the wiring between DbContext and ObjectResult
            A.CallTo(() => dbContext.GetRankingsDefensive(A<int>.Ignored)).Returns(fakeObjectResult);
        }

        public static void SetUpFakeRankingsTotal(this ProFootballEntities dbContext,
            IEnumerable<GetRankingsTotal_Result> totalRankingsEnumerable = null)
        {
            if (totalRankingsEnumerable == null)
            {
                totalRankingsEnumerable = new List<GetRankingsTotal_Result>();
            }
            var totalRankingsQueryable = totalRankingsEnumerable.AsQueryable();

            var fakeObjectResult = A.Fake<ObjectResult<GetRankingsTotal_Result>>(d =>
                d.Implements(typeof(IEnumerable<GetRankingsTotal_Result>)));

            // Setup all IEnumerable methods using what you have from "seasonStandings"
            A.CallTo(() => (fakeObjectResult as IEnumerable<GetRankingsTotal_Result>).GetEnumerator())
                .Returns(totalRankingsQueryable.GetEnumerator());

            // Do the wiring between DbContext and ObjectResult
            A.CallTo(() => dbContext.GetRankingsTotal(A<int>.Ignored)).Returns(fakeObjectResult);
        }

        public static void SetUpFakeSeasonStandingsForConference(this ProFootballEntities dbContext,
            IEnumerable<GetSeasonStandingsForConference_Result> seasonStandingsEnumerable = null)
        {
            if (seasonStandingsEnumerable == null)
            {
                seasonStandingsEnumerable = new List<GetSeasonStandingsForConference_Result>();
            }
            var seasonStandingsQueryable = seasonStandingsEnumerable.AsQueryable();

            var fakeObjectResult = A.Fake<ObjectResult<GetSeasonStandingsForConference_Result>>(d =>
                d.Implements(typeof(IEnumerable<GetSeasonStandingsForConference_Result>)));

            // Setup all IEnumerable methods using what you have from "seasonStandings"
            A.CallTo(
                    () => (fakeObjectResult as IEnumerable<GetSeasonStandingsForConference_Result>).GetEnumerator())
                .Returns(seasonStandingsQueryable.GetEnumerator());

            // Do the wiring between DbContext and ObjectResult
            A.CallTo(() => dbContext.GetSeasonStandingsForConference(A<int>.Ignored, A<string>.Ignored))
                .Returns(fakeObjectResult);
        }

        public static void SetUpFakeSeasonStandingsForDivision(this ProFootballEntities dbContext,
            IEnumerable<GetSeasonStandingsForDivision_Result> seasonStandingsEnumerable = null)
        {
            if (seasonStandingsEnumerable == null)
            {
                seasonStandingsEnumerable = new List<GetSeasonStandingsForDivision_Result>();
            }
            var seasonStandingsQueryable = seasonStandingsEnumerable.AsQueryable();

            var fakeObjectResult = A.Fake<ObjectResult<GetSeasonStandingsForDivision_Result>>(d =>
                d.Implements(typeof(IEnumerable<GetSeasonStandingsForDivision_Result>)));

            // Setup all IEnumerable methods using what you have from "seasonStandings"
            A.CallTo(
                    () => (fakeObjectResult as IEnumerable<GetSeasonStandingsForDivision_Result>).GetEnumerator())
                .Returns(seasonStandingsQueryable.GetEnumerator());

            // Do the wiring between DbContext and ObjectResult
            A.CallTo(() => dbContext.GetSeasonStandingsForDivision(A<int>.Ignored, A<string>.Ignored))
                .Returns(fakeObjectResult);
        }

        public static void SetUpFakeTeamSeasonScheduleProfile(this ProFootballEntities dbContext,
            IEnumerable<GetTeamSeasonScheduleProfile_Result> teamSeasonScheduleProfile = null)
        {
            if (teamSeasonScheduleProfile == null)
            {
                teamSeasonScheduleProfile = new List<GetTeamSeasonScheduleProfile_Result>();
            }

            var fakeObjectResult = A.Fake<ObjectResult<GetTeamSeasonScheduleProfile_Result>>(d =>
                d.Implements(typeof(IEnumerable<GetTeamSeasonScheduleProfile_Result>)));

            // Setup all IEnumerable methods using what you have from "fakeObjectResult"
            A.CallTo(() => (fakeObjectResult as IEnumerable<GetTeamSeasonScheduleProfile_Result>).GetEnumerator()).
                Returns(teamSeasonScheduleProfile.GetEnumerator());

            // Do the wiring between DbContext and ObjectResult
            A.CallTo(() => dbContext.GetTeamSeasonScheduleProfile(A<string>.Ignored, A<int>.Ignored))
                .Returns(fakeObjectResult);
        }

        public static void SetUpFakeTeamSeasonScheduleTotals(this ProFootballEntities dbContext,
            IEnumerable<GetTeamSeasonScheduleTotals_Result> teamSeasonScheduleTotals = null)
        {
            if (teamSeasonScheduleTotals == null)
            {
                teamSeasonScheduleTotals = new List<GetTeamSeasonScheduleTotals_Result>();
            }

            var fakeObjectResult = A.Fake<ObjectResult<GetTeamSeasonScheduleTotals_Result>>(d =>
                d.Implements(typeof(IEnumerable<GetTeamSeasonScheduleTotals_Result>)));

            // Setup all IEnumerable methods using what you have from "fakeObjectResult"
            A.CallTo(() => (fakeObjectResult as IEnumerable<GetTeamSeasonScheduleTotals_Result>).GetEnumerator())
                .Returns(teamSeasonScheduleTotals.GetEnumerator());

            // Do the wiring between DbContext and ObjectResult
            A.CallTo(() => dbContext.GetTeamSeasonScheduleTotals(A<string>.Ignored, A<int>.Ignored))
                .Returns(fakeObjectResult);
        }

        public static void SetUpFakeTeamSeasonScheduleAverages(this ProFootballEntities dbContext,
            IEnumerable<GetTeamSeasonScheduleAverages_Result> teamSeasonScheduleAverages = null)
        {
            if (teamSeasonScheduleAverages == null)
            {
                teamSeasonScheduleAverages = new List<GetTeamSeasonScheduleAverages_Result>();
            }

            var fakeObjectResult = A.Fake<ObjectResult<GetTeamSeasonScheduleAverages_Result>>(d =>
                d.Implements(typeof(IEnumerable<GetTeamSeasonScheduleAverages_Result>)));

            // Setup all IEnumerable methods using what you have from "fakeObjectResult"
            A.CallTo(() => (fakeObjectResult as IEnumerable<GetTeamSeasonScheduleAverages_Result>).GetEnumerator())
                .Returns(teamSeasonScheduleAverages.GetEnumerator());

            // Do the wiring between DbContext and ObjectResult
            A.CallTo(() => dbContext.GetTeamSeasonScheduleAverages(A<string>.Ignored, A<int>.Ignored))
                .Returns(fakeObjectResult);
        }

        public static void SetUpFakeWeekCounts(this ProFootballEntities dbContext,
            IEnumerable<WeekCount> weekCountsEnumerable = null)
        {
            if (weekCountsEnumerable == null)
            {
                weekCountsEnumerable = new List<WeekCount>
                {
                    new WeekCount { SeasonID = 2018, Count = 1 },
                    new WeekCount { SeasonID = 2017, Count = 2 },
                    new WeekCount { SeasonID = 2016, Count = 3 },
                };
            }
            var weekCountsQueryable = weekCountsEnumerable.AsQueryable();

            var fakeDbSet = A.Fake<DbSet<WeekCount>>(d => d.Implements(typeof(IQueryable<WeekCount>)));

            // Setup all IQueryable methods using what you have from "WeekCounts"
            A.CallTo(() => (fakeDbSet as IQueryable<WeekCount>).ElementType).Returns(weekCountsQueryable.ElementType);
            A.CallTo(() => (fakeDbSet as IQueryable<WeekCount>).Expression).Returns(weekCountsQueryable.Expression);
            A.CallTo(() => (fakeDbSet as IQueryable<WeekCount>).Provider).Returns(weekCountsQueryable.Provider);
            A.CallTo(() => (fakeDbSet as IQueryable<WeekCount>).GetEnumerator())
                .Returns(weekCountsQueryable.GetEnumerator());

            // Do the wiring between DbContext and DbSet
            A.CallTo(() => dbContext.WeekCounts).Returns(fakeDbSet);
        }
    }
}
