-- Create a user and a database.
-- Grant him a full access to the database.

CREATE USER SC_owner WITH PASSWORD 'SC_owner';

CREATE DATABASE "SCompany-Main-DB" owner SC_owner;
ALTER DATABASE "SCompany-Main-DB" SET TIMEZONE = 'UTC';

GRANT ALL PRIVILEGES ON DATABASE "SCompany-Main-DB" TO SC_owner;
