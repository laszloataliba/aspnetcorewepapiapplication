CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

CREATE TABLE "Suppliers" (
    "Id" BLOB NOT NULL CONSTRAINT "PK_Suppliers" PRIMARY KEY,
    "Name" varchar(200) NOT NULL,
    "IdentificationNumber" varchar(14) NULL,
    "SupplierType" INTEGER NOT NULL,
    "Active" bit NOT NULL DEFAULT 0
);

CREATE TABLE "Addresses" (
    "Id" BLOB NOT NULL CONSTRAINT "PK_Addresses" PRIMARY KEY,
    "SupplierId" BLOB NOT NULL,
    "StreetAddress" varchar(200) NOT NULL,
    "Number" varchar(50) NOT NULL,
    "AddressAddOn" varchar(250) NULL,
    "ZipCode" varchar(8) NOT NULL,
    "Neighborhood" varchar(100) NOT NULL,
    "City" varchar(100) NOT NULL,
    "State" varchar(50) NOT NULL,
    CONSTRAINT "FK_Addresses_Suppliers_SupplierId" FOREIGN KEY ("SupplierId") REFERENCES "Suppliers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Products" (
    "Id" BLOB NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY,
    "SupplierId" BLOB NOT NULL,
    "Name" varchar(200) NOT NULL,
    "Description" varchar(400) NULL,
    "Image" varchar(100) NULL,
    "Value" decimal(18, 2) NOT NULL,
    "CreationDate" datetime NOT NULL DEFAULT (GETDATE()),
    "Active" INTEGER NOT NULL,
    CONSTRAINT "FK_Products_Suppliers_SupplierId" FOREIGN KEY ("SupplierId") REFERENCES "Suppliers" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Addresses_SupplierId" ON "Addresses" ("SupplierId");

CREATE INDEX "IX_Products_SupplierId" ON "Products" ("SupplierId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200723042152_Initial', '2.2.6-servicing-10079');

