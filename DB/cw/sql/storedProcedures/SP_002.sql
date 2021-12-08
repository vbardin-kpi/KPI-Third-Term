-- SP_002

CREATE OR REPLACE PROCEDURE sp_illness_history(client_id UUID)
AS
$$
BEGIN
    CREATE TEMP TABLE IF NOT EXISTS "ClientIllnesses" AS
    SELECT MR.*
    FROM "MedicalCard" AS MC
        JOIN "MedicalRecord" MR on MC."Id" = MR."MedicalCardId"
    WHERE MC."ClientId" = client_id;

    CREATE OR REPLACE VIEW "Client's illnesses history" AS
    SELECT * FROM "ClientIllnesses";

END;
$$
    LANGUAGE plpgsql;

CALL sp_illness_history('789ea97b-a0e1-4e17-b1ef-d11bf82ef1c3');

SELECT *
FROM "Client's illnesses history";
DROP TABLE IF EXISTS "ClientIllnesses" CASCADE;
