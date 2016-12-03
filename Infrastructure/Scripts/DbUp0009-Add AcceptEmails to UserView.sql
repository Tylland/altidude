ALTER TABLE UserView ADD [AcceptsEmails] bit NULL
GO
UPDATE UserView SET [AcceptsEmails] = 1 
GO
ALTER TABLE UserView ALTER COLUMN [AcceptsEmails] bit NOT NULL
GO