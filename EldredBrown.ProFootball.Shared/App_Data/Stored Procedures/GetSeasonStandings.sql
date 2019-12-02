SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2017-01-14
-- Description:	A procedure to return a conference's season standings
-- Revision history:
-- =============================================
CREATE PROCEDURE dbo.GetSeasonStandings
	-- Add the parameters for the stored procedure here
	@seasonID int,
	@groupByDivision bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN
		-- Insert statements for procedure here
		SELECT
			TeamName as Team,
			ConferenceName as Conference,
			DivisionName as Division,
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
					ELSE (PointsFor / Games)
				END,
			AvgPointsAgainst =
				CASE
					WHEN Games = 0 THEN NULL
					ELSE (PointsAgainst / Games)
				END
		FROM dbo.TeamSeasons
		WHERE SeasonID = @seasonID
		ORDER BY
			CASE
				WHEN @groupByDivision = 0 THEN ConferenceName
				WHEN @groupByDivision = 1 THEN DivisionName
			END,
			WinningPercentage DESC

	END
END
GO
