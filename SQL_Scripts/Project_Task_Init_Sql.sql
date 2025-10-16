Create table Appuser
([ID] [int] IDENTITY(1,1) NOT NULL,
 [Email] nvarchar(255) not null,
 [Password] nvarchar(255) not null,
 [PhoneNumber] nvarchar(255) null,
 [Created] [datetime] NOT NULL CONSTRAINT [Appuser_Created] DEFAULT (GetDate()),
 [LastModified] [datetime] NULL,
 Primary key ([ID])
)
 go 


CREATE TRIGGER trg_UpdateLastModified_Appuser
ON Appuser
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE t
    SET LastModified = GETDATE()
    FROM Appuser t
    INNER JOIN inserted i ON t.ID = i.ID;
END;
--commit
go

Create table Appointments
([ID] [int] IDENTITY(1,1) NOT NULL,
 [Appointment] datetime not null,
 [Note] nvarchar(255) not null,
 [AppUser_ID] int not null,
 [Created] [datetime] NOT NULL CONSTRAINT [Appointments_Created] DEFAULT (GetDate()),
 [LastModified] [datetime] NULL,
 Primary key ([ID])
)

go 

ALTER TABLE [dbo].[Appointments]    ADD  CONSTRAINT [Appointments_Appuser_FK] FOREIGN KEY([AppUser_ID])
REFERENCES [dbo].[Appuser] ([ID])
GO

ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [Appointments_Appuser_FK]
GO

CREATE TRIGGER trg_UpdateLastModified_Appointments
ON Appointments
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE t
    SET LastModified = GETDATE()
    FROM Appointments t
    INNER JOIN inserted i ON t.ID = i.ID;
END;