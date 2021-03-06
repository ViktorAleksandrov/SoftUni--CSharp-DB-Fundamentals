   SELECT m.Manufacturer,
		  m.Model,
	      COUNT(o.Id) AS TimesOrdered
     FROM Models AS m
     JOIN Vehicles AS v
       ON v.ModelId = m.Id
LEFT JOIN Orders AS o
       ON o.VehicleId = v.Id
 GROUP BY m.Id,
 		  m.Manufacturer,
 		  m.Model
 ORDER BY TimesOrdered DESC,
 		  m.Manufacturer DESC,
 		  m.Model