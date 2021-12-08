-- FN_003

CREATE OR REPLACE FUNCTION get_profit_by_client(client_id UUID)
    RETURNS DECIMAL
AS
$$
DECLARE
    profit DECIMAL;
BEGIN
    CREATE TEMP TABLE IF NOT EXISTS "Client's Services" AS
    SELECT S."Price"
    FROM "MedicalCard" AS MC
             JOIN "MedicalRecord" MR on MC."Id" = MR."MedicalCardId"
             JOIN "MedicalRecord_Services" MRS on MR."Id" = MRS."MedicalRecordId"
             JOIN "Services" S on MRS."ServiceId" = S."Id"
    WHERE "ClientId" = client_id;

    SELECT sum(CS."Price")
    INTO profit
    FROM "Client's Services" AS CS;

    DROP TABLE "Client's Services" CASCADE;
    RETURN profit;
END;
$$ LANGUAGE plpgsql;

SELECT get_profit_by_client('63d066b9-a92c-466f-a2fd-1cc6fd48f0ce');