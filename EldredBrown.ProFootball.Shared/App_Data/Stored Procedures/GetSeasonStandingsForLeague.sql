SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2019-02-10
-- Description:	A procedure to return a league's season standings
-- Revision history:
-- =============================================
CREATE PROCEDURE dbo.GetSeasonStandingsForLeague
	-- Add the parameters for the stored procedure here
	@seasonID int,
	@leagueName varchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate argument.
	IF EXISTS (
		SELECT ID FROM dbo.Seasons WHERE ID = @seasonID
	)
	AND EXISTS (
		SELECT [Name] FROM dbo.Leagues WHERE [Name] = @leagueName
	)
	BEGIN
		-- Insert statements for procedure here
		SELECT
			TeamName as Team,
			Wins,
			Losses,
			Ties,
			WinningPercentage =
				CASE
					WHEN Games = 0 THEN NULL
					ELSE WinningPercentage
				END,
			PointsFor,
			PointsAgainst,
			AvgPointsFor =
				CASE
					WHEN Games = 0 THEN NULL
					ELSE PointsFor / Games
				END,
			AvgPointsAgainst =
				CASE
					WHEN Games = 0 THEN NULL
					ELSE PointsAgainst / Games
				END
		FROM
			dbo.TeamSeasons
		WHERE
			SeasonID = @seasonID
			AND
			LeagueName = @leagueName
		ORDER BY WinningPercentage DESC, Wins DESC, TeamName ASC

	END
END
GO
