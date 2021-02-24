USE [master]
GO

CREATE DATABASE [integration_tests]
GO

USE [integration_tests]
GO

CREATE TABLE [dbo].[TODO](
	[id] [uniqueidentifier] NULL,
	[done] [bit] NULL,
	[name] [nvarchar](250) NULL
) ON [PRIMARY]
GO