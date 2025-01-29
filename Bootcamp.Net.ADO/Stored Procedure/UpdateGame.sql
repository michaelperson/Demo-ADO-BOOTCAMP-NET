CREATE PROCEDURE [dbo].[UpdateGame]
    @Id INT,
	@Title NVARCHAR(100), 
    @ReleaseDate DATETIME,
    @Description NVARCHAR(MAX) ,
    @Price MONEY,
    @IsNew BIT,
    @PEGI SMALLINT, 
    @StudioId INT
AS
BEGIN
    UPDATE [dbo].[Game]
    SET [Title]= @Title,
        [ReleaseDate]= @ReleaseDate,
        [Description]= @Description,
        [Price]= @Price,
        [IsNew]= @IsNew,
        [PEGI]= @PEGI,
        [StudioId]= @StudioId
    WHERE [Id] = @Id;
END