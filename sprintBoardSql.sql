USE [master]
GO
/****** Object:  Database [sprintboarddb]    Script Date: 16.5.2018 08:42:29 ******/
CREATE DATABASE [sprintboarddb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'sprintboarddb', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\sprintboarddb.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'sprintboarddb_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\sprintboarddb_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [sprintboarddb] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [sprintboarddb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [sprintboarddb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [sprintboarddb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [sprintboarddb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [sprintboarddb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [sprintboarddb] SET ARITHABORT OFF 
GO
ALTER DATABASE [sprintboarddb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [sprintboarddb] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [sprintboarddb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [sprintboarddb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [sprintboarddb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [sprintboarddb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [sprintboarddb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [sprintboarddb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [sprintboarddb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [sprintboarddb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [sprintboarddb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [sprintboarddb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [sprintboarddb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [sprintboarddb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [sprintboarddb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [sprintboarddb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [sprintboarddb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [sprintboarddb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [sprintboarddb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [sprintboarddb] SET  MULTI_USER 
GO
ALTER DATABASE [sprintboarddb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [sprintboarddb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [sprintboarddb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [sprintboarddb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [sprintboarddb]
GO
/****** Object:  Table [dbo].[Stories]    Script Date: 16.5.2018 08:42:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stories](
	[StoryID] [int] IDENTITY(1,1) NOT NULL,
	[StoryName] [nvarchar](150) NULL,
	[StoryText] [text] NULL,
	[StoryStart] [nvarchar](50) NULL,
	[Last] [nvarchar](50) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Stories] PRIMARY KEY CLUSTERED 
(
	[StoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 16.5.2018 08:42:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[StoryID] [int] NULL,
	[TeamMemberID] [int] NULL,
	[TaskName] [nvarchar](150) NULL,
	[TaskText] [text] NULL,
	[Start] [nvarchar](50) NULL,
	[Last] [nvarchar](50) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TeamMembers]    Script Date: 16.5.2018 08:42:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamMembers](
	[MemberID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Surname] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[ImagePath] [nvarchar](250) NULL,
	[Aktif] [tinyint] NULL,
	[Color] [nvarchar](150) NULL,
 CONSTRAINT [PK_TeamMembers] PRIMARY KEY CLUSTERED 
(
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[BoardData]    Script Date: 16.5.2018 08:42:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[BoardData] AS
SELECT        
dbo.Tasks.TaskID, 
dbo.Tasks.TaskName, 
dbo.Tasks.StoryID,
dbo.Stories.StoryName, 
dbo.Tasks.TeamMemberID, 
dbo.TeamMembers.Name, 
dbo.TeamMembers.Surname,
dbo.Stories.StoryText, 
dbo.Stories.StoryStart, 
dbo.Stories.Deathline,
dbo.Stories.Status, 
dbo.TeamMembers.Title, 
dbo.TeamMembers.ImagePath, 
dbo.TeamMembers.Aktif, 
dbo.Tasks.TaskText, 
dbo.Tasks.Percentage, 
dbo.Tasks.Start, 
dbo.Tasks.Deathline AS TaskDeathline, 
dbo.Tasks.Priority, 
dbo.Tasks.Status AS TaskStatus

FROM            dbo.Stories INNER JOIN
                         dbo.Tasks ON dbo.Stories.StoryID = dbo.Tasks.StoryID CROSS JOIN
                         dbo.TeamMembers
GO
USE [master]
GO
ALTER DATABASE [sprintboarddb] SET  READ_WRITE 
GO
