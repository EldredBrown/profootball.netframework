SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE ProFootball
GO
-- =============================================
-- Author:		Eldred Brown
-- Create date: 2016-11-25
-- Description:	A procedure to compute and return final offensive ratings for all teams
-- Revision History:
--	2017-01-17	Eldred Brown	Added logic to restrict results to one season
-- =============================================
CREATE PROCEDURE dbo.GetRankingsOffensive
	-- Add the parameters for the stored procedure here
	@seasonID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT
		TeamName AS Team,
		Wins,
		Losses,
		Ties,
		OffensiveAverage,
		OffensiveFactor,
		OffensiveIndex
	FROM
		dbo.TeamSeasons
	WHERE
		SeasonID = @seasonID
	ORDER BY
		OffensiveIndex DESC
END
GO