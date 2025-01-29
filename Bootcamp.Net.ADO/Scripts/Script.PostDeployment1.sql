/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


INSERT INTO [dbo].[Studio] ([Name], [CreationDate], [City], [EmployeNbr], [IsIndependant])
VALUES ('Ubisoft', '1986-03-28', 'Montreuil', 20000, 0),
        ('Nintendo', '1889-09-23', 'Kyoto', 6500, 0),
        ('Rockstar Games', '1998-12-01', 'New York', 2200, 0),
        ('CD Projekt Red', '1994-05-01', 'Varsovie', 1100, 1),
        ('Bethesda', '1986-06-28', 'Rockville', 1200, 0),
        ('Facepunch Studios','2004-06-01', 'Birmingham', 20, 1);

INSERT INTO [dbo].[Game] ([Title], [ReleaseDate], [Description], [Price], [IsNew], [PEGI], [StudioId])
VALUES 
    ('The Legend of Zelda', '1986-02-21', 'Premier jeu de la série Zelda.', 49.99, 0, 7, 2),
    ('Super Mario Bros.', '1985-09-13', 'Jeu de plateforme révolutionnaire.', 39.99, 0, 3, 2),
    ('The Witcher 3', '2015-05-19', 'RPG en monde ouvert.', 59.99, 0, 18, 4),
    ('Cyberpunk 2077', '2020-12-10', 'RPG futuriste immersif.', 59.99, 1, 18, 4),
    ('Grand Theft Auto V', '2013-09-17', 'Jeu en monde ouvert.', 59.99, 0, 18, 3),
    ('Skyrim', '2011-11-11', 'RPG en monde ouvert légendaire.', 49.99, 0, 16, 5),
    ('Assassin''s Creed II', '2009-11-17', 'Aventures historiques en Italie.', 39.99, 0, 18, 1),
    ('Far Cry 3', '2012-11-29', 'FPS en monde ouvert.', 44.99, 0, 18, 1),
    ('Red Dead Redemption 2', '2018-10-26', 'Aventures western réalistes.', 69.99, 0, 18, 3),
    ('Metroid Prime', '2002-11-17', 'Action et exploration en 3D.', 29.99, 0, 12, 2),
    ('Rust','2013-12-11','Jeu de survie', 39.99, 1, 16, 6);
