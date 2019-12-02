SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2016-11-21
-- Description:	A function to return a table showing how all 
--				of a team's opponents performed against their other opponents
-- Revision History:
--	2017-08-08	Eldred Brown
--	*	Added logic to prevent DivisionByZero exceptions.
--	2017-01-05	Eldred Brown
--	*	Added parameter to restrict results to a single season
-- =============================================
CREATE FUNCTION dbo.fn_GetTeamSeasonScheduleData 
(	
	-- Add the parameters for the function here
	@teamName varchar(50),
	@seasonID int
)
RETURNS @tbl TABLE
(
	ID								int,
	Opponent						varchar(50),
	OpponentWins					float,
	OpponentLosses					float,
	OpponentTies					float,
	OpponentWinningPercentage		float,
	OpponentWeightedGames			float,
	OpponentWeightedPointsFor		float,
	OpponentWeightedPointsAgainst	float
)
AS
BEGIN
	-- Add the SELECT statement with parameter references here
	BEGIN
		INSERT @tbl

		SELECT
			tsg.ID,
			tsg.Opponent AS Opponent,
			ts.Wins AS OpponentWins,
			ts.Losses AS OpponentLosses,
			ts.Ties AS OpponentTies,
			OpponentWinningPercentage = 
				CASE
					WHEN ts.Games = 0 THEN NULL		-- Prevent division by zero.
					ELSE (2 * ts.Wins + ts.Ties) / (2 * ts.Games)
				END,
			(ts.Games - 1) AS OpponentWeightedGames,
			(ts.PointsFor - tsg.GamePointsAgainst) AS OpponentWeightedPointsFor,
			(ts.PointsAgainst - tsg.GamePointsFor) AS OpponentWeightedPointsAgainst
		FROM dbo.TeamSeasons AS ts
			INNER JOIN dbo.fn_GetTeamSeasonGames(@teamName, @seasonID) AS tsg
				ON ts.TeamName = tsg.Opponent
		WHERE
			ts.SeasonID = @seasonID
	END

	RETURN
END
GO
