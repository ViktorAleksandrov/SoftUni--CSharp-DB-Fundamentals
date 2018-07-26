CREATE TRIGGER tr_ItemsPurchaseRestrictions
ON UserGameItems
INSTEAD OF INSERT
AS
BEGIN
	INSERT INTO UserGameItems
         SELECT ItemId, 
                UserGameId 
           FROM inserted
          WHERE ItemId IN (
				SELECT Id
				  FROM Items
				 WHERE MinLevel <= (
						SELECT [Level]
						  FROM UsersGames
						 WHERE Id = UserGameId
				 )
		  )
END

UPDATE UsersGames
   SET Cash += 50000 + (
		SELECT SUM(i.Price)
		  FROM Items AS i
		  JOIN UserGameItems AS ugi
		    ON ugi.ItemId = i.Id
	     WHERE ugi.UserGameId = UsersGames.Id
   )
 WHERE UserId IN (
	SELECT Id
	  FROM Users
     WHERE Username IN ('baleremuda', 'loosenoise', 'inguinalself', 'buildingdeltoid', 'monoxidecos')
 )
   AND GameId = (
		SELECT Id
	      FROM Games
		 WHERE [Name] = 'Bali' 
   )

INSERT INTO UserGameItems (UserGameId, ItemId)
	 SELECT UsersGames.Id,
	        i.Id
	   FROM UsersGames,
	        Items AS i
	  WHERE UserId IN (
			SELECT Id
			  FROM Users
			 WHERE Username IN ('baleremuda', 'loosenoise', 'inguinalself', 'buildingdeltoid', 'monoxidecos'
			 )
			   AND GameId = (
			  		SELECT Id
			  	      FROM Games
			  		 WHERE [Name] = 'Bali' 
			   )
			   AND (
				     (i.Id > 250 AND i.Id < 300)
				  OR (i.Id > 500 AND i.Id < 540)
			   )
	  )

UPDATE UsersGames
SET Cash -= (
	SELECT SUM(i.Price)
		  FROM Items AS i
		  JOIN UserGameItems AS ugi
		    ON ugi.ItemId = i.Id
	     WHERE ugi.UserGameId = UsersGames.Id
)
 WHERE UserId IN (
	SELECT Id
	  FROM Users
     WHERE Username IN ('baleremuda', 'loosenoise', 'inguinalself', 'buildingdeltoid', 'monoxidecos')
 )
   AND GameId = (
		SELECT Id
	      FROM Games
		 WHERE [Name] = 'Bali' 
   )

  SELECT u.Username,
  	     g.[Name],
  	     ug.Cash,
  	   i.[Name] AS [Item Name]
    FROM UsersGames AS ug
    JOIN Games AS g
      ON g.Id = ug.GameId
    JOIN Users AS u
      ON u.Id = ug.UserId
    JOIN UserGameItems AS ugi
      ON ugi.UserGameId = ug.Id
    JOIN Items AS i
      ON i.Id = ugi.ItemId
   WHERE g.[Name] = 'Bali'
ORDER BY Username,
         [Item Name]