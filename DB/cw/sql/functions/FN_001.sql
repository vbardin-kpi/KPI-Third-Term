-- FN_001

CREATE OR REPLACE FUNCTION get_most_popular_service_at_timespan(
    start_date DATE, end_date DATE)
    RETURNS RECORD
AS
$$
DECLARE
    result "Services";
BEGIN
    SELECT s."Id", count(s."Id")
    INTO result
    FROM "Services" AS S
        JOIN "MedicalRecord_Services" MRS on S."Id" = MRS."ServiceId"
        JOIN "MedicalRecord" MR on MRS."MedicalRecordId" = MR."Id"
    WHERE MR."CreateTs" >= start_date AND MR."CreateTs" <= end_date
    GROUP BY S."Id"
    ORDER BY count(s."Id") DESC
    LIMIT 1;

    RETURN result;
END;
$$
    LANGUAGE plpgsql;

SELECT get_most_popular_service_at_timespan('12.01.2020', '12.08.2021');