CREATE TRIGGER tr_ReportsCloseDateEntered
ON Reports
FOR UPDATE
AS
BEGIN
	DECLARE @completedStatusId INT = (
		SELECT Id
		  FROM [Status]
		 WHERE Label = 'completed'
	)

	UPDATE Reports
	   SET StatusId = @completedStatusId
	 WHERE Id IN (
			SELECT Id
			  FROM inserted
			 WHERE Id IN (
					SELECT Id
					  FROM deleted
					 WHERE CloseDate IS NULL
			 )
			   AND CloseDate IS NOT NULL
	 )
END