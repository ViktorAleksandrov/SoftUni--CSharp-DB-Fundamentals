  SELECT TripId,
	     SUM(Luggage) AS Luggage,
	     CONCAT('$', SUM(Luggage) * 
			CASE 
				WHEN SUM(Luggage) > 5 THEN 5 
				ELSE 0 
			END) AS Fee
    FROM AccountsTrips
GROUP BY TripId
  HAVING SUM(Luggage) > 0
ORDER BY Luggage DESC