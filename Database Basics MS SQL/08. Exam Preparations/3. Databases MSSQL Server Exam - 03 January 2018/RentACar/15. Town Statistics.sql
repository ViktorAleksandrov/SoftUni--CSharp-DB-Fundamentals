  SELECT t.[Name] AS TownName,
         SUM(M) * 100 / (ISNULL(SUM(M), 0) + ISNULL(SUM(F), 0)) AS MalePercent,
         SUM(F) * 100 / (ISNULL(SUM(M), 0) + ISNULL(SUM(F), 0)) AS FemalePercent
    FROM (SELECT o.TownId,
		         CASE
				     WHEN c.Gender = 'M'
				     THEN COUNT(o.Id)
					 ELSE NULL
	             END AS M,
	             CASE
				     WHEN c.Gender = 'F'
				     THEN COUNT(o.Id)
				     ELSE NULL
	             END AS F
	        FROM Orders AS o
	        JOIN Clients AS c 
	    	  ON c.Id = o.ClientId
	    GROUP BY o.TownId,
	   	         c.Gender
    ) AS h
	JOIN Towns AS t
	  ON t.Id = TownId
GROUP BY t.Id,
		 t.[Name]
ORDER BY TownName,
         t.Id