CREATE PROC usp_SwitchRoom (@TripId INT, @TargetRoomId INT)
AS
BEGIN
	DECLARE @roomHotelId INT = (
		SELECT HotelId
		  FROM Rooms
		 WHERE Id = @TargetRoomId
	)
	
	DECLARE @tripHotelId INT = (
		SELECT r.HotelId
		  FROM Trips AS t
		  JOIN Rooms AS r
		    ON r.Id = t.RoomId
		 WHERE t.Id = @TripId
	)

	IF (@roomHotelId <> @tripHotelId)
	BEGIN
		RAISERROR('Target room is in another hotel!', 16, 1)
		RETURN
	END
	
	DECLARE @tripAccountsCount INT = (
		SELECT COUNT(AccountId)
		  FROM AccountsTrips
		 WHERE TripId = @TripId
	)

	DECLARE @roomBeds INT = (
		SELECT Beds
		  FROM Rooms
		 WHERE Id = @TargetRoomId
	)

	IF (@tripAccountsCount > @roomBeds)
	BEGIN
		RAISERROR('Not enough beds in target room!', 16, 2)
		RETURN
	END
	
	UPDATE Trips
	   SET RoomId = @TargetRoomId
	 WHERE Id = @TripId
END