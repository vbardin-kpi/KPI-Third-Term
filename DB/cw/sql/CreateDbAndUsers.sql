-- Create DB owner user, DB and grant him all privileges
CREATE USER "HospitalOwner" WITH PASSWORD 'hospital_owner';

CREATE DATABASE "HospitalDb" OWNER "HospitalOwner";
ALTER DATABASE "HospitalDb" SET TIMEZONE = 'UTC';

GRANT ALL PRIVILEGES ON DATABASE "HospitalDb" TO "HospitalOwner";

-- Create user with ReadOnly access to public schema
CREATE USER "HospitalReader" WITH PASSWORD 'hospital_reader';
GRANT CONNECT ON DATABASE "HospitalDb" TO "HospitalReader";
GRANT USAGE ON SCHEMA public TO "HospitalReader";
GRANT SELECT ON ALL TABLES IN SCHEMA public TO "HospitalReader";