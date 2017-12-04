# Instructions to set up database

CREATE TABLE [dbo].[RequestStatus](
	RequestStatusId int identity(1, 1) NOT NULL,
	RequestStatusCode varchar(50) NOT NULL,
	RequestStatusName varchar(200) NOT NULL,
 CONSTRAINT [PK_RequestStatus] PRIMARY KEY CLUSTERED 
(
	[RequestStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[RequestStatus]  WITH CHECK ADD  CONSTRAINT [CK_RequestStatus_RequestStatusCode_Unique] UNIQUE ([RequestStatusCode])
GO

CREATE TABLE [dbo].[EmployeeType](
	EmployeeTypeId int identity(1, 1) NOT NULL,
	EmployeeTypeCode varchar(50) NOT NULL,
	EmployeeTypeName varchar(200) NOT NULL,
	IsManager bit NOT NULL default(0),
 CONSTRAINT [PK_EmployeeType] PRIMARY KEY CLUSTERED 
(
	[EmployeeTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[EmployeeType]  WITH CHECK ADD  CONSTRAINT [CK_EmployeeType_EmployeeTypeCode_Unique] UNIQUE ([EmployeeTypeCode])
GO

CREATE TABLE [dbo].[Team](
	TeamId int identity(1, 1) NOT NULL,
	TeamName varchar(200) NOT NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Team] WITH CHECK ADD  CONSTRAINT [CK_Team_TeamName_Unique] UNIQUE ([TeamName])
GO

CREATE TABLE [dbo].[Employee](
	EmployeeId int identity(1, 1) NOT NULL,
	FullName varchar(200) NOT NULL,
	EmployeeNumber varchar(20),
	EmailAddress varchar(250),
	CellphoneNumber varchar(20),
	TeamId int NOT NULL,
	EmployeeTypeId int NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Team_TeamId] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_EmployeeType_EmployeeTypeId] FOREIGN KEY([EmployeeTypeId])
REFERENCES [dbo].[EmployeeType] ([EmployeeTypeId])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [CK_Employee_EmailAddress_Unique] UNIQUE ([EmailAddress])
GO

CREATE TABLE [dbo].[LeaveRequest](
    LeaveRequestId int identity(1, 1) NOT NULL,
	EmployeeId int NOT NULL,
	LeaveFrom DateTime NOT NULL,
	LeaveTo DateTime NOT NULL,
	LeaveDescription varchar(4000),
	ActionManagerId int NULL,
	RequestStatusId int NOT NULL,
 CONSTRAINT [PK_LeaveRequest] PRIMARY KEY CLUSTERED 
(
	[LeaveRequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[LeaveRequest]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequest_Employee_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[LeaveRequest]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequest_RequestStatus_RequestStatusId] FOREIGN KEY([RequestStatusId])
REFERENCES [dbo].[RequestStatus] ([RequestStatusId])
GO
ALTER TABLE [dbo].[LeaveRequest]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequest_Employee_ActionManagerId] FOREIGN KEY([ActionManagerId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO

# Run below scripts for data database
SET IDENTITY_INSERT [dbo].[Team] ON
INSERT INTO [dbo].[Team] ([TeamId], [TeamName]) VALUES (1, N'Dev Team')
INSERT INTO [dbo].[Team] ([TeamId], [TeamName]) VALUES (2, N'Management Team')
INSERT INTO [dbo].[Team] ([TeamId], [TeamName]) VALUES (3, N'Support Team')
SET IDENTITY_INSERT [dbo].[Team] OFF

SET IDENTITY_INSERT [dbo].[RequestStatus] ON
INSERT INTO [dbo].[RequestStatus] ([RequestStatusId], [RequestStatusCode], [RequestStatusName]) VALUES (1, N'PENDING', N'PENDING')
INSERT INTO [dbo].[RequestStatus] ([RequestStatusId], [RequestStatusCode], [RequestStatusName]) VALUES (3, N'DECLINED', N'DECLINED')
INSERT INTO [dbo].[RequestStatus] ([RequestStatusId], [RequestStatusCode], [RequestStatusName]) VALUES (4, N'APPROVED', N'APPROVED')
SET IDENTITY_INSERT [dbo].[RequestStatus] OFF

SET IDENTITY_INSERT [dbo].[EmployeeType] ON
INSERT INTO [dbo].[EmployeeType] ([EmployeeTypeId], [EmployeeTypeCode], [EmployeeTypeName], [IsManager]) VALUES (1, N'EMPLOYEE', N'EMPLOYEE', 0)
INSERT INTO [dbo].[EmployeeType] ([EmployeeTypeId], [EmployeeTypeCode], [EmployeeTypeName], [IsManager]) VALUES (2, N'TEAMLEAD', N'TEAM LEAD', 1)
INSERT INTO [dbo].[EmployeeType] ([EmployeeTypeId], [EmployeeTypeCode], [EmployeeTypeName], [IsManager]) VALUES (3, N'CEO', N'CEO', 1)
SET IDENTITY_INSERT [dbo].[EmployeeType] OFF

SET IDENTITY_INSERT [dbo].[Employee] ON
INSERT INTO [dbo].[Employee] ([EmployeeId], [FullName], [EmployeeNumber], [EmailAddress], [CellphoneNumber], [TeamId], [EmployeeTypeId]) VALUES (2, N'Milton Coleman', N'0002', N'MiltonColeman@acme.com', NULL, 3, 2)
INSERT INTO [dbo].[Employee] ([EmployeeId], [FullName], [EmployeeNumber], [EmailAddress], [CellphoneNumber], [TeamId], [EmployeeTypeId]) VALUES (3, N'Charlotte Osborne', N'1005', N'charlotteosborne@acme.com', N'+36 55 760 177', 3, 1)
INSERT INTO [dbo].[Employee] ([EmployeeId], [FullName], [EmployeeNumber], [EmailAddress], [CellphoneNumber], [TeamId], [EmployeeTypeId]) VALUES (4, N'Marie Walters', N'1006', N'mariewalters@acme.com', N'+353 20 918 6908', 3, 1)
INSERT INTO [dbo].[Employee] ([EmployeeId], [FullName], [EmployeeNumber], [EmailAddress], [CellphoneNumber], [TeamId], [EmployeeTypeId]) VALUES (5, N'Leonard Gill', N'1008', N'leonardgill@acme.com', N'+36 55 525 585', 3, 1)
INSERT INTO [dbo].[Employee] ([EmployeeId], [FullName], [EmployeeNumber], [EmailAddress], [CellphoneNumber], [TeamId], [EmployeeTypeId]) VALUES (8, N'Enrique Thomas', N'1009', N'enriquethomas@acme.com', N'+353 20 916 1335', 3, 1)
SET IDENTITY_INSERT [dbo].[Employee] OFF
