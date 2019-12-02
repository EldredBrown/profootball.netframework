SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2016-11-21
-- Description:	A function to return a team's profile
-- Revision History:
--	2017-01-05	Eldred Brown
--	*	Added parameter to restrict results to a single season	
-- =============================================
CREATE FUNCTION dbo.fn_GetTeamSeasonScheduleProfile 
(	
	-- Add the parameters for the function here
	@teamName varchar(50),
	@seasonID int
)
RETURNS @tbl TABLE
(
	ID								int,
	Opponent						varchar(50),
	GamePointsFor					float,
	GamePointsAgainst				float,
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
	INSERT @tbl

	SELECT
		tsg.ID,
		tsg.Opponent,
		tsg.GamePointsFor,
		tsg.GamePointsAgainst,
		tssd.OpponentWins,
		tssd.OpponentLosses,
		tssd.OpponentTies,
		tssd.OpponentWinningPercentage,
		tssd.OpponentWeightedGames,
		tssd.OpponentWeightedPointsFor,
		tssd.OpponentWeightedPointsAgainst
	FROM dbo.fn_GetTeamSeasonGames(@teamName, @seasonID) AS tsg
		INNER JOIN dbo.fn_GetTeamSeasonScheduleData(@teamName, @seasonID) AS tssd
			ON tsg.ID = tssd.ID

	RETURN
END
