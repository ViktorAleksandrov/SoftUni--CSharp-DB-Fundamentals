CREATE PROC usp_GetHoldersWithBalanceHigherThan (@number DECIMAL(15, 2))
AS
  SELECT ah.FirstName,
  	     ah.LastName
    FROM AccountHolders AS ah
    JOIN (SELECT AccountHolderId,
	             SUM(Balance) AS TotalBalance
		    FROM Accounts
        GROUP BY AccountHolderId) AS a
      ON a.AccountHolderId = ah.Id
   WHERE a.TotalBalance > @number
ORDER BY ah.LastName,
		 ah.FirstName