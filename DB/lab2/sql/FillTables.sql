-- Fill developers table
INSERT INTO "Developers" ("INN", "FirstName", "LastName", "BirthDate", "Occupation", "Salary", "Experience")
VALUES (1, 'Vladislav', 'Bardin', '2002-10-08', 'Junior .NET Developer', 1000, '[2021-02-05, 2021-09-19)'),
       (2, 'Igor', 'Erokhin', '2003-11-19', 'Junior Java Script Developer', 600, '[2021-08-05, 2021-10-21)');


-- Testers table
INSERT INTO "Testers" ("INN", "FirstName", "LastName", "BirthDate", "Occupation", "Salary", "Experience")
VALUES (1, 'Maxime', 'Kurkin', '2002-09-10', 'Junior QA', 500, '[2021-08-05, 2021-10-21)');


-- Individual customers table
INSERT INTO "Individuals" ("INN", "Id", "FirstName", "LastName", "BirthDate")
VALUES (1, '95959856-05b3-42fd-a5f3-e3f3fa500112', 'Nastya', 'Stepanova', '1987-07-03');


-- Organizations customers table
INSERT INTO "Organizations" ("Id", "OrganizationCode", "CompanyName")
VALUES ('aa08491d-d8fc-4216-ae45-d31459751382', '1', 'TranStar');


-- Customers table
INSERT INTO "Customers" ("Id", "CustomerType", "CustomerCode")
VALUES ('782e88d2-e7ba-433b-b3f9-7cea29f8fbfc', 'Individual', 1),
       ('100f6315-6fd2-41c8-9f14-4078c49795c9', 'Organization', 1);


-- Documentations table
INSERT INTO "Documentations" ("Id", "Pages")
VALUES ('47d7e17e-8b4b-4c1c-ab1e-3cf95ab6ecfa', 263),
       ('c864872b-c01d-4c69-92ea-a697854a6c39', 725),
       ('3c050e26-2209-4bf9-87d0-47ce455c5fe8', 315);


-- Terms table
INSERT INTO "Terms" ("Id", "Pages")
VALUES ('556cadac-f75a-46a3-bb95-0a6493135031', 104),
       ('405f241d-9ddc-4a7d-a3af-f3415a5c81d1', 536),
       ('0b9fe7dd-57f6-4949-a427-e363d3eaeff3', 917);


-- Distributives table
INSERT INTO "Distributives" ("Id", "Name", "BuildDate", "Hash", "Version", "BuildNumber")
VALUES ('6993a04f-d313-4857-96f0-96b75c19a9fc', 'Vanity Fair', '2021-10-20', 'b08a86450cd4ede972c1b8ebe3aed3b8', 1, 1),
       ('bfd140da-cfae-4d35-997a-896e1f908cc2', 'X-Oreo', '2021-10-24', '83f9825cbd677c74dfebf6a8079e60d5', 1, 2);


-- Products table
INSERT INTO "Products" ("Id", "Price", "CustomerId")
VALUES ('4a276c2e-ec71-416c-afc0-a28e24ad7b0c', 1491122, '782e88d2-e7ba-433b-b3f9-7cea29f8fbfc'),
       ('f8b73f40-d501-4ed2-96d9-461204aaa191', 100000, '100f6315-6fd2-41c8-9f14-4078c49795c9');


-- Licences table
INSERT INTO "Licences" ("Id", "Price", "ProductId")
VALUES ('6cdf8d30-9c0c-4be2-ac42-3304b2ab3742', 500, '4a276c2e-ec71-416c-afc0-a28e24ad7b0c'),
       ('78a7cd89-c13e-4e2f-aa41-50b9f27d75fb', 649.99, 'f8b73f40-d501-4ed2-96d9-461204aaa191');


-- ProductDevelopers table
INSERT INTO "ProductDevelopers" ("ProductId", "DeveloperId")
VALUES ('4a276c2e-ec71-416c-afc0-a28e24ad7b0c', 1),
       ('f8b73f40-d501-4ed2-96d9-461204aaa191', 2);


-- ProductTesters table
INSERT INTO "ProductTesters" ("ProductId", "TesterId")
VALUES ('4a276c2e-ec71-416c-afc0-a28e24ad7b0c', 1),
       ('f8b73f40-d501-4ed2-96d9-461204aaa191', 1);


-- ProductDistributives table
INSERT INTO "ProductDistributives" ("ProductId", "DistributiveId")
VALUES ('4a276c2e-ec71-416c-afc0-a28e24ad7b0c', '6993a04f-d313-4857-96f0-96b75c19a9fc'),
       ('f8b73f40-d501-4ed2-96d9-461204aaa191', 'bfd140da-cfae-4d35-997a-896e1f908cc2');


-- ProductDocumentations table
INSERT INTO "ProductDocumentations" ("ProductId", "DocumentationId")
VALUES ('4a276c2e-ec71-416c-afc0-a28e24ad7b0c', '47d7e17e-8b4b-4c1c-ab1e-3cf95ab6ecfa'),
       ('f8b73f40-d501-4ed2-96d9-461204aaa191', 'c864872b-c01d-4c69-92ea-a697854a6c39'),
       ('f8b73f40-d501-4ed2-96d9-461204aaa191', '3c050e26-2209-4bf9-87d0-47ce455c5fe8');


-- ProductTerms table
INSERT INTO "ProductTerms" ("ProductId", "TermsId")
VALUES ('4a276c2e-ec71-416c-afc0-a28e24ad7b0c', '556cadac-f75a-46a3-bb95-0a6493135031'),
       ('f8b73f40-d501-4ed2-96d9-461204aaa191', '405f241d-9ddc-4a7d-a3af-f3415a5c81d1'),
       ('f8b73f40-d501-4ed2-96d9-461204aaa191', '0b9fe7dd-57f6-4949-a427-e363d3eaeff3');