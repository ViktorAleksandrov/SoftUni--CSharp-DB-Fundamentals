WITH CTE_OpenedReports (EmployeeId, Count) AS (
   	  SELECT e.Id, 
  	         COUNT(r.Id) 
  	    FROM Employees AS e
  	    JOIN Reports AS r
  	      ON e.Id = r.EmployeeId
  	   WHERE YEAR(r.OpenDate) = 2016
    GROUP BY e.Id
),

CTE_ClosedReports (EmployeeId, Count) AS (
	  SELECT e.Id, 
		     COUNT(r.Id) 
		FROM Employees AS e
	    JOIN Reports AS r
	      ON e.Id = r.EmployeeId
	   WHERE YEAR(r.CloseDate) = 2016
	GROUP BY e.Id
)

    SELECT CONCAT(e.FirstName, ' ', e.LastName) AS [Name],
      	   CONCAT(ISNULL(c.Count, 0), '/', ISNULL(o.Count, 0)) AS [Closed Open Reports]
      FROM CTE_ClosedReports AS c
RIGHT JOIN CTE_OpenedReports AS o
        ON c.EmployeeId = o.EmployeeId
      JOIN Employees AS e
        ON c.EmployeeId = e.Id 
		OR o.EmployeeId = e.Id
  ORDER BY [Name], 
		   e.Id