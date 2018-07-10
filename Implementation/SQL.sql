/*Businesses Table*/

CREATE TABLE [dbo].[Businesses] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (255) NOT NULL,
    [Type]          NVARCHAR (255) NOT NULL,
    [StartDate]     DATETIME       NOT NULL,
    [EndDate]       DATETIME       NOT NULL,
    [AddressLine1]  NVARCHAR (255) NULL,
    [AddressLine2]  NVARCHAR (255) NULL,
    [Postcode]      NVARCHAR (10)  NULL,
    [Town]          NVARCHAR (40)  NULL,
    [Region]        NVARCHAR (40)  NULL,
    [Country]       NVARCHAR (40)  NULL,
    [CardType]      NVARCHAR (10)  NULL,
    [CardNumber]    NVARCHAR (16)  NULL,
    [ExpiryMonth]   NVARCHAR (2)   NULL,
    [ExpiryYear]    NVARCHAR (2)   NULL,
    [SecuriyNumber] NVARCHAR (3)   NULL,
    [Plan]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Businesses] PRIMARY KEY CLUSTERED ([Id] ASC)
);











/*DataOverTimes Table*/

CREATE TABLE [dbo].[DataOverTimes] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [DateTime]    DATETIME NOT NULL,
    [MemoryMB]    REAL     NOT NULL,
    [Business_Id] INT      NULL,
    CONSTRAINT [PK_dbo.DataOverTimes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.DataOverTimes_dbo.Businesses_Business_Id] FOREIGN KEY ([Business_Id]) REFERENCES [dbo].[Businesses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Business_Id]
    ON [dbo].[DataOverTimes]([Business_Id] ASC);













/*Updates Table*/

CREATE TABLE [dbo].[Updates] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Business_Id] INT            NULL,
    CONSTRAINT [PK_dbo.Updates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Updates_dbo.Businesses_Business_Id] FOREIGN KEY ([Business_Id]) REFERENCES [dbo].[Businesses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Business_Id]
    ON [dbo].[Updates]([Business_Id] ASC);














/*Transactions Table*/

CREATE TABLE [dbo].[Transactions] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Type]         NVARCHAR (MAX) NOT NULL,
    [Amount]       REAL           NOT NULL,
    [VAT]          REAL           NOT NULL,
    [Total]        REAL           NOT NULL,
    [DateSent]     DATETIME       NOT NULL,
    [DateRefunded] DATETIME       NOT NULL,
    [Recipient_Id] INT            NULL,
    [Sender_Id]    INT            NULL,
    CONSTRAINT [PK_dbo.Transactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Transactions_dbo.Businesses_Recipient_Id] FOREIGN KEY ([Recipient_Id]) REFERENCES [dbo].[Businesses] ([Id]),
    CONSTRAINT [FK_dbo.Transactions_dbo.Businesses_Sender_Id] FOREIGN KEY ([Sender_Id]) REFERENCES [dbo].[Businesses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Recipient_Id]
    ON [dbo].[Transactions]([Recipient_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Sender_Id]
    ON [dbo].[Transactions]([Sender_Id] ASC);














/*Errors Table*/

CREATE TABLE [dbo].[Errors] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Type]     NVARCHAR (100) NOT NULL,
    [DateTime] DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.Errors] PRIMARY KEY CLUSTERED ([Id] ASC)
);












/*BusinessUserTypes Table*/

CREATE TABLE [dbo].[BusinessUserTypes] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (50)  NOT NULL,
    [BusinessId]    INT            NOT NULL,
    [Administrator] NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Updates]       NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Store]         NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Messenger]     NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Meetings]      NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Calendar]      NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Holidays]      NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_dbo.BusinessUserTypes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.BusinessUserTypes_dbo.Businesses_BusinessId] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Businesses] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_BusinessId]
    ON [dbo].[BusinessUserTypes]([BusinessId] ASC);









/*EmployeeTypes Table*/

CREATE TABLE [dbo].[EmployeeTypes] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (70)  NOT NULL,
    [Description] NVARCHAR (255) NULL,
    [BaseSalary]  INT            NOT NULL,
    CONSTRAINT [PK_dbo.EmployeeTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

















/*AspNetUsers Table*/
CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                     NVARCHAR (128) NOT NULL,
    [Title]                  NVARCHAR (MAX) NULL,
    [FirstName]              NVARCHAR (MAX) NULL,
    [LastName]               NVARCHAR (MAX) NULL,
    [DateOfBirth]            DATETIME       NOT NULL,
    [BackupEmail]            NVARCHAR (MAX) NULL,
    [AddressLine1]           NVARCHAR (255) NULL,
    [AddressLine2]           NVARCHAR (255) NULL,
    [Postcode]               NVARCHAR (8)   NULL,
    [Blocked]                BIT            NOT NULL,
    [TimesLoggedIn]          INT            NOT NULL,
    [NIN]                    NVARCHAR (MAX) NULL,
    [EmergencyContact]       NVARCHAR (MAX) NULL,
    [Raise]                  REAL           NOT NULL,
    [Strikes]                INT            NOT NULL,
    [Email]                  NVARCHAR (256) NULL,
    [EmailConfirmed]         BIT            NOT NULL,
    [PasswordHash]           NVARCHAR (MAX) NULL,
    [SecurityStamp]          NVARCHAR (MAX) NULL,
    [PhoneNumber]            NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed]   BIT            NOT NULL,
    [TwoFactorEnabled]       BIT            NOT NULL,
    [LockoutEndDateUtc]      DATETIME       NULL,
    [LockoutEnabled]         BIT            NOT NULL,
    [AccessFailedCount]      INT            NOT NULL,
    [UserName]               NVARCHAR (256) NOT NULL,
    [BusinessUserTypeid]     INT            NOT NULL,
    [EmployeeTypeId]         INT            NOT NULL,
    [GoogleCalendarFilePath] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUsers_dbo.BusinessUserTypes_BusinessUserTypeid] FOREIGN KEY ([BusinessUserTypeid]) REFERENCES [dbo].[BusinessUserTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUsers_dbo.EmployeeTypes_EmployeeTypeId] FOREIGN KEY ([EmployeeTypeId]) REFERENCES [dbo].[EmployeeTypes] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTypeId]
    ON [dbo].[AspNetUsers]([EmployeeTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BusinessUserTypeId]
    ON [dbo].[AspNetUsers]([BusinessUserTypeid] ASC);



















/*Items Table*/

CREATE TABLE [dbo].[Items] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [PhotoPath]   NVARCHAR (MAX) NULL,
    [User_Id]     NVARCHAR (128) NULL,
    CONSTRAINT [PK_dbo.Items] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Items_dbo.AspNetUsers_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
);






/*DataTransferRate Table*/

CREATE TABLE [dbo].[DataTransferRates] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [StartTime] DATETIME       NOT NULL,
    [EndTime]   DATETIME       NOT NULL,
    [StartPage] NVARCHAR (MAX) NOT NULL,
    [EndPage]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.DataTransferRates] PRIMARY KEY CLUSTERED ([Id] ASC)
);











/**/











