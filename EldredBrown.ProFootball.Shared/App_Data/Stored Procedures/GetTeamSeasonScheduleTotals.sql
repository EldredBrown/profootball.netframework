SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2016-11-21
-- Description:	A procedure to compute and return the totals of a team's 
--				schedule final data
-- Revision History:
--	2017-01-05	Eldred Brown
--	*	Added parameter to restrict results to a single season
-- =============================================
CREATE PROCEDURE dbo.GetTeamSeasonScheduleTotals 
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
			Games,
			PointsFor,
			PointsAgainst,
			ScheduleWins,
			ScheduleLosses,
			ScheduleTies,
			ScheduleWinningPercentage,
			ScheduleGames,
			SchedulePointsFor,
			SchedulePointsAgainst
		FROM dbo.fn_GetTeamSeasonScheduleTotals(@teamName, @seasonID)

	END
END
GO
