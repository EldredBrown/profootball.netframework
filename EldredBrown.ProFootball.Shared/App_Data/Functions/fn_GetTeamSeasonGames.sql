SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2016-11-21
-- Description:	A function to return all games played by a team
-- Revision History:
--	2017-01-05	Eldred Brown
--	*	Added parameter to restrict results to a single season
-- =============================================
CREATE FUNCTION dbo.fn_GetTeamSeasonGames
(	
	-- Add the parameters for the function here
	@teamName varchar(50),
	@seasonID int
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	SELECT
		ID,
		SeasonID AS Season,
		HostName AS Opponent,
		GuestScore AS GamePointsFor,
		HostScore AS GamePointsAgainst
	FROM
		dbo.Games
	WHERE GuestName = @teamName AND SeasonID = @seasonID
	UNION
	SELECT
		ID,
		SeasonID AS Season,
		GuestName AS Opponent,
		HostScore AS GamePointsFor,
		GuestScore AS GamePointsAgainst
	FROM
		dbo.Games
	WHERE HostName = @teamName AND SeasonID = @seasonID
)
