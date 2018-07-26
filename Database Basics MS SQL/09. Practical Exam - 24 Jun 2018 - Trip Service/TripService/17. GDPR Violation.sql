  SELECT t.Id,
	     CONCAT(a.FirstName, ' ' + a.MiddleName, ' ', a.LastName) AS [Full Name],
		 ca.[Name] AS [From],
		 hc.[Name] AS [To],
		 CASE
			WHEN t.CancelDate IS NOT NULL THEN 'Canceled'
			ELSE CONCAT(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate), ' days')
		 END AS Duration
    FROM Trips AS t
	JOIN AccountsTrips AS ac
	  ON ac.TripId = t.Id
	JOIN Accounts AS a
	  ON a.Id = ac.AccountId
	JOIN Rooms AS r
	  ON r.Id = t.RoomId
	JOIN Hotels AS h
	  ON h.Id = r.HotelId
	JOIN Cities AS hc
	  ON hc.Id = h.CityId
	JOIN Cities AS ca
	  ON ca.Id = a.CityId
ORDER BY [Full Name],
		 t.Id