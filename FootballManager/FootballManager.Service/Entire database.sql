USE [master]
GO
/****** Object:  Database [FootballManager]    Script Date: 1/28/2015 8:56:10 AM ******/
CREATE DATABASE [FootballManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FotbalManager', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\FotbalManager.mdf' , SIZE = 26624KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FotbalManager_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\FotbalManager_log.ldf' , SIZE = 470144KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [FootballManager] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FootballManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FootballManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FootballManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FootballManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FootballManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FootballManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [FootballManager] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FootballManager] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [FootballManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FootballManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FootballManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FootballManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FootballManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FootballManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FootballManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FootballManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FootballManager] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FootballManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FootballManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FootballManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FootballManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FootballManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FootballManager] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FootballManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FootballManager] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FootballManager] SET  MULTI_USER 
GO
ALTER DATABASE [FootballManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FootballManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FootballManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FootballManager] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [FootballManager]
GO
/****** Object:  User [UMTDEV\umt-users]    Script Date: 1/28/2015 8:56:11 AM ******/
CREATE USER [UMTDEV\umt-users] FOR LOGIN [UMTDEV\umt-users]
GO
ALTER ROLE [db_owner] ADD MEMBER [UMTDEV\umt-users]
GO
/****** Object:  StoredProcedure [dbo].[getRandomBoot]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE procedure [dbo].[getRandomBoot]
    @countryId int
as 
 select top 1 t.* from Teams t
  inner join Series s on t.SeriesId=s.Id
  where s.LeagesConfigurationId=(
  select min(s.LeagesConfigurationId) from Teams  t
  inner join Series s on t.SeriesId=s.Id
  where t.IsBoot=1 and s.CountryId=@countryId)
  and t.IsBoot=1 and s.CountryId=@countryId
  order by newid()




GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Countries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[NrOfLeagesToAdd] [int] NOT NULL,
	[CurrentNrOfLeages] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LeagesConfigurations]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeagesConfigurations](
	[Id] [int] NOT NULL,
	[NrOfTeamsPromoted] [int] NULL,
	[NrOfTeamsReleagated] [int] NULL,
	[CurrentNrOfTeams] [int] NOT NULL,
	[NrOfBranchSeries] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Matches]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Matches](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HomeTeamId] [int] NOT NULL,
	[AwayTeamId] [int] NOT NULL,
	[Data] [datetime] NOT NULL,
	[MatchType] [int] NOT NULL,
	[HalfTimeScoreHome] [int] NULL,
	[HalfTimeScoreAway] [int] NULL,
	[FinalTimeScoreHome] [int] NULL,
	[FinalTimeScoreAway] [int] NULL,
 CONSTRAINT [PK__Matches__3214EC07C3558719] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Players]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Players](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[BirthDate] [datetime] NOT NULL,
	[TeamId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PlayersAttributesValues]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayersAttributesValues](
	[PlayerId] [int] NOT NULL,
	[AttributeId] [int] NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_PlayersAttributesValues] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[AttributeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Series]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Series](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LeagesConfigurationId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
	[ParentId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Teams]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Teams](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[SeriesId] [int] NOT NULL,
	[IsBoot] [bit] NOT NULL,
 CONSTRAINT [PK__Teams__3214EC0797EE9062] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TeamTactics]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TeamTactics](
	[TeamId] [int] NOT NULL,
	[SelectedPlayers] [varchar](max) NOT NULL,
	[ShotsType] [int] NOT NULL,
	[MarkingType] [int] NOT NULL,
	[AggressivityType] [int] NOT NULL,
	[OffsideTrapType] [int] NOT NULL,
	[Mentality] [decimal](18, 2) NOT NULL,
	[Pressure] [decimal](18, 2) NOT NULL,
	[Tempo] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK__TeamTact__123AE7997AC9E17D] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/28/2015 8:56:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[Password] [varchar](1000) NOT NULL,
	[CountryId] [int] NOT NULL,
	[TeamId] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Countries] ADD  DEFAULT ((0)) FOR [NrOfLeagesToAdd]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK__Matches__AwayTea__5DCAEF64] FOREIGN KEY([AwayTeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK__Matches__AwayTea__5DCAEF64]
GO
ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK__Matches__HomeTea__5CD6CB2B] FOREIGN KEY([HomeTeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK__Matches__HomeTea__5CD6CB2B]
GO
ALTER TABLE [dbo].[Players]  WITH CHECK ADD FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[PlayersAttributesValues]  WITH NOCHECK ADD  CONSTRAINT [FK__PlayersAt__Playe__5629CD9C] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Players] ([Id])
GO
ALTER TABLE [dbo].[PlayersAttributesValues] CHECK CONSTRAINT [FK__PlayersAt__Playe__5629CD9C]
GO
ALTER TABLE [dbo].[Series]  WITH CHECK ADD FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Series]  WITH CHECK ADD FOREIGN KEY([ParentId])
REFERENCES [dbo].[Series] ([Id])
GO
ALTER TABLE [dbo].[Teams]  WITH CHECK ADD FOREIGN KEY([SeriesId])
REFERENCES [dbo].[Series] ([Id])
GO
ALTER TABLE [dbo].[TeamTactics]  WITH CHECK ADD  CONSTRAINT [FK__TeamTacti__TeamI__6FE99F9F] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[TeamTactics] CHECK CONSTRAINT [FK__TeamTacti__TeamI__6FE99F9F]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK__Users__TeamId__2B3F6F97] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK__Users__TeamId__2B3F6F97]
GO
USE [master]
GO
ALTER DATABASE [FootballManager] SET  READ_WRITE 
GO
