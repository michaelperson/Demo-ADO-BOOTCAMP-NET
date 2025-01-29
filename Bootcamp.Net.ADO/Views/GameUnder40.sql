CREATE VIEW [GameUnder40]
	AS SELECT * FROM [Game]
	WHERE [Price] < 40
