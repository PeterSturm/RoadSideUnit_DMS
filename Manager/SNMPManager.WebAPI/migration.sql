CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318182157_InitialMigrations') THEN
    CREATE TYPE log_type AS ENUM ('db', 'security', 'apicall', 'snmp');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318182157_InitialMigrations') THEN
    CREATE TABLE "ManagerLogs" (
        "TimeStamp" timestamp without time zone NOT NULL,
        "Type" log_type NOT NULL,
        "Message" text NULL,
        CONSTRAINT "PK_ManagerLogs" PRIMARY KEY ("TimeStamp", "Type")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318182157_InitialMigrations') THEN
    CREATE TABLE "ManagerSettings" (
        "Id" serial NOT NULL,
        "Timeout" integer NOT NULL,
        CONSTRAINT "PK_ManagerSettings" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318182157_InitialMigrations') THEN
    CREATE TABLE "Roles" (
        "Id" serial NOT NULL,
        "Name" text NULL,
        CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318182157_InitialMigrations') THEN
    CREATE TABLE "RSUs" (
        "Id" serial NOT NULL,
        "IP" inet NOT NULL,
        "Port" integer NOT NULL,
        "Name" text NOT NULL,
        "Latitude" double precision NOT NULL,
        "Longitude" double precision NOT NULL,
        "Active" boolean NOT NULL,
        "MIBVersion" character varying(32) NULL,
        "FirmwareVersion" character varying(32) NULL,
        "LocationDescription" character varying(140) NULL,
        "Manufacturer" character varying(32) NULL,
        "NotificationIP" inet NOT NULL,
        "NotificationPort" integer NOT NULL,
        CONSTRAINT "PK_RSUs" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318182157_InitialMigrations') THEN
    CREATE TABLE "TrapLogs" (
        "TimeStamp" timestamp without time zone NOT NULL,
        "Type" log_type NOT NULL,
        "SourceRSU" integer NOT NULL,
        "Message" text NULL,
        CONSTRAINT "PK_TrapLogs" PRIMARY KEY ("TimeStamp", "Type", "SourceRSU")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318182157_InitialMigrations') THEN
    CREATE TABLE "Users" (
        "Id" serial NOT NULL,
        "UserName" text NOT NULL,
        "Token" text NULL,
        "RoleId" integer NULL,
        "LastName" text NULL,
        "FirstName" text NULL,
        "SNMPv3Auth" text NULL,
        "SNMPv3Priv" text NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id"),
        CONSTRAINT "AK_Users_UserName" UNIQUE ("UserName"),
        CONSTRAINT "FK_Users_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318182157_InitialMigrations') THEN
    CREATE INDEX "IX_Users_RoleId" ON "Users" ("RoleId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318182157_InitialMigrations') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190318182157_InitialMigrations', '2.2.2-servicing-10034');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318184511_SeedData') THEN
    INSERT INTO "RSUs" ("Id", "Active", "FirmwareVersion", "IP", "Latitude", "LocationDescription", "Longitude", "MIBVersion", "Manufacturer", "Name", "NotificationIP", "NotificationPort", "Port")
    VALUES (1, TRUE, '', INET '172.168.45.27', 17.449999999999999, '', 24.120000000000001, '', 'Commsignia', 'TestRSU', INET '186.56.123.84', 161, 162);
    INSERT INTO "RSUs" ("Id", "Active", "FirmwareVersion", "IP", "Latitude", "LocationDescription", "Longitude", "MIBVersion", "Manufacturer", "Name", "NotificationIP", "NotificationPort", "Port")
    VALUES (2, TRUE, '', INET '112.111.45.89', 19.449999999999999, '', 45.119999999999997, '', 'Commsignia', 'RSUuu', INET '186.56.123.84', 161, 162);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318184511_SeedData') THEN
    INSERT INTO "Roles" ("Id", "Name")
    VALUES (1, 'Admin');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318184511_SeedData') THEN
    INSERT INTO "Users" ("Id", "FirstName", "LastName", "RoleId", "SNMPv3Auth", "SNMPv3Priv", "Token", "UserName")
    VALUES (1, 'Péter', 'Sturm', 1, 'authpass012', 'privpass012', 'test', 'sturm');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190318184511_SeedData') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190318184511_SeedData', '2.2.2-servicing-10034');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190328201817_SeedManagerSettings') THEN
    INSERT INTO "ManagerSettings" ("Id", "Timeout")
    VALUES (1, 2000);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190328201817_SeedManagerSettings') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190328201817_SeedManagerSettings', '2.2.2-servicing-10034');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190328204559_SeedRSUforJavaAgent') THEN
    INSERT INTO "RSUs" ("Id", "Active", "FirmwareVersion", "IP", "Latitude", "LocationDescription", "Longitude", "MIBVersion", "Manufacturer", "Name", "NotificationIP", "NotificationPort", "Port")
    VALUES (3, TRUE, '', INET '127.0.0.1', 13.449999999999999, '', 32.119999999999997, '', 'Commsignia', 'RSUjavaagent', INET '127.0.0.1', 162, 161);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190328204559_SeedRSUforJavaAgent') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190328204559_SeedRSUforJavaAgent', '2.2.2-servicing-10034');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190406120019_UserModified') THEN
    DELETE FROM "Users"
    WHERE "Id" = 1;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190406120019_UserModified') THEN
    ALTER TABLE "Users" DROP COLUMN "FirstName";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190406120019_UserModified') THEN
    ALTER TABLE "Users" DROP COLUMN "LastName";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190406120019_UserModified') THEN
    INSERT INTO "Roles" ("Id", "Name")
    VALUES (2, 'Monitor');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190406120019_UserModified') THEN
    INSERT INTO "Users" ("Id", "RoleId", "SNMPv3Auth", "SNMPv3Priv", "Token", "UserName")
    VALUES (1, 1, 'authpass012', 'privpass012', 'Adminpass01', 'admin');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190406120019_UserModified') THEN
    INSERT INTO "Users" ("Id", "RoleId", "SNMPv3Auth", "SNMPv3Priv", "Token", "UserName")
    VALUES (2, 2, 'authpass012', 'privpass012', 'Monitorpass01', 'monitor');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190406120019_UserModified') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190406120019_UserModified', '2.2.2-servicing-10034');
    END IF;
END $$;
