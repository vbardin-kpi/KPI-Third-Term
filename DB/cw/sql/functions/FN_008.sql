-- FN_008

CREATE OR REPLACE FUNCTION sp_get_all_receptions_during_timespan(
    start_date DATE, end_date date)
    RETURNS SETOF RECORD
AS
$$
SELECT C."Id", C."FirstName", C."LastName", MR."Info"
FROM "MedicalRecord" AS MR
    JOIN "MedicalCard" MC on MR."MedicalCardId" = MC."Id"
    JOIN "Clients" C on MC."ClientId" = C."Id"
WHERE MR."CreateTs" >= start_date AND MR."CreateTs" <= end_date;
$$ LANGUAGE sql;

SELECT sp_get_all_receptions_during_timespan('12.01.2021', '12.08.2021');