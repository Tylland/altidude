 CREATE TABLE "UserView"
(
  "Id" UniqueIdentifier PRIMARY KEY,
  "UserName" VARCHAR(100) NULL,
  "Email" VARCHAR(100) NULL,
  "FirstName" VARCHAR(100) NULL,
  "LastName" VARCHAR(100) NULL,
  "CreatedTime" DATETIME NOT NULL
);