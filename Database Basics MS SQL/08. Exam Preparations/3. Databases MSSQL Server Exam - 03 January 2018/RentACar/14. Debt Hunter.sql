  SELECT [Category Name],
		 Email,
		 Bill,
		 Town
    FROM (
	     SELECT c.Id,
	   		    CONCAT(c.FirstName, ' ', c.LastName) AS [Category Name],
	   		    c.Email,
	   		    o.Bill,
	   		    t.[Name] AS Town,
	   		    ROW_NUMBER() OVER (PARTITION BY t.[Name] ORDER BY o.Bill DESC) AS [Rank]
	       FROM Clients AS c
	       JOIN Orders AS o
	         ON o.ClientId = c.Id
	       JOIN Towns AS t
	         ON t.Id = o.TownId
	      WHERE c.CardValidity < o.CollectionDate
	        AND o.Bill IS NOT NULL
       ) AS H
   WHERE [Rank] <= 2
ORDER BY Town,
		 Bill,
		 Id