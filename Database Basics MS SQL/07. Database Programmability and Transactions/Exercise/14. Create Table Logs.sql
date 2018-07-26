CREATE TABLE Logs (
	LogId INT PRIMARY KEY IDENTITY,
	AccountId INT FOREIGN KEY REFERENCES Accounts(Id),
	OldSum DECIMAL(15, 2) NOT NULL,
	NewSum DECIMAL(15, 2) NOT NULL
)

CREATE TRIGGER tr_AccountsBalanceUpdate 
ON Accounts 
FOR UPDATE
AS
BEGIN
	DECLARE @accountId INT = (
		SELECT Id
		  FROM inserted
	)
	DECLARE @oldBalance DECIMAL(15, 2) = (
		SELECT Balance
		  FROM deleted
	)
	DECLARE @newBalance DECIMAL(15, 2) = (
		SELECT Balance
		  FROM inserted
	)

	INSERT INTO Logs
	VALUES (@accountId, @oldBalance, @newBalance)
END