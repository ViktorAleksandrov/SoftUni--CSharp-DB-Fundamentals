CREATE PROC usp_TransferMoney (@senderId INT, @receiverId INT, @amount DECIMAL(15, 4)) 
AS
BEGIN
	BEGIN TRANSACTION

		EXEC usp_WithdrawMoney @senderId, @amount

		DECLARE @senderBalance DECIMAL(15, 4) = (
			SELECT Balance FROM Accounts WHERE Id = @senderId
		)

		IF (@senderBalance < 0)
		BEGIN
			ROLLBACK;
			RAISERROR('Insufficient funds!', 16, 1);
			RETURN;
		END

		EXEC usp_DepositMoney  @receiverId, @amount

	COMMIT
END