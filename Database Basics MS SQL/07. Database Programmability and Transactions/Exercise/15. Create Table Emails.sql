CREATE TABLE NotificationEmails (
	Id INT PRIMARY KEY IDENTITY,
	Recipient INT FOREIGN KEY REFERENCES Accounts(Id),
	[Subject] VARCHAR(MAX) NOT NULL,
	Body VARCHAR(MAX) NOT NULL
)

CREATE TRIGGER tr_LogsInsert
ON Logs
FOR INSERT
AS
BEGIN
	INSERT INTO NotificationEmails (Recipient, [Subject], Body)
	SELECT AccountId, 
		   CONCAT('Balance change for account: ', AccountId),
		   CONCAT('On ', GETDATE(), ' your balance was changed from ', OldSum,' to ', NewSum, '.')
	  FROM inserted
END