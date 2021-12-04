-- 2.a

CREATE VIEW "Docs Released at December 2021" AS
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

-- 2.b Select products which has docs that released in this month

CREATE VIEW "Products With Docs Released In December 2021" AS
SELECT P."Id" AS "Product Id"
     , "DOCS_2021".*
FROM "Documentations" AS DOCS
         JOIN "Docs Released at December 2021" "DOCS_2021" on DOCS."Id" = "DOCS_2021"."DocsId"
         FULL OUTER JOIN "ProductDocumentations" PD on DOCS."Id" = PD."DocumentationId"
         JOIN "Products" P ON PD."ProductId" = P."Id"
WHERE DOCS."Pages" > 100
  AND (date_part('year', now()) = date_part('year', DOCS."ReleaseDate")
    AND date_trunc('month', now()) = date_trunc('month', DOCS."ReleaseDate"));

-- 2.c Alter view

ALTER VIEW IF EXISTS "Products With Docs Released In December 2021"
    RENAME TO "Docs Released At Dec.2021";

-- 2.d View info

SELECT pg_describe_object(refclassid, refobjid, refobjsubid)
FROM pg_depend
WHERE objid = (SELECT objid FROM "SCompany-Main-DB".pg_catalog.pg_views WHERE viewname = 'Docs Released at December 2021');