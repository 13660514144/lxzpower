USE [master]
GO
/****** Object:  Database [SysUser]    Script Date: 01/06/2021 20:39:42 ******/
CREATE DATABASE [SysUser] ON  PRIMARY 
( NAME = N'SysUser', FILENAME = N'C:\DATA\SysUser.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SysUser_log', FILENAME = N'C:\DATA\SysUser_1.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SysUser] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SysUser].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SysUser] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [SysUser] SET ANSI_NULLS OFF
GO
ALTER DATABASE [SysUser] SET ANSI_PADDING OFF
GO
ALTER DATABASE [SysUser] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [SysUser] SET ARITHABORT OFF
GO
ALTER DATABASE [SysUser] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [SysUser] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [SysUser] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [SysUser] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [SysUser] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [SysUser] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [SysUser] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [SysUser] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [SysUser] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [SysUser] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [SysUser] SET  DISABLE_BROKER
GO
ALTER DATABASE [SysUser] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [SysUser] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [SysUser] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [SysUser] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [SysUser] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [SysUser] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [SysUser] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [SysUser] SET  READ_WRITE
GO
ALTER DATABASE [SysUser] SET RECOVERY FULL
GO
ALTER DATABASE [SysUser] SET  MULTI_USER
GO
ALTER DATABASE [SysUser] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [SysUser] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'SysUser', N'ON'
GO
USE [SysUser]
GO
/****** Object:  User [ysmx]    Script Date: 01/06/2021 20:39:42 ******/
CREATE USER [ysmx] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Tbl_User]    Script Date: 01/06/2021 20:39:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_User](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Account] [nvarchar](50) NULL,
	[Pwd] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[NickName] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[RoleCode] [nvarchar](50) NULL,
	[Tel] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Qq] [nvarchar](50) NULL,
	[WeChat] [nvarchar](50) NULL,
	[SysCode] [nvarchar](50) NULL,
	[IsEnabled] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[Remarks] [nvarchar](100) NULL,
	[CreateDate] [datetime] NULL,
	[LastModiDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[Manager] [nvarchar](50) NULL,
	[DelFlg] [int] NULL,
	[UnitCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Tbl_User] ON [dbo].[Tbl_User] 
(
	[Code] ASC,
	[Account] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帐号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Account'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Pwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呢称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'NickName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户类型 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'RoleCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Tel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QQ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Qq'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'WeChat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用系统编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'SysCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Remarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'LastModiDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记 1.正常 0.删除(所有查询强制过滤)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'DelFlg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据分组标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User', @level2type=N'COLUMN',@level2name=N'UnitCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_User'
GO
/****** Object:  Default [DF_Tbl_User_Pwd]    Script Date: 01/06/2021 20:39:42 ******/
ALTER TABLE [dbo].[Tbl_User] ADD  CONSTRAINT [DF_Tbl_User_Pwd]  DEFAULT ('202cb962ac59075b964b07152d234b70') FOR [Pwd]
GO
/****** Object:  Default [DF_Tbl_User_IsEnabled]    Script Date: 01/06/2021 20:39:42 ******/
ALTER TABLE [dbo].[Tbl_User] ADD  CONSTRAINT [DF_Tbl_User_IsEnabled]  DEFAULT ('000001') FOR [IsEnabled]
GO
/****** Object:  Default [DF_Tbl_User_Status]    Script Date: 01/06/2021 20:39:42 ******/
ALTER TABLE [dbo].[Tbl_User] ADD  CONSTRAINT [DF_Tbl_User_Status]  DEFAULT ('001001') FOR [Status]
GO
/****** Object:  Default [DF_Tbl_User_CreateDate]    Script Date: 01/06/2021 20:39:42 ******/
ALTER TABLE [dbo].[Tbl_User] ADD  CONSTRAINT [DF_Tbl_User_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tbl_User_LastModiDate]    Script Date: 01/06/2021 20:39:42 ******/
ALTER TABLE [dbo].[Tbl_User] ADD  CONSTRAINT [DF_Tbl_User_LastModiDate]  DEFAULT (getdate()) FOR [LastModiDate]
GO
/****** Object:  Default [DF_Tbl_User_Author]    Script Date: 01/06/2021 20:39:42 ******/
ALTER TABLE [dbo].[Tbl_User] ADD  CONSTRAINT [DF_Tbl_User_Author]  DEFAULT ('') FOR [Author]
GO
/****** Object:  Default [DF_Tbl_User_Manager]    Script Date: 01/06/2021 20:39:42 ******/
ALTER TABLE [dbo].[Tbl_User] ADD  CONSTRAINT [DF_Tbl_User_Manager]  DEFAULT ('') FOR [Manager]
GO
/****** Object:  Default [DF_Tbl_User_DelFlg]    Script Date: 01/06/2021 20:39:42 ******/
ALTER TABLE [dbo].[Tbl_User] ADD  CONSTRAINT [DF_Tbl_User_DelFlg]  DEFAULT ((1)) FOR [DelFlg]
GO
/****** Object:  Default [DF_Tbl_User_UnitCode]    Script Date: 01/06/2021 20:39:42 ******/
ALTER TABLE [dbo].[Tbl_User] ADD  CONSTRAINT [DF_Tbl_User_UnitCode]  DEFAULT ('005') FOR [UnitCode]
GO
