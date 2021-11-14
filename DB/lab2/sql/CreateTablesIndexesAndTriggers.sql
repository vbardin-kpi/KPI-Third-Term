-- Create custom types

CREATE TYPE "CustomerType" AS ENUM ('Individual', 'Organization');


-- Create tables and set all the required constraints.

CREATE TABLE IF NOT EXISTS "People"
(
    "INN"       INTEGER,
    "FirstName" VARCHAR(64),
    "LastName"  VARCHAR(64),
    "BirthDate" DATE CHECK ( "BirthDate" <= now() ) DEFAULT now(),
    CONSTRAINT people_pk PRIMARY KEY ("INN")
);

CREATE TABLE IF NOT EXISTS "Developers"
(
    "Occupation" VARCHAR(64),
    "Salary"     DECIMAL,
    "Experience" DATERANGE NOT NULL,
    CONSTRAINT developers_pk PRIMARY KEY ("INN")
) INHERITS ("People");

CREATE TABLE IF NOT EXISTS "Testers"
(
    "Occupation" VARCHAR(64),
    "Salary"     DECIMAL,
    "Experience" DATERANGE NOT NULL,
    CONSTRAINT testers_pk PRIMARY KEY ("INN")
) INHERITS ("People");

CREATE TABLE IF NOT EXISTS "Individuals"
(
    "Id" UUID,
    CONSTRAINT id_pk PRIMARY KEY ("Id")
) INHERITS ("People");

CREATE TABLE IF NOT EXISTS "Organizations"
(
    "Id"               UUID,
    "OrganizationCode" INTEGER,
    "CompanyName"      VARCHAR,
    CONSTRAINT organizations_pk PRIMARY KEY ("Id"),
    UNIQUE ("OrganizationCode")
);

