  SELECT c.[Name] AS CategoryName,
  	     COUNT(e.Id) AS [Employees Number]
    FROM Categories AS c
    JOIN Employees AS e
      ON e.DepartmentId = c.DepartmentId
GROUP BY c.Id,
		 c.[Name]
ORDER BY CategoryName