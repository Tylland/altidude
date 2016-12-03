CREATE TABLE "TrackBoundaryView" 
(
  "TrackId" UniqueIdentifier PRIMARY KEY, 
  "South" FLOAT NOT NULL, 
  "North" FLOAT NOT NULL, 
  "West" FLOAT NOT NULL, 
  "East" FLOAT NOT NULL, 
  "OverlapCount" INTEGER NOT NULL 
); 