-- 1.a

CREATE OR REPLACE PROCEDURE sp_create_temp_table()
AS
$$
BEGIN
    CREATE TEMP TABLE IF NOT EXISTS devs_temp AS
    SELECT concat(D."FirstName", ' ', D."LastName")
    FROM "Developers" AS D;
END;
$$ LANGUAGE plpgsql;

CALL sp_create_temp_table();
SELECT *
FROM devs_temp;
DROP TABLE IF EXISTS devs_temp;

-- 1.b

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

CREATE OR REPLACE PROCEDURE sp_create_devs_temp_if_not_exists()
AS
$$
BEGIN
    IF check_if_table_exists('public', 'devs_temp') = FALSE THEN
        CALL sp_create_temp_table();
    END if;
END;
$$ LANGUAGE plpgsql;

CALL sp_create_devs_temp_if_not_exists();
SELECT *
FROM devs_temp;
DROP TABLE IF EXISTS devs_temp;

-- 1.c

CREATE OR REPLACE PROCEDURE sp_create_and_fill_table()
AS
$$
DECLARE
    i_increment INT := 1;
    i_current   INT := 1;
    i_end       INT := 350;
BEGIN
    DROP TABLE IF EXISTS test_1c;

    CREATE TEMP TABLE test_1c
    (
        num INTEGER
    );

    WHILE i_current <= i_end
        LOOP
            i_current := i_current + i_increment;
            INSERT INTO test_1c (num)
            SELECT i_current;
        END LOOP;
END;
$$
    LANGUAGE plpgsql;

CALL sp_create_and_fill_table();

SELECT *
FROM test_1c;
DROP TABLE IF EXISTS test_1c;

-- 1.d
-- this was done at 1.a, 1.b and 1.c

-- 1.e

CREATE OR REPLACE PROCEDURE get_test_1c_stat(
    out min_val int,
    out max_val int,
    out avg_val numeric)
AS
$$
BEGIN
    SELECT min(num),
           max(num),
           avg(num)
    INTO min_val, max_val, avg_val
    FROM test_1c;

END ;
$$ LANGUAGE plpgsql;

CALL sp_create_and_fill_table();
CALL get_test_1c_stat(0, 0, 0);
DROP TABLE IF EXISTS test_1c;

-- 1.f
-- impossible to complete at PostgreSQL because stored procedures can't RETURN anything

-- 1.g
CREATE OR REPLACE PROCEDURE sp_update_table()
AS
$$
    UPDATE test_1c
    SET num = 1;
$$
    LANGUAGE sql;

CALL sp_create_and_fill_table();
SELECT *
FROM test_1c;
CALL sp_update_table();
SELECT *
FROM test_1c;
DROP TABLE IF EXISTS test_1c;

-- 1.f
CREATE OR REPLACE PROCEDURE sp_select_all_developers()
AS
$$
SELECT 1
INTO b;
$$
    LANGUAGE sql;

CALL sp_select_all_developers();
SELECT *
FROM b;
DROP TABLE IF EXISTS b;