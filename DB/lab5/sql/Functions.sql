-- 2.a
CREATE OR REPLACE FUNCTION check_if_table_exists(schema_name varchar, _table_name varchar)
    RETURNS BOOLEAN
AS
$$
SELECT EXISTS(
               SELECT
               FROM information_schema.tables
               WHERE table_schema = schema_name
                 AND table_name = _table_name
           );
$$ LANGUAGE sql;

SELECT check_if_table_exists('public', 'Developers');

-- 2.b
CREATE TABLE t1
(
    id   serial PRIMARY KEY,
    col1 text
    -- infowindow does not exist
);
INSERT INTO t1(col1)
VALUES ('foo1'),
       ('bar1');

CREATE OR REPLACE FUNCTION f_tbl_plus_info_window(_table regclass)
    RETURNS void AS -- no direct return type
$func$
DECLARE
    -- appending _tmp for temp table
    _tmp text := quote_ident(_table::text || '_tmp');
BEGIN

    -- Create temp table only for duration of transaction
    EXECUTE format(
            'CREATE TEMP TABLE %s ON COMMIT DROP AS TABLE %s LIMIT 0', _tmp, _table);

    IF EXISTS(
            SELECT 1
            FROM pg_attribute a
            WHERE a.attrelid = _table
              AND a.attname = 'infowindow'
              AND a.attisdropped = FALSE)
    THEN
        EXECUTE format('INSERT INTO %s SELECT * FROM %s', _tmp, _table);
    ELSE
        -- This is assuming a NOT NULL column named "id"!
        EXECUTE format($x$
      ALTER  TABLE %1$s ADD COLUMN infowindow text;
      INSERT INTO %1$s
      SELECT *, 'ID: ' || id::text
      FROM   %2$s $x$
            , _tmp, _table);
    END IF;

END
$func$ LANGUAGE plpgsql;

BEGIN;
SELECT f_tbl_plus_info_window('t1');
SELECT *
FROM t1_tmp; -- do something with the returned rows
COMMIT;

-- 2.c
CREATE OR REPLACE FUNCTION get_developers(_pattern VARCHAR)
    RETURNS TABLE
            (
                dev_full_name VARCHAR
            )
    LANGUAGE plpgsql
AS
$$
BEGIN
    RETURN QUERY
        SELECT concat(d."FirstName", ' ', D."LastName")::VARCHAR as dev_full_name
        FROM "Developers" AS D
        WHERE concat(d."FirstName", ' ', D."LastName") ILIKE _pattern;
END;
$$;

SELECT * FROM get_developers ('Vl%');