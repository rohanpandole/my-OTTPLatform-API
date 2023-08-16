USE [OTTPlatform]
GO

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'UserLoginDetail'))
BEGIN

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[UserLoginDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](255) NULL,
	[Password] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[PhoneNumber] [int] NULL,
	[RoleId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[UserLoginDetail]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])

END

