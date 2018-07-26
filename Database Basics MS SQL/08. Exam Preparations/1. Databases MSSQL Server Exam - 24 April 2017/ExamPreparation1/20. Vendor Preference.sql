WITH CTE_PartsCountByMechanic AS (
	  SELECT m.MechanicId,
	         v.VendorId,
			 SUM(op.Quantity) AS VendorParts
	    FROM Mechanics AS m
	    JOIN Jobs AS j
	      ON j.MechanicId = m.MechanicId
	    JOIN Orders AS o
	      ON o.JobId = j.JobId
	    JOIN OrderParts AS op
	      ON op.OrderId = o.OrderId
	    JOIN Parts AS p
	      ON p.PartId = op.PartId
	    JOIN Vendors AS v
	      ON v.VendorId = p.VendorId
	GROUP BY m.MechanicId,
	         v.VendorId
)

  SELECT CONCAT(m.FirstName, ' ', m.LastName) AS Mechanic,
    	 v.[Name] AS Vendor,
  		 cte.VendorParts AS Parts,
		 CAST(CAST(CAST(VendorParts AS DECIMAL(6, 2)) / (SELECT SUM(VendorParts) 
		                                                   FROM CTE_PartsCountByMechanic 
														  WHERE MechanicId = cte.MechanicId)
												* 100 AS INT) 
										AS VARCHAR) + '%' AS Preference
    FROM CTE_PartsCountByMechanic AS cte
    JOIN Mechanics AS m
      ON m.MechanicId = cte.MechanicId
    JOIN Vendors AS v
      ON v.VendorId = cte.VendorId
ORDER BY Mechanic,
		 Parts DESC,
		 Vendor