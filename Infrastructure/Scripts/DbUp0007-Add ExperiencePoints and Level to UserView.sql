ALTER TABLE UserView ADD [ExperiencePoints] int NULL
GO
UPDATE UserView SET [ExperiencePoints] = 0 
GO
ALTER TABLE UserView ALTER COLUMN [ExperiencePoints] int NOT NULL
GO

ALTER TABLE UserView ADD [Level] int NULL
GO
UPDATE UserView SET [Level] = 0 
GO
ALTER TABLE UserView ALTER COLUMN [Level] int NOT NULL
GO