  SELECT u.Username, 
  	     g.[Name] AS Game,
  	     MAX(c.[Name]) AS [Character],
  	     SUM(si.Strength) + MAX(sg.Strength) + MAX(sc.Strength) AS Strength,
  	     SUM(si.Defence) + MAX(sg.Defence) + MAX(sc.Defence) AS Defence,
  	     SUM(si.Speed) + MAX(sg.Speed) + MAX(sc.Speed) AS Speed,
  	     SUM(si.Mind) + MAX(sg.Mind) + MAX(sc.Mind) AS Mind,
  	     SUM(si.Luck) + MAX(sg.Luck) + MAX(sc.Luck) AS Luck
    FROM Users AS u
    JOIN UsersGames AS ug
      ON ug.UserId = u.Id
    JOIN Games AS g
      ON g.Id = ug.GameId
    JOIN Characters AS c
      ON c.Id = ug.CharacterId
    JOIN GameTypes AS gt
      ON gt.Id = g.GameTypeId
    JOIN UserGameItems ugi
      ON ugi.UserGameId = ug.Id
    JOIN Items AS i
      ON i.Id = ugi.ItemId
    JOIN [Statistics] AS si
      ON si.Id = i.StatisticId
    JOIN [Statistics] AS sg
      ON sg.Id = gt.BonusStatsId
    JOIN [Statistics] AS sc
      ON sc.Id = c.StatisticId
GROUP BY u.Username, 
         g.[Name]
ORDER BY Strength DESC, 
		 Defence DESC, 
		 Speed DESC, 
		 Mind DESC, 
		 Luck DESC