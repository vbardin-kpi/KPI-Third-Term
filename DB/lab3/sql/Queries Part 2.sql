-- 1. Select all product's developers
SELECT concat(devs."FirstName", ' ', devs."LastName") AS "Name"
     , "Occupation"
     , "Salary"
     , prodDevs."ProductId" AS "ProjectId"
FROM "Developers" AS devs
LEFT JOIN "ProductDevelopers" AS prodDevs ON (devs."INN" = prodDevs."DeveloperId"
                                                 AND prodDevs."ProductId" = '4a276c2e-ec71-416c-afc0-a28e24ad7b0c')
ORDER BY devs."INN";

-- 2. Select testers for product
SELECT concat(QAs."FirstName", ' ', QAs."LastName") AS "Name"
     , QAs."Occupation"
     , QAs."Salary"
     , age(lower(QAs."Experience")) AS "Experience"
FROM "Testers" AS QAs
WHERE "INN" = (
    SELECT "TesterId"
    FROM "ProductTesters"
    WHERE "ProductId" = '4a276c2e-ec71-416c-afc0-a28e24ad7b0c'
          )
ORDER BY "FirstName", "LastName";

-- 3. Select all products distributives by developer's id
SELECT concat(devs."FirstName", ' ', devs."LastName")
     , prods."Id" AS "ProductId"
     , distrs."Id" AS "TermsId"
     , distrs."BuildDate"
     , distrs."Version"
     , distrs."Hash"
FROM "Distributives" AS distrs
JOIN "Developers" AS devs ON devs."INN" = 1
JOIN "ProductDevelopers" AS prodDevs ON prodDevs."DeveloperId" = 1
JOIN "Products" AS prods ON prodDevs."ProductId" = prods."Id"
JOIN "ProductDistributives" AS prodDistr ON prods."Id" = prodDistr."ProductId"
                                                AND distrs."Id" = prodDistr."DistributiveId";

-- 4. Select all products terms by developer's id
SELECT concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , prods."Id" AS "ProductId"
     , terms."Id" AS "TermsId"
     , terms."Pages"
     , terms."ReleaseDate"
     , terms."Version"
FROM "Terms" AS terms
JOIN "Developers" AS devs ON devs."INN" = 1
JOIN "ProductDevelopers" AS prodDevs ON prodDevs."DeveloperId" = 1
JOIN "Products" AS prods ON prodDevs."ProductId" = prods."Id"
JOIN "ProductTerms" AS prodTerms ON prods."Id" = prodTerms."ProductId"
                                                AND terms."Id" = prodTerms."TermsId";

-- 5. Select developers for products
SELECT prods."Id" AS "ProductId"
     , devs."INN" AS "DevTIN"
     , concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , devs."Occupation"
     , devs."Salary"
     , age(lower(devs."Experience")) AS "Experience"
FROM "Products" AS prods
JOIN "ProductDevelopers" AS prodDevs ON prods."Id" = prodDevs."ProductId"
FULL OUTER JOIN "Developers" AS devs ON prodDevs."DeveloperId" = devs."INN"
ORDER BY prods."Id";

-- 6. Select licenses for products
SELECT licenses."ProductId"
     , licenses."Id" AS "LicenseId"
     , licenses."SellDate"
     , licenses."ExpirationDate"
FROM "Licences" AS licenses
JOIN "Products" AS prods ON licenses."ProductId" = prods."Id"
ORDER BY licenses."Id";

-- 7. Select all individuals customers
SELECT inds."Id" AS "CustomerId"
     , inds."INN" AS "TIN"
     , concat(inds."FirstName", ' ', inds."LastName")
FROM "Individuals" AS inds
JOIN "Customers" AS custs ON ("CustomerType" = 'Individual'
                                  AND custs."CustomerCode" = inds."INN");

-- 8. Select documentations for products
SELECT prods."Id"
     , docs."Id"
     , docs."ReleaseDate"
     , docs."Pages"
     , docs."LastUpdateDate"
FROM "Products" AS prods
JOIN "ProductDocumentations" AS prodDocs ON prods."Id" = prodDocs."ProductId"
LEFT JOIN "Documentations" AS docs on prodDocs."DocumentationId" = docs."Id"
ORDER BY prods."Id";

-- 9. Select terms for projects
SELECT prods."Id"
     , terms."Id"
     , terms."Pages"
     , terms."ReleaseDate"
FROM "Terms" AS terms
JOIN "ProductTerms" AS prodTerms on terms."Id" = prodTerms."TermsId"
RIGHT JOIN "Products" AS prods on prods."Id" = prodTerms."ProductId"
ORDER BY prods."Id";

-- 10. Show all possible pairs Developer-Tester
SELECT concat(devs."FirstName", ' ', devs."LastName") AS dev
     , concat(QAs."FirstName", ' ', QAs."LastName") AS QA
FROM "Developers" AS devs
CROSS JOIN "Testers" AS QAs;

-- 11. Get terms that contains from 300 to 450 pages for products
SELECT prods."Id"
     , terms."Id"
     , terms."Pages"
FROM "Terms" AS terms
JOIN "ProductTerms" AS prodTerms ON terms."Id" = prodTerms."TermsId"
FULL JOIN "Products" AS prods ON prodTerms."ProductId" = prods."Id"
WHERE '[300,450]'::int4range @> terms."Pages";

-- 12. Select developers for projects
SELECT prods."Id"
     , devs."FirstName"
     , devs."LastName"
     , devs."Occupation"
     , devs."Salary"
FROM "Developers" AS devs
JOIN "ProductDevelopers" AS prodDevs on devs."INN" = prodDevs."DeveloperId"
FULL OUTER JOIN "Products" AS prods ON prodDevs."ProductId" = prods."Id"
ORDER BY prods."Id", devs."INN";

-- 13. Select all testers for customer
SELECT prods."Id"
     , custs."Id"
     , QAs."FirstName"
     , QAs."LastName"
     , QAs."Occupation"
     , QAs."Salary"
FROM "Testers" AS QAs
JOIN "ProductTesters" AS prodQAs ON QAs."INN" = prodQAs."TesterId"
JOIN "Products" AS prods ON prodQAs."ProductId" = prods."Id"
FULL JOIN "Customers" AS custs ON prods."CustomerId" = custs."Id"
ORDER BY prods."Id", QAs."INN";

-- 14. Select products for individual customers
SELECT concat(inds."FirstName", ' ', inds."LastName") AS "CustomerName"
     , prods."Id" AS "ProjectId"
FROM "Individuals" AS inds
JOIN "Customers" AS custs ON (custs."CustomerType" = 'Individual'
                                  AND custs."CustomerCode" = inds."INN")
JOIN "Products" AS prods ON custs."Id" = prods."CustomerId"
ORDER BY prods."Id";

-- 15. Select products for organization customers
SELECT orgs."CompanyName"
     , prods."Id" AS "ProjectId"
FROM "Organizations" AS orgs
JOIN "Customers" AS custs ON (custs."CustomerType" = 'Organization'
                                  AND custs."CustomerCode" = orgs."OrganizationCode")
JOIN "Products" AS prods ON custs."Id" = prods."CustomerId"
ORDER BY prods."Id";
