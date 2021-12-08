-- FN_006

CREATE OR REPLACE FUNCTION get_doctors_with_experience_greater_than_years(years_exp INTEGER)
    RETURNS SETOF RECORD
AS
$$
SELECT D."Id", SP."Name"
FROM "DoctorSpecializations" AS SP
         JOIN "Doctors" D on SP."Id" = D."DoctorSpecializationId"
WHERE extract(YEAR FROM age(D."HiringDate")) >= years_exp;
$$ LANGUAGE sql;

SELECT get_doctors_with_experience_greater_than_years(0);