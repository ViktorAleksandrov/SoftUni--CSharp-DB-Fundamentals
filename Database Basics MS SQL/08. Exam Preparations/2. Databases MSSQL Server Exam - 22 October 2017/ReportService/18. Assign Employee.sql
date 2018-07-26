CREATE PROC usp_AssignEmployeeToReport (@employeeId INT, @reportId INT)
AS
BEGIN  
	DECLARE @employeeDepartmentId INT = (
		SELECT DepartmentId
		  FROM Employees
		 WHERE @employeeId = Id
	)

	DECLARE @reportDepartmentId INT = (
		SELECT c.DepartmentId
		  FROM Reports AS r
		  JOIN Categories AS c
		    ON c.Id = r.CategoryId
		 WHERE @reportId = r.Id
	)

	IF (@employeeDepartmentId <> @reportDepartmentId)
	BEGIN
		RAISERROR('Employee doesn''t belong to the appropriate department!', 16, 1)
		RETURN
	END

	UPDATE Reports
	   SET EmployeeId = @employeeId
	 WHERE Id = @reportId
END