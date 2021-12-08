-- FN_005

CREATE OR REPLACE FUNCTION get_doctors_by_specialization(specialization_id INTEGER)
    RETURNS TABLE
            (
                "DoctorId" UUID
            )
AS
$$
DECLARE
BEGIN
    RETURN QUERY SELECT D."Id"
    FROM "DoctorSpecializations" AS SP
        JOIN "Doctors" D on SP."Id" = D."DoctorSpecializationId"
    WHERE SP."Id" = specialization_id;
END;
$$ LANGUAGE plpgsql;

SELECT get_doctors_by_specialization(1);