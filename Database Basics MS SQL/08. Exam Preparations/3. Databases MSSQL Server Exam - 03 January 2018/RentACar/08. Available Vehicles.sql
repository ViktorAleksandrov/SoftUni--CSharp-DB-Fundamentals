  SELECT m.Model,
	     m.Seats,
	     v.Mileage
    FROM Models AS m
    JOIN Vehicles AS v
      ON v.ModelId = m.Id
   WHERE v.Id NOT IN (
			SELECT VehicleId
			  FROM Orders 
			 WHERE ReturnDate IS NULL
		 )
ORDER BY v.Mileage,
	     m.Seats DESC,
		 m.Id