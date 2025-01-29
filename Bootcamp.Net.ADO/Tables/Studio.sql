CREATE TABLE [dbo].[Studio]
(
	[Id] INT NOT NULL IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [CreationDate] DATETIME NULL, 
    [City] NVARCHAR(50) NULL, 
    [EmployeNbr] INT NULL, 
    [IsIndependant] BIT NOT NULL,
    [IsActive] BIT NULL DEFAULT 1, 
    CONSTRAINT [PK_Studio_Id] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_Studio_EmployeNbr] CHECK ([EmployeNbr] > 0),
    CONSTRAINT [CK_Studio_CreationDate] CHECK ([CreationDate] <= GETDATE())

)

GO

CREATE TRIGGER [dbo].[trg_SetActivity]
    ON [dbo].[Studio]
    INSTEAD OF DELETE
    AS
    BEGIN
            DELETE FROM [dbo].[Studio] 
            WHERE Id IN (SELECT Id FROM deleted) AND IsActive = 0;

            UPDATE [dbo].[Studio]
            SET IsActive = 0
            WHERE Id IN (SELECT Id FROM deleted) AND IsActive = 1;
    END