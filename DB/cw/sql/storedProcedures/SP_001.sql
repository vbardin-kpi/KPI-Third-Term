-- SP_001

CREATE OR REPLACE PROCEDURE sp_doctor_patients_at_timespan(
    doctor_id UUID,
    start_date DATE,
    end_date DATE)
AS
$$
    CREATE TEMP TABLE IF NOT EXISTS "DocsPatientsTempTable" AS
    SELECT concat(C."FirstName", ' ', C."LastName") AS "Client name"
    FROM "MedicalRecord" MR
             JOIN "MedicalCard" MC ON MR."MedicalCardId" = MC."Id"
             JOIN "Clients" C ON MC."ClientId" = C."Id"
    WHERE MR."CreateTs" >= start_date
      AND MR."CreateTs" <= end_date
      AND MR."DoctorId" = doctor_id;
    $$ LANGUAGE sql;


CREATE OR REPLACE PROCEDURE sp_create_view_for_doctors_patients(
    doctor_id UUID,
    start_date DATE,
    end_date DATE)
AS
$$
BEGIN
    CALL sp_doctor_patients_at_timespan(
            doctor_id,
            start_date,
            end_date);

    CREATE OR REPLACE VIEW "Doctor's () Patients for 01/12/2020 to 12/08/2021" AS
    SELECT *
    FROM "DocsPatientsTempTable";
END;
$$
    LANGUAGE plpgsql;

CALL sp_create_view_for_doctors_patients('08713628-1646-490b-98cd-0c96a4b7cbf4', '12.01.2020', '12.08.2021');

SELECT *
FROM "Doctor's () Patients for 01/12/2020 to 12/08/2021";
DROP TABLE IF EXISTS "DocsPatientsTempTable" CASCADE;