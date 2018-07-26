CREATE PROC usp_PlaceOrder (@jobId INT, @serialNumber VARCHAR(50), @quantity INT)
AS
BEGIN
	IF (@quantity <= 0)
	BEGIN
		;THROW 50012, 'Part quantity must be more than zero!', 1
	END

	DECLARE @jobStatus VARCHAR(11) = (
		SELECT [Status]
		  FROM Jobs 
		 WHERE JobId = @jobId
	)

	IF (@jobStatus = 'Finished')
	BEGIN
		;THROW 50011, 'This job is not active!', 1
	END

	DECLARE @neededJobId INT = (
		SELECT JobId 
		  FROM Jobs
		 WHERE JobId = @jobId
	)

	IF (@neededJobId IS NULL)
	BEGIN
		;THROW 50013, 'Job not found!', 1
	END

	DECLARE @partId INT = (
		SELECT PartId 
		  FROM Parts 
		 WHERE SerialNumber = @serialNumber
	)

	IF (@partId IS NULL)
	BEGIN
		;THROW 50014, 'Part not found!', 1
	END

	DECLARE @orderId INT = (
		SELECT TOP (1)
			   OrderId 
		  FROM Orders 
		 WHERE JobId = @jobId
		   AND IssueDate IS NULL
	)

	IF (@orderId IS NULL)
	BEGIN
		INSERT INTO Orders (JobId, IssueDate)
		     VALUES (@jobId, NULL)

		INSERT INTO OrderParts (OrderId, PartId, Quantity)
		     VALUES (IDENT_CURRENT('Orders'), @partId, @quantity)
	END

	ELSE
	BEGIN
		DECLARE @partIdInOrder INT = (
			SELECT PartId
			  FROM OrderParts
			 WHERE OrderId = @orderId
			   AND PartId = @partId
		)

		IF (@partIdInOrder IS NULL)
		BEGIN
			INSERT INTO OrderParts (OrderId, PartId, Quantity) 
				 VALUES (@orderId, @partId, @quantity)
		END

		ELSE
		BEGIN
			UPDATE OrderParts
               SET Quantity += @quantity
             WHERE OrderId = @orderId 
			   AND PartId = @partId
		END
	END
END