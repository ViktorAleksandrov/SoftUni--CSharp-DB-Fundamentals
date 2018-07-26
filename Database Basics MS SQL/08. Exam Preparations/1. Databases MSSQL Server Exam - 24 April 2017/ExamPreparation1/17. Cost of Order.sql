CREATE FUNCTION udf_GetCost (@jobId INT)
RETURNS DECIMAL(15, 2)
AS
BEGIN
	DECLARE @totalCost DECIMAL(15, 2) = (
		SELECT ISNULL(SUM(p.Price * op.Quantity), 0)
		  FROM Orders AS o
		  JOIN OrderParts AS op
		    ON op.OrderId = o.OrderId
		  JOIN Parts AS p
		    ON p.PartId = op.PartId
		 WHERE o.JobId = @jobId
	 )

	 RETURN @totalCost
END