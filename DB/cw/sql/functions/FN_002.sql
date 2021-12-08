-- FN_002

CREATE OR REPLACE FUNCTION get_most_popular_service_type_at_timespan(
    start_date DATE, end_date DATE)
    RETURNS RECORD
AS
$$
DECLARE
    result "ServiceTypes";
BEGIN
    SELECT ST."Id", ST."ServiceType"
    INTO result
    FROM "Services" AS S
        JOIN "MedicalRecord_Services" MRS on S."Id" = MRS."ServiceId"
        JOIN "MedicalRecord" MR on MRS."MedicalRecordId" = MR."Id"
        JOIN "Service_ServiceTypes" SST on S."Id" = SST."ServiceId"
        JOIN "ServiceTypes" ST on SST."ServiceTypeId" = ST."Id"
    WHERE MR."CreateTs" >= start_date AND MR."CreateTs" <= end_date
    GROUP BY ST."Id", ST."ServiceType"
    ORDER BY count(ST."ServiceType") DESC
    LIMIT 1;

    RETURN result;
END;
$$
    LANGUAGE plpgsql;

SELECT get_most_popular_service_type_at_timespan('12.01.2020', '12.08.2021');
