SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2016-12-06
-- Description:	A procedure to compute and return the NFL's total scoring
-- Revision History:
--	2017-03-08	Eldred Brown	Added parameter to limit results to one league and one season
-- =============================================
CREATE PROCEDURE dbo.GetLeagueSeasonTotals
	-- Add the parameters for the stored procedure here
	@leagueName varchar(4),
	@seasonID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		SUM(Games) AS TotalGames,
		SUM(PointsFor) AS TotalPoints,
		AveragePoints =
			CASE
				WHEN SUM(Games) = 0 THEN NULL
				ELSE ROUND(SUM(PointsFor) / SUM(Games), 2)
			END,
		ROUND(AVG(Games), 0) AS WeekCount
	FROM
		dbo.TeamSeasons
	WHERE
		LeagueName = @leagueName
		AND
		SeasonID = @seasonID
END
GO
