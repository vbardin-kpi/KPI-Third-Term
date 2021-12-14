CREATE TABLE test
(
    col text
);
INSERT INTO test
VALUES ('123');

CREATE FUNCTION reffunc(refcursor)
    RETURNS refcursor
AS
$$
BEGIN
    OPEN $1 FOR SELECT col FROM test;
    RETURN $1;
END;
$$
    LANGUAGE plpgsql;

BEGIN;
SELECT reffunc('funccursor');
FETCH ALL IN funccursor;
COMMIT;

