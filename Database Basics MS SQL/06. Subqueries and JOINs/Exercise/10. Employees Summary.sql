  SELECT TOP (50)
  	     e.EmployeeID,
  	     CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
		 CONCAT(emp.FirstName, ' ', emp.LastName) AS ManagerName,
		 d.[Name] AS DepartmentName
    FROM Employees AS e
	JOIN Employees AS emp
	  ON emp.EmployeeID = e.ManagerID
    JOIN Departments AS d
	  ON d.DepartmentID = e.DepartmentID
ORDER BY e.EmployeeID