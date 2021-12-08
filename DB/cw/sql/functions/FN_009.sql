-- FN_009

CREATE OR REPLACE FUNCTION get_doctors_personal_patients(doctor_id UUID)
    RETURNS SETOF RECORD
AS
$$
SELECT C.*
FROM "Clients" AS C
         JOIN "MedicalCard" MC on C."Id" = MC."ClientId"
         JOIN "Doctors" D on D."Id" = MC."PersonalDoctorId"
WHERE D."Id" = doctor_id;
$$
    LANGUAGE sql;

SELECT get_doctors_personal_patients('491d8628-3f96-4e98-874e-754cf4526713');