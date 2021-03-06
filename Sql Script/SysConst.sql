USE [master]
GO
/****** Object:  Database [SysConst]    Script Date: 01/06/2021 20:41:43 ******/
CREATE DATABASE [SysConst] ON  PRIMARY 
( NAME = N'SysConst', FILENAME = N'C:\DATA\SysConst.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SysConst_log', FILENAME = N'C:\dATA\SysConst_1.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SysConst] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SysConst].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SysConst] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [SysConst] SET ANSI_NULLS OFF
GO
ALTER DATABASE [SysConst] SET ANSI_PADDING OFF
GO
ALTER DATABASE [SysConst] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [SysConst] SET ARITHABORT OFF
GO
ALTER DATABASE [SysConst] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [SysConst] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [SysConst] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [SysConst] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [SysConst] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [SysConst] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [SysConst] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [SysConst] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [SysConst] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [SysConst] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [SysConst] SET  DISABLE_BROKER
GO
ALTER DATABASE [SysConst] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [SysConst] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [SysConst] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [SysConst] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [SysConst] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [SysConst] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [SysConst] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [SysConst] SET  READ_WRITE
GO
ALTER DATABASE [SysConst] SET RECOVERY FULL
GO
ALTER DATABASE [SysConst] SET  MULTI_USER
GO
ALTER DATABASE [SysConst] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [SysConst] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'SysConst', N'ON'
GO
USE [SysConst]
GO
/****** Object:  User [ysmx]    Script Date: 01/06/2021 20:41:44 ******/
CREATE USER [ysmx] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Tbl_Const]    Script Date: 01/06/2021 20:41:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Const](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[CodeCn] [nvarchar](50) NULL,
	[ConstValue] [nvarchar](50) NULL,
	[UpperCode] [nvarchar](50) NULL,
	[ReMarks] [nvarchar](50) NULL,
	[IsEnabled] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[LastModiDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[Manager] [nvarchar](50) NULL,
	[DelFlg] [int] NULL,
	[UnitCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Const] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键 ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'常量编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'常量名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'CodeCn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'常量值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'ConstValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'UpperCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'ReMarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'LastModiDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记 1.正常 0.删除(所有查询强制过滤)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'DelFlg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据分组标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const', @level2type=N'COLUMN',@level2name=N'UnitCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'常量表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Const'
GO
/****** Object:  UserDefinedFunction [dbo].[code2name]    Script Date: 01/06/2021 20:41:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
Create function [dbo].[code2name](@temp nvarchar(50))
--f_code2name 为方法名
--将分类代码转换为分类名称
returns varchar(50)
as
begin
declare @name varchar(50)
select @name=A.CodeCn from Tbl_Const(nolock) A where A.Code=@temp
if @name is null
	begin
		set @name=@temp
	end
return @name
end
GO
/****** Object:  Default [DF_Tbl_Const_IsEnabled]    Script Date: 01/06/2021 20:41:44 ******/
ALTER TABLE [dbo].[Tbl_Const] ADD  CONSTRAINT [DF_Tbl_Const_IsEnabled]  DEFAULT ('000001') FOR [IsEnabled]
GO
/****** Object:  Default [DF_Tbl_Const_Status]    Script Date: 01/06/2021 20:41:44 ******/
ALTER TABLE [dbo].[Tbl_Const] ADD  CONSTRAINT [DF_Tbl_Const_Status]  DEFAULT ('001001') FOR [Status]
GO
/****** Object:  Default [DF_Tbl_Const_CreateDate]    Script Date: 01/06/2021 20:41:44 ******/
ALTER TABLE [dbo].[Tbl_Const] ADD  CONSTRAINT [DF_Tbl_Const_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tbl_Const_LastModiDate]    Script Date: 01/06/2021 20:41:44 ******/
ALTER TABLE [dbo].[Tbl_Const] ADD  CONSTRAINT [DF_Tbl_Const_LastModiDate]  DEFAULT (getdate()) FOR [LastModiDate]
GO
/****** Object:  Default [DF_Tbl_Const_Author]    Script Date: 01/06/2021 20:41:44 ******/
ALTER TABLE [dbo].[Tbl_Const] ADD  CONSTRAINT [DF_Tbl_Const_Author]  DEFAULT ('') FOR [Author]
GO
/****** Object:  Default [DF_Tbl_Const_Manager]    Script Date: 01/06/2021 20:41:44 ******/
ALTER TABLE [dbo].[Tbl_Const] ADD  CONSTRAINT [DF_Tbl_Const_Manager]  DEFAULT ('') FOR [Manager]
GO
/****** Object:  Default [DF_Tbl_Const_DelFlg]    Script Date: 01/06/2021 20:41:44 ******/
ALTER TABLE [dbo].[Tbl_Const] ADD  CONSTRAINT [DF_Tbl_Const_DelFlg]  DEFAULT ((1)) FOR [DelFlg]
GO
/****** Object:  Default [DF_Tbl_Const_UnitCode]    Script Date: 01/06/2021 20:41:44 ******/
ALTER TABLE [dbo].[Tbl_Const] ADD  CONSTRAINT [DF_Tbl_Const_UnitCode]  DEFAULT ('005') FOR [UnitCode]
GO
