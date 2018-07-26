  SELECT c.[Name] AS [Category Name],
  		 COUNT(r.Id) AS [Reports Number],
		 CASE
			 WHEN SUM(CASE WHEN s.Label = 'in progress' THEN 1 ELSE 0 END) >
			 	  SUM(CASE WHEN s.Label = 'waiting' THEN 1 ELSE 0 END) THEN 'in progress'
			 WHEN SUM(CASE WHEN s.Label = 'in progress' THEN 1 ELSE 0 END) <
			 	  SUM(CASE WHEN s.Label = 'waiting' THEN 1 ELSE 0 END) THEN 'waiting'
			 ELSE 'equal'
		 END AS [Main Status]
    FROM Categories AS c
    JOIN Reports AS r
      ON r.CategoryId = c.Id
    JOIN [Status] AS s
      ON s.Id = r.StatusId
   WHERE s.Label = 'waiting' 
      OR s.Label = 'in progress'
GROUP BY c.Id,
		 c.[Name]
ORDER BY [Category Name],
		 [Reports Number],
		 [Main Status]