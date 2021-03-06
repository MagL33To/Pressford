USE [master]
GO
/****** Object:  Database [KPMGTest]    Script Date: 11/11/2015 00:58:29 ******/
CREATE DATABASE [KPMGTest]
GO
ALTER DATABASE [KPMGTest] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KPMGTest].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KPMGTest] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KPMGTest] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KPMGTest] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KPMGTest] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KPMGTest] SET ARITHABORT OFF 
GO
ALTER DATABASE [KPMGTest] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [KPMGTest] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KPMGTest] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KPMGTest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KPMGTest] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KPMGTest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KPMGTest] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KPMGTest] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KPMGTest] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KPMGTest] SET  ENABLE_BROKER 
GO
ALTER DATABASE [KPMGTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KPMGTest] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KPMGTest] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KPMGTest] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [KPMGTest] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KPMGTest] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [KPMGTest] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [KPMGTest] SET RECOVERY FULL 
GO
ALTER DATABASE [KPMGTest] SET  MULTI_USER 
GO
ALTER DATABASE [KPMGTest] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KPMGTest] SET DB_CHAINING OFF 
GO
USE [KPMGTest]
GO
/****** Object:  Table [dbo].[Articles]    Script Date: 11/11/2015 00:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_Articles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Comments]    Script Date: 11/11/2015 00:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[ArticleId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL
)

GO
/****** Object:  Table [dbo].[Likes]    Script Date: 11/11/2015 00:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Likes](
	[UserId] [int] NOT NULL,
	[ArticleId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Likes] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ArticleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/11/2015 00:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Forename] [nvarchar](max) NOT NULL,
	[Surname] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
ALTER TABLE [dbo].[Articles]  WITH CHECK ADD  CONSTRAINT [FK_Articles_UsersCreated] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Articles] CHECK CONSTRAINT [FK_Articles_UsersCreated]
GO
ALTER TABLE [dbo].[Articles]  WITH CHECK ADD  CONSTRAINT [FK_Articles_UsersUpdated] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Articles] CHECK CONSTRAINT [FK_Articles_UsersUpdated]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Articles] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Articles] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Articles]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users]
GO
ALTER TABLE [dbo].[Likes]  WITH CHECK ADD  CONSTRAINT [FK_Likes_Articles] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Articles] ([Id])
GO
ALTER TABLE [dbo].[Likes] CHECK CONSTRAINT [FK_Likes_Articles]
GO
ALTER TABLE [dbo].[Likes]  WITH CHECK ADD  CONSTRAINT [FK_Likes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Likes] CHECK CONSTRAINT [FK_Likes_Users]
GO
USE [master]
GO
ALTER DATABASE [KPMGTest] SET  READ_WRITE 
GO
USE [KPMGTest]
GO
INSERT INTO [dbo].[Users]
VALUES ('William', 'Pressford', 'w.pressford@pressford.com', 'pressford4life', 1)
INSERT INTO [dbo].[Users]
VALUES ('Luke', 'Skywalker', 'l.skywalker@pressford.com', 'xwingrules', 0)
INSERT INTO [dbo].[Users]
VALUES ('Obi-Wan', 'Kenobi', 'o.kenobi@pressford.com', 'forcemaster1', 0)
INSERT INTO [dbo].[Users]
VALUES ('Anakin', 'Skywalker', 'a.skywalker@pressford.com', 'darksidelols', 1)