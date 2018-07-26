CREATE PROC usp_EmployeesBySalaryLevel (@salaryLevel CHAR(7))
AS
	SELECT FirstName,
		   LastName
	  FROM Employees
	 WHERE dbo.ufn_GetSalaryLevel (Salary) = @salaryLevel