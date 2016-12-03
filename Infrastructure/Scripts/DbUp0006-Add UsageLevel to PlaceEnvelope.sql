ALTER TABLE PlaceEnvelope ADD [UsageLevel] int NULL
GO
UPDATE PlaceEnvelope SET UsageLevel = 2 
GO
ALTER TABLE PlaceEnvelope ALTER COLUMN [UsageLevel] int NOT NULL
GO