  SELECT d.[Name] AS [Department Name],
		 c.[Name] AS [Category Name],
		 ROUND(
			  CAST(COUNT(r.Id) AS FLOAT) / 
			  (SELECT COUNT(r.Id) 
			     FROM Departments AS de 
				 JOIN Categories AS c 
				   ON c.DepartmentId = de.Id 
				 JOIN Reports AS r 
				   ON r.CategoryId = c.Id 
		     GROUP BY de.Id 
			   HAVING de.Id = d.Id) * 100, 0
			 ) AS [Percentage]
    FROM Departments AS d
    JOIN Categories AS c
      ON c.DepartmentId = d.Id
    JOIN Reports AS r
      ON r.CategoryId = c.Id
GROUP BY d.Id,
		 d.[Name],
		 c.Id,
		 c.[Name]
ORDER BY [Department Name],
		 [Category Name],
		 [Percentage]