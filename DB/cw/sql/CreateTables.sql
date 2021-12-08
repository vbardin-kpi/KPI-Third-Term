-- Create types (if required)

CREATE TYPE "Sex" AS ENUM ('Male', 'Female', 'X');

-- Create tables

-- Common/Base tables
CREATE TABLE IF NOT EXISTS "People"
(
    "TIN"       INTEGER PRIMARY KEY,
    "FirstName" VARCHAR(64)                         NOT NULL,
    "LastName"  VARCHAR(64)                         NOT NULL,
    "Sex"       "Sex"                               NOT NULL,
    "BirthDate" DATE CHECK ( "BirthDate" <= NOW() ) NOT NULL
);

CREATE UNIQUE INDEX "People_UIndex" ON "People" (
                                                 "TIN" ASC
    );

-- Doctors tables

CREATE TABLE IF NOT EXISTS "DoctorSpecializations"
(
    "Id"   INTEGER PRIMARY KEY,
    "Name" VARCHAR NOT NULL UNIQUE
);

CREATE UNIQUE INDEX "DoctorSpecializations_UIndex" ON "DoctorSpecializations" (
                                                                               "Id" ASC
    );

CREATE TABLE IF NOT EXISTS "Doctors"
(
    "Id"                     UUID PRIMARY KEY,
    "HiringDate"             DATE CHECK ( "HiringDate" <= now() ),
    "DoctorSpecializationId" INTEGER,
    CONSTRAINT "Doctors_DoctorsSpecializations__fk"
        FOREIGN KEY ("DoctorSpecializationId")
            REFERENCES "DoctorSpecializations" ("Id")
) INHERITS ("People");

CREATE UNIQUE INDEX "Doctors_UIndex" ON "Doctors" (
                                                   "Id" ASC
    );

CREATE TABLE IF NOT EXISTS "ServiceTypes"
(
    "Id"          UUID PRIMARY KEY,
    "ServiceType" VARCHAR NOT NULL
);

CREATE UNIQUE INDEX "ServiceTypes_UIndex" ON "ServiceTypes" (
                                                             "Id" ASC,
                                                             "ServiceType" ASC
    );


CREATE TABLE IF NOT EXISTS "Services"
(
    "Id"    UUID PRIMARY KEY,
    "Name"  VARCHAR NOT NULL,
    "Price" DECIMAL CHECK ( "Price" >= 0 )
);

CREATE UNIQUE INDEX "Services_UIndex" ON "Services" (
                                                     "Id" ASC
    );

CREATE TABLE IF NOT EXISTS "Service_ServiceTypes"
(
    "ServiceId"     UUID,
    "ServiceTypeId" UUID,
    CONSTRAINT "Service_ServiceTypes_pk"
        PRIMARY KEY ("ServiceId", "ServiceTypeId"),
    CONSTRAINT "Service_ServiceTypes_Service__fk"
        FOREIGN KEY ("ServiceId") REFERENCES "Services" ("Id"),
    CONSTRAINT "Service_ServiceTypes_ServiceType__fk"
        FOREIGN KEY ("ServiceTypeId") REFERENCES "ServiceTypes" ("Id")
);

-- Clients tables

CREATE TABLE IF NOT EXISTS "Clients"
(
    "Id" UUID PRIMARY KEY
) INHERITS ("People");

CREATE UNIQUE INDEX "Customers_UIndex" ON "Clients" (
                                                     "Id" ASC
    );

-- General tables

CREATE TABLE IF NOT EXISTS "MedicalCard"
(
    "Id"               UUID PRIMARY KEY,
    "CardOpenTs"       DATE DEFAULT NOW(),
    "ClientId"         UUID UNIQUE NOT NULL,
    "PersonalDoctorId" UUID        NOT NULL,
    CONSTRAINT "MedicalCard_Clients__fk" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id"),
    CONSTRAINT "MedicalCard_Doctors__fk" FOREIGN KEY ("PersonalDoctorId") REFERENCES "Doctors" ("Id")
);

CREATE UNIQUE INDEX "MedicalCard_UIndex" ON "MedicalCard" (
                                                           "Id" ASC
    );

CREATE TABLE IF NOT EXISTS "MedicalRecord"
(
    "Id"            UUID PRIMARY KEY,
    "CreateTs"      DATE DEFAULT NOW(),
    "Info"          VARCHAR,
    "DoctorId"      UUID,
    "MedicalCardId" UUID,
    "ServiceId"     UUID,
    CONSTRAINT "MedicalRecord_Doctors__fk"
        FOREIGN KEY ("DoctorId") REFERENCES "Doctors" ("Id"),
    CONSTRAINT "MedicalRecord_MedicalCard__fk"
        FOREIGN KEY ("MedicalCardId") REFERENCES "MedicalCard" ("Id"),
    CONSTRAINT "MedicalRecord_Service__fk"
        FOREIGN KEY ("ServiceId") REFERENCES "Services" ("Id")
);

CREATE UNIQUE INDEX "MedicalRecord_UIndex" ON "MedicalRecord" (
                                                               "Id" ASC
    );

CREATE TABLE IF NOT EXISTS "MedicalRecord_Services"
(
    "Id"              UUID PRIMARY KEY,
    "MedicalRecordId" UUID,
    "ServiceId"       UUID,
    CONSTRAINT "MedicalRecord_Services_MedicalRecord__fk"
        FOREIGN KEY ("MedicalRecordId") REFERENCES "MedicalRecord" ("Id"),
    CONSTRAINT "MedicalRecord_Services_Services__fk"
        FOREIGN KEY ("ServiceId") REFERENCES "Services" ("Id")
);

CREATE INDEX "MedicalRecord_Services_Index" ON "MedicalRecord_Services" (
                                                                         "Id" ASC,
                                                                         "ServiceId" ASC,
                                                                         "MedicalRecordId" ASC
    );

