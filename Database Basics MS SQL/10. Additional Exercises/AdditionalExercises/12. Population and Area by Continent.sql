  SELECT con.ContinentName,
  		 SUM(c.AreaInSqKm) AS CountriesArea,
		 SUM(CAST(c.[Population] AS BIGINT)) AS CountriesPopulation
    FROM Continents AS con
    JOIN Countries AS c
      ON c.ContinentCode = con.ContinentCode
GROUP BY con.ContinentName
ORDER BY CountriesPopulation DESC