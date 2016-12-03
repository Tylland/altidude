CREATE TABLE "PlaceEnvelope" 
(
  "Id" UniqueIdentifier PRIMARY KEY, 
  "UserId" UniqueIdentifier NOT NULL, 
  "Name" VARCHAR(100) NULL, 
  "Namespace" VARCHAR(1000) NULL, 
  "Latitude" FLOAT NOT NULL, 
  "Longitude" FLOAT NOT NULL, 
  "Altitude" FLOAT NOT NULL, 
  "BoundaryLatitudeSouth" FLOAT NOT NULL, 
  "BoundaryLatitudeNorth" FLOAT NOT NULL, 
  "BoundaryLongitudeWest" FLOAT NOT NULL, 
  "BoundaryLongitudeEast" FLOAT NOT NULL, 
  "JsonPayload" VARCHAR(8000) NULL 
); 


CREATE TABLE "ProfileEnvelope" 
(
  "Id" UniqueIdentifier PRIMARY KEY, 
  "UserId" UniqueIdentifier NOT NULL, 
  "Name" VARCHAR(100) NULL, 
  "Payload" VARCHAR(8000) NULL 
); 