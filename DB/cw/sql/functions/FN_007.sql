-- FN_007

CREATE OR REPLACE FUNCTION get_all_doctors_who_worked_with_client(client_id UUID)
    RETURNS SETOF RECORD
AS
$$
SELECT D."Id", D."FirstName", D."LastName"
FROM "Doctors" AS D
         JOIN "MedicalRecord" MR on D."Id" = MR."DoctorId"
         JOIN "MedicalCard" MC on MC."Id" = MR."MedicalCardId"
         JOIN "Clients" C on MC."ClientId" = C."Id"
WHERE C."Id" = client_id;
$$ LANGUAGE sql;

SELECT get_all_doctors_who_worked_with_client('fd45d03b-c870-4e32-a70a-44df891f4113');