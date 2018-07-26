UPDATE Countries
   SET CountryName = 'Burma'
 WHERE CountryName = 'Myanmar'

INSERT INTO Monasteries ([Name], CountryCode)
     VALUES ('Hanga Abbey', (
				SELECT CountryCode 
				  FROM Countries 
				 WHERE CountryName = 'Tanzania'
	            )
	        ),
			('Myin-Tin-Daik', (
				SELECT CountryCode 
				  FROM Countries 
				 WHERE CountryName = 'Myanmar'
				)
	        )

   SELECT con.ContinentName,
          c.CountryName,
		  COUNT(m.Id) AS MonasteriesCount
     FROM Continents AS con
     JOIN Countries AS c
       ON c.ContinentCode = con.ContinentCode
LEFT JOIN Monasteries AS m
       ON m.CountryCode = c.CountryCode
	WHERE c.IsDeleted = 0
 GROUP BY con.ContinentName,
          c.CountryName
 ORDER BY COUNT(m.Id) DESC,
		  c.CountryName