   SELECT CONCAT(e.FirstName, ' ', e.LastName) AS [Name],
 		  COUNT(DISTINCT r.UserId) AS [Users Number]
     FROM Employees AS e
LEFT JOIN Reports AS r
       ON r.EmployeeId = e.Id
 GROUP BY e.Id,
 		  e.FirstName,
 		  e.LastName
 ORDER BY [Users Number] DESC,
 		  [Name]