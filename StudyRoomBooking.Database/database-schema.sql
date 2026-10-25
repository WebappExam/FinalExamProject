CREATE TABLE "Locations" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Locations" PRIMARY KEY AUTOINCREMENT,
    "Building" TEXT NOT NULL,
    "Floor" INTEGER NOT NULL,
    "RoomNumber" TEXT NOT NULL
);


CREATE TABLE "Facilities" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Facilities" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Capacity" INTEGER NOT NULL,
    "LocationId" INTEGER NOT NULL,
    CONSTRAINT "CK_Facility_Capacity" CHECK (Capacity >= 1 AND Capacity <= 1000),
    CONSTRAINT "FK_Facilities_Locations_LocationId" FOREIGN KEY ("LocationId") REFERENCES "Locations" ("Id") ON DELETE RESTRICT
);


CREATE INDEX "IX_Facilities_LocationId" ON "Facilities" ("LocationId");


