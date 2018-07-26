CREATE FUNCTION udf_GetAvailableRoom (@hotelId INT, @date DATE, @people INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @roomId INT = (
		SELECT TOP (1) 
			   r.Id 
		  FROM Trips AS t
		  JOIN Rooms AS r 
			ON r.Id = t.RoomId
		  JOIN Hotels AS h
			ON h.Id = r.HotelId
		 WHERE r.Id NOT IN (
			   SELECT RoomId 
				 FROM Trips 
				WHERE @date > ArrivalDate 
				  AND @date < ReturnDate
			   ) 
		   AND r.HotelId = @hotelId
		   AND r.Beds >= @people 
	  ORDER BY (h.BaseRate + r.Price) * @people DESC
	)

	IF (@roomId IS NULL)
	BEGIN
		RETURN 'No rooms available'
	END

	DECLARE @roomType NVARCHAR(20) = (
		  SELECT [Type]
			FROM Rooms
		   WHERE Id = @roomId
	)

	DECLARE @roomBeds INT = (
		  SELECT Beds
			FROM Rooms
		   WHERE Id = @roomId
	)

	DECLARE @roomPrice DECIMAL(15, 2) = (
		  SELECT (h.BaseRate + r.Price) * @people
			FROM Rooms AS r
			JOIN Hotels AS h
			  ON h.Id = r.HotelId
		   WHERE r.Id = @roomId
	)

	RETURN CONCAT('Room ', @roomId, ': ', @roomType, ' (', @roomBeds, ' beds) - $', @roomPrice)
END