WITH CTE_AverageStats (AverageMind, AverageLuck, AverageSpeed) AS (
	SELECT AVG(Mind),
		   AVG(Luck),
		   AVG(Speed)
	  FROM [Statistics]
)  
  
  SELECT i.[Name],
		 i.Price, 
		 i.MinLevel,
		 s.Strength,
		 s.Defence,
		 s.Speed,
		 s.Luck,
		 s.Mind
    FROM Items AS i
    JOIN [Statistics] AS s
      ON s.Id = i.StatisticId
   WHERE s.Mind > (SELECT AverageMind FROM CTE_AverageStats)
     AND s.Luck > (SELECT AverageLuck FROM CTE_AverageStats)
     AND s.Speed > (SELECT AverageSpeed FROM CTE_AverageStats)
ORDER BY i.[Name]