CREATE OR ALTER FUNCTION dbo.GetJsonValue
(
    @json NVARCHAR(MAX),
    @path NVARCHAR(200)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN
    (
        CASE 
            WHEN ISJSON(@json) = 1
            THEN JSON_VALUE(@json, @path)
            ELSE NULL
        END
    );
END;
GO
