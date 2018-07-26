CREATE TRIGGER tr_OrdersUpdateMileage
ON Orders
FOR UPDATE
AS
BEGIN
	DECLARE @startMileage INT = (
		SELECT TotalMileage
		  FROM deleted
	)
	
	DECLARE @endMileage INT = (
		SELECT TotalMileage
		  FROM inserted
	)

	DECLARE @vehicleId INT = (
		SELECT VehicleId 
		  FROM inserted
	)

	IF (@startMileage IS NULL)
	BEGIN
		UPDATE Vehicles
		   SET Mileage += @endMileage
		 WHERE Id = @vehicleId
	END
END