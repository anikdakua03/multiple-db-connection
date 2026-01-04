CREATE OR ALTER PROCEDURE GetAllPersons
AS
BEGIN
    -- SET NOCOUNT ON prevents extra result sets for performance
    SET NOCOUNT ON;

    -- Select all columns from the person table
    SELECT [Id]
          ,[FirstName]
          ,[LastName]
          ,[Email]
          ,[Address]
          ,[CreatedTimestamp]
          ,[UpdatedTimestamp]
          ,[Details]
          ,[History]
  FROM [dbo].[Persons]

END
GO