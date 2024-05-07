DECLARE
V_COUNT INTEGER;
BEGIN
SELECT COUNT(TABLE_NAME) INTO V_COUNT from USER_TABLES where TABLE_NAME = '__EFMigrationsHistory';
IF V_COUNT = 0 THEN
Begin
BEGIN 
EXECUTE IMMEDIATE 'CREATE TABLE 
"__EFMigrationsHistory" (
    "MigrationId" NVARCHAR2(150) NOT NULL,
    "ProductVersion" NVARCHAR2(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
)';
END;

End;

END IF;
EXCEPTION
WHEN OTHERS THEN
    IF(SQLCODE != -942)THEN
        RAISE;
    END IF;
END;
/

BEGIN 
EXECUTE IMMEDIATE 'CREATE TABLE 
"Shelters" (
    "ShelterId" RAW(16) NOT NULL,
    "Login" NVARCHAR2(2000) NOT NULL,
    "Password" NVARCHAR2(2000) NOT NULL,
    "ShelterName" NVARCHAR2(2000) NOT NULL,
    "Address" NVARCHAR2(2000) NOT NULL,
    CONSTRAINT "PK_Shelters" PRIMARY KEY ("ShelterId")
)';
END;
/

BEGIN 
EXECUTE IMMEDIATE 'CREATE TABLE 
"ShelterNeeds" (
    "ShelterNeedsId" RAW(16) NOT NULL,
    "ShelterId" RAW(16) NOT NULL,
    "AcceptingVolunteers" NUMBER(10) NOT NULL,
    "AcceptingDoctors" NUMBER(10) NOT NULL,
    "AcceptingVeterinarians" NUMBER(10) NOT NULL,
    "AcceptingDonations" NUMBER(10) NOT NULL,
    "DonationDescription" NVARCHAR2(2000),
    "VolunteersSubscriptionLink" NVARCHAR2(2000),
    "UpdatedAt" TIMESTAMP(7) WITH TIME ZONE NOT NULL,
    CONSTRAINT "PK_ShelterNeeds" PRIMARY KEY ("ShelterNeedsId"),
    CONSTRAINT "FK_ShelterNeeds_Shelters_ShelterId" FOREIGN KEY ("ShelterId") REFERENCES "Shelters" ("ShelterId") ON DELETE CASCADE
)';
END;
/

BEGIN 
EXECUTE IMMEDIATE 'CREATE TABLE 
"Families" (
    "FamilyId" RAW(16) NOT NULL,
    "ShelterId" RAW(16) NOT NULL,
    "ResponsableId" RAW(16) NOT NULL,
    "RegisteredAt" TIMESTAMP(7) WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP(7) WITH TIME ZONE NOT NULL,
    CONSTRAINT "PK_Families" PRIMARY KEY ("FamilyId"),
    CONSTRAINT "FK_Families_Shelters_ShelterId" FOREIGN KEY ("ShelterId") REFERENCES "Shelters" ("ShelterId") ON DELETE CASCADE
)';
END;
/

BEGIN 
EXECUTE IMMEDIATE 'CREATE TABLE 
"Houseds" (
    "HousedId" RAW(16) NOT NULL,
    "FamilyId" RAW(16) NOT NULL,
    "Name" NVARCHAR2(2000) NOT NULL,
    "Age" NUMBER(10) NOT NULL,
    "Cellphone" NVARCHAR2(2000),
    "IsFamilyResponsable" NUMBER(10) NOT NULL,
    "FamilyResponsableId" RAW(16) NOT NULL,
    "RegisteredAt" TIMESTAMP(7) WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP(7) WITH TIME ZONE NOT NULL,
    CONSTRAINT "PK_Houseds" PRIMARY KEY ("HousedId"),
    CONSTRAINT "FK_Houseds_Families_FamilyId" FOREIGN KEY ("FamilyId") REFERENCES "Families" ("FamilyId") ON DELETE CASCADE
)';
END;
/

CREATE UNIQUE INDEX "IX_Families_ResponsableId" ON "Families" ("ResponsableId")
/

CREATE INDEX "IX_Families_ShelterId" ON "Families" ("ShelterId")
/

CREATE INDEX "IX_Houseds_FamilyId" ON "Houseds" ("FamilyId")
/

CREATE UNIQUE INDEX "IX_ShelterNeeds_ShelterId" ON "ShelterNeeds" ("ShelterId")
/

ALTER TABLE "Families" ADD CONSTRAINT "FK_Families_Houseds_ResponsableId" FOREIGN KEY ("ResponsableId") REFERENCES "Houseds" ("HousedId") ON DELETE CASCADE
/

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES (N'20240507145303_Initial', N'8.0.4')
/

ALTER TABLE "Shelters" ADD "Adm" NUMBER(10) DEFAULT 0 NOT NULL
/

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES (N'20240507162456_AdmShelter', N'8.0.4')
/

