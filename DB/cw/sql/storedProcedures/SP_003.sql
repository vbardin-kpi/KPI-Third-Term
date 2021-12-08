-- SP_003

CREATE OR REPLACE PROCEDURE sp_get_reception_invoice(med_record_id UUID)
AS
$$
BEGIN
    CREATE TEMP TABLE IF NOT EXISTS "Invoice" AS
    SELECT MR."Id"         AS "Visit Id",
           SUM(S2."Price") AS "Visit Price"
    FROM "MedicalRecord" AS MR
             JOIN "MedicalRecord_Services" MRS on MR."Id" = MRS."MedicalRecordId"
             JOIN "Services" S2 on MRS."ServiceId" = S2."Id"
    WHERE MR."Id" = med_record_id
    GROUP BY MR."Id";

    CREATE OR REPLACE VIEW "Visit total Price" AS
    SELECT * FROM "Invoice";
END;
$$ LANGUAGE plpgsql;

CALL sp_get_reception_invoice('d1c04734-05cb-4f01-ba99-c018bac7e99c');
SELECT *
FROM "Visit total Price";
DROP TABLE IF EXISTS "Invoice" CASCADE;
