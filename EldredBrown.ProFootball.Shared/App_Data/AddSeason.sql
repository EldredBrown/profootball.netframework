-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/06/1985 17:18:59
-- Generated from EDMX file: D:\Documents\BitBucket\CST 4X2 - SeniorProject\FootballApplication\FootballApplicationWeb\Models\FootballDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE ProFootball
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA dbo');
GO

-- --------------------------------------------------
-- Populating all tables
-- --------------------------------------------------

-- Populating table 'Seasons'
INSERT INTO dbo.Seasons VALUES (1985)

-- Populating table 'Leagues'
--INSERT INTO dbo.Leagues VALUES ('NFL', 'National Football League', 1923, NULL)
--INSERT INTO dbo.Leagues VALUES ('AFL', 'American Football League', 1960, 1969)

-- Populating table 'Conferences'
--INSERT INTO dbo.Conferences VALUES ('AFC', 'American Football Conference', 'NFL', 1970, NULL)
--INSERT INTO dbo.Conferences VALUES ('NFC', 'National Football Conference', 'NFL', 1970, NULL)

-- Populating table 'Divisions'
INSERT INTO dbo.Divisions VALUES ('AFC Central', 'NFL', 'AFC', 1970, NULL)
INSERT INTO dbo.Divisions VALUES ('NFC Central', 'NFL', 'NFC', 1970, NULL)

-- Populating table 'Teams'
INSERT INTO dbo.Teams VALUES ('Houston Oilers')
INSERT INTO dbo.Teams VALUES ('Los Angeles Raiders')
INSERT INTO dbo.Teams VALUES ('San Diego Chargers')
INSERT INTO dbo.Teams VALUES ('St. Louis Cardinals')

-- Populating table 'LeagueSeasons'
INSERT INTO dbo.LeagueSeasons (LeagueName, SeasonID) VALUES ('NFL', 1985)

-- Populating table 'TeamSeasons'
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Buffalo Bills', 1985, 'NFL', 'AFC', 'AFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Indianapolis Colts', 1985, 'NFL', 'AFC', 'AFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Miami Dolphins', 1985, 'NFL', 'AFC', 'AFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('New England Patriots', 1985, 'NFL', 'AFC', 'AFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('New York Jets', 1985, 'NFL', 'AFC', 'AFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Cincinnati Bengals', 1985, 'NFL', 'AFC', 'AFC Central')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Cleveland Browns', 1985, 'NFL', 'AFC', 'AFC Central')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Houston Oilers', 1985, 'NFL', 'AFC', 'AFC Central')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Pittsburgh Steelers', 1985, 'NFL', 'AFC', 'AFC Central')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Denver Broncos', 1985, 'NFL', 'AFC', 'AFC West')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Kansas City Chiefs', 1985, 'NFL', 'AFC', 'AFC West')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Los Angeles Raiders', 1985, 'NFL', 'AFC', 'AFC West')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('San Diego Chargers', 1985, 'NFL', 'AFC', 'AFC West')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Seattle Seahawks', 1985, 'NFL', 'NFC', 'AFC West')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Dallas Cowboys', 1985, 'NFL', 'NFC', 'NFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('New York Giants', 1985, 'NFL', 'NFC', 'NFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Philadelphia Eagles', 1985, 'NFL', 'NFC', 'NFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('St. Louis Cardinals', 1985, 'NFL', 'NFC', 'NFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Washington Redskins', 1985, 'NFL', 'NFC', 'NFC East')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Chicago Bears', 1985, 'NFL', 'NFC', 'NFC Central')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Detroit Lions', 1985, 'NFL', 'NFC', 'NFC Central')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Green Bay Packers', 1985, 'NFL', 'NFC', 'NFC Central')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Minnesota Vikings', 1985, 'NFL', 'NFC', 'NFC Central')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Tampa Bay Buccaneers', 1985, 'NFL', 'NFC', 'NFC Central')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Atlanta Falcons', 1985, 'NFL', 'NFC', 'NFC West')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Los Angeles Rams', 1985, 'NFL', 'NFC', 'NFC West')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('New Orleans Saints', 1985, 'NFL', 'NFC', 'NFC West')
INSERT INTO dbo.TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('San Francisco 49ers', 1985, 'NFL', 'NFC', 'NFC West')

INSERT INTO dbo.WeekCounts VALUES (1985, 1)

-- --------------------------------------------------
-- Verifying all tables
-- --------------------------------------------------

SELECT * FROM dbo.Seasons ORDER BY ID DESC
SELECT * FROM dbo.Leagues ORDER BY [Name] ASC
SELECT * FROM dbo.Conferences ORDER BY [Name] ASC
SELECT * FROM dbo.Divisions ORDER BY [Name] ASC
SELECT * FROM dbo.Teams ORDER BY [Name] ASC
SELECT * FROM dbo.LeagueSeasons ORDER BY SeasonID DESC, LeagueName ASC
SELECT * FROM dbo.TeamSeasons ORDER BY SeasonID DESC, TeamName ASC
SELECT * FROM dbo.Games ORDER BY SeasonID DESC
SELECT * FROM dbo.WeekCounts

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
