-- 1.a Count total amount of products for individual customers
SELECT CONCAT(INDS."FirstName", ' ', INDS."LastName") AS "Customer Name"
     , COUNT(PRODS."CustomerId")                      AS "Projects Amount"
FROM "Individuals" AS INDS
         JOIN "Customers" AS custs ON (custs."CustomerType" = 'Individual'
    AND custs."CustomerCode" = INDS."INN")
         JOIN "Products" AS PRODS ON custs."Id" = PRODS."CustomerId"
GROUP BY "Customer Name";

-- 1.b Summed price for all organizations customers price
SELECT ORGS."CompanyName"
     , SUM(prods."Price") AS "Total Price"
FROM "Organizations" AS ORGS
         JOIN "Customers" AS custs ON (custs."CustomerType" = 'Organization'
    AND custs."CustomerCode" = ORGS."OrganizationCode")
         JOIN "Products" AS prods ON custs."Id" = prods."CustomerId"
GROUP BY ORGS."CompanyName";

-- 1.c Select developers for products (with lower function)
SELECT prods."Id"                                     AS "ProductId"
     , devs."INN"                                     AS "DevTIN"
     , CONCAT(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , devs."Occupation"
     , devs."Salary"
     , AGE(LOWER(devs."Experience"))                  AS "Experience"
FROM "Products" AS prods
         JOIN "ProductDevelopers" AS prodDevs ON prods."Id" = prodDevs."ProductId"
         FULL OUTER JOIN "Developers" AS devs ON prodDevs."DeveloperId" = devs."INN"
ORDER BY prods."Id";

-- 1.d Select documentations released during current month and that contains more that 500 pages
SELECT DOCS."Id"        AS "DocsId"
     , DOCS."Pages"
     , DOCS."ReleaseDate"
     , DOCS."LastUpdateDate"
     , C."CustomerCode" AS "Customer Id"
FROM "Documentations" AS DOCS
         JOIN "ProductDocumentations" PD on DOCS."Id" = PD."DocumentationId"
         JOIN "Products" P on P."Id" = PD."ProductId"
         JOIN "Customers" C on P."CustomerId" = C."Id"
WHERE DOCS."Pages" > 500
  AND (date_part('year', now()) = date_part('year', DOCS."ReleaseDate")
    AND date_trunc('month', now()) = date_trunc('month', DOCS."ReleaseDate"));

-- 1.e Price for individual customers
SELECT CONCAT(INDS."FirstName", ' ', INDS."LastName") AS "Customer Name"
     , SUM(PRODS."Price")                             AS "Total Price"
FROM "Individuals" AS INDS
         JOIN "Customers" AS CUSTS ON (CUSTS."CustomerType" = 'Individual'
    AND CUSTS."CustomerCode" = INDS."INN")
         JOIN "Products" AS PRODS ON CUSTS."Id" = PRODS."CustomerId"
GROUP BY "Customer Name";

-- 1.f Price for individual customers
SELECT CONCAT(INDS."FirstName", ' ', INDS."LastName") AS "Customer Name"
     , COUNT(PRODS."CustomerId")                      AS "Products For Customer"
     , SUM(PRODS."Price")                             AS "Total Price"
FROM "Individuals" AS INDS
         JOIN "Customers" AS CUSTS ON (CUSTS."CustomerType" = 'Individual'
    AND CUSTS."CustomerCode" = INDS."INN")
         JOIN "Products" AS PRODS ON CUSTS."Id" = PRODS."CustomerId"
GROUP BY "Customer Name", PRODS."CustomerId"
HAVING SUM(PRODS."Price") > 120000;

-- 1.g Developers with 2 or more projects
SELECT CONCAT(DEVS."FirstName", ' ', DEVS."LastName") AS "Dev Name"
     , COUNT(PD."DeveloperId")                        AS "Projects Amount"
     , SUM(DEVS."Salary")
FROM "Developers" AS DEVS
         JOIN "ProductDevelopers" PD on DEVS."INN" = PD."DeveloperId"
GROUP BY "Dev Name"
HAVING COUNT(PD."DeveloperId") >= 2;

-- 1.i Select all products terms by developer's id
SELECT concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , prods."Id"                                     AS "ProductId"
     , terms."Id"                                     AS "TermsId"
     , terms."Pages"
     , terms."ReleaseDate"
     , terms."Version"
FROM "Terms" AS terms
         JOIN "Developers" AS devs ON devs."INN" = 1
         JOIN "ProductDevelopers" AS prodDevs ON prodDevs."DeveloperId" = 1
         JOIN "Products" AS prods ON prodDevs."ProductId" = prods."Id"
         JOIN "ProductTerms" AS prodTerms ON prods."Id" = prodTerms."ProductId"
    AND terms."Id" = prodTerms."TermsId"
ORDER BY terms."Id";