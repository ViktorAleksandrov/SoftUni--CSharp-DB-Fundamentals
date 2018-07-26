DECLARE @alexId INT = (
	SELECT Id
	  FROM Users
	 WHERE Username = 'Alex'
)

DECLARE @edinburghGameId INT = (
	SELECT Id
	  FROM Games
	 WHERE [Name] = 'Edinburgh'
)

DECLARE @alexEdinburghGameId INT = (
	SELECT Id
	  FROM UsersGames
	 WHERE GameId = @edinburghGameId
	   AND UserId = @alexId
)

DECLARE @totalPrice DECIMAL(15, 2) = (
	SELECT SUM(Price)
	  FROM Items
	 WHERE [Name] IN ('Blackguard', 
					  'Bottomless Potion of Amplification', 
					  'Eye of Etlich (Diablo III)', 
					  'Gem of Efficacious Toxin', 
					  'Golden Gorget of Leoric', 
					  'Hellfire Amulet')
)

INSERT INTO UserGameItems (ItemId, UserGameId)
     SELECT Id,
	        @alexEdinburghGameId
	   FROM Items
	  WHERE [Name] IN ('Blackguard', 
					   'Bottomless Potion of Amplification', 
					   'Eye of Etlich (Diablo III)', 
					   'Gem of Efficacious Toxin', 
					   'Golden Gorget of Leoric', 
					   'Hellfire Amulet')

UPDATE UsersGames
   SET Cash -= @totalPrice
 WHERE Id = @alexEdinburghGameId


  SELECT u.Username,
  	     g.[Name],
  	     ug.Cash,
  	     i.[Name] AS [Item Name]
    FROM Users AS u
    JOIN UsersGames AS ug
      ON ug.UserId = u.Id
    JOIN Games AS g
      ON g.Id = ug.GameId
    JOIN UserGameItems AS ugi
      ON ugi.UserGameId = ug.Id
    JOIN Items AS i
      ON i.Id = ugi.ItemId
   WHERE g.[Name] = 'Edinburgh'
ORDER BY [Item Name]