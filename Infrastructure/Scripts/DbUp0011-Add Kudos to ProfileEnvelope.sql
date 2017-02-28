ALTER TABLE ProfileEnvelope ADD [Kudos] int NULL
GO
UPDATE ProfileEnvelope SET [Kudos] = 0 
GO
ALTER TABLE ProfileEnvelope ALTER COLUMN [Kudos] int NOT NULL
GO