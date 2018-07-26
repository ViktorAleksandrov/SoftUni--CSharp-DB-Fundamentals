  SELECT Id,
	     Email,
	     CountryCode,
	     Trips
   FROM (
	  SELECT a.Id,
			 a.Email,
			 c.CountryCode,
			 COUNT(t.Id) AS Trips,
			 ROW_NUMBER() OVER (PARTITION BY c.CountryCode ORDER BY COUNT(t.Id) DESC) AS [Rank]
		FROM Accounts AS a
		JOIN AccountsTrips AS ac 
		  ON ac.AccountId = a.Id
		JOIN Trips AS t
		  ON t.Id = ac.TripId
		JOIN Rooms AS r
  		  ON r.Id = t.RoomId
		JOIN Hotels AS h 
		  ON h.Id = r.HotelId
		JOIN Cities AS c
		  ON c.Id = h.CityId
	GROUP BY a.Id,
			 a.Email,
			 c.CountryCode
    ) AS h
   WHERE [Rank] = 1
ORDER BY Trips DESC,
		 Id