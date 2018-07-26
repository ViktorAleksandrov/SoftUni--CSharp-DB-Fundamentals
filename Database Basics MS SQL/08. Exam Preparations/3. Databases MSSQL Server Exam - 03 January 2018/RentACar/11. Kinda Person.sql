  SELECT Names,
	     Class
    FROM (
	      SELECT c.Id,
		    	 CONCAT(c.FirstName, ' ', c.LastName) AS Names,
		         m.Class,
		         RANK() OVER (PARTITION BY c.Id ORDER BY COUNT(m.Class) DESC) AS [Rank]
	        FROM Clients AS c
	        JOIN Orders AS o
	          ON o.ClientId = c.Id
	        JOIN Vehicles AS v
	          ON v.Id = o.VehicleId
	        JOIN Models AS m
	          ON m.Id = v.ModelId
		GROUP BY c.Id,
			     c.FirstName,
			     c.LastName,
			     m.Class
	) AS r
   WHERE [Rank] = 1
ORDER BY Names,
	     Class,
		 Id