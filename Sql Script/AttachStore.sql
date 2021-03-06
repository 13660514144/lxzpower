USE [master]
GO
/****** Object:  Database [AttachStore]    Script Date: 01/06/2021 20:43:59 ******/
CREATE DATABASE [AttachStore] ON  PRIMARY 
( NAME = N'AttachStore', FILENAME = N'C:\DATA\AttachStore.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AttachStore_log', FILENAME = N'C:\DATA\AttachStore_1.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [AttachStore] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AttachStore].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AttachStore] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [AttachStore] SET ANSI_NULLS OFF
GO
ALTER DATABASE [AttachStore] SET ANSI_PADDING OFF
GO
ALTER DATABASE [AttachStore] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [AttachStore] SET ARITHABORT OFF
GO
ALTER DATABASE [AttachStore] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [AttachStore] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [AttachStore] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [AttachStore] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [AttachStore] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [AttachStore] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [AttachStore] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [AttachStore] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [AttachStore] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [AttachStore] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [AttachStore] SET  DISABLE_BROKER
GO
ALTER DATABASE [AttachStore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [AttachStore] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [AttachStore] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [AttachStore] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [AttachStore] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [AttachStore] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [AttachStore] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [AttachStore] SET  READ_WRITE
GO
ALTER DATABASE [AttachStore] SET RECOVERY FULL
GO
ALTER DATABASE [AttachStore] SET  MULTI_USER
GO
ALTER DATABASE [AttachStore] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [AttachStore] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'AttachStore', N'ON'
GO
USE [AttachStore]
GO
/****** Object:  User [ysmx]    Script Date: 01/06/2021 20:43:59 ******/
CREATE USER [ysmx] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Tbl_AttachList]    Script Date: 01/06/2021 20:44:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_AttachList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentCode] [nvarchar](100) NULL,
	[SourceDb] [nvarchar](100) NULL,
	[SourceTb] [nvarchar](100) NULL,
	[WebFilePath] [nvarchar](100) NULL,
	[AgainName] [nvarchar](100) NULL,
	[SourceName] [nvarchar](100) NULL,
	[SysCode] [nvarchar](50) NULL,
	[FileType] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[LastModiDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[Manager] [nvarchar](50) NULL,
	[DelFlg] [int] NULL,
	[UnitCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_AttachList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Tbl_AttachList] ON [dbo].[Tbl_AttachList] 
(
	[ParentCode] ASC,
	[AgainName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键 ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联表Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'源业务库' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'SourceDb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'源业务表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'SourceTb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'存储文件路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'WebFilePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'存储文件名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'AgainName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'源文件名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'SourceName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'SysCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型  附件还是LOGO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'FileType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'LastModiDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记 1.正常 0.删除(所有查询强制过滤)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'DelFlg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据分组标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList', @level2type=N'COLUMN',@level2name=N'UnitCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_AttachList'
GO
/****** Object:  Default [DF_Tbl_AttachList_CreateDate]    Script Date: 01/06/2021 20:44:00 ******/
ALTER TABLE [dbo].[Tbl_AttachList] ADD  CONSTRAINT [DF_Tbl_AttachList_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tbl_AttachList_LastModiDate]    Script Date: 01/06/2021 20:44:00 ******/
ALTER TABLE [dbo].[Tbl_AttachList] ADD  CONSTRAINT [DF_Tbl_AttachList_LastModiDate]  DEFAULT (getdate()) FOR [LastModiDate]
GO
/****** Object:  Default [DF_Tbl_AttachList_Author]    Script Date: 01/06/2021 20:44:00 ******/
ALTER TABLE [dbo].[Tbl_AttachList] ADD  CONSTRAINT [DF_Tbl_AttachList_Author]  DEFAULT ('') FOR [Author]
GO
/****** Object:  Default [DF_Tbl_AttachList_Manager]    Script Date: 01/06/2021 20:44:00 ******/
ALTER TABLE [dbo].[Tbl_AttachList] ADD  CONSTRAINT [DF_Tbl_AttachList_Manager]  DEFAULT ('') FOR [Manager]
GO
/****** Object:  Default [DF_Tbl_AttachList_DelFlg]    Script Date: 01/06/2021 20:44:00 ******/
ALTER TABLE [dbo].[Tbl_AttachList] ADD  CONSTRAINT [DF_Tbl_AttachList_DelFlg]  DEFAULT ((1)) FOR [DelFlg]
GO
/****** Object:  Default [DF_Tbl_AttachList_UnitCode]    Script Date: 01/06/2021 20:44:00 ******/
ALTER TABLE [dbo].[Tbl_AttachList] ADD  CONSTRAINT [DF_Tbl_AttachList_UnitCode]  DEFAULT ('005') FOR [UnitCode]
GO
