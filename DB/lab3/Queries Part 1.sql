-- 1. Show all developers
SELECT devs."INN" AS "DevTIN"
     , concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , devs."Occupation"
     , devs."Salary"
     , age(lower(devs."Experience")) AS "Experience"
FROM "Developers" AS devs;

-- 2. Select developer with TIN "1"
SELECT devs."INN" AS "DevTIN"
     , concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , devs."Occupation"
     , devs."Salary"
     , age(lower(devs."Experience")) AS "Experience"
FROM "Developers" AS devs
WHERE devs."INN" = 1;

-- 3. Select developer by its first and last name
SELECT devs."INN" AS "DevTIN"
     , concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , devs."Occupation"
     , devs."Salary"
     , age(lower(devs."Experience")) AS "Experience"
FROM "Developers" AS devs
WHERE devs."FirstName" = 'Vladislav' AND devs."LastName" = 'Bardin';

-- 4. Select developers by names
SELECT devs."INN" AS "DevTIN"
     , concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , devs."Occupation"
     , devs."Salary"
     , age(lower(devs."Experience")) AS "Experience"
FROM "Developers" AS devs
WHERE devs."FirstName" = 'Vladislav' OR devs."FirstName" = 'Ivan';

-- 5. Select developers whose names not like a specified one
SELECT devs."INN" AS "DevTIN"
     , concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , devs."Occupation"
     , devs."Salary"
     , age(lower(devs."Experience")) AS "Experience"
FROM "Developers" AS devs
WHERE devs."FirstName" NOT LIKE 'Vladislav';

-- 6. Select developers whose salary is higher than 700 and less then 1500
SELECT devs."INN" AS "DevTIN"
     , concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , devs."Occupation"
     , devs."Salary"
     , age(lower(devs."Experience")) AS "Experience"
FROM "Developers" AS devs
WHERE '[700,1500]'::numrange @> devs."Salary";

-- 7. Select all testers who is older than 18
SELECT QAs."INN" AS "DevTIN"
     , concat(QAs."FirstName", ' ', QAs."LastName") AS "DevName"
     , age(QAs."BirthDate") AS "Age"
     , QAs."Occupation"
     , QAs."Salary"
     , age(lower(QAs."Experience")) AS "Experience"
FROM "Testers" AS QAs
WHERE extract(YEAR FROM age(QAs."BirthDate"::date)) > 18;

-- 8. Select testers whose birthday is less than in month
SELECT concat(QAs."FirstName", ' ', QAs."LastName") AS "Name"
FROM "Testers" AS QAs
WHERE to_char(QAs."BirthDate", 'MM-DD') <= to_char(now() + '1 month'::interval, 'MM-DD');

-- 9. Select all distributives released at 2021
SELECT distrs."Id" AS "DistributiveId"
     , distrs."Name" AS "Name"
     , distrs."BuildDate"
     , distrs."Version"
     , distrs."Hash"
FROM "Distributives" AS distrs
WHERE date_part('year', distrs."BuildDate") = 2021;

-- 10. Select documentations released during current month and that contains more that 500 pages
SELECT docs."Id" AS "DocsId"
     , docs."Pages"
     , docs."ReleaseDate"
     , docs."LastUpdateDate"
FROM "Documentations" AS docs
WHERE docs."Pages" > 500 AND (date_part('year', now()) = date_part('year', docs."ReleaseDate")
  AND date_trunc('month', now()) = date_trunc('month', docs."ReleaseDate"));

-- 11. Select developers whose first + last name ILIKE 'Vladislav Bardin'
SELECT devs."INN" AS "DevTIN"
     , concat(devs."FirstName", ' ', devs."LastName") AS "DevName"
     , devs."Occupation"
     , devs."Salary"
     , age(lower(devs."Experience")) AS "Experience"
FROM "Developers" AS devs
WHERE concat(devs."FirstName", ' ', devs."LastName") ILIKE 'Vladislav Bardin';

-- 12. Select licenses that costs more than 500$ and will ends in a month or costs more than 100$ and 'll ends in a week
SELECT licenses."ProductId"
     , licenses."Id" AS "LicenseId"
     , licenses."SellDate"
     , licenses."ExpirationDate"
FROM "Licences" AS licenses
WHERE licenses."Price" > 500 AND licenses."ExpirationDate" <= now() + '1 month'::interval
   OR licenses."Price" > 100 AND licenses."ExpirationDate" <= now() + '1 week'::interval;

-- 13. Select all distributives that match 'X-%' pattern
SELECT distrs."Id" AS "DistributiveId"
     , distrs."Name" AS "Name"
     , distrs."BuildDate"
     , distrs."Version"
     , distrs."Hash"
FROM "Distributives" AS distrs
WHERE distrs."Name" LIKE 'X-%';

-- 14. Select terms that consists of [300 - 450] pages
SELECT terms."Id"
     , terms."Pages"
     , terms."ReleaseDate"
FROM "Terms" AS terms
WHERE '[300,450]'::int4range @> terms."Pages";

-- 15. Select organizations that contains 'Star' in its name
SELECT orgs."Id" AS "OrganizationId"
     , orgs."OrganizationCode"
     , orgs."CompanyName"
FROM "Organizations" AS orgs
WHERE "CompanyName" ILIKE '%star%';
