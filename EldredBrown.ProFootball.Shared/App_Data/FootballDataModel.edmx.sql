-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/06/2018 17:18:59
-- Generated from EDMX file: D:\Documents\BitBucket\CST 4X2 - SeniorProject\FootballApplication\FootballApplicationWeb\Models\FootballDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ProFootballDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Conference_Division]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Divisions] DROP CONSTRAINT [FK_Conference_Division];
GO
IF OBJECT_ID(N'[dbo].[FK_Conference_TeamSeason]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamSeasons] DROP CONSTRAINT [FK_Conference_TeamSeason];
GO
IF OBJECT_ID(N'[dbo].[FK_League_Conference]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Conferences] DROP CONSTRAINT [FK_League_Conference];
GO
IF OBJECT_ID(N'[dbo].[FK_Season_ConferenceFirstOf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Conferences] DROP CONSTRAINT [FK_Season_ConferenceFirstOf];
GO
IF OBJECT_ID(N'[dbo].[FK_Season_ConferenceLastOf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Conferences] DROP CONSTRAINT [FK_Season_ConferenceLastOf];
GO
IF OBJECT_ID(N'[dbo].[FK_Division_TeamSeason]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamSeasons] DROP CONSTRAINT [FK_Division_TeamSeason];
GO
IF OBJECT_ID(N'[dbo].[FK_League_Division]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Divisions] DROP CONSTRAINT [FK_League_Division];
GO
IF OBJECT_ID(N'[dbo].[FK_Season_DivisionFirstOf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Divisions] DROP CONSTRAINT [FK_Season_DivisionFirstOf];
GO
IF OBJECT_ID(N'[dbo].[FK_Season_DivisionLastOf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Divisions] DROP CONSTRAINT [FK_Season_DivisionLastOf];
GO
IF OBJECT_ID(N'[dbo].[FK_Season_Game]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Games] DROP CONSTRAINT [FK_Season_Game];
GO
IF OBJECT_ID(N'[dbo].[FK_Team_GameGuestOf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Games] DROP CONSTRAINT [FK_Team_GameGuestOf];
GO
IF OBJECT_ID(N'[dbo].[FK_Team_GameHostOf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Games] DROP CONSTRAINT [FK_Team_GameHostOf];
GO
IF OBJECT_ID(N'[dbo].[FK_Team_GameLost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Games] DROP CONSTRAINT [FK_Team_GameLost];
GO
IF OBJECT_ID(N'[dbo].[FK_Team_GameWon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Games] DROP CONSTRAINT [FK_Team_GameWon];
GO
IF OBJECT_ID(N'[dbo].[FK_League_LeagueSeason]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LeagueSeasons] DROP CONSTRAINT [FK_League_LeagueSeason];
GO
IF OBJECT_ID(N'[dbo].[FK_League_TeamSeason]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamSeasons] DROP CONSTRAINT [FK_League_TeamSeason];
GO
IF OBJECT_ID(N'[dbo].[FK_Season_LeagueFirstOf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Leagues] DROP CONSTRAINT [FK_Season_LeagueFirstOf];
GO
IF OBJECT_ID(N'[dbo].[FK_Season_LeagueLastOf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Leagues] DROP CONSTRAINT [FK_Season_LeagueLastOf];
GO
IF OBJECT_ID(N'[dbo].[FK_Season_LeagueSeason]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LeagueSeasons] DROP CONSTRAINT [FK_Season_LeagueSeason];
GO
IF OBJECT_ID(N'[dbo].[FK_Season_TeamSeason]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamSeasons] DROP CONSTRAINT [FK_Season_TeamSeason];
GO
IF OBJECT_ID(N'[dbo].[FK_Team_TeamSeason]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamSeasons] DROP CONSTRAINT [FK_Team_TeamSeason];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Conferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Conferences];
GO
IF OBJECT_ID(N'[dbo].[Divisions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Divisions];
GO
IF OBJECT_ID(N'[dbo].[Games]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Games];
GO
IF OBJECT_ID(N'[dbo].[Leagues]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Leagues];
GO
IF OBJECT_ID(N'[dbo].[LeagueSeasons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LeagueSeasons];
GO
IF OBJECT_ID(N'[dbo].[Seasons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Seasons];
GO
IF OBJECT_ID(N'[dbo].[Teams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teams];
GO
IF OBJECT_ID(N'[dbo].[TeamSeasons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TeamSeasons];
GO
IF OBJECT_ID(N'[dbo].[WeekCounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WeekCounts];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Conferences'
CREATE TABLE [dbo].[Conferences] (
    [Name] nchar(3)  NOT NULL,
    [LongName] nvarchar(30)  NOT NULL,
    [LeagueName] nvarchar(4)  NOT NULL,
    [FirstSeasonID] int  NOT NULL,
    [LastSeasonID] int  NULL
);
GO

-- Creating table 'Divisions'
CREATE TABLE [dbo].[Divisions] (
    [Name] nvarchar(25)  NOT NULL,
    [LeagueName] nvarchar(4)  NOT NULL,
    [ConferenceName] nchar(3)  NULL,
    [FirstSeasonID] int  NOT NULL,
    [LastSeasonID] int  NULL
);
GO

-- Creating table 'Games'
CREATE TABLE [dbo].[Games] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SeasonID] int  NOT NULL,
    [Week] int  NOT NULL,
    [GuestName] nvarchar(50)  NOT NULL,
    [GuestScore] float  NOT NULL,
    [HostName] nvarchar(50)  NOT NULL,
    [HostScore] float  NOT NULL,
    [WinnerName] nvarchar(50)  NULL,
    [WinnerScore] float  NULL,
    [LoserName] nvarchar(50)  NULL,
    [LoserScore] float  NULL,
    [IsPlayoffGame] bit  NOT NULL,
    [Notes] nvarchar(max)  NULL
);
GO

-- Creating table 'Leagues'
CREATE TABLE [dbo].[Leagues] (
    [Name] nvarchar(4)  NOT NULL,
    [LongName] nvarchar(25)  NOT NULL,
    [FirstSeasonID] int  NOT NULL,
    [LastSeasonID] int  NULL
);
GO

-- Creating table 'LeagueSeasons'
CREATE TABLE [dbo].[LeagueSeasons] (
    [LeagueName] nvarchar(4)  NOT NULL,
    [SeasonID] int  NOT NULL,
    [TotalGames] float DEFAULT 0  NOT NULL,
    [TotalPoints] float DEFAULT 0  NOT NULL,
    [AveragePoints] float  NULL
);
GO

-- Creating table 'Seasons'
CREATE TABLE [dbo].[Seasons] (
    [ID] int  NOT NULL
);
GO

-- Creating table 'Teams'
CREATE TABLE [dbo].[Teams] (
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'TeamSeasons'
CREATE TABLE [dbo].[TeamSeasons] (
    [TeamName] nvarchar(50)  NOT NULL,
    [SeasonID] int  NOT NULL,
    [LeagueName] nvarchar(4)  NOT NULL,
    [ConferenceName] nchar(3)  NULL,
    [DivisionName] nvarchar(25)  NULL,
    [Games] float DEFAULT 0  NOT NULL,
    [Wins] float DEFAULT 0  NOT NULL,
    [Losses] float DEFAULT 0  NOT NULL,
    [Ties] float DEFAULT 0  NOT NULL,
    [WinningPercentage] float  NULL,
    [PointsFor] float DEFAULT 0  NOT NULL,
    [PointsAgainst] float DEFAULT 0  NOT NULL,
    [PythagoreanWins] float DEFAULT 0  NOT NULL,
    [PythagoreanLosses] float DEFAULT 0  NOT NULL,
    [OffensiveAverage] float  NULL,
    [OffensiveFactor] float  NULL,
    [OffensiveIndex] float  NULL,
    [DefensiveAverage] float  NULL,
    [DefensiveFactor] float  NULL,
    [DefensiveIndex] float  NULL,
    [FinalPythagoreanWinningPercentage] float  NULL
);
GO

-- Creating table 'WeekCounts'
CREATE TABLE [dbo].[WeekCounts] (
    [SeasonID] int NOT NULL,
    [Count] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Name] in table 'Conferences'
ALTER TABLE [dbo].[Conferences]
ADD CONSTRAINT [PK_Conferences]
    PRIMARY KEY CLUSTERED ([Name] ASC);
GO

-- Creating primary key on [Name] in table 'Divisions'
ALTER TABLE [dbo].[Divisions]
ADD CONSTRAINT [PK_Divisions]
    PRIMARY KEY CLUSTERED ([Name] ASC);
GO

-- Creating primary key on [ID] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [PK_Games]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Name] in table 'Leagues'
ALTER TABLE [dbo].[Leagues]
ADD CONSTRAINT [PK_Leagues]
    PRIMARY KEY CLUSTERED ([Name] ASC);
GO

-- Creating primary key on [LeagueName], [SeasonID] in table 'LeagueSeasons'
ALTER TABLE [dbo].[LeagueSeasons]
ADD CONSTRAINT [PK_LeagueSeasons]
    PRIMARY KEY CLUSTERED ([LeagueName], [SeasonID] ASC);
GO

-- Creating primary key on [ID] in table 'Seasons'
ALTER TABLE [dbo].[Seasons]
ADD CONSTRAINT [PK_Seasons]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Name] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [PK_Teams]
    PRIMARY KEY CLUSTERED ([Name] ASC);
GO

-- Creating primary key on [TeamName], [SeasonID] in table 'TeamSeasons'
ALTER TABLE [dbo].[TeamSeasons]
ADD CONSTRAINT [PK_TeamSeasons]
    PRIMARY KEY CLUSTERED ([TeamName], [SeasonID] ASC);
GO

-- Creating primary key on [ID] in table 'WeekCounts'
ALTER TABLE [dbo].[WeekCounts]
ADD CONSTRAINT [PK_WeekCounts]
    PRIMARY KEY CLUSTERED ([SeasonID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ConferenceName] in table 'Divisions'
ALTER TABLE [dbo].[Divisions]
ADD CONSTRAINT [FK_Conference_Division]
    FOREIGN KEY ([ConferenceName])
    REFERENCES [dbo].[Conferences]
        ([Name])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Conference_Division'
CREATE INDEX [IX_FK_Conference_Division]
ON [dbo].[Divisions]
    ([ConferenceName]);
GO

-- Creating foreign key on [ConferenceName] in table 'TeamSeasons'
ALTER TABLE [dbo].[TeamSeasons]
ADD CONSTRAINT [FK_Conference_TeamSeason]
    FOREIGN KEY ([ConferenceName])
    REFERENCES [dbo].[Conferences]
        ([Name])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Conference_TeamSeason'
CREATE INDEX [IX_FK_Conference_TeamSeason]
ON [dbo].[TeamSeasons]
    ([ConferenceName]);
GO

-- Creating foreign key on [LeagueName] in table 'Conferences'
ALTER TABLE [dbo].[Conferences]
ADD CONSTRAINT [FK_League_Conference]
    FOREIGN KEY ([LeagueName])
    REFERENCES [dbo].[Leagues]
        ([Name])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_League_Conference'
CREATE INDEX [IX_FK_League_Conference]
ON [dbo].[Conferences]
    ([LeagueName]);
GO

-- Creating foreign key on [FirstSeasonID] in table 'Conferences'
ALTER TABLE [dbo].[Conferences]
ADD CONSTRAINT [FK_Season_ConferenceFirstOf]
    FOREIGN KEY ([FirstSeasonID])
    REFERENCES [dbo].[Seasons]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Season_ConferenceFirstOf'
CREATE INDEX [IX_FK_Season_ConferenceFirstOf]
ON [dbo].[Conferences]
    ([FirstSeasonID]);
GO

-- Creating foreign key on [LastSeasonID] in table 'Conferences'
ALTER TABLE [dbo].[Conferences]
ADD CONSTRAINT [FK_Season_ConferenceLastOf]
    FOREIGN KEY ([LastSeasonID])
    REFERENCES [dbo].[Seasons]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Season_ConferenceLastOf'
CREATE INDEX [IX_FK_Season_ConferenceLastOf]
ON [dbo].[Conferences]
    ([LastSeasonID]);
GO

-- Creating foreign key on [DivisionName] in table 'TeamSeasons'
ALTER TABLE [dbo].[TeamSeasons]
ADD CONSTRAINT [FK_Division_TeamSeason]
    FOREIGN KEY ([DivisionName])
    REFERENCES [dbo].[Divisions]
        ([Name])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Division_TeamSeason'
CREATE INDEX [IX_FK_Division_TeamSeason]
ON [dbo].[TeamSeasons]
    ([DivisionName]);
GO

-- Creating foreign key on [LeagueName] in table 'Divisions'
ALTER TABLE [dbo].[Divisions]
ADD CONSTRAINT [FK_League_Division]
    FOREIGN KEY ([LeagueName])
    REFERENCES [dbo].[Leagues]
        ([Name])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_League_Division'
CREATE INDEX [IX_FK_League_Division]
ON [dbo].[Divisions]
    ([LeagueName]);
GO

-- Creating foreign key on [FirstSeasonID] in table 'Divisions'
ALTER TABLE [dbo].[Divisions]
ADD CONSTRAINT [FK_Season_DivisionFirstOf]
    FOREIGN KEY ([FirstSeasonID])
    REFERENCES [dbo].[Seasons]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Season_DivisionFirstOf'
CREATE INDEX [IX_FK_Season_DivisionFirstOf]
ON [dbo].[Divisions]
    ([FirstSeasonID]);
GO

-- Creating foreign key on [LastSeasonID] in table 'Divisions'
ALTER TABLE [dbo].[Divisions]
ADD CONSTRAINT [FK_Season_DivisionLastOf]
    FOREIGN KEY ([LastSeasonID])
    REFERENCES [dbo].[Seasons]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Season_DivisionLastOf'
CREATE INDEX [IX_FK_Season_DivisionLastOf]
ON [dbo].[Divisions]
    ([LastSeasonID]);
GO

-- Creating foreign key on [SeasonID] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [FK_Season_Game]
    FOREIGN KEY ([SeasonID])
    REFERENCES [dbo].[Seasons]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Season_Game'
CREATE INDEX [IX_FK_Season_Game]
ON [dbo].[Games]
    ([SeasonID]);
GO

-- Creating foreign key on [GuestName] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [FK_Team_GameGuestOf]
    FOREIGN KEY ([GuestName])
    REFERENCES [dbo].[Teams]
        ([Name])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Team_GameGuestOf'
CREATE INDEX [IX_FK_Team_GameGuestOf]
ON [dbo].[Games]
    ([GuestName]);
GO

-- Creating foreign key on [HostName] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [FK_Team_GameHostOf]
    FOREIGN KEY ([HostName])
    REFERENCES [dbo].[Teams]
        ([Name])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Team_GameHostOf'
CREATE INDEX [IX_FK_Team_GameHostOf]
ON [dbo].[Games]
    ([HostName]);
GO

-- Creating foreign key on [LoserName] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [FK_Team_GameLost]
    FOREIGN KEY ([LoserName])
    REFERENCES [dbo].[Teams]
        ([Name])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Team_GameLost'
CREATE INDEX [IX_FK_Team_GameLost]
ON [dbo].[Games]
    ([LoserName]);
GO

-- Creating foreign key on [WinnerName] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [FK_Team_GameWon]
    FOREIGN KEY ([WinnerName])
    REFERENCES [dbo].[Teams]
        ([Name])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Team_GameWon'
CREATE INDEX [IX_FK_Team_GameWon]
ON [dbo].[Games]
    ([WinnerName]);
GO

-- Creating foreign key on [LeagueName] in table 'LeagueSeasons'
ALTER TABLE [dbo].[LeagueSeasons]
ADD CONSTRAINT [FK_League_LeagueSeason]
    FOREIGN KEY ([LeagueName])
    REFERENCES [dbo].[Leagues]
        ([Name])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [LeagueName] in table 'TeamSeasons'
ALTER TABLE [dbo].[TeamSeasons]
ADD CONSTRAINT [FK_League_TeamSeason]
    FOREIGN KEY ([LeagueName])
    REFERENCES [dbo].[Leagues]
        ([Name])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_League_TeamSeason'
CREATE INDEX [IX_FK_League_TeamSeason]
ON [dbo].[TeamSeasons]
    ([LeagueName]);
GO

-- Creating foreign key on [FirstSeasonID] in table 'Leagues'
ALTER TABLE [dbo].[Leagues]
ADD CONSTRAINT [FK_Season_LeagueFirstOf]
    FOREIGN KEY ([FirstSeasonID])
    REFERENCES [dbo].[Seasons]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Season_LeagueFirstOf'
CREATE INDEX [IX_FK_Season_LeagueFirstOf]
ON [dbo].[Leagues]
    ([FirstSeasonID]);
GO

-- Creating foreign key on [LastSeasonID] in table 'Leagues'
ALTER TABLE [dbo].[Leagues]
ADD CONSTRAINT [FK_Season_LeagueLastOf]
    FOREIGN KEY ([LastSeasonID])
    REFERENCES [dbo].[Seasons]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Season_LeagueLastOf'
CREATE INDEX [IX_FK_Season_LeagueLastOf]
ON [dbo].[Leagues]
    ([LastSeasonID]);
GO

-- Creating foreign key on [SeasonID] in table 'LeagueSeasons'
ALTER TABLE [dbo].[LeagueSeasons]
ADD CONSTRAINT [FK_Season_LeagueSeason]
    FOREIGN KEY ([SeasonID])
    REFERENCES [dbo].[Seasons]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Season_LeagueSeason'
CREATE INDEX [IX_FK_Season_LeagueSeason]
ON [dbo].[LeagueSeasons]
    ([SeasonID]);
GO

-- Creating foreign key on [SeasonID] in table 'TeamSeasons'
ALTER TABLE [dbo].[TeamSeasons]
ADD CONSTRAINT [FK_Season_TeamSeason]
    FOREIGN KEY ([SeasonID])
    REFERENCES [dbo].[Seasons]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Season_TeamSeason'
CREATE INDEX [IX_FK_Season_TeamSeason]
ON [dbo].[TeamSeasons]
    ([SeasonID]);
GO

-- Creating foreign key on [TeamName] in table 'TeamSeasons'
ALTER TABLE [dbo].[TeamSeasons]
ADD CONSTRAINT [FK_Team_TeamSeason]
    FOREIGN KEY ([TeamName])
    REFERENCES [dbo].[Teams]
        ([Name])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Populating all tables
-- --------------------------------------------------

-- Populating table 'Seasons'
INSERT INTO [dbo].Seasons VALUES (2018)
INSERT INTO [dbo].Seasons VALUES (2002)
INSERT INTO [dbo].Seasons VALUES (1970)
INSERT INTO [dbo].Seasons VALUES (1969)
INSERT INTO [dbo].Seasons VALUES (1960)
INSERT INTO [dbo].Seasons VALUES (1923)

-- Populating table 'Leagues'
INSERT INTO [dbo].Leagues VALUES ('NFL', 'National Football League', 1923, NULL)
INSERT INTO [dbo].Leagues VALUES ('AFL', 'American Football League', 1960, 1969)

-- Populating table 'Conferences'
INSERT INTO [dbo].Conferences VALUES ('AFC', 'American Football Conference', 'NFL', 1970, NULL)
INSERT INTO [dbo].Conferences VALUES ('NFC', 'National Football Conference', 'NFL', 1970, NULL)

-- Populating table 'Divisions'
INSERT INTO [dbo].Divisions VALUES ('AFC East', 'NFL', 'AFC', 1970, NULL)
INSERT INTO [dbo].Divisions VALUES ('AFC North', 'NFL', 'AFC', 2002, NULL)
INSERT INTO [dbo].Divisions VALUES ('AFC South', 'NFL', 'AFC', 2002, NULL)
INSERT INTO [dbo].Divisions VALUES ('AFC West', 'NFL', 'AFC', 1970, NULL)
INSERT INTO [dbo].Divisions VALUES ('NFC East', 'NFL', 'NFC', 1970, NULL)
INSERT INTO [dbo].Divisions VALUES ('NFC North', 'NFL', 'NFC', 2002, NULL)
INSERT INTO [dbo].Divisions VALUES ('NFC South', 'NFL', 'NFC', 2002, NULL)
INSERT INTO [dbo].Divisions VALUES ('NFC West', 'NFL', 'NFC', 1970, NULL)

-- Populating table 'Teams'
INSERT INTO [dbo].Teams VALUES ('Arizona Cardinals')
INSERT INTO [dbo].Teams VALUES ('Atlanta Falcons')
INSERT INTO [dbo].Teams VALUES ('Baltimore Ravens')
INSERT INTO [dbo].Teams VALUES ('Buffalo Bills')
INSERT INTO [dbo].Teams VALUES ('Carolina Panthers')
INSERT INTO [dbo].Teams VALUES ('Chicago Bears')
INSERT INTO [dbo].Teams VALUES ('Cincinnati Bengals')
INSERT INTO [dbo].Teams VALUES ('Cleveland Browns')
INSERT INTO [dbo].Teams VALUES ('Dallas Cowboys')
INSERT INTO [dbo].Teams VALUES ('Denver Broncos')
INSERT INTO [dbo].Teams VALUES ('Detroit Lions')
INSERT INTO [dbo].Teams VALUES ('Green Bay Packers')
INSERT INTO [dbo].Teams VALUES ('Houston Texans')
INSERT INTO [dbo].Teams VALUES ('Indianapolis Colts')
INSERT INTO [dbo].Teams VALUES ('Jacksonville Jaguars')
INSERT INTO [dbo].Teams VALUES ('Kansas City Chiefs')
INSERT INTO [dbo].Teams VALUES ('Los Angeles Chargers')
INSERT INTO [dbo].Teams VALUES ('Los Angeles Rams')
INSERT INTO [dbo].Teams VALUES ('Miami Dolphins')
INSERT INTO [dbo].Teams VALUES ('Minnesota Vikings')
INSERT INTO [dbo].Teams VALUES ('New England Patriots')
INSERT INTO [dbo].Teams VALUES ('New Orleans Saints')
INSERT INTO [dbo].Teams VALUES ('New York Giants')
INSERT INTO [dbo].Teams VALUES ('New York Jets')
INSERT INTO [dbo].Teams VALUES ('Oakland Raiders')
INSERT INTO [dbo].Teams VALUES ('Philadelphia Eagles')
INSERT INTO [dbo].Teams VALUES ('Pittsburgh Steelers')
INSERT INTO [dbo].Teams VALUES ('San Francisco 49ers')
INSERT INTO [dbo].Teams VALUES ('Seattle Seahawks')
INSERT INTO [dbo].Teams VALUES ('Tampa Bay Buccaneers')
INSERT INTO [dbo].Teams VALUES ('Tennessee Titans')
INSERT INTO [dbo].Teams VALUES ('Washington Redskins')

-- Populating table 'LeagueSeasons'
INSERT INTO [dbo].LeagueSeasons (LeagueName, SeasonID) VALUES ('NFL', 2018)

-- Populating table 'TeamSeasons'
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Arizona Cardinals', 2018, 'NFL', 'NFC', 'NFC West')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Atlanta Falcons', 2018, 'NFL', 'NFC', 'NFC South')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Baltimore Ravens', 2018, 'NFL', 'AFC', 'AFC North')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Buffalo Bills', 2018, 'NFL', 'AFC', 'AFC East')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Carolina Panthers', 2018, 'NFL', 'NFC', 'NFC South')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Chicago Bears', 2018, 'NFL', 'NFC', 'NFC North')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Cincinnati Bengals', 2018, 'NFL', 'AFC', 'AFC North')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Cleveland Browns', 2018, 'NFL', 'AFC', 'AFC North')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Dallas Cowboys', 2018, 'NFL', 'NFC', 'NFC East')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Denver Broncos', 2018, 'NFL', 'AFC', 'AFC West')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Detroit Lions', 2018, 'NFL', 'NFC', 'NFC North')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Green Bay Packers', 2018, 'NFL', 'NFC', 'NFC North')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Houston Texans', 2018, 'NFL', 'AFC', 'AFC South')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Indianapolis Colts', 2018, 'NFL', 'AFC', 'AFC South')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Jacksonville Jaguars', 2018, 'NFL', 'AFC', 'AFC South')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Kansas City Chiefs', 2018, 'NFL', 'AFC', 'AFC West')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Los Angeles Chargers', 2018, 'NFL', 'AFC', 'AFC West')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Los Angeles Rams', 2018, 'NFL', 'NFC', 'NFC West')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Miami Dolphins', 2018, 'NFL', 'AFC', 'AFC East')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Minnesota Vikings', 2018, 'NFL', 'NFC', 'NFC North')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('New England Patriots', 2018, 'NFL', 'AFC', 'AFC East')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('New Orleans Saints', 2018, 'NFL', 'NFC', 'NFC South')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('New York Giants', 2018, 'NFL', 'NFC', 'NFC East')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('New York Jets', 2018, 'NFL', 'AFC', 'AFC East')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Oakland Raiders', 2018, 'NFL', 'AFC', 'AFC West')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Philadelphia Eagles', 2018, 'NFL', 'NFC', 'NFC East')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Pittsburgh Steelers', 2018, 'NFL', 'AFC', 'AFC North')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('San Francisco 49ers', 2018, 'NFL', 'NFC', 'NFC West')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Seattle Seahawks', 2018, 'NFL', 'NFC', 'NFC West')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Tampa Bay Buccaneers', 2018, 'NFL', 'NFC', 'NFC South')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Tennessee Titans', 2018, 'NFL', 'AFC', 'AFC South')
INSERT INTO [dbo].TeamSeasons (TeamName, SeasonID, LeagueName, ConferenceName, DivisionName)
	VALUES ('Washington Redskins', 2018, 'NFL', 'NFC', 'NFC East')

INSERT INTO [dbo].WeekCounts VALUES (2018, 1)

-- --------------------------------------------------
-- Verifying all tables
-- --------------------------------------------------

SELECT * FROM [dbo].Seasons ORDER BY [ID] DESC
SELECT * FROM [dbo].Leagues ORDER BY [Name] ASC
SELECT * FROM [dbo].Conferences ORDER BY [Name] ASC
SELECT * FROM [dbo].Divisions ORDER BY [Name] ASC
SELECT * FROM [dbo].Teams ORDER BY [Name] ASC
SELECT * FROM [dbo].LeagueSeasons ORDER BY [SeasonID] DESC, [LeagueName] ASC
SELECT * FROM [dbo].TeamSeasons ORDER BY [SeasonID] DESC, [TeamName] ASC
SELECT * FROM [dbo].Games ORDER BY [SeasonID] DESC
SELECT * FROM [dbo].WeekCounts

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------