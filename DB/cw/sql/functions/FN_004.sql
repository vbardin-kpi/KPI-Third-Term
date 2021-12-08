-- FN_003

CREATE OR REPLACE FUNCTION get_profit_by_doctor(doctor_id UUID)
    RETURNS DECIMAL
AS
$$
DECLARE
    profit DECIMAL;
BEGIN
    CREATE TEMP TABLE IF NOT EXISTS "Doctor's Services" AS
    SELECT S."Price"
    FROM "MedicalRecord" MR
             JOIN "MedicalRecord_Services" MRS on MR."Id" = MRS."MedicalRecordId"
             JOIN "Services" S on MRS."ServiceId" = S."Id"
    WHERE MR."DoctorId" = doctor_id;

    SELECT sum(CS."Price")
    INTO profit
    FROM "Doctor's Services" AS CS;

    DROP TABLE "Doctor's Services" CASCADE;
    RETURN profit;
END;
$$ LANGUAGE plpgsql;

SELECT get_profit_by_doctor('37abe20e-fb67-448d-a929-11048de547c7');