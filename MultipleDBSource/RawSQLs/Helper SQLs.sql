-- Check SQL server version


SELECT @@VERSION;

SELECT
    SERVERPROPERTY('ProductVersion') AS ProductVersion,
    SERVERPROPERTY('ProductLevel') AS ProductLevel,
    SERVERPROPERTY('Edition') AS Edition;


-- Backup DB [ If needed ]

BACKUP DATABASE [Persons_1_Db] TO DISK = '<PATH_TO_YOUR_FOLDER>\[NAME].bak';
BACKUP DATABASE [Persons_2_Db] TO DISK = '<PATH_TO_YOUR_FOLDER>\[NAME].bak';
BACKUP DATABASE [Players] TO DISK = '<PATH_TO_YOUR_FOLDER>\[NAME].bak';