CREATE TABLE IF NOT EXISTS "Customers"
(
    "Id"           UUID,
    "CustomerType" "CustomerType",
    "CustomerCode" INTEGER,
    CONSTRAINT customers_pk PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Documentations"
(
    "Id"             UUID,
    "Pages"          INTEGER,
    "ReleaseDate"    DATE NOT NULL CHECK ( "ReleaseDate" <= now() )    DEFAULT now(),
    "LastUpdateDate" DATE NOT NULL CHECK ( "LastUpdateDate" <= now() ) DEFAULT now(),
    CONSTRAINT documentations_pk PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Terms"
(
    "Id"          UUID,
    "ReleaseDate" DATE CHECK ( "ReleaseDate" <= now() ) DEFAULT now(),
    "Pages"       INTEGER,
    "Version"     INTEGER NOT NULL                      DEFAULT 0,
    CONSTRAINT terms_pk PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Distributives"
(
    "Id"          UUID,
    "Name"        VARCHAR(256),
    "BuildDate"   DATE CHECK ( "BuildDate" <= now() ) DEFAULT now(),
    -- md5 hash is 128-bit length hash that is equal to 32 characters.
    "Hash"        CHAR(32),
    "Version"     INTEGER NOT NULL                    DEFAULT 0,
    "BuildNumber" INTEGER NOT NULL                    DEFAULT 0,
    CONSTRAINT distributives_pk PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Products"
(
    "Id"         UUID,
    "Price"      DECIMAL,
    "CustomerId" UUID,
    CONSTRAINT products_pk PRIMARY KEY ("Id"),
    CONSTRAINT product_customer_fk
        FOREIGN KEY ("CustomerId")
            REFERENCES "Customers" ("Id")
);

CREATE TABLE IF NOT EXISTS "Licences"
(
    "Id"             UUID,
    "Price"          DECIMAL,
    "SellDate"       DATE CHECK ( "SellDate" <= now() )       DEFAULT now(),
    "ExpirationDate" DATE CHECK ( "ExpirationDate" <= now() ) DEFAULT now(),
    "ProductId"      UUID,
    CONSTRAINT licences_pk PRIMARY KEY ("Id"),
    CONSTRAINT licences_products_fk
        FOREIGN KEY ("ProductId")
            REFERENCES "Products" ("Id")
);

CREATE TABLE IF NOT EXISTS "ProductDevelopers"
(
    "ProductId"   UUID,
    "DeveloperId" INTEGER,
    CONSTRAINT product_developers_pk
        PRIMARY KEY ("ProductId", "DeveloperId"),
    CONSTRAINT product_developers__product_fk
        FOREIGN KEY ("ProductId")
            REFERENCES "Products" ("Id"),
    CONSTRAINT product_developers__developer_fk
        FOREIGN KEY ("DeveloperId")
            REFERENCES "Developers" ("INN")
);

CREATE TABLE IF NOT EXISTS "ProductTesters"
(
    "ProductId" UUID,
    "TesterId"  INTEGER,
    CONSTRAINT product_testers_pk
        PRIMARY KEY ("ProductId", "TesterId"),
    CONSTRAINT product_testers__product_fk
        FOREIGN KEY ("ProductId")
            REFERENCES "Products" ("Id"),
    CONSTRAINT product_testers__tester_fk
        FOREIGN KEY ("TesterId")
            REFERENCES "Testers" ("INN")
);

CREATE TABLE IF NOT EXISTS "ProductDocumentations"
(
    "ProductId"       UUID,
    "DocumentationId" UUID,
    CONSTRAINT product_documentations_pk
        PRIMARY KEY ("ProductId", "DocumentationId"),
    CONSTRAINT product_documentations__product_fk
        FOREIGN KEY ("ProductId")
            REFERENCES "Products" ("Id"),
    CONSTRAINT product_documentations__documentation_fk
        FOREIGN KEY ("DocumentationId")
            REFERENCES "Documentations" ("Id")
);

CREATE TABLE IF NOT EXISTS "ProductTerms"
(
    "ProductId" UUID,
    "TermsId"   UUID,
    CONSTRAINT product_terms_pk
        PRIMARY KEY ("ProductId", "TermsId"),
    CONSTRAINT product_terms__product_fk
        FOREIGN KEY ("ProductId")
            REFERENCES "Products" ("Id"),
    CONSTRAINT product_terms__terms_fk
        FOREIGN KEY ("TermsId")
            REFERENCES "Terms" ("Id")
);

CREATE TABLE IF NOT EXISTS "ProductDistributives"
(
    "ProductId"      UUID,
    "DistributiveId" UUID,
    CONSTRAINT product_distributives_pk
        PRIMARY KEY ("ProductId", "DistributiveId"),
    CONSTRAINT product_distributives__product_fk
        FOREIGN KEY ("ProductId")
            REFERENCES "Products" ("Id"),
    CONSTRAINT product_distributives__distributives_fk
        FOREIGN KEY ("DistributiveId")
            REFERENCES "Distributives" ("Id")
);


-- Create indexes

CREATE UNIQUE INDEX developers_uindex ON "Developers" (
                                                       "INN" ASC
    );

CREATE UNIQUE INDEX testers_uindex ON "Testers" (
                                                 "INN" ASC
    );

CREATE UNIQUE INDEX organizations_uindex ON "Organizations" (
                                                             "Id" ASC
    );

CREATE UNIQUE INDEX customers_uindex ON "Customers" (
                                                     "Id" ASC
    );

CREATE UNIQUE INDEX products_uindex ON "Products" (
                                                   "Id" ASC,
                                                   "CustomerId" ASC
    );

CREATE INDEX licences_index ON "Licences" (
                                           "Id" ASC,
                                           "SellDate" DESC,
                                           "ExpirationDate" DESC
    );

CREATE INDEX licences_sell_date_index ON "Licences" (
                                                     "SellDate" DESC
    );

CREATE INDEX licences_expiration_date_index ON "Licences" (
                                                           "ExpirationDate" DESC
    );

CREATE UNIQUE INDEX documentations_uindex ON "Documentations" (
                                                               "Id" ASC
    );

CREATE UNIQUE INDEX terms_uindex ON "Terms" (
                                             "Id" ASC
    );

CREATE UNIQUE INDEX distributives_uindex ON "Distributives" (
                                                             "Id" ASC
    );

CREATE UNIQUE INDEX product_developers__uindex ON "ProductDevelopers" (
                                                                       "ProductId" ASC,
                                                                       "DeveloperId" ASC
    );

CREATE UNIQUE INDEX product_testers__uindex ON "ProductTesters" (
                                                                 "ProductId" ASC,
                                                                 "TesterId" ASC
    );

CREATE UNIQUE INDEX product_documentations__uindex ON "ProductDocumentations" (
                                                                               "ProductId" ASC,
                                                                               "DocumentationId" ASC
    );

CREATE UNIQUE INDEX product_distributives__uindex ON "ProductDistributives" (
                                                                             "ProductId" ASC,
                                                                             "DistributiveId" ASC
    );

CREATE UNIQUE INDEX product_terms__uindex ON "ProductTerms" (
                                                             "ProductId" ASC,
                                                             "TermsId" ASC
    );

CREATE UNIQUE INDEX individual_customers__uindex ON "Individuals" (
                                                                   "INN" ASC,
                                                                   "Id" ASC
    );

CREATE UNIQUE INDEX organizations_customers__uindex ON "Organizations" (
                                                                        "Id" ASC
    );


-- Functions

CREATE OR REPLACE FUNCTION increment_version()
    RETURNS TRIGGER
AS
$body$
BEGIN
    NEW."Version" := NEW."Version" + 1;
    RETURN NEW;
END
$body$
    LANGUAGE plpgsql;

ALTER FUNCTION increment_version() OWNER TO CURRENT_USER;


-- Triggers

CREATE TRIGGER version_trigger_terms
    before update
    on "Terms"
    for EACH ROW
EXECUTE PROCEDURE increment_version();

CREATE TRIGGER version_trigger_documentations
    before update
    on "Terms"
    for EACH ROW
EXECUTE PROCEDURE increment_version();
