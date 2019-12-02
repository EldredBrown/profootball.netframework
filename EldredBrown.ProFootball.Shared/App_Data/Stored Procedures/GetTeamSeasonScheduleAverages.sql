SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2016-11-23
-- Description:	A procedure to return the averages of a team's schedule data
-- Revision History:
--	2017-01-05	Eldred Brown
--	*	Added parameter to restrict results to a single season
-- =============================================
CREATE PROCEDURE dbo.GetTeamSeasonScheduleAverages
	-- Add the parameters for the stored procedure here
	@teamName varchar(50),
	@seasonID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate arguments.
	IF EXISTS (
		-- Verify that @teamName is the name of a valid team.
		SELECT [Name] FROM dbo.Teams WHERE [Name] = @teamName
	)
	AND EXISTS (
		-- Verify that @seasonID is the ID of a valid season.
		SELECT ID FROM dbo.Seasons WHERE ID = @seasonID
	)
	BEGIN
		-- Insert statements for procedure here
		SELECT
			PointsFor = 
				CASE
					WHEN Games = 0 THEN NULL
					ELSE ROUND(PointsFor / Games, 2)
				END,
			PointsAgainst = 
				CASE
					WHEN Games = 0 THEN NULL
					ELSE ROUND(PointsAgainst / Games, 2)
				END,
			SchedulePointsFor = 
				CASE
					WHEN ScheduleGames = 0 THEN NULL
					ELSE ROUND(SchedulePointsFor / ScheduleGames, 2)
				END,
			SchedulePointsAgainst = 
				CASE
					WHEN ScheduleGames = 0 THEN NULL
					ELSE ROUND(SchedulePointsAgainst / ScheduleGames, 2)
				END
		FROM
			dbo.fn_GetTeamSeasonScheduleTotals(@teamName, @seasonID)

	END
END
GO
