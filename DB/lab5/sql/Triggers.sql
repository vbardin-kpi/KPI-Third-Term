CREATE TABLE ProductsAudit
(
    "Operation" CHAR(1)   NOT NULL,
    "Stamp"     TIMESTAMP NOT NULL,
    "UserId"    TEXT      NOT NULL,
    "ProductId" UUID      NOT NULL
);

CREATE OR REPLACE FUNCTION process_products_audit() RETURNS TRIGGER AS
$product_audit$
BEGIN
    --
    -- Create a row in emp_audit to reflect the operation performed on emp,
    -- making use of the special variable TG_OP to work out the operation.
    --
    IF (TG_OP = 'DELETE') THEN
        INSERT INTO ProductsAudit SELECT 'D', now(), user, OLD."Id";
    ELSIF (TG_OP = 'UPDATE') THEN
        INSERT INTO ProductsAudit SELECT 'U', now(), user, NEW."Id";
    ELSIF (TG_OP = 'INSERT') THEN
        INSERT INTO ProductsAudit SELECT 'I', now(), user, NEW."Id";
    END IF;
    RETURN NULL; -- result is ignored since this is an AFTER trigger
END;
$product_audit$ LANGUAGE plpgsql;

CREATE TRIGGER products_audit
    AFTER INSERT OR UPDATE OR DELETE
    ON "Products"
    FOR EACH ROW
EXECUTE FUNCTION process_products_audit();

INSERT INTO "Products" ("Id", "Price", "CustomerId")
VALUES ('1a116c2e-ec71-416c-afc0-a28e24ad7b0c', 1491122, '782e88d2-e7ba-433b-b3f9-7cea29f8fbfc');

UPDATE "Products"
SET "Price" = 10000000
WHERE "Id" = '1a116c2e-ec71-416c-afc0-a28e24ad7b0c';

DELETE FROM "Products" WHERE "Id" = '1a116c2e-ec71-416c-afc0-a28e24ad7b0c';
