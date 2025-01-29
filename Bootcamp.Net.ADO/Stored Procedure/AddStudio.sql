CREATE PROCEDURE [dbo].[AddStudio]
	@Name NVARCHAR(100),
    @CreationDate DATETIME,
    @City NVARCHAR(50),
    @EmployeNbr INT,
    @IsIndependant BIT
AS
BEGIN

    INSERT INTO [dbo].[Studio] ([Name], [CreationDate], [City], [EmployeNbr], [IsIndependant])
    VALUES (@Name, @CreationDate, @City, @EmployeNbr, @IsIndependant)
END
