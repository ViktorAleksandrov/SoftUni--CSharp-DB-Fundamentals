WITH CTE_C (ReturnOfficeId, OfficeId, VehicleId, Manufacturer, Model) AS (
	SELECT w.ReturnOfficeId,
           w.OfficeId,
           w.Id,
           w.Manufacturer,
           w.Model
      FROM (SELECT DENSE_RANK() OVER(PARTITION BY v.Id ORDER BY o.CollectionDate DESC) AS [Rank],
                   o.ReturnOfficeId,
                   v.OfficeId,
                   v.Id,
                   m.Manufacturer,
                   m.Model
              FROM Models AS m
              JOIN Vehicles AS v 
			    ON m.Id = v.ModelId
         LEFT JOIN Orders AS o 
		        ON o.VehicleId = v.Id
       ) AS w
      WHERE w.[Rank] = 1
)
  
  SELECT CONCAT(Manufacturer, ' - ', Model) AS Vehicle,
		 CASE
		    WHEN (
				SELECT COUNT(*) 
				  FROM Orders 
				 WHERE VehicleId = c.VehicleId
			) = 0 
				  OR c.OfficeId = c.ReturnOfficeId
			THEN 'home'
		    WHEN c.ReturnOfficeId IS NULL THEN 'on a rent'
		    WHEN c.ReturnOfficeId <> c.OfficeId THEN (
			  	SELECT CONCAT(t.[Name], ' - ', ofi.[Name])
			  	  FROM Offices AS ofi
			  	  JOIN Towns AS t
			  	    ON t.Id = ofi.TownId
			  	 WHERE c.ReturnOfficeId = ofi.Id)
		 END AS [Location]
    FROM CTE_C AS c
ORDER BY Vehicle,
		 c.VehicleId