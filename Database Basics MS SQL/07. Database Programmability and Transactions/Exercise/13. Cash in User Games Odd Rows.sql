CREATE FUNCTION ufn_CashInUsersGames (@gameName VARCHAR(50))
RETURNS TABLE
AS
RETURN (
	SELECT SUM(s.Cash) AS SumCash
	  FROM (
		SELECT ug.Cash,
			   ROW_NUMBER() OVER (ORDER BY ug.Cash DESC) AS RowNumber
		  FROM Games AS g
		  JOIN UsersGames AS ug
		    ON ug.GameId = g.Id
		 WHERE g.[Name] = @gameName) AS s
	 WHERE s.RowNumber % 2 = 1
)