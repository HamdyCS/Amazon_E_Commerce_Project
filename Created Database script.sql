USE [master]
GO
/****** Object:  Database [Amazon_E_Commerce_DB]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE DATABASE [Amazon_E_Commerce_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Amazon_E_Commerce_DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Amazon_E_Commerce_DB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Amazon_E_Commerce_DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Amazon_E_Commerce_DB_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Amazon_E_Commerce_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET  MULTI_USER 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Amazon_E_Commerce_DB', N'ON'
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET QUERY_STORE = ON
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Amazon_E_Commerce_DB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationOrders]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationOrders](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [bigint] NOT NULL,
	[ApplicationOrderTypeId] [bigint] NOT NULL,
	[ShoppingCartId] [bigint] NOT NULL,
	[PaymentId] [bigint] NOT NULL,
	[DeliveryId] [nvarchar](450) NULL,
	[CreatedBy] [nvarchar](450) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__Applicat__3214EC07270C916F] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationOrdersTypes]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationOrdersTypes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[DescriptionAr] [nvarchar](1000) NULL,
	[DescriptionEn] [nvarchar](1000) NULL,
 CONSTRAINT [PK__Applicat__3214EC0724535CF2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Applications]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applications](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ApplicationTypeId] [bigint] NOT NULL,
	[ReturnApplicationId] [bigint] NULL,
 CONSTRAINT [PK__Applicat__3214EC072B1F9265] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationTypes]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationTypes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[DescriptionAr] [nvarchar](1000) NULL,
	[DescriptionEn] [nvarchar](1000) NULL,
 CONSTRAINT [PK__Applicat__3214EC0773737997] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brands]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brands](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name_En] [nvarchar](255) NOT NULL,
	[Name_Ar] [nvarchar](255) NOT NULL,
	[CreatedBy] [nvarchar](450) NOT NULL,
	[DateOfDeletion] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Image] [varbinary](max) NOT NULL,
 CONSTRAINT [PK__Brands__3214EC07FE6191F7] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name_En] [nvarchar](255) NOT NULL,
	[Name_Ar] [nvarchar](255) NOT NULL,
	[CreatedBy] [nvarchar](450) NOT NULL,
	[DateOfDelete] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Cities__3214EC07A2F63C83] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CitiesWhereDeliveiesWorks]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CitiesWhereDeliveiesWorks](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CityId] [bigint] NOT NULL,
	[DeliveryId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK__CitiesWh__3214EC0714D1D3E2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](max) NULL,
	[TimeStamp] [datetime] NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Otps]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Otps](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ExpiresAt] [datetime2](7) NOT NULL,
	[IsUsed] [bit] NOT NULL,
 CONSTRAINT [PK_Otps] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TotalPrice] [decimal](10, 2) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[PaymentTypeId] [bigint] NOT NULL,
	[ShoppingCartId] [bigint] NOT NULL,
	[shippingCostId] [bigint] NOT NULL,
	[UserAddressId] [bigint] NOT NULL,
	[PaymentStatusId] [int] NULL,
	[InvoiceId] [nvarchar](max) NULL,
	[SessionId] [nvarchar](max) NULL,
 CONSTRAINT [PK__Payments__3214EC07FFE12C42] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentStatuses]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentStatuses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[DescriptionAr] [nvarchar](1000) NULL,
	[DescriptionEn] [nvarchar](1000) NULL,
 CONSTRAINT [PK_PaymentStatuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentsTypes]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentsTypes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[DescriptionAr] [nvarchar](1000) NULL,
	[DescriptionEn] [nvarchar](1000) NULL,
 CONSTRAINT [PK__Payments__3214EC073B7ADCD8] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[People]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__People__3214EC0750368F63] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategories]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name_En] [nvarchar](255) NOT NULL,
	[Name_Ar] [nvarchar](255) NOT NULL,
	[Description_En] [nvarchar](1000) NULL,
	[Description_Ar] [nvarchar](1000) NULL,
	[CreatedBy] [nvarchar](450) NOT NULL,
	[DateOfDeletion] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__ProductC__3214EC07FFD11FC9] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategoryImages]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategoryImages](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductCategoryId] [bigint] NOT NULL,
	[Image] [varbinary](max) NOT NULL,
 CONSTRAINT [PK__ProductC__3214EC073F10B0DE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImages]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImages](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Image] [varbinary](max) NOT NULL,
 CONSTRAINT [PK__ProductI__3214EC07794FBA5A] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name_En] [nvarchar](255) NOT NULL,
	[Name_Ar] [nvarchar](255) NOT NULL,
	[Size] [nvarchar](50) NOT NULL,
	[Color] [nvarchar](50) NOT NULL,
	[Height] [decimal](10, 2) NOT NULL,
	[Length] [decimal](10, 2) NOT NULL,
	[CreatedBy] [nvarchar](450) NOT NULL,
	[ProductSubCategoryId] [bigint] NOT NULL,
	[BrandId] [bigint] NOT NULL,
	[DateOfDeletion] [datetime2](7) NULL,
	[DescriptionAr] [nvarchar](max) NULL,
	[DescriptionEn] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Products__3214EC07813EF3B4] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductSubCategories]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductSubCategories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NameAr] [nvarchar](max) NOT NULL,
	[DescriptionEn] [nvarchar](max) NULL,
	[DescriptionAr] [nvarchar](max) NULL,
	[Image] [varbinary](max) NOT NULL,
	[CreatedBy] [nvarchar](450) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateOfDeletion] [datetime2](7) NULL,
	[ProductCategoryId] [bigint] NOT NULL,
	[NameEn] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ProductSubCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshTokens]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ExpiresAt] [datetime] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK__RefreshT__3214EC075E8158E9] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerProductReviews]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerProductReviews](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NumberOfStars] [int] NOT NULL,
	[Message] [nvarchar](1000) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[SellerProductId] [bigint] NOT NULL,
	[DateOfDeletion] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK__ProductR__3214EC07ADEC10FB] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerProducts]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerProducts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SellerId] [nvarchar](450) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[NumberInStock] [int] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[DateOfDeletion] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__SellerPr__3214EC07E45442F5] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerProductsInShoppingCarts]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerProductsInShoppingCarts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[TotalPrice] [decimal](10, 2) NULL,
	[SellerProductId] [bigint] NOT NULL,
	[ShoppingCartId] [bigint] NOT NULL,
 CONSTRAINT [PK__Products__3214EC0709E6EA67] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShippingCosts]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShippingCosts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[CreatedBy] [nvarchar](450) NOT NULL,
	[CityId] [bigint] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[DateOfDeletion] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Shipping__3214EC075E9DA2D6] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShoppingCarts]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShoppingCarts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__Shopping__3214EC07B8EE46F4] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](450) NOT NULL,
	[PersonId] [bigint] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateOfDeletion] [datetime2](7) NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersAddresses]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersAddresses](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[CityId] [bigint] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[DateOfDeleted] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_UsersAddresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApplicationOrders_ApplicationId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ApplicationOrders_ApplicationId] ON [dbo].[ApplicationOrders]
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApplicationOrders_ApplicationOrderTypeId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ApplicationOrders_ApplicationOrderTypeId] ON [dbo].[ApplicationOrders]
(
	[ApplicationOrderTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ApplicationOrders_CreatedBy]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ApplicationOrders_CreatedBy] ON [dbo].[ApplicationOrders]
(
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ApplicationOrders_DeliveryId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ApplicationOrders_DeliveryId] ON [dbo].[ApplicationOrders]
(
	[DeliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApplicationOrders_PaymentId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ApplicationOrders_PaymentId] ON [dbo].[ApplicationOrders]
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApplicationOrders_ShoppingCartId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ApplicationOrders_ShoppingCartId] ON [dbo].[ApplicationOrders]
(
	[ShoppingCartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Applications_ApplicationTypeId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Applications_ApplicationTypeId] ON [dbo].[Applications]
(
	[ApplicationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Applications_ReturnApplicationId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Applications_ReturnApplicationId] ON [dbo].[Applications]
(
	[ReturnApplicationId] ASC
)
WHERE ([ReturnApplicationId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Applications_UserId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Applications_UserId] ON [dbo].[Applications]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Brands_CreatedBy]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Brands_CreatedBy] ON [dbo].[Brands]
(
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Cities_CreatedBy]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Cities_CreatedBy] ON [dbo].[Cities]
(
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CitiesWhereDeliveiesWorks_CityId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_CitiesWhereDeliveiesWorks_CityId] ON [dbo].[CitiesWhereDeliveiesWorks]
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CitiesWhereDeliveiesWorks_DeliveryId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_CitiesWhereDeliveiesWorks_DeliveryId] ON [dbo].[CitiesWhereDeliveiesWorks]
(
	[DeliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Payments_PaymentStatusId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Payments_PaymentStatusId] ON [dbo].[Payments]
(
	[PaymentStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Payments_PaymentTypeId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Payments_PaymentTypeId] ON [dbo].[Payments]
(
	[PaymentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Payments_shippingCostId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Payments_shippingCostId] ON [dbo].[Payments]
(
	[shippingCostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Payments_ShoppingCartId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Payments_ShoppingCartId] ON [dbo].[Payments]
(
	[ShoppingCartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Payments_UserAddressId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Payments_UserAddressId] ON [dbo].[Payments]
(
	[UserAddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ProductCategories_CreatedBy]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductCategories_CreatedBy] ON [dbo].[ProductCategories]
(
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__ProductC__33341C8A8951CC7B]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ__ProductC__33341C8A8951CC7B] ON [dbo].[ProductCategories]
(
	[Name_En] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__ProductC__33347C132A72C059]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ__ProductC__33347C132A72C059] ON [dbo].[ProductCategories]
(
	[Name_Ar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductCategoryImages_ProductCategoryId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductCategoryImages_ProductCategoryId] ON [dbo].[ProductCategoryImages]
(
	[ProductCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductImages_ProductId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductImages_ProductId] ON [dbo].[ProductImages]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_BrandId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_BrandId] ON [dbo].[Products]
(
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Products_CreatedBy]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_CreatedBy] ON [dbo].[Products]
(
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Products_Name_Ar]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_Name_Ar] ON [dbo].[Products]
(
	[Name_Ar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Products_Name_En]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_Name_En] ON [dbo].[Products]
(
	[Name_En] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_ProductSubCategoryId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_ProductSubCategoryId] ON [dbo].[Products]
(
	[ProductSubCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ProductSubCategories_CreatedBy]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductSubCategories_CreatedBy] ON [dbo].[ProductSubCategories]
(
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductSubCategories_ProductCategoryId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductSubCategories_ProductCategoryId] ON [dbo].[ProductSubCategories]
(
	[ProductCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RefreshTokens_UserId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId] ON [dbo].[RefreshTokens]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[Roles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SellerProductReviews_SellerProductId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_SellerProductReviews_SellerProductId] ON [dbo].[SellerProductReviews]
(
	[SellerProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SellerProductReviews_UserId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_SellerProductReviews_UserId] ON [dbo].[SellerProductReviews]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SellerProducts_ProductId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_SellerProducts_ProductId] ON [dbo].[SellerProducts]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SellerProducts_SellerId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_SellerProducts_SellerId] ON [dbo].[SellerProducts]
(
	[SellerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductsInShoppingCarts_SellerProductId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductsInShoppingCarts_SellerProductId] ON [dbo].[SellerProductsInShoppingCarts]
(
	[SellerProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductsInShoppingCarts_ShoppingCartId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductsInShoppingCarts_ShoppingCartId] ON [dbo].[SellerProductsInShoppingCarts]
(
	[ShoppingCartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShippingCosts_CityId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShippingCosts_CityId] ON [dbo].[ShippingCosts]
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ShippingCosts_CreatedBy]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShippingCosts_CreatedBy] ON [dbo].[ShippingCosts]
(
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ShoppingCarts_UserId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShoppingCarts_UserId] ON [dbo].[ShoppingCarts]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserRoles_RoleId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserRoles_RoleId] ON [dbo].[UserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[Users]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_PersonId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_Users_PersonId] ON [dbo].[Users]
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[Users]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UsersAddresses_CityId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_UsersAddresses_CityId] ON [dbo].[UsersAddresses]
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UsersAddresses_UserId]    Script Date: Thursday, October 16, 2025 6:30:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_UsersAddresses_UserId] ON [dbo].[UsersAddresses]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApplicationOrders] ADD  DEFAULT (N'') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[ApplicationOrders] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Brands] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Brands] ADD  DEFAULT (0x) FOR [Image]
GO
ALTER TABLE [dbo].[Cities] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Otps] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsUsed]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (CONVERT([bigint],(0))) FOR [ShoppingCartId]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (CONVERT([bigint],(0))) FOR [shippingCostId]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (CONVERT([bigint],(0))) FOR [UserAddressId]
GO
ALTER TABLE [dbo].[ProductCategories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ProductSubCategories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ProductSubCategories] ADD  DEFAULT (N'') FOR [NameEn]
GO
ALTER TABLE [dbo].[SellerProductReviews] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[SellerProductReviews] ADD  DEFAULT (N'') FOR [Name]
GO
ALTER TABLE [dbo].[SellerProducts] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ShippingCosts] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ShippingCosts] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ShoppingCarts] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[UsersAddresses] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ApplicationOrders]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationOrders_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Applications] ([Id])
GO
ALTER TABLE [dbo].[ApplicationOrders] CHECK CONSTRAINT [FK_ApplicationOrders_ApplicationId]
GO
ALTER TABLE [dbo].[ApplicationOrders]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationOrders_ApplicationOrderTypeId] FOREIGN KEY([ApplicationOrderTypeId])
REFERENCES [dbo].[ApplicationOrdersTypes] ([Id])
GO
ALTER TABLE [dbo].[ApplicationOrders] CHECK CONSTRAINT [FK_ApplicationOrders_ApplicationOrderTypeId]
GO
ALTER TABLE [dbo].[ApplicationOrders]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationOrders_PaymentId] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payments] ([Id])
GO
ALTER TABLE [dbo].[ApplicationOrders] CHECK CONSTRAINT [FK_ApplicationOrders_PaymentId]
GO
ALTER TABLE [dbo].[ApplicationOrders]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationOrders_ShoppingCartId] FOREIGN KEY([ShoppingCartId])
REFERENCES [dbo].[ShoppingCarts] ([Id])
GO
ALTER TABLE [dbo].[ApplicationOrders] CHECK CONSTRAINT [FK_ApplicationOrders_ShoppingCartId]
GO
ALTER TABLE [dbo].[ApplicationOrders]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationOrders_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ApplicationOrders] CHECK CONSTRAINT [FK_ApplicationOrders_Users_CreatedBy]
GO
ALTER TABLE [dbo].[ApplicationOrders]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationOrders_Users_DeliveryId] FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ApplicationOrders] CHECK CONSTRAINT [FK_ApplicationOrders_Users_DeliveryId]
GO
ALTER TABLE [dbo].[Applications]  WITH CHECK ADD  CONSTRAINT [FK_Application_ApplicationTypeId] FOREIGN KEY([ApplicationTypeId])
REFERENCES [dbo].[ApplicationTypes] ([Id])
GO
ALTER TABLE [dbo].[Applications] CHECK CONSTRAINT [FK_Application_ApplicationTypeId]
GO
ALTER TABLE [dbo].[Applications]  WITH CHECK ADD  CONSTRAINT [FK_Applications_Applications_ReturnApplicationId] FOREIGN KEY([ReturnApplicationId])
REFERENCES [dbo].[Applications] ([Id])
GO
ALTER TABLE [dbo].[Applications] CHECK CONSTRAINT [FK_Applications_Applications_ReturnApplicationId]
GO
ALTER TABLE [dbo].[Applications]  WITH CHECK ADD  CONSTRAINT [FK_Applications_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Applications] CHECK CONSTRAINT [FK_Applications_Users_UserId]
GO
ALTER TABLE [dbo].[Brands]  WITH CHECK ADD  CONSTRAINT [FK_Brands_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Brands] CHECK CONSTRAINT [FK_Brands_Users_CreatedBy]
GO
ALTER TABLE [dbo].[Cities]  WITH CHECK ADD  CONSTRAINT [FK_Cities_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Cities] CHECK CONSTRAINT [FK_Cities_Users_CreatedBy]
GO
ALTER TABLE [dbo].[CitiesWhereDeliveiesWorks]  WITH CHECK ADD  CONSTRAINT [FK_CitiesWhereDeliveiesWorks_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[CitiesWhereDeliveiesWorks] CHECK CONSTRAINT [FK_CitiesWhereDeliveiesWorks_CityId]
GO
ALTER TABLE [dbo].[CitiesWhereDeliveiesWorks]  WITH CHECK ADD  CONSTRAINT [FK_CitiesWhereDeliveiesWorks_Users_DeliveryId] FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[CitiesWhereDeliveiesWorks] CHECK CONSTRAINT [FK_CitiesWhereDeliveiesWorks_Users_DeliveryId]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_PaymentStatuses_PaymentStatusId] FOREIGN KEY([PaymentStatusId])
REFERENCES [dbo].[PaymentStatuses] ([Id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_PaymentStatuses_PaymentStatusId]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_PaymentTypeId] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[PaymentsTypes] ([Id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_PaymentTypeId]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_ShippingCosts_shippingCostId] FOREIGN KEY([shippingCostId])
REFERENCES [dbo].[ShippingCosts] ([Id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_ShippingCosts_shippingCostId]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_ShoppingCarts_ShoppingCartId] FOREIGN KEY([ShoppingCartId])
REFERENCES [dbo].[ShoppingCarts] ([Id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_ShoppingCarts_ShoppingCartId]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_UsersAddresses_UserAddressId] FOREIGN KEY([UserAddressId])
REFERENCES [dbo].[UsersAddresses] ([Id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_UsersAddresses_UserAddressId]
GO
ALTER TABLE [dbo].[ProductCategories]  WITH CHECK ADD  CONSTRAINT [FK_ProductCategories_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ProductCategories] CHECK CONSTRAINT [FK_ProductCategories_Users_CreatedBy]
GO
ALTER TABLE [dbo].[ProductCategoryImages]  WITH CHECK ADD  CONSTRAINT [FK_ProductCategoryImages_ProductCategoryId] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategories] ([Id])
GO
ALTER TABLE [dbo].[ProductCategoryImages] CHECK CONSTRAINT [FK_ProductCategoryImages_ProductCategoryId]
GO
ALTER TABLE [dbo].[ProductImages]  WITH CHECK ADD  CONSTRAINT [FK_ProductImages_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[ProductImages] CHECK CONSTRAINT [FK_ProductImages_ProductId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_BrandId] FOREIGN KEY([BrandId])
REFERENCES [dbo].[Brands] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_BrandId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_ProductSubCategories_ProductSubCategoryId] FOREIGN KEY([ProductSubCategoryId])
REFERENCES [dbo].[ProductSubCategories] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_ProductSubCategories_ProductSubCategoryId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Users_CreatedBy]
GO
ALTER TABLE [dbo].[ProductSubCategories]  WITH CHECK ADD  CONSTRAINT [FK_ProductSubCategories_ProductCategories_ProductCategoryId] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategories] ([Id])
GO
ALTER TABLE [dbo].[ProductSubCategories] CHECK CONSTRAINT [FK_ProductSubCategories_ProductCategories_ProductCategoryId]
GO
ALTER TABLE [dbo].[ProductSubCategories]  WITH CHECK ADD  CONSTRAINT [FK_ProductSubCategories_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ProductSubCategories] CHECK CONSTRAINT [FK_ProductSubCategories_Users_CreatedBy]
GO
ALTER TABLE [dbo].[RefreshTokens]  WITH CHECK ADD  CONSTRAINT [FK_RefreshTokens_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[RefreshTokens] CHECK CONSTRAINT [FK_RefreshTokens_Users_UserId]
GO
ALTER TABLE [dbo].[SellerProductReviews]  WITH CHECK ADD  CONSTRAINT [FK_ProductReviews_SellerProductId] FOREIGN KEY([SellerProductId])
REFERENCES [dbo].[SellerProducts] ([Id])
GO
ALTER TABLE [dbo].[SellerProductReviews] CHECK CONSTRAINT [FK_ProductReviews_SellerProductId]
GO
ALTER TABLE [dbo].[SellerProductReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerProductReviews_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[SellerProductReviews] CHECK CONSTRAINT [FK_SellerProductReviews_Users_UserId]
GO
ALTER TABLE [dbo].[SellerProducts]  WITH CHECK ADD  CONSTRAINT [FK_SellerProducts_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[SellerProducts] CHECK CONSTRAINT [FK_SellerProducts_ProductId]
GO
ALTER TABLE [dbo].[SellerProducts]  WITH CHECK ADD  CONSTRAINT [FK_SellerProducts_Users_SellerId] FOREIGN KEY([SellerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[SellerProducts] CHECK CONSTRAINT [FK_SellerProducts_Users_SellerId]
GO
ALTER TABLE [dbo].[SellerProductsInShoppingCarts]  WITH CHECK ADD  CONSTRAINT [FK_ProductsInShoppingCarts_SellerProductId] FOREIGN KEY([SellerProductId])
REFERENCES [dbo].[SellerProducts] ([Id])
GO
ALTER TABLE [dbo].[SellerProductsInShoppingCarts] CHECK CONSTRAINT [FK_ProductsInShoppingCarts_SellerProductId]
GO
ALTER TABLE [dbo].[SellerProductsInShoppingCarts]  WITH CHECK ADD  CONSTRAINT [FK_ProductsInShoppingCarts_ShoppingCartId] FOREIGN KEY([ShoppingCartId])
REFERENCES [dbo].[ShoppingCarts] ([Id])
GO
ALTER TABLE [dbo].[SellerProductsInShoppingCarts] CHECK CONSTRAINT [FK_ProductsInShoppingCarts_ShoppingCartId]
GO
ALTER TABLE [dbo].[ShippingCosts]  WITH CHECK ADD  CONSTRAINT [FK_ShippingCosts_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[ShippingCosts] CHECK CONSTRAINT [FK_ShippingCosts_CityId]
GO
ALTER TABLE [dbo].[ShippingCosts]  WITH CHECK ADD  CONSTRAINT [FK_ShippingCosts_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ShippingCosts] CHECK CONSTRAINT [FK_ShippingCosts_Users_CreatedBy]
GO
ALTER TABLE [dbo].[ShoppingCarts]  WITH CHECK ADD  CONSTRAINT [FK_ShoppingCarts_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ShoppingCarts] CHECK CONSTRAINT [FK_ShoppingCarts_Users_UserId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles_RoleId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users_UserId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_People_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_People_PersonId]
GO
ALTER TABLE [dbo].[UsersAddresses]  WITH CHECK ADD  CONSTRAINT [FK_UsersAddresses_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[UsersAddresses] CHECK CONSTRAINT [FK_UsersAddresses_CityId]
GO
ALTER TABLE [dbo].[UsersAddresses]  WITH CHECK ADD  CONSTRAINT [FK_UsersAddresses_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UsersAddresses] CHECK CONSTRAINT [FK_UsersAddresses_Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [Amazon_E_Commerce_DB] SET  READ_WRITE 
GO
