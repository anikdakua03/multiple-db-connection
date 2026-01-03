CREATE PROCEDURE GetAllPersons
AS
BEGIN
    -- SET NOCOUNT ON prevents extra result sets for performance
    SET NOCOUNT ON;

    -- Select all columns from the person table
    SELECT * FROM Persons;
END
GO