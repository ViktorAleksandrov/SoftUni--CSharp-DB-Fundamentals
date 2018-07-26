CREATE PROC usp_MoveVehicle (@vehicleId INT, @officeId INT)
AS
BEGIN
	DECLARE @occupiedParkingPlaces INT = (
		SELECT COUNT(Id)
		  FROM Vehicles
		 WHERE OfficeId = @officeId
	)

	DECLARE @officeParkingPlaces INT = (
		SELECT ParkingPlaces
		  FROM Offices
		 WHERE Id = @officeId
	)

	IF (@occupiedParkingPlaces >= @officeParkingPlaces)
	BEGIN
		RAISERROR('Not enough room in this office!', 16, 1)
		RETURN
	END

	UPDATE Vehicles
	   SET OfficeId = @officeId
	 WHERE Id = @vehicleId
END