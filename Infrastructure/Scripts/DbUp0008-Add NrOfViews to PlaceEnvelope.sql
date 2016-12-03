ALTER TABLE ProfileEnvelope ADD [NrOfViews] int NULL
GO
UPDATE ProfileEnvelope SET [NrOfViews] = 0 
GO
ALTER TABLE ProfileEnvelope ALTER COLUMN [NrOfViews] int NOT NULL
GO