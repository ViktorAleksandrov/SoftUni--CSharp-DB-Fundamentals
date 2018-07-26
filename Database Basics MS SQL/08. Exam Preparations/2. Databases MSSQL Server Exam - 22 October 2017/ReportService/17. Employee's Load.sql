CREATE FUNCTION udf_GetReportsCount (@employeeId INT, @statusId INT)
RETURNS INT
AS
BEGIN
	DECLARE @reportsCount INT = (
		SELECT COUNT(Id)
		  FROM Reports
		 WHERE EmployeeId = @employeeId
		   AND StatusId = @statusId
	)

	RETURN @reportsCount
END