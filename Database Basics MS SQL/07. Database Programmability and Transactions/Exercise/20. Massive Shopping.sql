DECLARE @userId INT = (
	SELECT Id 
	  FROM Users 
	 WHERE Username = 'Stamat'
)
DECLARE @gameId INT = (
	SELECT Id 
	  FROM Games 
	 WHERE [Name] = 'Safflower'
)
DECLARE @userGameId INT = (
	SELECT Id 
	  FROM UsersGames 
	 WHERE UserId = @userId
	   AND GameId = @gameId
)

BEGIN TRY
    BEGIN TRANSACTION

	UPDATE UsersGames
       SET Cash -= (
			SELECT SUM(Price) 
			  FROM Items 
			 WHERE MinLevel IN (11, 12)
       )
     WHERE Id = @userGameId

	DECLARE @userCash DECIMAL(15, 4) = (
		SELECT Cash 
		  FROM UsersGames 
		 WHERE Id = @userGameId
	)

	IF (@userCash < 0)
	BEGIN
		ROLLBACK;
		  RETURN;
	END

	INSERT INTO UserGameItems
	     SELECT Id, @userGameId
	       FROM Items
	      WHERE MinLevel IN (11, 12)

    COMMIT
END TRY

BEGIN CATCH
		ROLLBACK;
END CATCH

BEGIN TRY
	BEGIN TRANSACTION

	UPDATE UsersGames
	   SET Cash -= (
			SELECT SUM(Price) 
			  FROM Items 
			 WHERE MinLevel BETWEEN 19 AND 21
	   )
	 WHERE Id = @userGameId

	SET @userCash = (
		SELECT Cash 
		  FROM UsersGames 
		 WHERE Id = @userGameId
	)

	IF(@userCash < 0)
	BEGIN
		ROLLBACK;
		RETURN;
	END

	INSERT INTO UserGameItems
	     SELECT Id, 
		        @userGameId
	       FROM Items
	      WHERE MinLevel BETWEEN 19 AND 21

   COMMIT
END TRY

BEGIN CATCH
		ROLLBACK;
END CATCH

  SELECT i.[Name] AS [Item Name]
    FROM Items AS i
    JOIN UserGameItems AS ugi
      ON ugi.ItemId = i.Id
   WHERE ugi.UserGameId = @userGameId
ORDER BY i.Name ASC