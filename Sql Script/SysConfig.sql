USE [master]
GO
/****** Object:  Database [SysConfig]    Script Date: 01/06/2021 20:42:29 ******/
CREATE DATABASE [SysConfig] ON  PRIMARY 
( NAME = N'SysConfig', FILENAME = N'C:\DATA\SysConfig.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SysConfig_log', FILENAME = N'C:\DATA\SysConfig_1.ldf' , SIZE = 1536KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SysConfig] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SysConfig].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SysConfig] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [SysConfig] SET ANSI_NULLS OFF
GO
ALTER DATABASE [SysConfig] SET ANSI_PADDING OFF
GO
ALTER DATABASE [SysConfig] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [SysConfig] SET ARITHABORT OFF
GO
ALTER DATABASE [SysConfig] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [SysConfig] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [SysConfig] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [SysConfig] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [SysConfig] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [SysConfig] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [SysConfig] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [SysConfig] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [SysConfig] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [SysConfig] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [SysConfig] SET  DISABLE_BROKER
GO
ALTER DATABASE [SysConfig] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [SysConfig] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [SysConfig] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [SysConfig] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [SysConfig] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [SysConfig] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [SysConfig] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [SysConfig] SET  READ_WRITE
GO
ALTER DATABASE [SysConfig] SET RECOVERY FULL
GO
ALTER DATABASE [SysConfig] SET  MULTI_USER
GO
ALTER DATABASE [SysConfig] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [SysConfig] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'SysConfig', N'ON'
GO
USE [SysConfig]
GO
/****** Object:  User [ysmx]    Script Date: 01/06/2021 20:42:29 ******/
CREATE USER [ysmx] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Tbl_Uint]    Script Date: 01/06/2021 20:42:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Uint](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[UpperCode] [nvarchar](50) NULL,
	[CodeCn] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[Tel] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[LinkMan] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[IsEnabled] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[ReMarks] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[LastModiDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[Manager] [nvarchar](50) NULL,
	[DelFlg] [int] NULL,
	[UnitCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Uint] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键 ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机构编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'UpperCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机构名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'CodeCn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'Tel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机构类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'LinkMan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'ReMarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'LastModiDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记 1.正常 0.删除(所有查询强制过滤)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'DelFlg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据分组标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint', @level2type=N'COLUMN',@level2name=N'UnitCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机构配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Uint'
GO
/****** Object:  Table [dbo].[Tbl_SysModel]    Script Date: 01/06/2021 20:42:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_SysModel](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[CodeCn] [nvarchar](50) NULL,
	[UpperCode] [nvarchar](50) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[UnCode] [nvarchar](50) NULL,
	[SysCode] [nvarchar](50) NULL,
	[SysName] [nvarchar](50) NULL,
	[ServiceCode] [nvarchar](50) NULL,
	[ServiceName] [nvarchar](50) NULL,
	[ServiceType] [nvarchar](50) NULL,
	[ReqServercode] [nvarchar](50) NULL,
	[ReqType] [nvarchar](50) NULL,
	[IsCreateWin] [nvarchar](50) NULL,
	[WinMothed] [nvarchar](50) NULL,
	[ParasResult] [nvarchar](200) NULL,
	[BtnOwerId] [nvarchar](50) NULL,
	[IfGetId] [nvarchar](50) NULL,
	[ReMarks] [nvarchar](50) NULL,
	[IsEnabled] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[SortNum] [int] NULL,
	[CreateDate] [datetime] NULL,
	[LastModiDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[Manager] [nvarchar](50) NULL,
	[DelFlg] [int] NULL,
	[UnitCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_SysModel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键 ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'CodeCn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'UpperCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置表CODE 同一入口服务LIST或模块对应配置表Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Layui图标UNCODE码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'UnCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用系统编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'SysCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用系统名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'SysName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入口服务编码 请求服务编码(读数据,初始化获取数据)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'ServiceCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入口服务服务名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'ServiceName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入口服务模式 List  Rpt Get Add Edit Other Guest Login' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'ServiceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交互请求服务编码(向服务提交数据进行保存)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'ReqServercode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交互请求服务模式 List  Rpt Get Add Edit Other Guest Login' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'ReqType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否创建新窗口 默认是' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'IsCreateWin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建新窗口的方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'WinMothed'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'方法参数 固定有servercode _ParaMethod' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'ParasResult'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮的唯一ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'BtnOwerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否取主键ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'IfGetId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'ReMarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'SortNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'LastModiDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记 1.正常 0.删除(所有查询强制过滤)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'DelFlg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据分组标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel', @level2type=N'COLUMN',@level2name=N'UnitCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用系统表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_SysModel'
GO
/****** Object:  Table [dbo].[Tbl_RoleRights]    Script Date: 01/06/2021 20:42:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_RoleRights](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[CodeCn] [nvarchar](50) NULL,
	[RouleCode] [nvarchar](50) NULL,
	[ModelCode] [nvarchar](50) NULL,
	[SysCode] [nvarchar](50) NULL,
	[InterfaceCode] [nvarchar](50) NULL,
	[IsEnabled] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[ReMarks] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[LastModiDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[Manager] [nvarchar](50) NULL,
	[DelFlg] [int] NULL,
	[UnitCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_RouleRights] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键 ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'CodeCn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'RouleCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'ModelCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'SysCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入口模块编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'InterfaceCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'ReMarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'LastModiDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记 1.正常 0.删除(所有查询强制过滤)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'DelFlg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据分组标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights', @level2type=N'COLUMN',@level2name=N'UnitCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色权限配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_RoleRights'
GO
/****** Object:  Table [dbo].[Tbl_Role]    Script Date: 01/06/2021 20:42:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Role](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[CodeCn] [nvarchar](50) NULL,
	[UpperCode] [nvarchar](50) NULL,
	[SysCode] [nvarchar](50) NULL,
	[SysName] [nvarchar](50) NULL,
	[IsEnabled] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[ReMarks] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[LastModiDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[Manager] [nvarchar](50) NULL,
	[DelFlg] [int] NULL,
	[UnitCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Tbl_Role] ON [dbo].[Tbl_Role] 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键 ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'CodeCn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'UpperCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'SysCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'SysName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'ReMarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'LastModiDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记 1.正常 0.删除(所有查询强制过滤)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'DelFlg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据分组标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role', @level2type=N'COLUMN',@level2name=N'UnitCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色配置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Role'
GO
/****** Object:  Table [dbo].[Tbl_Config]    Script Date: 01/06/2021 20:42:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Config](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[ServiceName] [nvarchar](50) NULL,
	[SysCode] [nvarchar](50) NULL,
	[ServiceDefinition] [nvarchar](50) NULL,
	[Url] [nvarchar](50) NULL,
	[UrlType] [nvarchar](50) NULL,
	[Port] [nvarchar](50) NULL,
	[Api] [nvarchar](150) NULL,
	[RequestMothed] [nvarchar](50) NULL,
	[RequestType] [nvarchar](50) NULL,
	[ReMarks] [nvarchar](50) NULL,
	[IsEnabled] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[CodeType] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[LastModiDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[Manager] [nvarchar](50) NULL,
	[DelFlg] [int] NULL,
	[UnitCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Config] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键 ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'ServiceName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用系统编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'SysCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务定义[API服务目录 物理目录 可选]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'ServiceDefinition'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Url domain.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Url类型 http Or HTTPS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'UrlType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'定义服务名称 网关根据服务名寻找HTTP 可重复' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'Port'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'API /API/DIR/METHOD' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'Api'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求方法 List  Rpt Get Add Edit Other Guest' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'RequestMothed'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求类型(POST OR GET)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'RequestType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'ReMarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否缓存' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编码类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'CodeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'LastModiDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记 1.正常 0.删除(所有查询强制过滤)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'DelFlg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据分组标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config', @level2type=N'COLUMN',@level2name=N'UnitCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务配置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tbl_Config'
GO
/****** Object:  Default [DF_Tbl_Uint_IsEnabled]    Script Date: 01/06/2021 20:42:29 ******/
ALTER TABLE [dbo].[Tbl_Uint] ADD  CONSTRAINT [DF_Tbl_Uint_IsEnabled]  DEFAULT ('000001') FOR [IsEnabled]
GO
/****** Object:  Default [DF_Tbl_Uint_Status]    Script Date: 01/06/2021 20:42:29 ******/
ALTER TABLE [dbo].[Tbl_Uint] ADD  CONSTRAINT [DF_Tbl_Uint_Status]  DEFAULT ('001001') FOR [Status]
GO
/****** Object:  Default [DF_Tbl_Uint_CreateDate]    Script Date: 01/06/2021 20:42:29 ******/
ALTER TABLE [dbo].[Tbl_Uint] ADD  CONSTRAINT [DF_Tbl_Uint_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tbl_Uint_LastModiDate]    Script Date: 01/06/2021 20:42:29 ******/
ALTER TABLE [dbo].[Tbl_Uint] ADD  CONSTRAINT [DF_Tbl_Uint_LastModiDate]  DEFAULT (getdate()) FOR [LastModiDate]
GO
/****** Object:  Default [DF_Tbl_Uint_Author]    Script Date: 01/06/2021 20:42:29 ******/
ALTER TABLE [dbo].[Tbl_Uint] ADD  CONSTRAINT [DF_Tbl_Uint_Author]  DEFAULT ('') FOR [Author]
GO
/****** Object:  Default [DF_Tbl_Uint_Manager]    Script Date: 01/06/2021 20:42:29 ******/
ALTER TABLE [dbo].[Tbl_Uint] ADD  CONSTRAINT [DF_Tbl_Uint_Manager]  DEFAULT ('') FOR [Manager]
GO
/****** Object:  Default [DF_Tbl_Uint_DelFlg]    Script Date: 01/06/2021 20:42:29 ******/
ALTER TABLE [dbo].[Tbl_Uint] ADD  CONSTRAINT [DF_Tbl_Uint_DelFlg]  DEFAULT ((1)) FOR [DelFlg]
GO
/****** Object:  Default [DF_Tbl_Uint_UnitCode]    Script Date: 01/06/2021 20:42:29 ******/
ALTER TABLE [dbo].[Tbl_Uint] ADD  CONSTRAINT [DF_Tbl_Uint_UnitCode]  DEFAULT ('005') FOR [UnitCode]
GO
/****** Object:  Default [DF_Tbl_SysModel_UnCode]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_UnCode]  DEFAULT ('&#xe705;') FOR [UnCode]
GO
/****** Object:  Default [DF_Tbl_SysModel_IsCreateWin]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_IsCreateWin]  DEFAULT ('000001') FOR [IsCreateWin]
GO
/****** Object:  Default [DF_Tbl_SysModel_IfGetId]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_IfGetId]  DEFAULT ('000001') FOR [IfGetId]
GO
/****** Object:  Default [DF_Tbl_SysModel_IsEnabled]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_IsEnabled]  DEFAULT ('000001') FOR [IsEnabled]
GO
/****** Object:  Default [DF_Tbl_SysModel_Status]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_Status]  DEFAULT ('001001') FOR [Status]
GO
/****** Object:  Default [DF_Tbl_SysModel_SortNum]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_SortNum]  DEFAULT ((100)) FOR [SortNum]
GO
/****** Object:  Default [DF_Tbl_SysModel_CreateDate]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tbl_SysModel_LastModiDate]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_LastModiDate]  DEFAULT (getdate()) FOR [LastModiDate]
GO
/****** Object:  Default [DF_Tbl_SysModel_Author]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_Author]  DEFAULT ('') FOR [Author]
GO
/****** Object:  Default [DF_Tbl_SysModel_Manager]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_Manager]  DEFAULT ('') FOR [Manager]
GO
/****** Object:  Default [DF_Tbl_SysModel_DelFlg]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_DelFlg]  DEFAULT ((1)) FOR [DelFlg]
GO
/****** Object:  Default [DF_Tbl_SysModel_UnitCode]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_SysModel] ADD  CONSTRAINT [DF_Tbl_SysModel_UnitCode]  DEFAULT ('005') FOR [UnitCode]
GO
/****** Object:  Default [DF_Tbl_RouleRights_IsEnabled]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_RoleRights] ADD  CONSTRAINT [DF_Tbl_RouleRights_IsEnabled]  DEFAULT ('000001') FOR [IsEnabled]
GO
/****** Object:  Default [DF_Tbl_RouleRights_Status]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_RoleRights] ADD  CONSTRAINT [DF_Tbl_RouleRights_Status]  DEFAULT ('001001') FOR [Status]
GO
/****** Object:  Default [DF_Tbl_RouleRights_CreateDate]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_RoleRights] ADD  CONSTRAINT [DF_Tbl_RouleRights_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tbl_RouleRights_LastModiDate]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_RoleRights] ADD  CONSTRAINT [DF_Tbl_RouleRights_LastModiDate]  DEFAULT (getdate()) FOR [LastModiDate]
GO
/****** Object:  Default [DF_Tbl_RouleRights_Author]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_RoleRights] ADD  CONSTRAINT [DF_Tbl_RouleRights_Author]  DEFAULT ('') FOR [Author]
GO
/****** Object:  Default [DF_Tbl_RouleRights_Manager]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_RoleRights] ADD  CONSTRAINT [DF_Tbl_RouleRights_Manager]  DEFAULT ('') FOR [Manager]
GO
/****** Object:  Default [DF_Tbl_RouleRights_DelFlg]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_RoleRights] ADD  CONSTRAINT [DF_Tbl_RouleRights_DelFlg]  DEFAULT ((1)) FOR [DelFlg]
GO
/****** Object:  Default [DF_Tbl_RouleRights_UnitCode]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_RoleRights] ADD  CONSTRAINT [DF_Tbl_RouleRights_UnitCode]  DEFAULT ('005') FOR [UnitCode]
GO
/****** Object:  Default [DF_Tbl_Role_IsEnabled]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Role] ADD  CONSTRAINT [DF_Tbl_Role_IsEnabled]  DEFAULT ('000001') FOR [IsEnabled]
GO
/****** Object:  Default [DF_Tbl_Role_Status]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Role] ADD  CONSTRAINT [DF_Tbl_Role_Status]  DEFAULT ('001001') FOR [Status]
GO
/****** Object:  Default [DF_Tbl_Role_CreateDate]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Role] ADD  CONSTRAINT [DF_Tbl_Role_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tbl_Role_LastModiDate]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Role] ADD  CONSTRAINT [DF_Tbl_Role_LastModiDate]  DEFAULT (getdate()) FOR [LastModiDate]
GO
/****** Object:  Default [DF_Tbl_Role_Author]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Role] ADD  CONSTRAINT [DF_Tbl_Role_Author]  DEFAULT ('') FOR [Author]
GO
/****** Object:  Default [DF_Tbl_Role_Manager]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Role] ADD  CONSTRAINT [DF_Tbl_Role_Manager]  DEFAULT ('') FOR [Manager]
GO
/****** Object:  Default [DF_Tbl_Role_DelFlg]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Role] ADD  CONSTRAINT [DF_Tbl_Role_DelFlg]  DEFAULT ((1)) FOR [DelFlg]
GO
/****** Object:  Default [DF_Tbl_Role_UnitCode]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Role] ADD  CONSTRAINT [DF_Tbl_Role_UnitCode]  DEFAULT ('005') FOR [UnitCode]
GO
/****** Object:  Default [DF_Tbl_Config_UrlType]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Config] ADD  CONSTRAINT [DF_Tbl_Config_UrlType]  DEFAULT ('http://') FOR [UrlType]
GO
/****** Object:  Default [DF_Tbl_Config_IsEnabled]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Config] ADD  CONSTRAINT [DF_Tbl_Config_IsEnabled]  DEFAULT ('000000') FOR [IsEnabled]
GO
/****** Object:  Default [DF_Tbl_Config_Status]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Config] ADD  CONSTRAINT [DF_Tbl_Config_Status]  DEFAULT ('001000') FOR [Status]
GO
/****** Object:  Default [DF_Tbl_Config_CreateDate]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Config] ADD  CONSTRAINT [DF_Tbl_Config_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tbl_Config_LastModiDate]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Config] ADD  CONSTRAINT [DF_Tbl_Config_LastModiDate]  DEFAULT (getdate()) FOR [LastModiDate]
GO
/****** Object:  Default [DF_Tbl_Config_Author]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Config] ADD  CONSTRAINT [DF_Tbl_Config_Author]  DEFAULT ('') FOR [Author]
GO
/****** Object:  Default [DF_Tbl_Config_Manager]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Config] ADD  CONSTRAINT [DF_Tbl_Config_Manager]  DEFAULT ('') FOR [Manager]
GO
/****** Object:  Default [DF_Tbl_Config_DelFlg]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Config] ADD  CONSTRAINT [DF_Tbl_Config_DelFlg]  DEFAULT ((1)) FOR [DelFlg]
GO
/****** Object:  Default [DF_Tbl_Config_UnitCode]    Script Date: 01/06/2021 20:42:30 ******/
ALTER TABLE [dbo].[Tbl_Config] ADD  CONSTRAINT [DF_Tbl_Config_UnitCode]  DEFAULT ('005') FOR [UnitCode]
GO
