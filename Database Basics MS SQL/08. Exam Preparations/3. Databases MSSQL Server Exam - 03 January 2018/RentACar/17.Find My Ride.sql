CREATE FUNCTION udf_CheckForVehicle (@townName NVARCHAR(50), @seatsNumber INT)
RETURNS NVARCHAR(100)
AS
BEGIN
	DECLARE @result NVARCHAR(100) = (
		SELECT TOP (1)
			   CONCAT(o.[Name], ' - ', m.Model)
		  FROM Towns AS t
		  JOIN Offices AS o
			ON o.TownId = t.Id
		  JOIN Vehicles AS v
			ON v.OfficeId = o.Id
		  JOIN Models AS m
			ON m.Id = v.ModelId
		 WHERE t.[Name] = @townName
		   AND m.Seats = @seatsNumber
	  ORDER BY o.[Name]
    )

	IF (@result IS NULL)
	BEGIN
		SET @result = 'NO SUCH VEHICLE FOUND'
	END

	RETURN @result
END