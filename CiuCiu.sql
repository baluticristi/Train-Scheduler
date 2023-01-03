USE [master]
GO
/****** Object:  Database [CiuCiu]    Script Date: 12/19/2022 11:55:30 PM ******/
CREATE DATABASE [CiuCiu]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CiuCiu', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CiuCiu.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CiuCiu_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CiuCiu_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CiuCiu] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CiuCiu].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CiuCiu] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CiuCiu] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CiuCiu] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CiuCiu] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CiuCiu] SET ARITHABORT OFF 
GO
ALTER DATABASE [CiuCiu] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CiuCiu] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CiuCiu] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CiuCiu] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CiuCiu] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CiuCiu] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CiuCiu] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CiuCiu] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CiuCiu] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CiuCiu] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CiuCiu] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CiuCiu] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CiuCiu] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CiuCiu] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CiuCiu] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CiuCiu] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CiuCiu] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CiuCiu] SET RECOVERY FULL 
GO
ALTER DATABASE [CiuCiu] SET  MULTI_USER 
GO
ALTER DATABASE [CiuCiu] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CiuCiu] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CiuCiu] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CiuCiu] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CiuCiu] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CiuCiu] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CiuCiu', N'ON'
GO
ALTER DATABASE [CiuCiu] SET QUERY_STORE = OFF
GO
USE [CiuCiu]
GO
/****** Object:  Table [dbo].[Line]    Script Date: 12/19/2022 11:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Line](
	[Line_id] [int] IDENTITY(300,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Line_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LineStations]    Script Date: 12/19/2022 11:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LineStations](
	[LineStations_id] [int] IDENTITY(400,1) NOT NULL,
	[Line_id] [int] NULL,
	[Station_id] [int] NULL,
	[Distance] [int] NULL,
	[DepartureTime] [time](7) NULL,
	[ArrivalTime] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[LineStations_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Requests]    Script Date: 12/19/2022 11:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Requests](
	[Request_id] [int] IDENTITY(1,1) NOT NULL,
	[User_id] [int] NULL,
	[date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Request_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 12/19/2022 11:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Role_id] [int] IDENTITY(1,1) NOT NULL,
	[Role_name] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[Role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Station]    Script Date: 12/19/2022 11:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Station](
	[Station_id] [int] IDENTITY(200,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Desctiption] [nvarchar](1024) NULL,
PRIMARY KEY CLUSTERED 
(
	[Station_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticket]    Script Date: 12/19/2022 11:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticket](
	[Ticket_id] [int] IDENTITY(5000,1) NOT NULL,
	[Price] [float] NULL,
	[User_id] [int] NULL,
	[Wagon_id] [int] NULL,
	[DayAndTime] [datetime] NULL,
	[DepartureStation_id] [int] NULL,
	[ArrivalStation_id] [int] NULL,
	[NumberOfSeat] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Ticket_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Train]    Script Date: 12/19/2022 11:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Train](
	[Train_id] [int] IDENTITY(1000,1) NOT NULL,
	[Line_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Train_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12/19/2022 11:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_id] [int] IDENTITY(100,1) NOT NULL,
	[FirstName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[email] [nvarchar](255) NULL,
	[phone] [nvarchar](255) NULL,
	[password] [nvarchar](255) NULL,
	[Role_id] [int] NULL,
	[is_verified] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[User_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wagons]    Script Date: 12/19/2022 11:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wagons](
	[Wagon_id] [int] IDENTITY(2000,1) NOT NULL,
	[Train_id] [int] NULL,
	[Capacity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Wagon_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Line] ON 

INSERT [dbo].[Line] ([Line_id], [Name]) VALUES (300, N'Bucuresti_Nord-Constanta')
INSERT [dbo].[Line] ([Line_id], [Name]) VALUES (301, N'Brasov-Bucuresti_Nord')
INSERT [dbo].[Line] ([Line_id], [Name]) VALUES (302, N'Craiova-Bucuresti_Nord')
SET IDENTITY_INSERT [dbo].[Line] OFF
GO
SET IDENTITY_INSERT [dbo].[LineStations] ON 

INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (400, 300, 200, 0, CAST(N'07:20:00' AS Time), NULL)
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (401, 300, 201, 7, CAST(N'07:30:00' AS Time), CAST(N'07:29:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (402, 300, 202, 109, CAST(N'08:17:00' AS Time), CAST(N'08:16:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (403, 300, 203, 146, CAST(N'08:37:00' AS Time), CAST(N'08:36:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (404, 300, 204, 166, CAST(N'08:51:00' AS Time), CAST(N'08:50:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (405, 300, 205, 190, CAST(N'09:08:00' AS Time), CAST(N'09:07:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (406, 300, 206, 225, NULL, CAST(N'09:31:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (407, 301, 207, 0, CAST(N'03:43:00' AS Time), NULL)
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (408, 301, 208, 26, CAST(N'04:41:00' AS Time), CAST(N'04:32:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (409, 301, 209, 45, CAST(N'04:59:00' AS Time), CAST(N'04:58:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (410, 301, 210, 74, CAST(N'05:22:00' AS Time), CAST(N'05:21:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (411, 301, 211, 88, CAST(N'05:34:00' AS Time), CAST(N'05:33:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (412, 301, 212, 107, CAST(N'05:47:00' AS Time), CAST(N'05:46:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (413, 301, 200, 166, NULL, CAST(N'06:27:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (414, 302, 214, 0, CAST(N'12:39:00' AS Time), NULL)
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (415, 302, 215, 53, CAST(N'13:27:00' AS Time), CAST(N'13:26:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (416, 302, 216, 71, CAST(N'13:45:00' AS Time), CAST(N'13:44:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (417, 302, 217, 109, CAST(N'14:21:00' AS Time), CAST(N'14:15:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (418, 302, 218, 155, CAST(N'15:11:00' AS Time), CAST(N'15:10:00' AS Time))
INSERT [dbo].[LineStations] ([LineStations_id], [Line_id], [Station_id], [Distance], [DepartureTime], [ArrivalTime]) VALUES (419, 302, 200, 209, NULL, CAST(N'16:00:00' AS Time))
SET IDENTITY_INSERT [dbo].[LineStations] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Role_id], [Role_name]) VALUES (1, N'Administrator')
INSERT [dbo].[Roles] ([Role_id], [Role_name]) VALUES (2, N'Adult')
INSERT [dbo].[Roles] ([Role_id], [Role_name]) VALUES (3, N'Student')
INSERT [dbo].[Roles] ([Role_id], [Role_name]) VALUES (4, N'Elder')
INSERT [dbo].[Roles] ([Role_id], [Role_name]) VALUES (5, N'Thief')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Station] ON 

INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (200, N'Bucuresti Nord', N'Capitala')
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (201, N'Bucuresti Baneasa', N'Margine Capitala')
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (202, N'Ciulnita', NULL)
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (203, N'Fetesti', N'Oras Frumos')
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (204, N'Cernavoda Pod', N'Aglomerat')
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (205, N'Medgidia', NULL)
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (206, N'Constanta', N'Mare')
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (207, N'Brasov', N'Cel Mai Frumos Oras')
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (208, N'Predeal', N'Partie de Schi')
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (209, N'Sinaia', NULL)
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (210, N'Campina', NULL)
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (211, N'Floresti Prahova', NULL)
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (212, N'Ploiesti Vest', NULL)
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (214, N'Craiova', N'Centru Universtar')
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (215, N'Caracal', NULL)
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (216, N'Draganesti Olt', NULL)
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (217, N'Roisori Nord', NULL)
INSERT [dbo].[Station] ([Station_id], [Name], [Desctiption]) VALUES (218, N'Videle', NULL)
SET IDENTITY_INSERT [dbo].[Station] OFF
GO
SET IDENTITY_INSERT [dbo].[Train] ON 

INSERT [dbo].[Train] ([Train_id], [Line_id]) VALUES (1000, 300)
INSERT [dbo].[Train] ([Train_id], [Line_id]) VALUES (1001, 301)
INSERT [dbo].[Train] ([Train_id], [Line_id]) VALUES (1002, 302)
SET IDENTITY_INSERT [dbo].[Train] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([User_id], [FirstName], [LastName], [email], [phone], [password], [Role_id], [is_verified]) VALUES (106, N'test', N'test', N'test@yahoo.com', N'0727272727', N'04wXAdKvE3I1aCzRpvgvFYuOTou8t1RH0h+HgFeHhYE=', 3, 1)
INSERT [dbo].[Users] ([User_id], [FirstName], [LastName], [email], [phone], [password], [Role_id], [is_verified]) VALUES (1106, N'test2', N'test2', N'test2@yahoo.com', N'0999999999', N'C5g2Web/aITklHTATEWRomnwNN7adgi7O7gPbhnahrU=', 5, 1)
INSERT [dbo].[Users] ([User_id], [FirstName], [LastName], [email], [phone], [password], [Role_id], [is_verified]) VALUES (1107, NULL, NULL, N'admin', NULL, N'OUpTJnL9FJMuMgrR1UHzQTvHBE1HvqAiy7MSEgvDXi4=', 1, 1)
INSERT [dbo].[Users] ([User_id], [FirstName], [LastName], [email], [phone], [password], [Role_id], [is_verified]) VALUES (1108, N'Vlad', N'Popescu', N'popescu@yahoo.com', N'0722932321', N'h092KqOFDPeHw9qDB8DIJKjX1D1gYMldPsg5aAQCXWg=', 3, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Wagons] ON 

INSERT [dbo].[Wagons] ([Wagon_id], [Train_id], [Capacity]) VALUES (2000, 1000, 60)
INSERT [dbo].[Wagons] ([Wagon_id], [Train_id], [Capacity]) VALUES (2001, 1000, 60)
INSERT [dbo].[Wagons] ([Wagon_id], [Train_id], [Capacity]) VALUES (2002, 1000, 60)
INSERT [dbo].[Wagons] ([Wagon_id], [Train_id], [Capacity]) VALUES (2003, 1001, 75)
INSERT [dbo].[Wagons] ([Wagon_id], [Train_id], [Capacity]) VALUES (2006, 1001, 55)
INSERT [dbo].[Wagons] ([Wagon_id], [Train_id], [Capacity]) VALUES (2007, 1002, 80)
INSERT [dbo].[Wagons] ([Wagon_id], [Train_id], [Capacity]) VALUES (2008, 1002, 70)
INSERT [dbo].[Wagons] ([Wagon_id], [Train_id], [Capacity]) VALUES (2009, 1002, 75)
SET IDENTITY_INSERT [dbo].[Wagons] OFF
GO
ALTER TABLE [dbo].[LineStations]  WITH CHECK ADD FOREIGN KEY([Line_id])
REFERENCES [dbo].[Line] ([Line_id])
GO
ALTER TABLE [dbo].[LineStations]  WITH CHECK ADD FOREIGN KEY([Station_id])
REFERENCES [dbo].[Station] ([Station_id])
GO
ALTER TABLE [dbo].[Requests]  WITH CHECK ADD FOREIGN KEY([User_id])
REFERENCES [dbo].[Users] ([User_id])
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD FOREIGN KEY([ArrivalStation_id])
REFERENCES [dbo].[Station] ([Station_id])
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD FOREIGN KEY([DepartureStation_id])
REFERENCES [dbo].[Station] ([Station_id])
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD FOREIGN KEY([User_id])
REFERENCES [dbo].[Users] ([User_id])
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD FOREIGN KEY([Wagon_id])
REFERENCES [dbo].[Wagons] ([Wagon_id])
GO
ALTER TABLE [dbo].[Train]  WITH CHECK ADD FOREIGN KEY([Line_id])
REFERENCES [dbo].[Line] ([Line_id])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([Role_id])
REFERENCES [dbo].[Roles] ([Role_id])
GO
ALTER TABLE [dbo].[Wagons]  WITH CHECK ADD FOREIGN KEY([Train_id])
REFERENCES [dbo].[Train] ([Train_id])
GO
USE [master]
GO
ALTER DATABASE [CiuCiu] SET  READ_WRITE 
GO
