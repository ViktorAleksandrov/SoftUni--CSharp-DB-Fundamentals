  SELECT a.Id AS AccountId,
         CONCAT(a.FirstName, ' ', a.LastName) AS FullName,
		 MAX(DATEDIFF(DAY, t.Arrivaldate, t.ReturnDate)) AS LongestTrip,
		 MIN(DATEDIFF(DAY, t.Arrivaldate, t.ReturnDate)) AS ShortestTrip
    FROM Accounts AS a
    JOIN AccountsTrips AS ac
  	  ON ac.AccountId = a.Id
    JOIN Trips AS t
  	  ON t.Id = ac.TripId
   WHERE a.MiddleName IS NULL
     AND t.CancelDate IS NULL
GROUP BY a.Id,
		 a.FirstName,
		 a.LastName
ORDER BY LongestTrip DESC,
		 AccountId