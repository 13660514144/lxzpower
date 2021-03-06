USE [master]
GO
/****** Object:  Database [SysLog]    Script Date: 01/06/2021 20:40:40 ******/
CREATE DATABASE [SysLog] ON  PRIMARY 
( NAME = N'SysLog', FILENAME = N'C:\DATA\SysLog.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SysLog_log', FILENAME = N'C:\DATA\SysLog_1.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SysLog] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SysLog].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SysLog] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [SysLog] SET ANSI_NULLS OFF
GO
ALTER DATABASE [SysLog] SET ANSI_PADDING OFF
GO
ALTER DATABASE [SysLog] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [SysLog] SET ARITHABORT OFF
GO
ALTER DATABASE [SysLog] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [SysLog] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [SysLog] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [SysLog] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [SysLog] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [SysLog] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [SysLog] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [SysLog] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [SysLog] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [SysLog] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [SysLog] SET  DISABLE_BROKER
GO
ALTER DATABASE [SysLog] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [SysLog] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [SysLog] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [SysLog] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [SysLog] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [SysLog] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [SysLog] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [SysLog] SET  READ_WRITE
GO
ALTER DATABASE [SysLog] SET RECOVERY FULL
GO
ALTER DATABASE [SysLog] SET  MULTI_USER
GO
ALTER DATABASE [SysLog] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [SysLog] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'SysLog', N'ON'
GO
USE [SysLog]
GO
/****** Object:  User [ysmx]    Script Date: 01/06/2021 20:40:40 ******/
CREATE USER [ysmx] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Tbl_Log]    Script Date: 01/06/2021 20:40:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Log](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentId] [nvarchar](50) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[OldRemarks] [nvarchar](max) NULL,
	[NewRemarks] [nvarchar](max) NULL,
	[Db] [nvarchar](50) NULL,
	[Tb] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[LastModiDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[Manager] [nvarchar](50) NULL,
	[DelFlg] [int] NULL,
	[UnitCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键 ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主表流水号ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主表流水号Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作前记录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'OldRemarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作后记录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'NewRemarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'源库' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'Db'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'源表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'Tb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'LastModiDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记 1.正常 0.删除(所有查询强制过滤)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'DelFlg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据分组标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log', @level2type=N'COLUMN',@level2name=N'UnitCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Log'
GO
/****** Object:  Default [DF_Tbl_Log_CreateDate]    Script Date: 01/06/2021 20:40:41 ******/
ALTER TABLE [dbo].[Tbl_Log] ADD  CONSTRAINT [DF_Tbl_Log_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tbl_Log_LastModiDate]    Script Date: 01/06/2021 20:40:41 ******/
ALTER TABLE [dbo].[Tbl_Log] ADD  CONSTRAINT [DF_Tbl_Log_LastModiDate]  DEFAULT (getdate()) FOR [LastModiDate]
GO
/****** Object:  Default [DF_Tbl_Log_Author]    Script Date: 01/06/2021 20:40:41 ******/
ALTER TABLE [dbo].[Tbl_Log] ADD  CONSTRAINT [DF_Tbl_Log_Author]  DEFAULT ('') FOR [Author]
GO
/****** Object:  Default [DF_Tbl_Log_Manager]    Script Date: 01/06/2021 20:40:41 ******/
ALTER TABLE [dbo].[Tbl_Log] ADD  CONSTRAINT [DF_Tbl_Log_Manager]  DEFAULT ('') FOR [Manager]
GO
/****** Object:  Default [DF_Tbl_Log_DelFlg]    Script Date: 01/06/2021 20:40:41 ******/
ALTER TABLE [dbo].[Tbl_Log] ADD  CONSTRAINT [DF_Tbl_Log_DelFlg]  DEFAULT ((1)) FOR [DelFlg]
GO
/****** Object:  Default [DF_Tbl_Log_UnitCode]    Script Date: 01/06/2021 20:40:41 ******/
ALTER TABLE [dbo].[Tbl_Log] ADD  CONSTRAINT [DF_Tbl_Log_UnitCode]  DEFAULT ('005') FOR [UnitCode]
GO
