CREATE PROCEDURE [dbo].[DeleteGame]
	@Id INT
AS
BEGIN
	DELETE FROM [dbo].[Game] WHERE [Id] = @Id;
END
