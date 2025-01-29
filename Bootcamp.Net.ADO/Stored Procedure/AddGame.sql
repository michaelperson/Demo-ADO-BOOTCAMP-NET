CREATE PROCEDURE [dbo].[AddGame]
    @Title NVARCHAR(100), 
    @ReleaseDate DATETIME,
    @Description NVARCHAR(MAX) ,
    @Price MONEY,
    @IsNew BIT,
    @PEGI SMALLINT, 
    @StudioId int
AS
BEGIN

    INSERT INTO [dbo].[Game] ([Title],[ReleaseDate],[Description],[Price],[IsNew],[PEGI],[StudioId])
    VALUES (@Title, @ReleaseDate, @Description, @Price, @IsNew, @PEGI, @StudioId);

END