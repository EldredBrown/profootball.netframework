SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2016-11-21
-- Description:	A function to compute and return the totals of a team's 
--				schedule final data
-- Revision History:
--	2017-08-08	Eldred Brown
--	*	Added logic to prevent DivisionByZero exceptions
--	2017-01-05	Eldred Brown
--	*	Added parameter to restrict results to a single season
-- =============================================
CREATE FUNCTION dbo.fn_GetTeamSeasonScheduleTotals
(	
	-- Add the parameters for the function here
	@teamName varchar(50),
	@seasonID int
)
RETURNS @tbl TABLE
(
	Games						float,
	PointsFor					float,
	PointsAgainst				float,
	ScheduleWins				float,
	ScheduleLosses				float,
	ScheduleTies				float,
	ScheduleWinningPercentage	float,
	ScheduleGames				float,
	SchedulePointsFor			float,
	SchedulePointsAgainst		float
)
AS
BEGIN
	-- Add the SELECT statement with parameter references here
	BEGIN
		INSERT @tbl

		SELECT
			COUNT(tssp.Opponent) AS Games,
			SUM(tssp.GamePointsFor) AS PointsFor,
			SUM(tssp.GamePointsAgainst) AS PointsAgainst,
			SUM(tssd.OpponentWins) AS ScheduleWins,
			SUM(tssd.OpponentLosses) AS ScheduleLosses,
			SUM(tssd.OpponentTies) AS ScheduleTies,
			ScheduleWinningPercentage = 
				CASE		-- Prevent division by zero.
					WHEN (SUM(tssd.OpponentWins) + SUM(tssd.OpponentLosses) + SUM(tssd.OpponentTies)) = 0 THEN NULL
					ELSE (SUM(tssd.OpponentWins) + SUM(tssd.OpponentLosses) + SUM(tssd.OpponentTies))
				END,
			SUM(tssd.OpponentWeightedGames) AS ScheduleGames,
			SUM(tssd.OpponentWeightedPointsFor) AS SchedulePointsFor,
			SUM(tssd.OpponentWeightedPointsAgainst) AS SchedulePointsAgainst
		FROM dbo.fn_GetTeamSeasonScheduleProfile(@teamName, @seasonID) AS tssp
			INNER JOIN dbo.fn_GetTeamSeasonScheduleData(@teamName, @seasonID) AS tssd
				ON tssp.Opponent = tssd.Opponent
	END

	RETURN
END
GO
